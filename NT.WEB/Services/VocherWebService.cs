using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.SHARED.Models;
namespace NT.WEB.Services
{
    public class VocherWebService : VocherService
    {
        public VocherWebService(IGenericRepository<Voucher> repository)
            : base(repository)
        {
        }
    public Task<IEnumerable<Voucher>> SearchByCodeAsync(string partialCode)
        {
            if (string.IsNullOrWhiteSpace(partialCode))
                return _repository.GetAllAsync();
            System.Linq.Expressions.Expression<System.Func<Voucher, bool>> predicate = v => v.Code.Contains(partialCode);
            return _repository.FindAsync(predicate);
        }
    }
}
