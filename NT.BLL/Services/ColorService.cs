using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ColorService : GenericService<Color>
    {
        public ColorService(IGenericRepository<Color> repository) : base(repository) { }
    }
}