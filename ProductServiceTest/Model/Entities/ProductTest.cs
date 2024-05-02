using ProductService.Model.Entities;
using Tynamix.ObjectFiller;

namespace ProductServiceTest.Model.Entities;

public class ProductTest
{
    [Fact]
    public void Update_Price_Product()
    {

        //Arrange
        //یعنی قرار دادن داده های پیش نیاز
        Product product = new Product()
        {
            CategoryId = Guid.NewGuid(),
            Name = "Lenovo",
            Description = "best product",
            Id = Guid.NewGuid(),
            Image = "1.png",
            Price = 850000
        };
        int price = 8569000;

        //Act
        //عملیاتی که قرار هستش انجام بشه
        product.UpdatePrice(price);

        //Assert
        //بررسسی اون چیزی کی میخوام انجام شده یا نه
        Assert.Equal(price, product.Price);
    }
    [Fact]
    public void Update_Price_Product_With_Zero_Value_Excetion()
    {
        //Arrange
        var product = new Filler<Product>().Create();

        //Act

        //Assert
        Assert.Throws<Exception>(() => product.UpdatePrice(0));


    }
}
