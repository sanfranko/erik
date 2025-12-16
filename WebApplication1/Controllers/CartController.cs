using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Extensions; 
using WebApplication1.Models; 
using System.Linq; 

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartItems"; 

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Действие для добавления товара в корзину
        // Важно: ID должен быть передан из кнопки "В корзину!"
        public IActionResult AddToCart(int id)
        {
            // 1. Считываем корзину из сессии (если null, создаем новый список)
            var cart = HttpContext.Session.Get<List<(int ProductId, int Quantity)>>(CartSessionKey) 
                       ?? new List<(int ProductId, int Quantity)>();

            // 2. Ищем индекс товара в текущей корзине
            int itemIndex = -1;
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == id)
                {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex != -1) // Если товар найден
            {
                // Увеличиваем количество и заменяем кортеж
                var existingItem = cart[itemIndex];
                
                // 🚨 КРИТИЧЕСКИЙ МОМЕНТ: Удаляем старый элемент
                cart.RemoveAt(itemIndex); 

                // Добавляем новый элемент с увеличенным количеством
                cart.Add((id, existingItem.Quantity + 1));
            }
            else
            {
                // Если товара нет, просто добавляем новый
                cart.Add((id, 1)); // Добавляем новый кортеж (ID, Quantity = 1)
            }

            // 🚨 КРИТИЧЕСКИЙ МОМЕНТ: Сохраняем обновленный список обратно в сессию
            HttpContext.Session.Set(CartSessionKey, cart);

            // Перенаправляем обратно на страницу каталога
            return RedirectToAction("Index", "Product");
        }
    }
}