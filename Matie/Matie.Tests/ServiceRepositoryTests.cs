using Matie.Data;
using Matie.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Matie.Tests;

public sealed class ServiceRepositoryTests
{
    [Fact]
    public async Task updateServiceAsync_sets_updated_at_to_recent_utc()
    {
        var options = new DbContextOptionsBuilder<DBCon>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var previousUpdatedAt = DateTime.UtcNow.AddDays(-3);

        await using (var db = new DBCon(options))
        {
            await db.Database.EnsureCreatedAsync();
            db.Categories.Add(new Category { Id = 1, Name = "Тестовая коллекция" });
            db.Services.Add(new Service
            {
                Id = 1,
                Name = "Старая",
                CategoryId = 1,
                UpdatedAt = previousUpdatedAt
            });
            await db.SaveChangesAsync();
        }

        var factory = new TestDbContextFactory(options);
        var repository = new ServiceRepository(factory);
        await Task.Delay(50);

        await repository.UpdateServiceAsync(
            new Service
            {
                Id = 1,
                Name = "Новая",
                CategoryId = 1,
                Price = 150
            },
            Array.Empty<int>());

        await using (var db = new DBCon(options))
        {
            var entity = await db.Services.AsNoTracking().SingleAsync(s => s.Id == 1);
            Assert.NotNull(entity.UpdatedAt);
            Assert.True(entity.UpdatedAt > previousUpdatedAt);
        }
    }

    private sealed class TestDbContextFactory : IDbContextFactory<DBCon>
    {
        private readonly DbContextOptions<DBCon> options;

        public TestDbContextFactory(DbContextOptions<DBCon> options)
        {
            this.options = options;
        }

        public DBCon CreateDbContext()
        {
            return new DBCon(options);
        }

        public ValueTask<DBCon> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(CreateDbContext());
        }
    }
}
