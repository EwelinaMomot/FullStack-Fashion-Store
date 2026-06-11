namespace server.DTOs
{
    public class CategoryListDto
    {
       public int TotalCategoriesNumber  {get;set;}
    public int CurrentPage  {get;set;}
public int totalPages  {get; set;}
          public List<CategoryDto> Categories { get; set; }
    }
}
