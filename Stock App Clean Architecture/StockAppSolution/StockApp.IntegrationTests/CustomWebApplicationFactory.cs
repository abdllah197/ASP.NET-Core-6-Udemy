using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services => {
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descripter != null)
                {
                    services.Remove(descripter);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatbaseForTesting");
                });
            });

            builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, Microsoft.Extensions.Configuration.IConfigurationBuilder config) =>
            {
                var newConfiguration = new Dictionary<string, string>() {
                 {
                        "FinnhubToken", "cc676uaad3i9rj8tb1s0" }
                 };

                config.AddInMemoryCollection(newConfiguration);
            });


        }
    }
}
