using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Interfaces;

public interface IAboutPageRepository
{
    Task<AboutPage?> GetAsync(CancellationToken cancellationToken = default);

    void Add(AboutPage entity);

    void Update(AboutPage entity);
}