using NT.SHARED.Models;

namespace NT.BLL.Interfaces
{
    /// <summary>
    /// D?ch v? qu?n l� thu?c t�nh s?n ph?m (?? c?ng, m�u s?c, ?? d�i).
    /// </summary>
    public interface IProductAttributeService
    {
        Task<IEnumerable<RodHardness>> GetAllHardnessAsync();
        Task<IEnumerable<RodColor>> GetAllColorsAsync();
        Task<IEnumerable<RodLength>> GetAllLengthsAsync();
    }
}
