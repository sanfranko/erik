using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        // /Brand
        public IActionResult Index()
        {
            var brands = _context.Brands
                .Include(b => b.Sellers)
                .ToList();

            return View(brands);
        }

        // POST: /Brand/AddSeller
        [HttpPost]
        public IActionResult AddSeller(string shopName, int brandId)
        {
            if (string.IsNullOrWhiteSpace(shopName))
                return RedirectToAction(nameof(Index));

            var seller = new Seller
            {
                ShopName = shopName,
                BrandId = brandId
            };

            _context.Sellers.Add(seller);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Brand/DeleteSeller
        [HttpPost]
        public IActionResult DeleteSeller(int id)
        {
            var seller = _context.Sellers.Find(id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Brand/EditSeller/5
        public IActionResult EditSeller(int id)
        {
            var seller = _context.Sellers
                .Include(s => s.Brand)
                .FirstOrDefault(s => s.SellerId == id);

            if (seller == null)
                return NotFound();

            ViewBag.Brands = _context.Brands.ToList();
            return View(seller);
        }

        // POST: /Brand/EditSeller
        [HttpPost]
        public IActionResult EditSeller(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = _context.Brands.ToList();
                return View(seller);
            }

            _context.Sellers.Update(seller);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Brand/Seller/5
        public IActionResult Seller(int id)
        {
            var seller = _context.Sellers
                .Include(s => s.Brand)
                .FirstOrDefault(s => s.SellerId == id);

            if (seller == null)
                return NotFound();

            return View(seller);
        }
    }
}
