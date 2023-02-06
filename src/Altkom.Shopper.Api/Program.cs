using Altkom.Shopper.Api.Extensions;
using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddScoped<ICustomerRepository, DbCustomerRepository>();

string connectionString = "Data Source=shopper.db";
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
app.MapGet("/api/customers/{id}", (int id, ShopperDb db) => db.Customers.Find(id) switch
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

app.Run();