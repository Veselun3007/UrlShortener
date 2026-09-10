using UrlShortener.Api.Data.Interfaces;
using UrlShortener.Api.DTOs;
using UrlShortener.Api.Exceptions;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Interfaces;

namespace UrlShortener.Api.Services;

public sealed class ShortUrlService(IUnitOfWork unitOfWork, IShortCodeGenerator shortCodeGenerator)
{
    public async Task<IEnumerable<ShortUrlDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var shortUrls = await unitOfWork.ShortUrls.GetAllAsync(cancellationToken);

        return shortUrls.Select(x => MapToDto(x)).ToList();
    }

    public async Task<ShortUrlDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shortUrl = await unitOfWork.ShortUrls.GetByIdAsync(id, cancellationToken);

        if (shortUrl is null)
        {
            throw new ShortUrlNotFoundException(id);
        }

        return MapToDto(shortUrl);
    }

    public async Task<string> GetOriginalUrlAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var shortUrl = await unitOfWork.ShortUrls.GetByCodeAsync(shortCode, cancellationToken);

        if (shortUrl is null)
        {
            throw new ShortCodeNotFoundException(shortCode);
        }

        return shortUrl.OriginalUrl;
    }

    public async Task<ShortUrlDto> CreateAsync(string originalUrl, string userId, string userName, CancellationToken cancellationToken = default)
    {
        var exists = await unitOfWork.ShortUrls.ExistsByOriginalUrlAsync(originalUrl, cancellationToken);

        if (exists)
        {
            throw new DuplicateUrlException(originalUrl);
        }

        var shortCode = await GenerateUniqueShortCodeAsync(cancellationToken);
        var shortUrl = ToShortUrl(originalUrl, userId, shortCode);

        unitOfWork.ShortUrls.Add(shortUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(shortUrl, userName);
    }

    public async Task DeleteAsync(Guid id, string userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var shortUrl = await unitOfWork.ShortUrls.GetByIdAsync(id, cancellationToken);

        if (shortUrl is null)
        {
            throw new ShortUrlNotFoundException(id);
        }

        var canDelete = isAdmin || shortUrl.CreatedById == userId;

        if (!canDelete)
        {
            throw new ForbiddenOperationException();
        }

        unitOfWork.ShortUrls.Remove(shortUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateUniqueShortCodeAsync(CancellationToken cancellationToken)
    {
        string shortCode;

        do
        {
            shortCode = shortCodeGenerator.Generate();
        }
        while (await unitOfWork.ShortUrls.ExistsByShortCodeAsync(shortCode, cancellationToken));

        return shortCode;
    }

    private static ShortUrlDto MapToDto(ShortUrl shortUrl, string? createdBy = null)
    {
        return new ShortUrlDto
        {
            Id = shortUrl.Id,
            OriginalUrl = shortUrl.OriginalUrl,
            ShortCode = shortUrl.ShortCode,
            CreatedDate = shortUrl.CreatedDate,
            CreatedBy = createdBy ?? shortUrl.CreatedBy.UserName ?? shortUrl.CreatedById
        };
    }

    private static ShortUrl ToShortUrl(string originalUrl, string userId, string shortCode)
    {
        return new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedDate = DateTime.UtcNow,
            CreatedById = userId
        };
    }
}