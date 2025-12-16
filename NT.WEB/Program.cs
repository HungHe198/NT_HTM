using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.DAL.ContextFile;
using NT.DAL.Repositories;
using NT.WEB.Services;
using NT.WEB.Authorization;
using System.Text;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to allow large file uploads (up to 2GB)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 2L * 1024 * 1024 * 1024; // 2GB
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Allow large form values
}).AddMvcOptions(options =>
{
    options.MaxModelBindingCollectionSize = int.MaxValue;
});

// Configure form options for large file uploads
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024; // 2GB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Ensure UTF-8 is used everywhere (helps avoid ? characters for Vietnamese text)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Register DbContext using connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NTAppDbContext>(options => { });
// Register open-generic repository implementation for IGenericRepository<T>
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IVocherService, VocherService>();
// Register web services used by controllers
builder.Services.AddScoped<ProductWebService>();
builder.Services.AddScoped<ProductDetailWebService>();
builder.Services.AddScoped<CartWebService>();
builder.Services.AddScoped<CartDetailWebService>();
builder.Services.AddScoped<BrandWebService>();
builder.Services.AddScoped<ColorWebService>();
builder.Services.AddScoped<LengthWebService>();
builder.Services.AddScoped<SurfaceFinishWebService>();
builder.Services.AddScoped<HardnessWebService>();
builder.Services.AddScoped<ElasticityWebService>();
builder.Services.AddScoped<OriginCountryWebService>();
builder.Services.AddScoped<ProductImageWebService>();
builder.Services.AddScoped<VocherWebService>();
builder.Services.AddScoped<OrdersWebService>();
builder.Services.AddScoped<CustomerWebService>();
builder.Services.AddScoped<AdminWebService>();
// Password hasher for User
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<NT.SHARED.Models.User>, Microsoft.AspNetCore.Identity.PasswordHasher<NT.SHARED.Models.User>>();
// Simple email service
builder.Services.AddScoped<NT.WEB.Services.IEmailService, NT.WEB.Services.SmtpEmailService>();

// Authorization services for endpoint-based permission
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<EndpointScannerService>();
builder.Services.AddScoped<NT.WEB.Authorization.RolePermissionService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Authentication - cookie
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Logout/Index";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        // Enforce 30-minute auth cookie expiration
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        // Enable sliding expiration if you want the 30 minutes to refresh on activity
        options.SlidingExpiration = true;
        // Optional: explicitly set MaxAge on cookie
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        // Redirect unauthenticated or unauthorized to client flows with messages
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                context.Response.Redirect($"/Account/Login?client=true&returnUrl={returnUrl}&msg=login_required");
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                context.Response.Redirect($"/Account/RegisterCustomer?client=true&returnUrl={returnUrl}&msg=customer_required");
                return Task.CompletedTask;
            }
        };
    }); 
builder.Services.AddAuthorization();

// Session for cart
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // Server-side session idle timeout 30 minutes
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Localization: default to Vietnamese and enforce UTF-8
var supportedCultures = new[] { new CultureInfo("vi-VN"), new CultureInfo("en-US") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("vi-VN");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Seed default roles at startup (runtime seeding with random GUIDs)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleRepo = services.GetRequiredService<NT.BLL.Interfaces.IGenericRepository<NT.SHARED.Models.Role>>();
        // Ensure roles: Admin, Customer, Employee
        var seedNames = new[] { "Admin", "Customer", "Employee" };
        foreach (var name in seedNames)
        {
            var found = await roleRepo.FindAsync(r => r.Name == name);
            var exists = System.Linq.Enumerable.Any(found);
            if (!exists)
            {
                var role = NT.SHARED.Models.Role.Create(name); // Role.Id generated with Guid.NewGuid()
                await roleRepo.AddAsync(role);
            }
        }
        await roleRepo.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // don't crash startup if seeding fails
        var logger = services.GetService<Microsoft.Extensions.Logging.ILoggerFactory>()?.CreateLogger("RoleSeed");
        logger?.LogError(ex, "Failed to seed roles");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// Apply localization
app.UseRequestLocalization();

// Ensure text/html responses explicitly declare UTF-8 charset
app.Use(async (context, next) =>
{
    await next();
    var ct = context.Response.ContentType;
    if (!string.IsNullOrEmpty(ct) && ct.StartsWith("text/html") && !ct.Contains("charset", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.ContentType = "text/html; charset=utf-8";
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
