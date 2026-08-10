using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MulliganDeck.Infrastructure;

namespace MulliganDeck.Tests;

public class TestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<MulliganDeckContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(MulliganDeckContext) ||
                    (d.ServiceType.Namespace != null &&
                    d.ServiceType.Namespace.Contains("EntityFrameworkCore")))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<MulliganDeckContext>(options =>
                options.UseSqlite(connection));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MulliganDeckContext>();
            db.Database.EnsureCreated();
        });
    }
}