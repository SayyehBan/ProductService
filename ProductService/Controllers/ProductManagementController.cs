using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductService.MessagingBus.Messages;
using ProductService.Model.Services;
using SayyehBanTools.MessagingBus.RabbitMQ.Model;
using SayyehBanTools.MessagingBus.RabbitMQ.SendMessage;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductManagementController : ControllerBase
{
    private readonly IProductService productService;
    private readonly ISendMessages messageBus;
    private readonly string exchangeName;
    public ProductManagementController(IProductService productService, IOptions<RabbitMqConnectionSettings> rabbitMqOptions, ISendMessages messageBus)
    {
        this.productService = productService;
        this.messageBus = messageBus;
        exchangeName = rabbitMqOptions.Value.queue;
    }

    [HttpPost]
    public IActionResult Post([FromBody] AddNewProductDto addNewProductDto)
    {
        productService.AddNewProduct(addNewProductDto);
        return Ok();
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
    public IActionResult Put(UpdateProductDto updateProduct)
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
            messageBus.SendMessage(updateProductNameMessage, exchangeName, null);
        }
        return Ok(result);
    }

}