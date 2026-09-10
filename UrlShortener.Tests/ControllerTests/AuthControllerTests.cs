using System.Net;
using System.Net.Http.Json;
using UrlShortener.Api.DTOs;
using UrlShortener.Tests.Base;
using UrlShortener.Tests.Config;
using Xunit.Abstractions;

namespace UrlShortener.Tests.ControllerTests;

public sealed class AuthControllerTests(TestWebApplicationFactory factory, ITestOutputHelper output) : BaseIntegrationTest(factory, output)
{
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var response = await Client.PostAsJsonAsync(
            Setup.AuthLoginUrl,
            new
            {
                userName = Setup.AdminUserName,
                password = Setup.AdminPassword
            });

        var result = await ReadResponseAsync<LoginResponse>(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync(
            Setup.AuthLoginUrl,
            new
            {
                userName = Setup.AdminUserName,
                password = "WrongPassword123!"
            });

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithUnknownUser_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync(
            Setup.AuthLoginUrl,
            new
            {
                userName = "unknown-user",
                password = "Password123!"
            });

        await PrintResponseAsync(response);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}