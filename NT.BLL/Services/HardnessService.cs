using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class HardnessService : GenericService<Hardness>
    {
        public HardnessService(IGenericRepository<Hardness> repository) : base(repository) { }
    }
}