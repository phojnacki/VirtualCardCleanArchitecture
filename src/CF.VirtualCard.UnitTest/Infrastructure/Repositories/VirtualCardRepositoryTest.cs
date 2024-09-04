using System.Data.Common;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Infrastructure.DbContext;
using CF.VirtualCard.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CF.VirtualCard.UnitTest.Infrastructure.Repositories;

public class VirtualCardRepositoryTest
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    [Fact]
    public async Task GetListTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();

        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCardOne = CreateVirtualCard();
        var virtualCardTwo = CreateVirtualCard();
        virtualCardTwo.Email = "email2@test.com";

        //Act
        var repository = new VirtualCardRepository(context);
        await repository.AddRangeAsync([virtualCardOne, virtualCardTwo]);
        await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        var filter = new VirtualCardFilter { FirstName = "FirstName" };
        var result = await repository.GetListByFilterAsync(filter, _cancellationTokenSource.Token);

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCardOne = CreateVirtualCard();

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCardOne);
        await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        var filter = new VirtualCardFilter { Email = "test1@test.com" };

        //Act
        var result = await repository.GetByFilterAsync(filter, _cancellationTokenSource.Token);

        //Assert
        Assert.Equal("test1@test.com", result.Email);
    }

    [Fact]
    public async Task CreateOkTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCard = CreateVirtualCard();
        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCard);

        //Act
        var result = await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        //Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var newVirtualCard = CreateVirtualCard();

        var repository = new VirtualCardRepository(context);
        repository.Add(newVirtualCard);
        await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        var storedVirtualCard = await repository.GetByIdAsync(newVirtualCard.Id, _cancellationTokenSource.Token);
        repository.Remove(storedVirtualCard);
        await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        //Act
        var nonExistentUser = await repository.GetByIdAsync(newVirtualCard.Id, _cancellationTokenSource.Token);

        //Assert
        Assert.Null(nonExistentUser);
    }

    [Fact]
    public async Task DuplicatedEmailTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCardOne = CreateVirtualCard();
        var virtualCardTwo = CreateVirtualCard();
        virtualCardTwo.FirstName = "FirstNameTwo";
        virtualCardTwo.Surname = "Surname Two";

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCardOne);
        await repository.SaveChangesAsync(_cancellationTokenSource.Token);

        repository.Add(virtualCardTwo);

        //Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveChangesAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CreateInvalidEmailTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Email = null;

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCard);

        //Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveChangesAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CreateInvalidPasswordTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Password = null;

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCard);

        //Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveChangesAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CreateInvalidFirstNameTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.FirstName = null;

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCard);

        //Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveChangesAsync(_cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CreateInvalidSurnameTestAsync()
    {
        await using var connection = await CreateAndOpenSqliteConnectionAsync();
        var options = await SetDbContextOptionsBuilderAsync(connection);

        await using var context = new VirtualCardContext(options);
        Assert.True(await context.Database.EnsureCreatedAsync());

        //Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Surname = null;

        var repository = new VirtualCardRepository(context);
        repository.Add(virtualCard);

        //Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveChangesAsync(_cancellationTokenSource.Token));
    }

    private static VirtualCard.Domain.Entities.VirtualCard CreateVirtualCard()
    {
        return new VirtualCard.Domain.Entities.VirtualCard
        {
            Password = "Password01@",
            Email = "test1@test.com",
            Surname = "Surname1",
            FirstName = "FirstName1",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };
    }

    private static async Task<DbContextOptions<VirtualCardContext>> SetDbContextOptionsBuilderAsync(
        DbConnection connection)
    {
        return await Task.FromResult(new DbContextOptionsBuilder<VirtualCardContext>()
            .UseSqlite(connection)
            .Options);
    }

    private static async Task<SqliteConnection> CreateAndOpenSqliteConnectionAsync()
    {
        var connection = await CreateSqLiteConnectionAsync();
        await connection.OpenAsync();
        return connection;
    }

    private static async Task<SqliteConnection> CreateSqLiteConnectionAsync()
    {
        return await Task.FromResult(new SqliteConnection("DataSource=:memory:"));
    }
}