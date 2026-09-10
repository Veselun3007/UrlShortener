using UrlShortener.Api.Data.Interfaces;
using UrlShortener.Api.DTOs;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services;

public sealed class AboutPageService(IUnitOfWork unitOfWork)
{
    public async Task<AboutPageResponse?> GetAsync(CancellationToken cancellationToken = default)
    {
        var page = await unitOfWork.AboutPages.GetAsync(cancellationToken);

        return page is null ? null : MapToDto(page);
    }

    public async Task<AboutPageResponse> UpdateAsync(string content, CancellationToken cancellationToken = default)
    {
        var page = await unitOfWork.AboutPages.GetAsync(cancellationToken);

        if (page is null)
        {
            page = ToAboutPage(content);

            unitOfWork.AboutPages.Add(page);
        }
        else
        {
            ToAboutPage(content, page);

            unitOfWork.AboutPages.Update(page);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(page);
    }

    private static AboutPageResponse MapToDto(AboutPage page)
    {
        return new AboutPageResponse
        {
            Content = page.Content,
            UpdatedDate = page.UpdatedDate
        };
    }

    private static void ToAboutPage(string content, AboutPage page)
    {
        page.Content = content;
        page.UpdatedDate = DateTime.UtcNow;
    }

    private static AboutPage ToAboutPage(string content)
    {
        return new AboutPage
        {
            Content = content,
            UpdatedDate = DateTime.UtcNow
        };
    }
}