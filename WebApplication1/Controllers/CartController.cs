using Microsoft.AspNetCore.Mvc;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartItems";

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
    }
}
