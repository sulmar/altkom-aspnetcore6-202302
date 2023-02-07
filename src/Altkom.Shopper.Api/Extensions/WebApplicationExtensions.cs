using Altkom.Shopper.Domain;

namespace Altkom.Shopper.Api
{
    static class WebApplicationExtensions
    {
        public static WebApplication MapCustomers(this WebApplication app)
        {
            app.MapGet("/api/customers", (ICustomerRepository repository) => repository.GetAll())
                .RequireAuthorization();

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

            /* wyrażenie ? true : false 

            app.MapGet("/api/customers/{id}", (int id, ShopperDb db) => db.Customers.Find(id) is Customer customer ? Results.Ok(customer) : Results.NotFound())
                .WithName("GetCustomerById");
            */

            // Match Patterns
            // Route Params GET api/customers/{id}
            app.MapGet("/api/customers/{id:min(1)}", (int id, ICustomerRepository repository) => repository.GetById(id) switch
            {
                Customer customer => Results.Ok(customer),
                _ => Results.NotFound()
            })
                .WithName("GetCustomerById")
                .Produces<Customer>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                ;


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

            // TODO: dodać link do przykładowej implementacji
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


            return app;

        }
    }
}
