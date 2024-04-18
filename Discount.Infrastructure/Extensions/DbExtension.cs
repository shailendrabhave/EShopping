using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using(var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Discount DB Migration Started");
                    ApplyMigration(configuration);
                    logger.LogInformation("Discount DB Migration Completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            return host;
        }

        private static void ApplyMigration(IConfiguration configuration)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();
            using var command = new NpgsqlCommand() { Connection = connection};
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                        ProductName VARCHAR(500) NOT NULL,
                                                        Description TEXT,
                                                        Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);";
            command.ExecuteNonQuery();
        }
    }
}
