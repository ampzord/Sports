namespace Sports.Api.IntegrationTests.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using Sports.Api.Database;

public class UnavailableServicesFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(ResolveApiContentRoot());
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveService<DbContextOptions<SportsDbContext>>(services);
            RemoveService<IConnection>(services);

            services.AddDbContext<SportsDbContext>(options =>
                options.UseSqlServer("Server=localhost,19999;Database=fake;User Id=sa;Password=fake;TrustServerCertificate=True;Connect Timeout=1"));

            var mockConnection = new Mock<IConnection>();
            mockConnection
                .Setup(c => c.CreateModel())
                .Throws(new AlreadyClosedException(
                    new ShutdownEventArgs(ShutdownInitiator.Library, 0, "Connection is closed")));
            services.AddSingleton(mockConnection.Object);
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(T));

        if (descriptor is not null)
            services.Remove(descriptor);
    }

    private static string ResolveApiContentRoot()
    {
        var dir = AppContext.BaseDirectory;

        while (dir is not null)
        {
            if (Directory.GetFiles(dir, "*.slnx").Length > 0 ||
                Directory.GetFiles(dir, "*.sln").Length > 0)
                return Path.Combine(dir, "src", "app", "Sports.Api");

            dir = Directory.GetParent(dir)?.FullName;
        }

        throw new InvalidOperationException("Solution directory not found.");
    }
}

[CollectionDefinition("Unavailable")]
public class UnavailableCollection : ICollectionFixture<UnavailableServicesFactory>;
