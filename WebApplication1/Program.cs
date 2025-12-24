using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// =====================
// DATABASE (SQL SERVER)
// =====================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// =====================
// IDENTITY
// =====================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// =====================
// SESSION
// =====================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// =====================
// MVC
// =====================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// =====================
// SEED (БЕЗ MIGRATE)
// =====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ❗ БАЗА УЖЕ ЕСТЬ — НИЧЕГО НЕ СОЗДАЁМ

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
}

// =====================
// MIDDLEWARE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

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
