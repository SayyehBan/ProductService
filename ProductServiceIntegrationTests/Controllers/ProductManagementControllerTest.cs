using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Model.Services;
using Tynamix.ObjectFiller;

namespace ProductServiceIntegrationTests.Controllers;

public class ProductManagementControllerTest : IClassFixture<ProductServiceFixture>
{
    private readonly ProductServiceFixture fixture;

    public ProductManagementControllerTest(ProductServiceFixture fixture)
    {
        this.fixture = fixture;
    }



    [Fact]
    public void Add_new_Product_In_DataBase()
    {
        //Arrange
        var newproduct = new Filler<AddNewProductDto>().Create();

        var newCategory = new Filler<CategoryDto>().Create();
        Guid catId = fixture._categoryService.AddNewCatrgory(newCategory);

        newproduct.CategoryId = catId;

        ProductManagementController managementController =
            new ProductManagementController(fixture._productService);

        //Act
        var result = managementController.Post(newproduct) as CreatedResult;

        //Assert
        var lastproduct = fixture.databaseContext.Products.FirstOrDefault(p =>
          p.Id == Guid.Parse(result.Value.ToString()));

        Assert.NotNull(lastproduct);
        Assert.Equal(newproduct.Name, lastproduct.Name);
        Assert.Equal(newproduct.Description, lastproduct.Description);
        Assert.Equal(newproduct.Price, lastproduct.Price);
        Assert.Equal(newproduct.CategoryId, lastproduct.CategoryId);

    }
}
