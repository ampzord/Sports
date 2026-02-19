namespace Sports.Api.IntegrationTests.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using RabbitMQ.Client;
using Sports.Api.Database;
using Sports.Tests.Shared;
using Testcontainers.MsSql;

public class SportsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder(DatabaseHelper.SqlServerImage)
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

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
                options.UseSqlServer(_container.GetConnectionString()));

            var mockChannel = new Mock<IModel>();
            var mockConnection = new Mock<IConnection>();
            mockConnection.Setup(c => c.CreateModel()).Returns(mockChannel.Object);
            services.AddSingleton(mockConnection.Object);
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SportsDbContext>();
        await db.Database.ResetAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        await _container.DisposeAsync();
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
