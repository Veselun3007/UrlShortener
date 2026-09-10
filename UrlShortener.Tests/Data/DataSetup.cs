using Microsoft.AspNetCore.Identity;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Tests.Config;

namespace UrlShortener.Tests.Data;

public sealed class DataSetup(UriShortenerDbContext dbContext, UserManager<ApplicationUser> userManager)
{
    public async Task SeedAsync()
    {
        await ClearAsync();
        await SeedShortUrlsAsync();
        await SeedAboutPageAsync();

        await dbContext.SaveChangesAsync();
    }

    private async Task ClearAsync()
    {
        dbContext.ShortUrls.RemoveRange(dbContext.ShortUrls);
        dbContext.AboutPages.RemoveRange(dbContext.AboutPages);

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedShortUrlsAsync()
    {
        var user = await userManager.FindByNameAsync(Setup.UserUserName);
        var admin = await userManager.FindByNameAsync(Setup.AdminUserName);

        if (user is null || admin is null)
        {
            throw new InvalidOperationException("Test users were not created.");
        }

        dbContext.ShortUrls.AddRange(
            new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/user",
                ShortCode = "user123",
                CreatedDate = DateTime.UtcNow.AddMinutes(-10),
                CreatedById = user.Id
            },
            new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com/admin",
                ShortCode = "admin12",
                CreatedDate = DateTime.UtcNow.AddMinutes(-5),
                CreatedById = admin.Id
            });
    }

    private async Task SeedAboutPageAsync()
    {
        dbContext.AboutPages.Add(new AboutPage
        {
            Content = "Test About page content.",
            UpdatedDate = DateTime.UtcNow
        });
    }
}