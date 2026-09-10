using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data.Interfaces;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Repositories;

public sealed class AboutPageRepository(UriShortenerDbContext context) : IAboutPageRepository
{
    public async Task<AboutPage?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await context.AboutPages.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(AboutPage entity)
    {
        context.AboutPages.Add(entity);
    }

    public void Update(AboutPage entity)
    {
        context.AboutPages.Update(entity);
    }
}