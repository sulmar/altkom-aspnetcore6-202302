using Altkom.Shopper.Domain;
using Altkom.Shopper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICustomerRepository, DbCustomerRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/api/customers", (ICustomerRepository repository) => repository.GetAll());
app.Run();