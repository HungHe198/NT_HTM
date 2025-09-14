using NT.SHARED.ErrorHandling;
using NT.SHARED.Logging;
using System.Net;

namespace NT.API.Middleware
{/// <summary>
 /// Middleware để xử lý exception toàn cục.
 /// Bắt mọi lỗi chưa được xử lý, log + báo bug nếu cần, và trả response phù hợp cho client.
 /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _loggerService;
        private readonly IBugReportService _bugReportService;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILoggerService loggerService,
            IBugReportService bugReportService)
        {
            _next = next;
            _loggerService = loggerService;
            _bugReportService = bugReportService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // note: log lỗi
                _loggerService.LogError("Unhandled exception caught in middleware", ex,
                    new Dictionary<string, object?> { { "Path", context.Request.Path }, { "QueryString", context.Request.QueryString.ToString() } });

                // báo bug nếu cần
                await _bugReportService.ReportBugAsync(ex, $"Request Path: {context.Request.Path}");

                // trả response lỗi cho client
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    Message = "Internal Server Error",
                    Detail = ex.Message // trong môi trường production bạn có thể chỉ trả thông tin chung, không trả stacktrace
                };
                var json = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
