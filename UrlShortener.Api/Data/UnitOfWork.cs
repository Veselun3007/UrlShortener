using UrlShortener.Api.Data.Interfaces;
using UrlShortener.Api.Data.Repositories;

namespace UrlShortener.Api.Data;

public sealed class UnitOfWork(UriShortenerDbContext context) : IUnitOfWork
{
    private IShortUrlRepository? _shortUrls;
    private IAboutPageRepository? _aboutPages;

    public IShortUrlRepository ShortUrls => _shortUrls ??= new ShortUrlRepository(context);

    public IAboutPageRepository AboutPages => _aboutPages ??= new AboutPageRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}