using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT.SHARED.Logging
{
    /// <summary>
    /// Giao diện để log thông tin trong ứng dụng.
    /// Bao gồm các mức log: Info, Warning, Error.
    /// Có thể thêm context (ví dụ: user, trace id, dữ liệu bổ sung).
    /// </summary>
    public interface ILoggerService
    {
        void LogInfo(string message, IDictionary<string, object>? context = null);
        void LogWarning(string message, IDictionary<string, object>? context = null);
        void LogError(string message, Exception exception, IDictionary<string, object>? context = null);
        Task LogErrorAsync(string message, Exception exception, IDictionary<string, object>? context = null);
    }
}
