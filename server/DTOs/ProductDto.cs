namespace server.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatorUserId {get;set;}

        public List<int> Categories { get; set; } = new();

        public List<CommentDto> Comments { get; set; } = new();
    }
}
