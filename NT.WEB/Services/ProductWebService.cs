using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text;
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;

namespace NT.WEB.Services
{
    public class ProductWebService : ProductService, ISearchByNameService<Product>
    {
        public ProductWebService(IGenericRepository<Product> repository) : base(repository)
        {
        }
        public Task<IEnumerable<Product>> SearchByNameAsync(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return _repository.GetAllAsync();

            var term = partialName.Trim().ToLowerInvariant();
            Expression<Func<Product, bool>> predicate = p =>
                (p.Name != null && p.Name.ToLower().Contains(term)) ||
                (p.ProductCode != null && p.ProductCode.ToLower().Contains(term));

            return _repository.FindAsync(predicate);
        }

        /// <summary>
        /// Fuzzy, accent-insensitive suggestions for product search box.
        /// Keeps DB filtering simple then ranks in-memory for best matches.
        /// </summary>
        public async Task<IEnumerable<Product>> SuggestAsync(string input, int max = 10)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Array.Empty<Product>();

            var termRaw = input.Trim();
            var termNorm = Normalize(termRaw);
            var firstToken = termNorm.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? termNorm;

            // coarse DB pre-filter by first token on name or code to avoid pulling entire table
            var tokenLower = firstToken.ToLowerInvariant();
            Expression<Func<Product, bool>> predicate = p =>
                (p.Name != null && p.Name.ToLower().Contains(tokenLower)) ||
                (p.ProductCode != null && p.ProductCode.ToLower().Contains(tokenLower));

            var candidates = (await _repository.FindAsync(predicate)).ToList();

            // if nothing found by first token, broaden to all products (last resort)
            if (candidates.Count == 0)
            {
                candidates = (await _repository.GetAllAsync()).ToList();
            }

            var ranked = candidates
                .Select(p => new
                {
                    Product = p,
                    Score = Score(Normalize(p.Name ?? string.Empty), Normalize(p.ProductCode ?? string.Empty), termNorm)
                })
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Product.Name)
                .Take(Math.Max(1, Math.Min(max, 20)))
                .Select(x => x.Product)
                .ToList();

            return ranked;
        }

        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            var formD = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var chars = formD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                             .Select(c => char.IsLetterOrDigit(c) ? c : (c == ' ' ? ' ' : '\0'))
                             .Where(c => c != '\0')
                             .ToArray();
            var result = new string(chars);
            // collapse multiple spaces
            return string.Join(' ', result.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static int Score(string nameNorm, string codeNorm, string termNorm)
        {
            if (string.IsNullOrEmpty(nameNorm) && string.IsNullOrEmpty(codeNorm)) return 0;
            int score = 0;

            // exact/contains boosts
            if (!string.IsNullOrEmpty(nameNorm))
            {
                if (nameNorm.Contains(termNorm)) score += 100;
                foreach (var tk in termNorm.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (nameNorm.Contains(tk)) score += 15;
                }
                score += Math.Max(0, 50 - 2 * Levenshtein(nameNorm, termNorm));
            }

            if (!string.IsNullOrEmpty(codeNorm))
            {
                if (codeNorm.Contains(termNorm)) score += 80;
                score += Math.Max(0, 30 - 2 * Levenshtein(codeNorm, termNorm));
            }

            return score;
        }

        private static int Levenshtein(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;
            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];
            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
