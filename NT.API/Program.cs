var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// trong ConfigureServices
builder.Services.AddSingleton<NT.SHARED.Logging.ILoggerService, NT.Infrastructure.Logging.LoggerService>();
builder.Services.AddSingleton<NT.SHARED.ErrorHandling.IBugReportService, NT.Infrastructure.ErrorHandling.BugReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// trong Configure phương thức hoặc pipeline của WebApplication
app.UseMiddleware<NT.API.Middleware.ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
