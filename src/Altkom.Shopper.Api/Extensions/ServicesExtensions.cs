using Microsoft.EntityFrameworkCore;

namespace Altkom.Shopper.Api.Extensions;

public static class ServicesExtensions
{
    public static async Task EnsureDatabase<TContext>(this IServiceProvider services)        
        where TContext : DbContext
    {
        await using var db = services.CreateScope().ServiceProvider.GetRequiredService<TContext>();

        await db.Database.MigrateAsync();

    }
}
