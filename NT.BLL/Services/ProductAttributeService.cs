using System.Collections.Generic;
using System.Threading.Tasks;
using NT.BLL.Interfaces;
using NT.SHARED.Models;

namespace NT.BLL.Services
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IGenericRepository<RodHardness> _hardnessRepo;
        private readonly IGenericRepository<RodColor> _colorRepo;
        private readonly IGenericRepository<RodLength> _lengthRepo;

        public ProductAttributeService(
            IGenericRepository<RodHardness> hardnessRepo,
            IGenericRepository<RodColor> colorRepo,
            IGenericRepository<RodLength> lengthRepo)
        {
            _hardnessRepo = hardnessRepo;
            _colorRepo = colorRepo;
            _lengthRepo = lengthRepo;
        }

        public Task<IEnumerable<RodHardness>> GetAllHardnessAsync() => _hardnessRepo.GetAllAsync();
        public Task<IEnumerable<RodColor>> GetAllColorsAsync() => _colorRepo.GetAllAsync();
        public Task<IEnumerable<RodLength>> GetAllLengthsAsync() => _lengthRepo.GetAllAsync();
    }
}
