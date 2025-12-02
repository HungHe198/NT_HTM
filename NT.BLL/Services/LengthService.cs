using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class LengthService : GenericService<Length>
    {
        public LengthService(IGenericRepository<Length> repository) : base(repository) { }
    }
}