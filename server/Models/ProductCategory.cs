using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
