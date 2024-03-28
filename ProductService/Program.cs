using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Services;
using SayyehBanTools.ConnectionDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductDatabaseContext>(p=>p.UseSqlServer(SqlServerConnection.ConnectionString("4Mgp5catJqMnBNvqAqdJ2w==", "eWyP6NKkWfiTzk6B1pz8gw==", "m6UQxl628s/a1Hx1CxA2LQ==", "xbfQyKCUrBvw5zxn8sMOfg==", "257ld6s4dsc16e2j", "69q18j991xl48u6u")));
builder.Services.AddTransient<ICategoryService, RCategoryService>();
builder.Services.AddTransient<IProductService, RProductService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
