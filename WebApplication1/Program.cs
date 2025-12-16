using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("DataSource=SmartphoneShop.db"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    context.Database.Migrate();

    if (!context.Categories.Any())
    {
        var categories = new[]
        {
            new Category { Name = "Флагманы" },
            new Category { Name = "Средний сегмент" },
            new Category { Name = "Бюджетные" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();
    }

    if (!context.Brands.Any())
    {
        var brands = new[]
        {
            new Brand { Name = "Apple", Country = "США" },
            new Brand { Name = "Samsung", Country = "Южная Корея" },
            new Brand { Name = "Xiaomi", Country = "Китай" }
        };
        context.Brands.AddRange(brands);
        context.SaveChanges();
    }

    if (!context.Products.Any())
    {
        var apple = context.Brands.First(b => b.Name == "Apple");
        var samsung = context.Brands.First(b => b.Name == "Samsung");
        var xiaomi = context.Brands.First(b => b.Name == "Xiaomi");
        var flagship = context.Categories.First(c => c.Name == "Флагманы");
        var midRange = context.Categories.First(c => c.Name == "Средний сегмент");
        
        var products = new[]
        {
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
        };
        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();