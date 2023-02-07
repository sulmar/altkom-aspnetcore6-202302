using Altkom.Shopper.Api;
using Altkom.Shopper.Api.Extensions;
using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Minimal Api

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;


// dotnet publish -c Debug -r win-x64 /p:EnvironmentName = Development

// builder.Configuration.AddJsonFile("appsettings.json", optional: false) // default
// builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true) // default
// builder.Configuration.AddJsonFile() // default
// builder.Configuration.AddCommandLine(args); // dotnet run --NbpApi:Url=domain.com // default
// builder.Configuration.AddEnvironmentVariables(); // DOTNET_ // default
builder.Configuration.AddXmlFile("appsettings.xml", optional: true);
builder.Configuration.AddXmlFile($"appsettings.{environmentName}.xml", optional: true);
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    ["NbpApi:Url"] = "domain.com",
    ["NbpApi:CurrencyCode"] = "EUR",
});

// YAML
// W≥asny dostawca konfiguracji (provider)
// https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-configuration-provider

builder.Services.AddScoped<ICustomerRepository, DbCustomerRepository>();
builder.Services.AddSingleton<IUserRepository, FakeUserRepository>();

// string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 1. dotnet add package Microsoft.EntityFrameworkCore.Sqlite
builder.Services.AddDbContext<ShopperDb>(options => options.UseSqlite(connectionString));

var url = builder.Configuration["NbpApi:Url"];

builder.Services.Configure<NbpCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApi"));

builder.Services.AddSingleton<ICurrencyService, NbpCurrencyService>();


// 1. dotnet tool install --global dotnet-ef
// 2. Przejdü do folderu projektu Altkom.Shopper.Infrastructure
// 3. dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0.13
// 4. dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.13
// 5. Zaimplementuj IDesignTimeDbContextFactory
// 6. dotnet ef migrations add Init




// dotnet add package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

var app = builder.Build();

await app.Services.EnsureDatabase<ShopperDb>();

app.MapGet("/", () => "Hello World!");

app.MapCustomers();

// Query Params
// GET api/products?color=red&maxprice=100
app.MapGet("/api/products", (string color, decimal maxPrice) => ProductEndpoints.GetAll(color, maxPrice));

// GET api/products/{id}
app.MapGet("/api/products/{id:int}", (int id, ICurrencyService currencyService) => $"Product {id} {currencyService.GetRatio()}");

// GET api/products/{symbol}
app.MapGet("/api/products/{symbol}", (string symbol) => $"Product {symbol}");


app.MapGet("/api/orders", (HttpContext context) =>
{
    var request = context.Request;

    string apiKey = request.Headers["X-ApiKey"];

    if (apiKey == "123")
    {
        context.Response.WriteAsync("Hello World!");
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
});


app.MapGet("/api/orders/{id}", (int id, HttpRequest request, HttpResponse response) =>
{    
    string apiKey = request.Headers["X-ApiKey"];

    if (apiKey == "123")
    {
        response.WriteAsync("Hello World!");
    }
    else
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
    }
});

// GET /api/vehicles/{id}?p=3&s=true
app.MapGet("/api/vehicles/{id}", (
    [FromRoute] int id,    
    [FromQuery(Name = "p")] int page,
    [FromQuery(Name = "s")] bool sort,
    [FromBody] string content,
    [FromServices] ICustomerRepository repository,    
    [FromHeader(Name = "X-ApiKey")] string apiKey) => Results.Ok())
    .ExcludeFromDescription()
    ;


app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // https://localhost:7201/swagger/v1/swagger.json
    app.UseSwaggerUI(); // https://localhost:7201/swagger
}

// TODO: Background Worker

app.Run();