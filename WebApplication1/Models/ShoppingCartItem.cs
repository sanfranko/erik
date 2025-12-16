using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [StringLength(200)]
        public string? ShoppingCartId { get; set; } 

        public virtual Product Product { get; set; } = null!;
    }
}