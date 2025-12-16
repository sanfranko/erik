using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Brand)
                                         .Include(p => p.Category)
                                         .ToListAsync();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}