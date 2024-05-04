using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Services;
using SayyehBanTools.ConnectionDB;

namespace ProductServiceIntegrationTests;

public class ProductServiceFixture : IDisposable
{
    public IProductService _productService { get; }
    public ICategoryService _categoryService { get; }
    public ProductDatabaseContext databaseContext { get; }

    public ProductServiceFixture()
    {
        Random random = new Random();
        DbContextOptionsBuilder<ProductDatabaseContext> builder =
            new DbContextOptionsBuilder<ProductDatabaseContext>();
        builder.UseSqlServer(SqlServerConnection.ConnectionString(".",$"ProductsDBTEST{random.Next(999999)}", "TestConnection","@123456"));
        databaseContext = new ProductDatabaseContext(builder.Options);
        databaseContext.Database.EnsureCreated();
        _productService = new RProductService(databaseContext);
        _categoryService = new RCategoryService(databaseContext);

    }

    public void Dispose()
    {
        databaseContext.Database.EnsureDeleted();
    }
}
