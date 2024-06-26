﻿using ProductService.Infrastructure.Contexts;
using ProductService.Model.Entities;

namespace ProductService.Model.Services;

public interface ICategoryService
{
    List<CategoryDto> GetCategories();
    Guid AddNewCatrgory(CategoryDto category);
    CategoryDto Getcategory(Guid categoryId);
}
public class RCategoryService : ICategoryService
{
    private readonly ProductDatabaseContext context;

    public RCategoryService(ProductDatabaseContext context)
    {
        this.context = context;
    }

    public Guid AddNewCatrgory(CategoryDto category)
    {
        Category newCategory = new Category
        {
            Description = category.Description,
            Name = category.Name,
        };
        context.Categories.Add(newCategory);
        context.SaveChanges();
        return newCategory.Id;
    }

    public List<CategoryDto> GetCategories()
    {
        var data = context.Categories
           .OrderBy(p => p.Name)
           .Select(p => new CategoryDto
           {
               Description = p.Description,
               Name = p.Name,
               Id = p.Id
           }).ToList();
        return data;
    }

    public CategoryDto Getcategory(Guid categoryId)
    {
        var category = context.Categories.Find(categoryId);
        return new CategoryDto
        {
            Description = category.Description,
            Name = category.Name,
            Id = category.Id
        };
    }
}
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}