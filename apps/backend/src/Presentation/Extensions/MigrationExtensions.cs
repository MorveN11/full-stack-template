using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions;

internal static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        await using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
