using Altkom.Shopper.Domain;
using Microsoft.EntityFrameworkCore;

namespace Altkom.Shopper.Infrastructure;

// dotnet add package Microsoft.EntityFrameworkCore
public class ShopperDb : DbContext
{
    public ShopperDb(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
