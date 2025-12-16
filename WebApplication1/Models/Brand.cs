using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}