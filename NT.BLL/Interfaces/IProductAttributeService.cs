using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// D?ch v? qu?n lý thu?c tính s?n ph?m (?? c?ng, màu s?c, ?? dài).
    /// </summary>
    public interface IProductAttributeService
    {
        Task<IEnumerable<RodHardness>> GetAllHardnessAsync();
        Task<IEnumerable<RodColor>> GetAllColorsAsync();
        Task<IEnumerable<RodLength>> GetAllLengthsAsync();
    }
}
