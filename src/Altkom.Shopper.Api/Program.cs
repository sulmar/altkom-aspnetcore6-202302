using Altkom.Shopper.Api;
using Altkom.Shopper.Api.Extensions;
using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;
using Microsoft.EntityFrameworkCore;

// Minimal Api

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;

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

var app = builder.Build();

await app.Services.EnsureDatabase<ShopperDb>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/customers", (ICustomerRepository repository) => repository.GetAll());

// GET  - 200 OK, 404 Not Found
// POST - 201 Created + Location

// app.MapGet("/api/customers", (ShopperDb db) => db.Customers.ToList());

/*
app.MapGet("/api/customers/{id}", (int id, ShopperDb db) =>
{
    var customer = db.Customers.Find(id);

    if (customer == null)
        return Results.NotFound();

    return Results.Ok(customer);
}
).WithName("GetCustomerById");

*/

/* wyraøenie ? true : false 
 
app.MapGet("/api/customers/{id}", (int id, ShopperDb db) => db.Customers.Find(id) is Customer customer ? Results.Ok(customer) : Results.NotFound())
    .WithName("GetCustomerById");
*/

// Match Patterns
// Route Params GET api/customers/{id}
app.MapGet("/api/customers/{id:min(1)}", (int id, ICustomerRepository repository) => repository.GetById(id) switch
{
    Customer customer => Results.Ok(customer),
    _ => Results.NotFound()
}).WithName("GetCustomerById");


app.MapPost("/api/customers", (Customer customer, ICustomerRepository repository) =>
{
    repository.Add(customer);

    // var url = linkGenerator.GetPathByName("GetCustomerById");
    // return Results.Created(url, new { Id = customer.Id }, customer);

    return Results.CreatedAtRoute("GetCustomerById", new { Id = customer.Id }, customer);
});

// PUT api/customers/{id} 
app.MapPut("/api/customers/{id:int}", (int id, Customer customer, ICustomerRepository repository) =>
{
    if (id != customer.Id)
        return Results.BadRequest();

    repository.Update(customer);    

    return Results.NoContent();
});

// TODO: dodaÊ link do przyk≥adowej implementacji
// JsonPatch https://jsonpatch.com/
// Json Merge Patch https://www.rfc-editor.org/rfc/rfc7386
// PATCH api/customers/{id} 
app.MapMethods("/api/customers/{id:int}", new string[] { "PATCH" }, () => Results.NoContent());

// DELETE api/customers/{id}
app.MapDelete("/api/customers/{id:int}", (int id, ICustomerRepository repository) =>
{
    repository.Remove(id);

    return Results.NoContent();
});

// HEAD api/customers/{id}
app.MapMethods("/api/customers/{id:int}", new string[] { "HEAD" }, (int id, ICustomerRepository repository) =>
{
    if (repository.Exists(id))
    {
        return Results.Ok();
    }
    else
        return Results.NotFound();
});

// Query Params
// GET api/products?color=red&maxprice=100
app.MapGet("/api/products", (string color, decimal maxPrice) => $"Product {color} {maxPrice}");

// GET api/products/{id}
app.MapGet("/api/products/{id:int}", (int id, ICurrencyService currencyService) => $"Product {id} {currencyService.GetRatio()}");

// GET api/products/{symbol}
app.MapGet("/api/products/{symbol}", (string symbol) => $"Product {symbol}");

// TODO: Background Worker

app.Run();