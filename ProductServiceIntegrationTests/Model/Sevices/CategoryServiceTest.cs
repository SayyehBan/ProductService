using ProductService.Model.Services;
using Tynamix.ObjectFiller;

namespace ProductServiceIntegrationTests.Model.Sevices;

public class CategoryServiceTest : IClassFixture<ProductServiceFixture>

{
    private readonly ProductServiceFixture fixture;

    public CategoryServiceTest(ProductServiceFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void AddNewCategoryAndGet()
    {
        //arange
        var category = new Filler<CategoryDto>().Create();
        //act
        var catId = fixture._categoryService.AddNewCatrgory(category);
        var categoryInDatabase = fixture._categoryService.Getcategory(catId);

        //assert
        Assert.NotNull(categoryInDatabase);
        Assert.IsType<CategoryDto>(categoryInDatabase);
        Assert.Equal(category.Name, categoryInDatabase.Name);
        Assert.Equal(category.Description, categoryInDatabase.Description);

    }
}