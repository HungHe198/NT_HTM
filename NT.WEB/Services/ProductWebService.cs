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
            var termLower = termRaw.ToLowerInvariant();
            var termNorm = Normalize(termRaw);
            var tokensNorm = termNorm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Stage 1: DB pre-filter using raw (diacritic-sensitive) to keep query cheap
            Expression<Func<Product, bool>> dbPredicate = p =>
                (p.Name != null && p.Name.ToLower().Contains(termLower)) ||
                (p.ProductCode != null && p.ProductCode.ToLower().Contains(termLower));

            var candidates = (await _repository.FindAsync(dbPredicate)).ToList();

            // Stage 2: If nothing matched in DB (e.g., user typed without accents),
            // fallback to in-memory accent-insensitive filter but DO NOT include all items –
            // keep only items whose normalized name/code contains any normalized token.
            if (candidates.Count == 0)
            {
                var all = (await _repository.GetAllAsync()).ToList();
                candidates = all.Where(p =>
                {
                    var nameN = Normalize(p.Name ?? string.Empty);
                    var codeN = Normalize(p.ProductCode ?? string.Empty);
                    if (!string.IsNullOrEmpty(nameN) && nameN.Contains(termNorm)) return true;
                    if (!string.IsNullOrEmpty(codeN) && codeN.Contains(termNorm)) return true;
                    if (tokensNorm.Length > 0)
                    {
                        foreach (var tk in tokensNorm)
                        {
                            if ((!string.IsNullOrEmpty(nameN) && nameN.Contains(tk)) ||
                                (!string.IsNullOrEmpty(codeN) && codeN.Contains(tk)))
                                return true;
                        }
                    }
                    return false;
                }).ToList();
            }

            // Rank results and drop weak matches by threshold to avoid showing everything
            var ranked = candidates
                .Select(p => new
                {
                    Product = p,
                    Score = Score(Normalize(p.Name ?? string.Empty), Normalize(p.ProductCode ?? string.Empty), termNorm)
                })
                .Where(x => x.Score >= 15) // threshold: filter out very weak matches
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
            var tokens = termNorm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // exact/contains boosts
            if (!string.IsNullOrEmpty(nameNorm))
            {
                if (nameNorm.Contains(termNorm)) score += 100;
                // Strong boost for starts-with (prefix) to bring closest names to top when typing first letters
                if (nameNorm.StartsWith(termNorm)) score += 120;
                foreach (var tk in termNorm.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (nameNorm.Contains(tk)) score += 15;
                    if (nameNorm.StartsWith(tk)) score += 40;
                }
                score += Math.Max(0, 50 - 2 * Levenshtein(nameNorm, termNorm));
            }

            if (!string.IsNullOrEmpty(codeNorm))
            {
                if (codeNorm.Contains(termNorm)) score += 80;
                if (codeNorm.StartsWith(termNorm)) score += 100;
                foreach (var tk in tokens)
                {
                    if (codeNorm.StartsWith(tk)) score += 35;
                }
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
