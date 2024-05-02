using ProductService.Model.Services;
using Tynamix.ObjectFiller;

namespace ProductServiceTest.MockData;

public class CatagoryMockData
{
    List<CategoryDto> Categories = new List<CategoryDto>();

    public List<CategoryDto> getCategories()
    {
        Categories.AddRange(new Filler<CategoryDto>().Create(20));
        return Categories;
    }
}
