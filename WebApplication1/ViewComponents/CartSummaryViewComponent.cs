using Microsoft.AspNetCore.Mvc;
using WebApplication1.Extensions;

namespace WebApplication1.ViewComponents
{
    // Компонент, который считает количество товаров в корзине
    public class CartSummaryViewComponent : ViewComponent
    {
        private const string CartSessionKey = "CartItems";

        public IViewComponentResult Invoke()
        {
            // 1. Получаем список кортежей (ID, Quantity) из сессии
            var cart = HttpContext.Session.Get<List<(int ProductId, int Quantity)>>(CartSessionKey);
            
            // 2. Считаем общее количество товаров
            // Если корзина пуста (null), itemCount = 0
            int itemCount = cart?.Sum(i => i.Quantity) ?? 0;
            
            // 3. Передаем это число в представление компонента
            return View("Default", itemCount);
        }
    }
}