using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api.Data;
using UrlShortener.Api.DTOs;
using UrlShortener.Api.Models;
using UrlShortener.Tests.Config;
using UrlShortener.Tests.Data;
using Xunit.Abstractions;

namespace UrlShortener.Tests.Base;

public abstract class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    protected readonly TestWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly UriShortenerDbContext DbContext;

    private readonly IServiceScope _scope;
    private readonly ITestOutputHelper _output;

    protected BaseIntegrationTest(TestWebApplicationFactory factory, ITestOutputHelper output)
    {
        Factory = factory;
        Client = factory.CreateClient();
        _output = output;

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider
            .GetRequiredService<UriShortenerDbContext>();

        var userManager = _scope.ServiceProvider
            .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();

        var dataSetup = new DataSetup(DbContext, userManager);

        dataSetup.SeedAsync().GetAwaiter().GetResult();
    }

    protected async Task<string> LoginAsync(string userName, string password)
    {
        var response = await Client.PostAsJsonAsync(
            Setup.AuthLoginUrl,
            new LoginRequest
            {
                UserName = userName,
                Password = password
            });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return result!.Token;
    }

    protected void Authenticate(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);
    }

    protected async Task PrintResponseAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        _output.WriteLine("");
        _output.WriteLine("========================================");
        _output.WriteLine($"{response.RequestMessage?.Method} {response.RequestMessage?.RequestUri}");
        _output.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        _output.WriteLine("Response:");
        _output.WriteLine(body);
        _output.WriteLine("========================================");
    }

    protected async Task<T?> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        await PrintResponseAsync(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public void Dispose()
    {
        _scope.Dispose();
        Client.Dispose();
    }
}