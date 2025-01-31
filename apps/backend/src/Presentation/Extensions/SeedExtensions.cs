using Infrastructure.Database;

namespace Presentation.Extensions;

internal static class SeedExtensions
{
    public static async Task SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        await using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.SeedDataAsync(scope);
    }
}
