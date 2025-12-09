
using NT.BLL.Interfaces;
using NT.BLL.Services;
using NT.DAL.ContextFile;
using NT.DAL.Repositories;
using NT.WEB.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext (NTAppDbContext has OnConfiguring with connection string fallback)
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
// Password hasher for User
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<NT.SHARED.Models.User>, Microsoft.AspNetCore.Identity.PasswordHasher<NT.SHARED.Models.User>>();
// Simple email service
builder.Services.AddScoped<NT.WEB.Services.IEmailService, NT.WEB.Services.SmtpEmailService>();

// Authentication - cookie
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Logout/Index";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    }); 
builder.Services.AddAuthorization();

// Session for cart
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
