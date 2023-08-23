using A_Domain.Models;
using B_DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace E_Commerce_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            SeedDbIfNotExists(host);

            host.Run();
        }

        private static void SeedDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<EcommerceIdentityDbContext>();
                var userManager = services.GetRequiredService<UserManager<UserIdentity>>();

                context.Database.EnsureCreated();

                if (!context.Users.Any(x => x.UserName == "Admin" || x.UserName == "Manager"))
                {
                    var adminUser = new UserIdentity()
                    {
                        UserName = "Admin",
                        Email = "admin@gmail.com"
                    };

                    var managerUser = new UserIdentity()
                    {
                        UserName = "Manager",
                        Email = "manager@gmail.com"
                    };

                    userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    userManager.CreateAsync(managerUser, "password").GetAwaiter().GetResult();

                    var adminClaim = new Claim("Role", "Admin");
                    var managerClaim = new Claim("Role", "Manager");

                    userManager.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                    userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

                    services.AddLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
