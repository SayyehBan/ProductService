using Microsoft.AspNetCore.Mvc;
using ProductService.Model.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }
    // GET: api/<ProductController>
    [HttpGet]
    public IActionResult Get()
    {
        var data=productService.GetProductList();
        return Ok(data);
    }
    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var data = productService.GetProduct(id);
        return Ok(data);
    }
    [HttpGet("/api/product/verify/{Id}")]
    public IActionResult Verify(Guid Id)
    {
        var data = productService.GetProduct(Id);
        return Ok(new ProductVeryfyDto
        {
            ProductId = data.Id,
            ProductName = data.Name
        });

    }
}
public class ProductVeryfyDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
}