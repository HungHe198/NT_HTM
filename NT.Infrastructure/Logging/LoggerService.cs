using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NT.SHARED.Logging;

namespace NT.Infrastructure.Logging
{
    /// <summary>
    /// LoggerService sử dụng ILogger từ Microsoft.Extensions.Logging để ghi log.
    /// Đây là implementation mặc định, có thể mở rộng hoặc thay thế nếu muốn dùng Serilog, NLog, etc.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, IDictionary<string, object>? context = null)
        {
            if (context != null)
            {
                using (_logger.BeginScope(context))
                {
                    _logger.LogInformation(message);
                }
            }
            else
            {
                _logger.LogInformation(message);
            }
        }

        public void LogWarning(string message, IDictionary<string, object>? context = null)
        {
            if (context != null)
            {
                using (_logger.BeginScope(context))
                {
                    _logger.LogWarning(message);
                }
            }
            else
            {
                _logger.LogWarning(message);
            }
        }

        public void LogError(string message, Exception exception, IDictionary<string, object>? context = null)
        {
            if (context != null)
            {
                using (_logger.BeginScope(context))
                {
                    _logger.LogError(exception, message);
                }
            }
            else
            {
                _logger.LogError(exception, message);
            }
        }

        public async Task LogErrorAsync(string message, Exception exception, IDictionary<string, object>? context = null)
        {
            // note: Microsoft.Extensions.Logging không hỗ trợ asynchronous logging theo mặc định
            // nếu muốn thực sự async, có thể dispatch sang queue hoặc sử dụng thư viện bên ngoài
            await Task.Run(() => LogError(message, exception, context));
        }
    }
}
