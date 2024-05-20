using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Links;
using ProductService.Model.Services;
using SayyehBanTools.ConfigureService;
using SayyehBanTools.ConnectionDB;
using SayyehBanTools.MessagingBus.RabbitMQ.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductDatabaseContext>(p => p.UseSqlServer(SqlServerConnection.ConnectionString("4Mgp5catJqMnBNvqAqdJ2w==", "eWyP6NKkWfiTzk6B1pz8gw==", "m6UQxl628s/a1Hx1CxA2LQ==", "xbfQyKCUrBvw5zxn8sMOfg==", "257ld6s4dsc16e2j", "69q18j991xl48u6u")));
builder.Services.AddTransient<ICategoryService, RCategoryService>();
builder.Services.AddTransient<IProductService, RProductService>();
//RabbitMQ
builder.Services.Configure<RabbitMqConnectionSettings>(builder.Configuration
    .GetSection("RabbitMq"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.Authority = LinkServices.IdentityServer;
                option.Audience = "productservice";
            });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProductAdmin",
        policy => policy.RequireClaim("scope", "productservice.admin"));
});
//builder.Services.AddScoped<IMessageBus, RabbitMQMessageBus>();
//پیکربندی های پیش فرض SayyehbanTools
var configureServices = new ConfigureServicesRabbitMQ();
configureServices.ConfigureService(builder.Services);
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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
