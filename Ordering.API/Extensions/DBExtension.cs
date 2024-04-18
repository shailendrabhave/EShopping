using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class DBExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) 
            where TContext: DbContext
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    logger.LogInformation($"Ordering DB migration started: {typeof(TContext).Name}");
                    SeedData(seeder, context, services);
                    logger.LogInformation($"Ordering DB migration completed: {typeof(TContext).Name}");
                }
                catch (SqlException se)
                {
                    logger.LogError(se,$"An error occured while migrating database {typeof(TContext).Name}");
                    throw;
                }
            }
            return host;
        }

        private static void SeedData<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
