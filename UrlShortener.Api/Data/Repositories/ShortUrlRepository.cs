using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data.Interfaces;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Repositories;

public sealed class ShortUrlRepository(UriShortenerDbContext context) : IShortUrlRepository
{
    public async Task<ShortUrl?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ShortUrl?> GetByCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);
    }

    public async Task<bool> ExistsByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AsNoTracking()
            .AnyAsync(x => x.OriginalUrl == originalUrl, cancellationToken);
    }

    public async Task<bool> ExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AsNoTracking()
            .AnyAsync(x => x.ShortCode == shortCode, cancellationToken);
    }

    public async Task<IEnumerable<ShortUrl>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.ShortUrls
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public void Add(ShortUrl entity)
    {
        context.ShortUrls.Add(entity);
    }

    public void Remove(ShortUrl entity)
    {
        context.ShortUrls.Remove(entity);
    }
}