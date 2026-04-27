using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public int ProductCategoryId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int CreatorUserId { get; set; }
        public string ImageUrl { get; set; } =string.Empty;
   
        public List<Comment> Comments { get; set; } = new List<Comment>();  

        public ProductCategory ProductCategory { get; set; } = null!;
    }

}
