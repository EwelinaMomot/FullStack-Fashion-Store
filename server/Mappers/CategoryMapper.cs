using server.DTOs;
using server.Models;
using System.Runtime.CompilerServices;

namespace server.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto toCategoryDto(this ProductCategory c)
        {
            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name

            };

        }

        public static ProductCategory  fromCategoryDto(this CategoryDto c)
        {
            return new ProductCategory
            {
                Id = c.Id,
                Name = c.Name

            };

        }


    }
}
