using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using WebApplication1.Extensions; // Эта строка не нужна в этом файле
// using WebApplication1.Controllers; // 🚨 ВАЖНО: УДАЛИТЕ ЛЮБОЙ КОД CartController ИЗ ЭТОГО ФАЙЛА!

namespace WebApplication1.Models
{
    // Модель для элемента, хранящегося в базе данных (если вы используете БД)
    public class ShoppingCartItem
    {
        // Первичный ключ
        public int ShoppingCartItemId { get; set; } 
        
        // Внешний ключ к продукту
        public int ProductId { get; set; } 
        
        // Количество данного продукта
        public int Quantity { get; set; }

        [StringLength(200)]
        // Идентификатор корзины (для сессии или пользователя)
        public string? ShoppingCartId { get; set; } 

        // Навигационное свойство к продукту
        public virtual Product Product { get; set; } = null!;
    }
}