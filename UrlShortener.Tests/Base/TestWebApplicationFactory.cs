using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api.Data;

namespace UrlShortener.Tests.Base;

public sealed class TestWebApplicationFactory(SqlServerFixture sqlServer) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(x 
                => x.ServiceType == typeof(DbContextOptions<UriShortenerDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UriShortenerDbContext>(options =>
                options.UseSqlServer(sqlServer.Container.GetConnectionString()));
        });
    }
}
