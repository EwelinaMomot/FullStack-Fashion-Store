namespace server.DTOs
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public int ProductCategoryId { get; set; }

        public int CreatorUserId {get;set;}

        public List<int> Categories { get; set; } = new();

        public List<CommentDto> Comments { get; set; } = new();
    }

    public class ProductListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public int CreatorUserId { get; set; }

    }
}
