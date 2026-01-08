namespace NT.SHARED.Constants
{
    /// <summary>
    /// Các hằng số trạng thái sản phẩm
    /// </summary>
    public static class ProductStatus
    {
        /// <summary>
        /// Sản phẩm đang hoạt động, hiển thị cho khách hàng
        /// </summary>
        public const string Active = "1";

        /// <summary>
        /// Sản phẩm ngừng bán, chỉ hiển thị cho Admin/Employee
        /// </summary>
        public const string Inactive = "0";

        /// <summary>
        /// Lấy tên hiển thị của trạng thái
        /// </summary>
        public static string GetDisplayName(string? status)
        {
            return status switch
            {
                Active => "Hoạt động",
                Inactive => "Ngừng bán",
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Kiểm tra sản phẩm có đang hoạt động không
        /// </summary>
        public static bool IsActive(string? status) => status == Active;

        /// <summary>
        /// Lấy tên hiển thị trạng thái tồn kho dựa trên số lượng
        /// </summary>
        public static string GetStockDisplayName(int totalStock)
        {
            return totalStock > 0 ? "Còn hàng" : "Hết hàng";
        }

        /// <summary>
        /// Kiểm tra sản phẩm còn hàng hay không
        /// </summary>
        public static bool IsInStock(int totalStock) => totalStock > 0;
    }
}
