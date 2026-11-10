namespace UrlShortener.Tests.Base;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    public SqlServerFixture SqlServer { get; } = new();
    public TestWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await SqlServer.InitializeAsync();

        Factory = new TestWebApplicationFactory(SqlServer);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await SqlServer.DisposeAsync();
    }
}