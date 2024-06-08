using App.Metrics;
using App.Metrics.Counter;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    private readonly ILogger<ProductController> logger;
    private readonly IMetrics metrics;

    public ProductController(IProductService productService, ILogger<ProductController> logger, IMetrics metrics)
    {
        this.productService = productService;
        this.logger = logger;
        this.metrics = metrics;
    }
    // GET: api/<ProductController>
    [HttpGet]
    public IActionResult Get()
    {
        metrics.Measure.Counter.Increment(new CounterOptions
        {
            Name = "get_list_product",

        });
        var data = productService.GetProductList();
        return Ok(data);
    }
    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        metrics.Measure.Counter.Increment(new CounterOptions
        {
            Name = "show_detail_product",

        });
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