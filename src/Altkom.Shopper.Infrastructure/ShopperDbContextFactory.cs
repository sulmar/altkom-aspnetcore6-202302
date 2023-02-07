using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Altkom.Shopper.Infrastructure
{
    internal class ShopperDbContextFactory : IDesignTimeDbContextFactory<ShopperDb>
    {
        public ShopperDb CreateDbContext(string[] args)
        {
            string connectionString = @"Data Source=shopper.db";

            var options = new DbContextOptionsBuilder<ShopperDb>()
                .UseSqlite(connectionString) // dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0.13
                .Options;

            return new ShopperDb(options);
        }
    }
}
