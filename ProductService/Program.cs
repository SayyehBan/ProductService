using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductService.HealthChecks;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Links;
using ProductService.Model.Services;
using SayyehBanTools.ConfigureService;
using SayyehBanTools.ConnectionDB;
using SayyehBanTools.MessagingBus.RabbitMQ.Model;

var builder = WebApplication.CreateBuilder(args);
var metrics = new MetricsBuilder();


builder.Services.AddMetricsEndpoints(options =>
{
    options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
    options.EnvironmentInfoEndpointEnabled = false;
});


builder.Services.AddMetrics(metrics);

builder.Services.AddMetricsTrackingMiddleware();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductDatabaseContext>(p => p.UseSqlServer(SqlServerConnection.ConnectionString("192.168.1.13", "ProductsDB", "TestConnection", "@123456")));
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

builder.Services.AddHealthChecks().
    AddRabbitMQ(RabbitMQConnection.DefaultConnection(), tags: new string[] { "rabbitMQ" }).
    AddSqlServer(SqlServerConnection.ConnectionString("192.168.1.13", "ProductsDB", "TestConnection", "@123456"));
//.AddCheck<DataBaseHealthCheck>("SqlCheck");


builder.Services.AddHealthChecksUI(p => p.AddHealthCheckEndpoint("ProductshealthCheck", "/health"))
          .AddInMemoryStorage();


builder.Services.AddHealthChecksUI(p => p.AddHealthCheckEndpoint("OrderhealthCheck", "health"))
 .AddInMemoryStorage();


//builder.Services.AddScoped<IMessageBus, RabbitMQMessageBus>();
//پیکربندی های پیش فرض SayyehbanTools
var configureServices = new ConfigureServicesRabbitMQ();
configureServices.ConfigureService(builder.Services);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMetricsAllMiddleware();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseHttpsRedirection();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});
app.UseHealthChecksUI(delegate (HealthChecks.UI.Configuration.Options options)
{
    options.UIPath = "/healthui";
    options.ApiPath = "/healthuiapi";
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapHealthChecks("/health");
app.Run();
