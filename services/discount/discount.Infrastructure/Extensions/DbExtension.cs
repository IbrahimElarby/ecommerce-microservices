using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDataBase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Discount DB migration Started");
                    ApplyMigration(config);
                    logger.LogInformation("Discount DB migration Completed");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,"Cannot Create Database Migration");
                    throw; 
                }
            }
            return host;
        }
        private static void ApplyMigration(IConfiguration config)
        {
            var retry = 5;
            while (retry > 0)
            {
                try
                {
                    using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var cmd = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    cmd.CommandText = "drop table if exists Coupon";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"CREATE TABLE Coupon(ID SERIAL PRIMARY KEY,
                                        ProductName varchar(500) not null, 
                                        Description text,
                                        Amount int)";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "insert into Coupon (ProductName,Description,Amount) values ('Egypt Adidas Quick Force Indoor Badminton Shoes','Adidas Discount',600)";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "insert into Coupon (ProductName,Description,Amount) values ('PowerFit 19 FH Rubber Spike Cricket Shoes','PowerFit Discount',500)";
                    cmd.ExecuteNonQuery();
                    break;
                }
                catch (Exception ex)
                {
                    retry--;
                    if (retry == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
