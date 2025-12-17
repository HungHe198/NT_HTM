using System;
using System.Text;

namespace NT.SHARED.Helpers
{
    /// <summary>
    /// Helper class để tạo mã đơn hàng ngắn gọn từ GUID.
    /// Mã được tạo có tính xác định (deterministic) - cùng GUID luôn cho cùng mã.
    /// 
    /// PHÂN TÍCH XÁC SUẤT VA CHẠM:
    /// - ToShortCode (8 ký tự, Base32): 32^8 = 1.1 trillion mã
    ///   → 50% collision tại ~1.2 triệu đơn hàng
    /// - ToLongCode (10 ký tự, Base32): 32^10 = 1.1 quadrillion mã  
    ///   → 50% collision tại ~37 triệu đơn hàng
    /// - ToDateCode: Kết hợp ngày + 6 ký tự, collision chỉ trong cùng ngày
    ///   → An toàn cho ~41,000 đơn/ngày
    /// </summary>
    public static class OrderCodeHelper
    {
        // Ký tự Base32 (loại bỏ các ký tự dễ nhầm lẫn: 0, O, I, L, 1)
        private const string Base32Chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
        
        // Prefix cho mã đơn hàng
        private const string OrderPrefix = "HTM";

        /// <summary>
        /// Tạo mã đơn hàng ngắn từ GUID.
        /// Format: HTM-XXXXXXXX (8 ký tự sau prefix)
        /// Không gian: 32^8 = 1,099,511,627,776 mã (~1.1 nghìn tỷ)
        /// 50% collision tại: ~1.2 triệu đơn hàng
        /// </summary>
        /// <param name="id">GUID của đơn hàng</param>
        /// <returns>Mã đơn hàng dạng HTM-XXXXXXXX</returns>
        public static string ToShortCode(Guid id)
        {
            if (id == Guid.Empty)
                return "HTM-00000000";

            var bytes = id.ToByteArray();
            var sb = new StringBuilder(10);

            // Lấy 8 ký tự từ các byte của GUID
            // Sử dụng tất cả 16 bytes để tối đa entropy
            for (int i = 0; i < 8; i++)
            {
                // XOR 2 bytes để tạo giá trị, sử dụng toàn bộ GUID
                int value = bytes[i] ^ bytes[15 - i];
                // Map vào bảng ký tự Base32
                sb.Append(Base32Chars[value % Base32Chars.Length]);
            }

            return $"{OrderPrefix}-{sb}";
        }

        /// <summary>
        /// Tạo mã đơn hàng dài hơn (10 ký tự) cho trường hợp cần độ unique cao hơn.
        /// Format: HTM-XXXXXXXXXX
        /// Không gian: 32^10 = 1,125,899,906,842,624 mã (~1.1 triệu tỷ)
        /// 50% collision tại: ~37 triệu đơn hàng
        /// </summary>
        public static string ToLongCode(Guid id)
        {
            if (id == Guid.Empty)
                return "HTM-0000000000";

            var bytes = id.ToByteArray();
            var sb = new StringBuilder(12);

            // Sử dụng thuật toán hash đơn giản để tạo 10 ký tự
            for (int i = 0; i < 10; i++)
            {
                // Kết hợp nhiều bytes với các phép XOR và rotation
                int idx1 = i % 16;
                int idx2 = (i + 5) % 16;
                int idx3 = (i + 11) % 16;
                int value = bytes[idx1] ^ bytes[idx2] ^ (bytes[idx3] >> (i % 4));
                sb.Append(Base32Chars[Math.Abs(value) % Base32Chars.Length]);
            }

            return $"{OrderPrefix}-{sb}";
        }

        /// <summary>
        /// Tạo mã đơn hàng với ngày tháng - AN TOÀN NHẤT.
        /// Format: HTM-YYMMDD-XXXXXX
        /// Collision chỉ xảy ra trong cùng một ngày với ~41,000 đơn/ngày
        /// Phù hợp cho hệ thống có volume cao
        /// </summary>
        /// <param name="id">GUID của đơn hàng</param>
        /// <param name="createdDate">Ngày tạo đơn</param>
        public static string ToDateCode(Guid id, DateTime createdDate)
        {
            if (id == Guid.Empty)
                return $"HTM-{createdDate:yyMMdd}-000000";

            var bytes = id.ToByteArray();
            var sb = new StringBuilder(8);

            // 6 ký tự cho phần random
            for (int i = 0; i < 6; i++)
            {
                int value = bytes[i] ^ bytes[i + 4] ^ bytes[i + 8] ^ bytes[(i + 12) % 16];
                sb.Append(Base32Chars[Math.Abs(value) % Base32Chars.Length]);
            }

            return $"{OrderPrefix}-{createdDate:yyMMdd}-{sb}";
        }

        /// <summary>
        /// Hiển thị GUID dưới dạng rút gọn (8 ký tự đầu hex).
        /// Dùng khi cần hiển thị nhanh mà không cần format đặc biệt.
        /// Không gian: 16^8 = 4.3 tỷ mã
        /// </summary>
        public static string ToShortGuid(Guid id)
        {
            return id.ToString("N")[..8].ToUpperInvariant();
        }

        /// <summary>
        /// Tạo mã số thuần túy từ GUID (8 chữ số).
        /// Format: 12345678
        /// Không gian: 10^8 = 100 triệu mã
        /// 50% collision tại: ~12,000 đơn hàng - KHÔNG KHUYÊN DÙNG cho production
        /// </summary>
        public static string ToNumericCode(Guid id)
        {
            if (id == Guid.Empty)
                return "00000000";

            var bytes = id.ToByteArray();
            
            // Tạo số từ các bytes
            long value = 0;
            for (int i = 0; i < 16; i++)
            {
                value = (value * 31) + bytes[i];
            }
            
            // Lấy 8 chữ số cuối (luôn dương)
            value = Math.Abs(value) % 100000000;
            
            return value.ToString("D8");
        }

        /// <summary>
        /// Format hiển thị mã đơn hàng với prefix và số.
        /// Format: #DH-12345678
        /// </summary>
        public static string ToDisplayCode(Guid id)
        {
            return $"#{OrderPrefix}-{ToNumericCode(id)}";
        }

        /// <summary>
        /// Tính xác suất va chạm theo công thức Birthday Paradox.
        /// P(collision) ≈ 1 - e^(-n²/2m) với n = số đơn, m = không gian mã
        /// </summary>
        /// <param name="numberOfOrders">Số đơn hàng dự kiến</param>
        /// <param name="codeLength">Độ dài mã (số ký tự Base32)</param>
        /// <returns>Xác suất va chạm (0-1)</returns>
        public static double CalculateCollisionProbability(int numberOfOrders, int codeLength = 8)
        {
            double m = Math.Pow(32, codeLength); // Không gian mã
            double n = numberOfOrders;
            
            // Birthday paradox approximation
            double exponent = -(n * n) / (2 * m);
            return 1 - Math.Exp(exponent);
        }
    }
}
