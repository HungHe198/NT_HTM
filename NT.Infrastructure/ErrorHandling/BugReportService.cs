using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NT.SHARED.ErrorHandling;
using NT.SHARED.Logging;
using Microsoft.Extensions.Configuration; // Add this namespace for extension methods




namespace NT.Infrastructure.ErrorHandling
{
    /// <summary>
    /// BugReportService: implementation để báo lỗi nghiêm trọng.
    /// Có thể gửi email, hoặc gửi đến hệ thống bên ngoài nếu có config,
    /// sử dụng LoggerService để ghi log, sau đó báo bug.
    /// </summary>
    public class BugReportService : IBugReportService
    {
        private readonly ILoggerService _loggerService;
        private readonly IConfiguration _configuration;

        public BugReportService(ILoggerService loggerService, IConfiguration configuration)
        {
            _loggerService = loggerService;
            _configuration = configuration;
        }

        public async Task ReportBugAsync(Exception exception, string extraDetails = null)
        {
            // log lỗi trước
            _loggerService.LogError("BugReportService caught an exception", exception,
                new Dictionary<string, object?> { { "ExtraDetails", extraDetails } });

            // gửi báo bug nếu được bật trong cấu hình
            // Existing code remains unchanged
            var isReportingEnabled = _configuration.GetValue<bool>("BugReporting:Enabled");

            if (!isReportingEnabled)
            {
                return;
            }

            // giả sử có config Email hoặc API để gửi
            var bugReportEndpoint = _configuration["BugReporting:Endpoint"];
            var apiKey = _configuration["BugReporting:ApiKey"];
            // ... code gửi HTTP request hoặc email
            // ví dụ giả định HTTP:
            if (!string.IsNullOrEmpty(bugReportEndpoint))
            {
                try
                {
                    // sử dụng HttpClient hoặc service khác để gửi
                    using var client = new System.Net.Http.HttpClient();
                    var payload = new
                    {
                        Message = exception.Message,
                        StackTrace = exception.StackTrace,
                        Extra = extraDetails,
                        Time = DateTime.UtcNow
                    };
                    // giả định JSON
                    var content = new System.Net.Http.StringContent(
                        System.Text.Json.JsonSerializer.Serialize(payload),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );
                    var response = await client.PostAsync(bugReportEndpoint, content);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    // nếu gửi bug report thất bại, không làm nát ứng dụng, chỉ log nội bộ
                    _loggerService.LogError("Failed to send bug report", ex, null);
                }
            }
        }
    }
}
