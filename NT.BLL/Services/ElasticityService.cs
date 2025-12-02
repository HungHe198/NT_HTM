using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ElasticityService : GenericService<Elasticity>
    {
        public ElasticityService(IGenericRepository<Elasticity> repository) : base(repository) { }
    }
}