using server.DTOs;
using server.Models;
using System.Linq;
using System.Collections.Generic;

namespace server.Mappers
{
    public static class ProductMapper
    {
        public static ProductListDto ToProductListDto(this Product p)
        {
            return new ProductListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreationDate = p.CreationDate,
                CreatorUserId = p.CreatorUserId
            };
        }

        public static ProductDto ToProductDto(this Product p)
        {
            return new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreationDate = p.CreationDate,
                CreatorUserId = p.CreatorUserId,
                ProductCategoryId = p.ProductCategoryId,
                Categories = p.ProductCategories?.Select(c => c.Id).ToList() ?? new List<int>(),
                Comments = p.Comments?.Where(c => !c.IsDeleted).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreationDate = c.CreationDate
                }).ToList() ?? new List<CommentDto>()
            };
        }

        public static Product FromProductsListDto(this ProductDto dto)
        {
            return new Product
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CreationDate = dto.CreationDate,
                CreatorUserId = dto.CreatorUserId,
                ProductCategoryId = dto.ProductCategoryId
            };
        }
    }
}