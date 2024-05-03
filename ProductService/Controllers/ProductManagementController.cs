using Microsoft.AspNetCore.Mvc;
using ProductService.MessagingBus.Messages;
using ProductService.Model.Links;
using ProductService.Model.Services;
using SayyehBanTools.MessagingBus.RabbitMQ.SendMessage;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductManagementController : ControllerBase
{
    private readonly IProductService productService;

    public ProductManagementController(IProductService productService)
    {
        this.productService = productService;
    }
    public ProductManagementController()
    {

    }

    [HttpPost]
    public IActionResult Post([FromBody] AddNewProductDto addNewProductDto)
    {
        var productId = productService.AddNewProduct(addNewProductDto);
        return Created($"/api/ProductManagement/get/{productId}", productId);
    }


    [HttpGet]
    public IActionResult Get()
    {
        var products = productService.GetProductList();
        return Ok(products);
    }


    [HttpGet("Id")]
    public IActionResult Get(Guid Id)
    {
        var product = productService.GetProduct(Id);
        return Ok(product);
    }

    [HttpPut]
    public IActionResult Put(UpdateProductDto updateProduct, ISendMessages sendMessages)
    {
        var result = productService.UpdateProductName(updateProduct);
        if (result)
        {
            UpdateProductNameMessage updateProductNameMessage = new UpdateProductNameMessage
            {
                Creationtime = DateTime.UtcNow,
                Id = updateProduct.ProductId,
                NewName = updateProduct.Name,
                MessageId = Guid.NewGuid()
            };
            sendMessages.SendMessage(updateProductNameMessage, LinkRabbitMQ.UpdateProductName, null);
        }
        return Ok(result);
    }

}