using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// =====================
// DATABASE
// =====================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("DataSource=SmartphoneShop.db"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// =====================
// IDENTITY
// =====================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// =====================
// SESSION (ВАЖНО)
// =====================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpContextAccessor можно оставить, но он НЕ обязателен,
// так как ViewComponent и Controller уже имеют HttpContext
builder.Services.AddHttpContextAccessor();

// =====================
// MVC
// =====================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// =====================
// DB INIT / SEED
// =====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "Флагманы" },
            new Category { Name = "Средний сегмент" },
            new Category { Name = "Бюджетные" }
        );
        context.SaveChanges();
    }

    if (!context.Brands.Any())
    {
        context.Brands.AddRange(
            new Brand { Name = "Apple", Country = "США" },
            new Brand { Name = "Samsung", Country = "Южная Корея" },
            new Brand { Name = "Xiaomi", Country = "Китай" }
        );
        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        var apple = context.Brands.First(b => b.Name == "Apple");
        var samsung = context.Brands.First(b => b.Name == "Samsung");
        var xiaomi = context.Brands.First(b => b.Name == "Xiaomi");

        var flagship = context.Categories.First(c => c.Name == "Флагманы");
        var midRange = context.Categories.First(c => c.Name == "Средний сегмент");

        context.Products.AddRange(
            new Product
            {
                Name = "iPhone 15 Pro Max",
                Price = 120000,
                BrandId = apple.BrandId,
                CategoryId = flagship.CategoryId,
                ImageUrl = "Apple.png"
            },
            new Product
            {
                Name = "Samsung Galaxy S24 Ultra",
                Price = 115000,
                BrandId = samsung.BrandId,
                CategoryId = flagship.CategoryId,
                ImageUrl = "Samsung.png"
            },
            new Product
            {
                Name = "Xiaomi 13T Pro",
                Price = 65000,
                BrandId = xiaomi.BrandId,
                CategoryId = midRange.CategoryId,
                ImageUrl = "Xiaomi.png"
            }
        );

        context.SaveChanges();
    }
}

// =====================
// MIDDLEWARE PIPELINE
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔴 СЕССИЯ СТРОГО ЗДЕСЬ
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// =====================
// ROUTES
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
