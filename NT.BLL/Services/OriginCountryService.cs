using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class OriginCountryService : GenericService<OriginCountry>
    {
        public OriginCountryService(IGenericRepository<OriginCountry> repository) : base(repository) { }
    }
}