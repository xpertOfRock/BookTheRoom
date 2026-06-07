using DotNet.Testcontainers.Builders;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Testcontainers.PostgreSql;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Shared xUnit fixture that starts a real PostgreSQL container once per test class.
    /// Each test creates its own DbContext via <see cref="CreateContext"/> to avoid
    /// shared-state issues between parallel or sequential test methods.
    /// </summary>
    public sealed class DatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("booktheroom_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        public string ConnectionString { get; private set; } = string.Empty;

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();
            ConnectionString = _postgres.GetConnectionString();

            // Create schema once; tests share the same DB but insert their own data.
            await using var ctx = CreateContext();
            await ctx.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgres.DisposeAsync();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            return new ApplicationDbContext(options);
        }

        // Cache that always reports misses — forces every repository read to hit the DB.
        public IDistributedCache CreateNoOpCache()
        {
            var memCache = new MemoryDistributedCache(
                Options.Create(new MemoryDistributedCacheOptions()));
            return memCache;
        }

        public Mock<Application.Interfaces.IPhotoService> CreatePhotoServiceMock()
        {
            var mock = new Mock<Application.Interfaces.IPhotoService>();
            mock.Setup(p => p.DeletePhotoAsync(It.IsAny<string>()))
                .ReturnsAsync(new CloudinaryDotNet.Actions.DeletionResult());
            return mock;
        }
    }
}
