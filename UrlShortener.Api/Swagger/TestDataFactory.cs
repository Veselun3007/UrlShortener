using UrlShortener.Api.DTOs;

namespace UrlShortener.Api.Swagger;

public static class TestDataFactory
{
    public static LoginRequest AuthLogin()
    {
        return new LoginRequest
        {
            UserName = "admin",
            Password = "Admin123!"
        };
    }

    public static CreateShortUrlRequest ShortUrlsCreate()
    {
        return new CreateShortUrlRequest
        {
            Url = "https://example.com"
        };
    }

    public static UpdateAboutPageRequest AboutUpdate()
    {
        return new UpdateAboutPageRequest
        {
            Content = "This is the About page."
        };
    }
}