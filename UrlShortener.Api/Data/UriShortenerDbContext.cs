using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data.Configurations;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data;

public class UriShortenerDbContext(DbContextOptions<UriShortenerDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();
    public DbSet<AboutPage> AboutPages => Set<AboutPage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ShortUrlConfiguration());
        builder.ApplyConfiguration(new AboutPageConfiguration());
    }
}