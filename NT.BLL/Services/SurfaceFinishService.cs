using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class SurfaceFinishService : GenericService<SurfaceFinish>
    {
        public SurfaceFinishService(IGenericRepository<SurfaceFinish> repository) : base(repository) { }
    }
}