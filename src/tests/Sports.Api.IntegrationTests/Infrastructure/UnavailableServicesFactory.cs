using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Sports.Api.Database;

namespace Sports.Api.IntegrationTests.Infrastructure;

public class UnavailableServicesFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask;

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
}

[CollectionDefinition("Unavailable")]
public class UnavailableCollection : ICollectionFixture<UnavailableServicesFactory>;
