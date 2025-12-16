using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Нужен для метода Include
using WebApplication1.Data;
using WebApplication1.Models; // Нужен для доступа к моделям (Product)

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Действие для отображения каталога товаров
        public async Task<IActionResult> Index()
        {
            // 1. Загружаем товары из БД. 
            // Используем Include, чтобы подгрузить связанные таблицы (Brand, Category)
            // Это нужно, если в представлении Index.cshtml используются эти поля.
            var applicationDbContext = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category);
            
            // 2. Возвращаем список товаров в представление (Views/Product/Index.cshtml)
            return View(await applicationDbContext.ToListAsync());
        }

        // Вы можете добавить сюда другие действия (Details, Create и т.д.)
    }
}