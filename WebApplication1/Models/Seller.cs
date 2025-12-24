using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Seller
    {
        public int SellerId { get; set; }

        [Required]
        [StringLength(100)]
        public string ShopName { get; set; } = string.Empty;

        // 🔗 Связь с брендом
        [Required]
        public int BrandId { get; set; }

        public Brand? Brand { get; set; }
    }
}
