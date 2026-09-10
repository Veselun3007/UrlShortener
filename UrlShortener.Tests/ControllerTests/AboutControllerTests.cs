using System.Net;
using System.Net.Http.Json;
using UrlShortener.Api.DTOs;
using UrlShortener.Tests.Base;
using UrlShortener.Tests.Config;
using Xunit.Abstractions;

namespace UrlShortener.Tests.ControllerTests;

public sealed class AboutControllerTests(TestWebApplicationFactory factory, ITestOutputHelper output) : BaseIntegrationTest(factory, output)
{
    [Fact]
    public async Task Get_ReturnsOk()
    {
        var response = await Client.GetAsync(Setup.AboutGetUrl);

        var result = await ReadResponseAsync<AboutPageResponse>(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Content));
    }

    [Fact]
    public async Task Update_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await Client.PutAsJsonAsync(
            Setup.AboutUpdateUrl,
            new UpdateAboutPageRequest
            {
                Content = "Updated content."
            });

        await PrintResponseAsync(response);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Update_AsUser_ReturnsForbidden()
    {
        var token = await LoginAsync(
            Setup.UserUserName,
            Setup.UserPassword);

        Authenticate(token);

        var response = await Client.PutAsJsonAsync(
            Setup.AboutUpdateUrl,
            new UpdateAboutPageRequest
            {
                Content = "Updated content."
            });

        await PrintResponseAsync(response);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Update_AsAdmin_ReturnsNoContent()
    {
        var token = await LoginAsync(
            Setup.AdminUserName,
            Setup.AdminPassword);

        Authenticate(token);

        var response = await Client.PutAsJsonAsync(
            Setup.AboutUpdateUrl,
            new UpdateAboutPageRequest
            {
                Content = "Updated by admin."
            });

        await PrintResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}