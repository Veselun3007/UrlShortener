using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Interfaces;

public interface IShortUrlRepository
{
    Task<ShortUrl?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetByCodeAsync(string shortCode, CancellationToken cancellationToken = default);

    Task<bool> ExistsByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken = default);

    Task<bool> ExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);

    Task<IEnumerable<ShortUrl>> GetAllAsync(CancellationToken cancellationToken = default);

    void Add(ShortUrl entity);

    void Remove(ShortUrl entity);
}