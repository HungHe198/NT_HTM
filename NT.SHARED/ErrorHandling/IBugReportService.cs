using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.ErrorHandling
{

    /// <summary>
    /// Giao diện để báo các lỗi nghiêm trọng (bug) lên hệ thống bên ngoài hoặc gửi thông báo.
    /// Ví dụ: gửi email, gửi vào ticket system, hoặc lưu vào log phân tích báo cáo bug.
    /// </summary>
    public interface IBugReportService
    {
        Task ReportBugAsync(Exception exception, string extraDetails = null);
    }
}
