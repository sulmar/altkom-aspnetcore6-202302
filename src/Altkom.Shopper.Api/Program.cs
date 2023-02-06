using Altkom.Shopper.Api.Extensions;
using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

app.MapGet("/api/customers", (ShopperDb db) => db.Customers.ToList());

app.Run();