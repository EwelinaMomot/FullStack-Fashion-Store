namespace server.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public int CreatorUserId { get; set; }
    }

    public class NewCommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int CreatorUserId { get; set; }

    }
}
