var app = WebApplication.Create(args);
app.MapGet("/", () => "Hello World!");
app.MapGet("/api/customers", () => "Hello Customers");
app.Run();