namespace UrlShortener.Api.Data.Interfaces;

public interface IUnitOfWork
{
    IShortUrlRepository ShortUrls { get; }

    IAboutPageRepository AboutPages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}