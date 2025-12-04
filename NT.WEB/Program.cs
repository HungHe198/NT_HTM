
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

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
