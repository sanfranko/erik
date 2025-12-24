using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartItems";
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var cart = HttpContext.Session
                .Get<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            HttpContext.Session.Set(CartSessionKey, cart);

            return RedirectToAction("Index", "Product");
        }

        // ✅ ВОТ ЭТО ДОБАВЛЯЕМ
        public IActionResult Index()
        {
            var cart = HttpContext.Session
                .Get<List<CartItem>>(CartSessionKey)
                ?? new List<CartItem>();

            var productIds = cart.Select(c => c.ProductId).ToList();

            var products = _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            var result = products.Select(p =>
            {
                var quantity = cart.First(c => c.ProductId == p.Id).Quantity;

                return new CartItemViewModel
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = quantity
                };
            }).ToList();

            return View(result);
        }
    }
}
