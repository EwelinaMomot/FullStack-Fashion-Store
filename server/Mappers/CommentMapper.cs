using server.DTOs;
using server.Models;

namespace server.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment c)
        {
            return new CommentDto
            {
                Id = c.Id,
                Description = c.Description,
                CreationDate = c.CreationDate,
                CreatorUserId=c.CreatorUserdId
            };
        }

        public static Comment FromNewCommentDto(this NewCommentDto c)
        {
            return new Comment
            {
                Id = c.Id,
                Description = c.Description,
                ProductId = c.ProductId,
                CreationDate = c.CreationDate,
                CreatorUserdId=c.CreatorUserId
            };
        }
    }
}
