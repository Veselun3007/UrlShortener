using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.DTOs;
using UrlShortener.Tests.Base;
using UrlShortener.Tests.Config;
using Xunit.Abstractions;

namespace UrlShortener.Tests.ControllerTests;

public sealed class ShortUrlsControllerTests(TestWebApplicationFactory factory, ITestOutputHelper output) : BaseIntegrationTest(factory, output)
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await Client.GetAsync(Setup.ShortUrlsGetAllUrl);

        var result = await ReadResponseAsync<List<ShortUrlDto>>(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetById_WithOwnUrl_ReturnsOk()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var user = await DbContext.Users
            .SingleAsync(x => x.UserName == Setup.UserUserName);

        var shortUrl = await DbContext.ShortUrls
            .SingleAsync(x => x.CreatedById == user.Id);

        var response = await Client.GetAsync(
            $"{Setup.ShortUrlsGetByIdUrl}/{shortUrl.Id}");

        var result = await ReadResponseAsync<ShortUrlDto>(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(shortUrl.Id, result.Id);
    }

    [Fact]
    public async Task GetById_AsAdmin_WithOtherUsersUrl_ReturnsOk()
    {
        var token = await LoginAsync(
            Setup.AdminUserName,
            Setup.AdminPassword);

        Authenticate(token);

        var user = await DbContext.Users
            .SingleAsync(x => x.UserName == Setup.UserUserName);

        var shortUrl = await DbContext.ShortUrls
            .SingleAsync(x => x.CreatedById == user.Id);

        var response = await Client.GetAsync(
            $"{Setup.ShortUrlsGetByIdUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var shortUrl = await DbContext.ShortUrls.FirstAsync();

        var response = await Client.GetAsync(
            $"{Setup.ShortUrlsGetByIdUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ReturnsNotFound()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var response = await Client.GetAsync(
            $"{Setup.ShortUrlsGetByIdUrl}/{Guid.NewGuid()}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithAuthenticatedUser_ReturnsCreated()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var url = $"https://example.com/{Guid.NewGuid()}";

        var response = await Client.PostAsJsonAsync(
            Setup.ShortUrlsCreateUrl,
            new CreateShortUrlRequest
            {
                Url = url
            });

        var result = await ReadResponseAsync<ShortUrlDto>(response);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(url, result.OriginalUrl);
        Assert.False(string.IsNullOrWhiteSpace(result.ShortCode));
        Assert.Equal(7, result.ShortCode.Length);
    }

    [Fact]
    public async Task Create_WithDuplicateUrl_ReturnsConflict()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var response = await Client.PostAsJsonAsync(
            Setup.ShortUrlsCreateUrl,
            new CreateShortUrlRequest
            {
                Url = "https://example.com/user"
            });

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync(
            Setup.ShortUrlsCreateUrl,
            new CreateShortUrlRequest
            {
                Url = "https://example.com/anonymous"
            });

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_OwnUrl_ReturnsNoContent()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var user = await DbContext.Users
            .SingleAsync(x => x.UserName == Setup.UserUserName);

        var shortUrl = await DbContext.ShortUrls
            .SingleAsync(x => x.CreatedById == user.Id);

        var response = await Client.DeleteAsync(
            $"{Setup.ShortUrlsDeleteUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_OtherUsersUrl_ReturnsForbidden()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var admin = await DbContext.Users
            .SingleAsync(x => x.UserName == Setup.AdminUserName);

        var shortUrl = await DbContext.ShortUrls
            .SingleAsync(x => x.CreatedById == admin.Id);

        var response = await Client.DeleteAsync(
            $"{Setup.ShortUrlsDeleteUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_AsAdmin_ReturnsNoContent()
    {
        var token = await LoginAsync(
            Setup.AdminUserName,
            Setup.AdminPassword);

        Authenticate(token);

        var user = await DbContext.Users
            .SingleAsync(x => x.UserName == Setup.UserUserName);

        var shortUrl = await DbContext.ShortUrls
            .SingleAsync(x => x.CreatedById == user.Id);

        var response = await Client.DeleteAsync(
            $"{Setup.ShortUrlsDeleteUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithoutAuthentication_ReturnsUnauthorized()
    {
        var shortUrl = await DbContext.ShortUrls.FirstAsync();

        var response = await Client.DeleteAsync(
            $"{Setup.ShortUrlsDeleteUrl}/{shortUrl.Id}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithUnknownId_ReturnsNotFound()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var response = await Client.DeleteAsync(
            $"{Setup.ShortUrlsDeleteUrl}/{Guid.NewGuid()}");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Redirect_WithExistingShortCode_ReturnsRedirect()
    {
        using var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/s/user123");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        Assert.Equal(
            "https://example.com/user",
            response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task Redirect_WithUnknownShortCode_ReturnsNotFound()
    {
        var response = await Client.GetAsync("/s/unknown");

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}