using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }
    // GET: api/<CategoryController>
    [HttpGet]
    public IActionResult Get()
    {
        var data = categoryService.GetCategories();
        return Ok(data);
    }
    // POST api/<CategoryController>
    [Authorize(Policy = "ProductAdmin")]
    [HttpPost]
    public IActionResult Post([FromForm] CategoryDto categoryDto)
    {
        categoryService.AddNewCatrgory(categoryDto);
        return Ok(categoryDto);
    }
}
