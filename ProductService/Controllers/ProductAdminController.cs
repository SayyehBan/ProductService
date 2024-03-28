using Microsoft.AspNetCore.Mvc;
using ProductService.Model.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductAdminController : ControllerBase
{
    private readonly IProductService productService;

    public ProductAdminController(IProductService productService)
    {
        this.productService = productService;
    }
    [HttpPost]
    public IActionResult Post([FromForm] AddNewProductDto addNewProductDto)
    {
        productService.AddNewProduct(addNewProductDto);
        return Ok();
    }
}
