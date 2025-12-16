using Microsoft.AspNetCore.Mvc;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private const string CartSessionKey = "CartItems";

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session
                .Get<List<CartItem>>(CartSessionKey);

            int itemCount = cart?.Sum(i => i.Quantity) ?? 0;

            return View(itemCount);
        }
    }
}
