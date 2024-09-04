using CF.VirtualCard.Domain.Exceptions;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Domain.Repositories;
using CF.VirtualCard.Domain.Services;
using CF.VirtualCard.Domain.Services.Interfaces;
using Moq;
using Xunit;

namespace CF.VirtualCard.UnitTest.Domain.Services;

public class VirtualCardServiceTest
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Mock<IPasswordHasherService> _mockPassword = new();
    private readonly Mock<IVirtualCardRepository> _mockRepository = new();

    [Fact]
    public async Task GetByFilterAsync_ReturnsCorrectVirtualCard()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();

        _mockRepository.Setup(x => x.GetByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token))
            .ReturnsAsync(virtualCard);
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act
        var result =
            await virtualCardService.GetByFilterAsync(new VirtualCardFilter { Id = 1 }, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(virtualCard.Id, result.Id);
    }

    [Fact]
    public async Task GetListTestAsync()
    {
        // Arrange
        var virtualCardOne = CreateVirtualCard();
        var virtualCardTwo = CreateVirtualCard(2, "test2@test.com");

        var virtualCards = new List<VirtualCard.Domain.Entities.VirtualCard>
        {
            virtualCardOne,
            virtualCardTwo
        };

        // Act
        var filter = new VirtualCardFilter { PageSize = 10, CurrentPage = 1 };
        _mockRepository.Setup(x => x.CountByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token))
            .ReturnsAsync(virtualCards.Count);
        _mockRepository.Setup(x => x.GetListByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token))
            .ReturnsAsync(virtualCards);
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);
        var result = await virtualCardService.GetListByFilterAsync(filter, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Theory]
    [InlineData(0, "F")]
    [InlineData(0, "")]
    [InlineData(0,
        "First Name First Name First Name First Name First Name First Name First Name First Name First Name First Name First Name.")]
    public async Task CreateAsync_InvalidFirstName_ThrowsValidationException(int id, string firstName)
    {
        // Arrange
        var virtualCard = CreateVirtualCard(id);
        virtualCard.FirstName = firstName;
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.CreateAsync(virtualCard, _cancellationTokenSource.Token));
    }

    [Theory]
    [InlineData("")]
    [InlineData("S")]
    [InlineData(
        "Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname Surname")]
    public async Task CreateAsync_InvalidSurname_ThrowsValidationException(string surname)
    {
        // Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Surname = surname;
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.CreateAsync(virtualCard, _cancellationTokenSource.Token));
    }

    [Theory]
    [InlineData("invalid_email")]
    [InlineData("")]
    public async Task CreateAsync_InvalidEmail_ThrowsValidationException(string email)
    {
        // Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Email = email;
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.CreateAsync(virtualCard, _cancellationTokenSource.Token));
    }

    [Theory]
    [InlineData("")]
    [InlineData("P@01")]
    public async Task CreateAsync_InvalidPassword_ThrowsValidationException(string password)
    {
        // Arrange
        var virtualCard = CreateVirtualCard();
        virtualCard.Password = password;
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.CreateAsync(virtualCard, _cancellationTokenSource.Token));
    }


    [Fact]
    public async Task UpdateInvalidIdTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard(0);

        // Act
        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.UpdateAsync(virtualCard.Id, virtualCard, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task UpdateInvalidVirtualCardIsNullTestAsync()
    {
        // Arrange
        const long id = 1;

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.UpdateAsync(id, null, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task UpdateInvalidVirtualCardNotFoundTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            virtualCardService.UpdateAsync(virtualCard.Id, virtualCard, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task DeleteInvalidIdTestAsync()
    {
        // Arrange
        const long id = 0;

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            virtualCardService.DeleteAsync(id, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task DeleteAsync_InvalidNotFoundTest_ThrowsValidationException()
    {
        // Arrange
        const long id = 1;

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            virtualCardService.DeleteAsync(id, _cancellationTokenSource.Token));
    }

    [Fact]
    public async Task IsAvailableEmailTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();

        _mockRepository.Setup(x => x.GetByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token))
            .ReturnsAsync((VirtualCard.Domain.Entities.VirtualCard)null);

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act
        var existingEmail = await virtualCardService.IsAvailableEmailAsync(virtualCard.Email, _cancellationTokenSource.Token);

        // Assert
        Assert.True(existingEmail);
    }

    [Fact]
    public async Task IsNotAvailableEmailTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();

        var filter = new VirtualCardFilter { Email = virtualCard.Email };
        _mockRepository.Setup(x => x.GetByFilterAsync(filter, _cancellationTokenSource.Token))
            .ReturnsAsync(virtualCard);

        var virtualCardService = new VirtualCardService(_mockRepository.Object, _mockPassword.Object);

        // Act
        var existingEmail = await virtualCardService.IsAvailableEmailAsync(virtualCard.Email, _cancellationTokenSource.Token);

        // Assert
        Assert.True(existingEmail);
    }

    private static VirtualCard.Domain.Entities.VirtualCard CreateVirtualCard(int id = 1, string email = "test1@test.com")
    {
        return new VirtualCard.Domain.Entities.VirtualCard
        {
            Id = id,
            Password = "Password@01",
            Email = email,
            Surname = "Surname",
            FirstName = "FirstName",
            Updated = DateTime.Now,
            Created = DateTime.Now
        };
    }
}