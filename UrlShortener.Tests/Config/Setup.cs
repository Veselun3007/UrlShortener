namespace UrlShortener.Tests.Config;

public static class Setup
{
    public const string AdminUserName = "admin";
    public const string AdminPassword = "Admin123!";

    public const string UserUserName = "user";
    public const string UserPassword = "User123!";

    public const string AdminRole = "Admin";
    public const string UserRole = "User";

    public const string AuthLoginUrl = "/api/Auth/Login";
    public const string ShortUrlsGetAllUrl = "/api/ShortUrls/GetAll";
    public const string ShortUrlsGetByIdUrl = "/api/ShortUrls/GetById";
    public const string ShortUrlsCreateUrl = "/api/ShortUrls/Create";
    public const string ShortUrlsDeleteUrl = "/api/ShortUrls/Delete";
    public const string AboutGetUrl = "/api/About/Get";
    public const string AboutUpdateUrl = "/api/About/Update";
}