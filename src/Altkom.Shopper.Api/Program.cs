using Altkom.Shopper.Api.Extensions;
using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

// Minimal Api

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddScoped<ICustomerRepository, DbCustomerRepository>();

// string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 1. dotnet add package Microsoft.EntityFrameworkCore.Sqlite
builder.Services.AddDbContext<ShopperDb>(options => options.UseSqlite(connectionString));


// 2. dotnet tool install --global dotnet-ef
// 3. dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.13
// 4. dotnet ef migrations add Init

var app = builder.Build();

await app.Services.EnsureDatabase<ShopperDb>();

app.MapGet("/", () => "Hello World!");
// app.MapGet("/api/customers", (ICustomerRepository repository) => repository.GetAll());

// GET  - 200 OK, 404 Not Found
// POST - 201 Created + Location

app.MapGet("/api/customers", (ShopperDb db) => db.Customers.ToList());

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

/* wyra¿enie ? true : false 
 
app.MapGet("/api/customers/{id}", (int id, ShopperDb db) => db.Customers.Find(id) is Customer customer ? Results.Ok(customer) : Results.NotFound())
    .WithName("GetCustomerById");
*/

// Match Patterns
// Route Params GET api/customers/{id}
app.MapGet("/api/customers/{id:min(1)}", (int id, ShopperDb db) => db.Customers.Find(id) switch
{
    Customer customer => Results.Ok(customer),
    _ => Results.NotFound()
}).WithName("GetCustomerById");


app.MapPost("/api/customers", (Customer customer, ShopperDb db, LinkGenerator linkGenerator) =>
{
    db.Customers.Add(customer);
    db.SaveChanges();

    // var url = linkGenerator.GetPathByName("GetCustomerById");
    // return Results.Created(url, new { Id = customer.Id }, customer);

    return Results.CreatedAtRoute("GetCustomerById", new { Id = customer.Id }, customer);
});

// PUT api/customers/{id} 
app.MapPut("/api/customers/{id:int}", (int id, Customer customer, ShopperDb db) =>
{
    if (id != customer.Id)
        return Results.BadRequest();

    db.Customers.Update(customer); 
    db.SaveChanges();

    return Results.NoContent();
});

// TODO: dodaæ link do przyk³adowej implementacji
// JsonPatch https://jsonpatch.com/
// Json Merge Patch https://www.rfc-editor.org/rfc/rfc7386
// PATCH api/customers/{id} 
app.MapMethods("/api/customers/{id:int}", new string[] { "PATCH" }, () => Results.NoContent());

// DELETE api/customers/{id}
app.MapDelete("/api/customers/{id:int}", (int id, ShopperDb db) =>
{
    var customer = db.Customers.Find(id);

    if (customer == null)
        return Results.NotFound();

    db.Customers.Remove(customer);
    db.SaveChanges();

    return Results.NoContent();
});

// HEAD api/customers/{id}
app.MapMethods("/api/customers/{id:int}", new string[] { "HEAD" }, (int id, ShopperDb db) =>
{
    if (db.Customers.Any(p => p.Id == id))
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
app.MapGet("/api/products/{id:int}", (int id) => $"Product {id}");

// GET api/products/{symbol}
app.MapGet("/api/products/{symbol}", (string symbol) => $"Product {symbol}");

// TODO: Background Worker

app.Run();