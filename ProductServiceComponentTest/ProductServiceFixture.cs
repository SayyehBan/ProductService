using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Services;

namespace ProductServiceComponentTest;

public class ProductServiceFixture
{
    public IProductService _productService { get; }
    public ICategoryService _categoryService { get; }
    public ProductDatabaseContext databaseContext { get; }

    public ProductServiceFixture()
    {
        DbContextOptionsBuilder<ProductDatabaseContext> builder =
            new DbContextOptionsBuilder<ProductDatabaseContext>();
        builder.UseInMemoryDatabase("testProductDatabase");
        databaseContext = new ProductDatabaseContext(builder.Options);
        _productService = new ProductService.Model.Services.RProductService(databaseContext);
        _categoryService = new RCategoryService(databaseContext);
    }
}
