using ims.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;

namespace ims.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task UseDatabaseSeederAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<ims.Infrastructure.Data.AppDbContext>();
            await dbContext.Database.MigrateAsync();

            await DatabaseSeeder.SeedAsync(services);
        }
    }
}
