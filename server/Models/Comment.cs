using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Description { get; set; }=string.Empty;
        public DateTime CreationDate { get; set; } =DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public int CreatorUserdId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
