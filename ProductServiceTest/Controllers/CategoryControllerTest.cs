using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Model.Services;
using ProductServiceTest.MockData;

namespace ProductServiceTest.Controllers;

public class CategoryControllerTest
{
    private CatagoryMockData catagoryMockData;
    public CategoryControllerTest()
    {
        catagoryMockData = new CatagoryMockData();
    }

    [Fact]
    public void Return_All_Category()
    {
        //Arrange
        var moq = new Mock<ICategoryService>();
        moq.Setup(p => p.GetCategories()).Returns(catagoryMockData.getCategories());
        CategoryController categoryController = new CategoryController(moq.Object);
        //Act
        var result = categoryController.Get();
        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var returnOk = result as OkObjectResult;
        Assert.NotNull(returnOk);
        Assert.IsType<List<CategoryDto>>(returnOk.Value);

    }
}
