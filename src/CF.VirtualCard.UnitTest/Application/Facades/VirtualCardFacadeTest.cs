using AutoMapper;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Application.Facades;
using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Domain.Services.Interfaces;
using Moq;
using Xunit;

namespace CF.VirtualCard.UnitTest.Application.Facades;

public class VirtualCardFacadeTest
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<IVirtualCardService> _mockService = new();

    [Fact]
    public async Task CreateTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();
        var virtualCardRequestDto = CreateVirtualCardRequestDto();
        const long id = 1;

        _mockMapper.Setup(x => x.Map<VirtualCard.Domain.Entities.VirtualCard>(virtualCardRequestDto)).Returns(virtualCard);
        _mockService.Setup(x => x.CreateAsync(virtualCard, _cancellationTokenSource.Token)).ReturnsAsync(id);
        var mockFacade = new VirtualCardFacade(_mockService.Object, _mockMapper.Object);

        // Act
        var result = await mockFacade.CreateAsync(virtualCardRequestDto, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(id, result);
        _mockService.Verify(
            x => x.CreateAsync(It.IsAny<VirtualCard.Domain.Entities.VirtualCard>(), _cancellationTokenSource.Token),
            Times.Once);
        _mockMapper.Verify(x => x.Map<VirtualCard.Domain.Entities.VirtualCard>(virtualCardRequestDto), Times.Once);
    }

    [Fact]
    public async Task GetTestAsync()
    {
        // Arrange
        var virtualCard = CreateVirtualCard();
        var virtualCardResponseDto = CreateVirtualCardResponseDto();

        var filterDto = new VirtualCardFilterDto { Id = 1 };
        var filter = new VirtualCardFilter { Id = 1 };

        _mockMapper.Setup(x => x.Map<VirtualCardResponseDto>(virtualCard)).Returns(virtualCardResponseDto);
        _mockMapper.Setup(x => x.Map<VirtualCardFilter>(filterDto)).Returns(filter);
        _mockService.Setup(x => x.GetByFilterAsync(filter, _cancellationTokenSource.Token)).ReturnsAsync(virtualCard);
        var mockFacade = new VirtualCardFacade(_mockService.Object, _mockMapper.Object);

        // Act
        var result = await mockFacade.GetByFilterAsync(filterDto, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(virtualCard.Id, result.Id);
        _mockService.Verify(x => x.GetByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token),
            Times.Once);
        _mockMapper.Verify(x => x.Map<VirtualCardFilter>(filterDto), Times.Once);
    }

    [Fact]
    public async Task GetListTestAsync()
    {
        // Arrange
        var virtualCards = new List<VirtualCard.Domain.Entities.VirtualCard>
        {
            CreateVirtualCard(),
            CreateVirtualCard(2)
        };
        var pagination = CreatePagination(virtualCards);

        var virtualCardsDto = new List<VirtualCardResponseDto>
        {
            CreateVirtualCardResponseDto(),
            CreateVirtualCardResponseDto(2)
        };
        var paginationDto = CreatePaginationDto(virtualCardsDto);

        var filterDto = new VirtualCardFilterDto { Id = 1 };
        var filter = new VirtualCardFilter { Id = 1 };

        _mockMapper.Setup(x => x.Map<VirtualCardFilter>(filterDto)).Returns(filter);
        _mockMapper.Setup(x => x.Map<PaginationDto<VirtualCardResponseDto>>(pagination)).Returns(paginationDto);
        _mockService.Setup(x => x.GetListByFilterAsync(filter, _cancellationTokenSource.Token))
            .ReturnsAsync(pagination);
        var mockFacade = new VirtualCardFacade(_mockService.Object, _mockMapper.Object);

        // Act
        var result = await mockFacade.GetListByFilterAsync(filterDto, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(paginationDto.Count, result.Count);
        _mockService.Verify(x => x.GetListByFilterAsync(It.IsAny<VirtualCardFilter>(), _cancellationTokenSource.Token),
            Times.Once);
        _mockMapper.Verify(x => x.Map<VirtualCardFilter>(filterDto), Times.Once);
        _mockMapper.Verify(x => x.Map<PaginationDto<VirtualCardResponseDto>>(pagination), Times.Once);
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        // Arrange
        var virtualCardRequestDto = CreateVirtualCardRequestDto();
        var virtualCard = CreateVirtualCard();
        const long id = 1;

        _mockMapper.Setup(x => x.Map<VirtualCard.Domain.Entities.VirtualCard>(virtualCardRequestDto)).Returns(virtualCard);
        var mockFacade = new VirtualCardFacade(_mockService.Object, _mockMapper.Object);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            mockFacade.UpdateAsync(id, virtualCardRequestDto, _cancellationTokenSource.Token));

        // Assert
        Assert.Null(exception);
        _mockMapper.Verify(x => x.Map<VirtualCard.Domain.Entities.VirtualCard>(virtualCardRequestDto), Times.Once);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        // Arrange
        const long id = 1;
        var mockFacade = new VirtualCardFacade(_mockService.Object, _mockMapper.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => mockFacade.DeleteAsync(id, _cancellationTokenSource.Token));

        // Assert
        Assert.Null(exception);
    }

    private static PaginationDto<VirtualCardResponseDto> CreatePaginationDto(List<VirtualCardResponseDto> virtualCardsDto)
    {
        return new PaginationDto<VirtualCardResponseDto>
        {
            PageSize = 10,
            CurrentPage = 1,
            Count = 2,
            TotalPages = 1,
            Result = virtualCardsDto
        };
    }

    private static Pagination<VirtualCard.Domain.Entities.VirtualCard> CreatePagination(
        List<VirtualCard.Domain.Entities.VirtualCard> virtualCards)
    {
        return new Pagination<VirtualCard.Domain.Entities.VirtualCard>
        {
            PageSize = 10,
            CurrentPage = 1,
            Count = 2,
            Result = virtualCards
        };
    }

    private static VirtualCardRequestDto CreateVirtualCardRequestDto()
    {
        return new VirtualCardRequestDto
        {
            CardNumber = "2345-1234-2345-3456",
            Surname = "Surname",
            FirstName = "First Name"
        };
    }

    private static VirtualCard.Domain.Entities.VirtualCard CreateVirtualCard(int id = 1)
    {
        return new VirtualCard.Domain.Entities.VirtualCard
        {
            Id = id,
            CardNumber = new CardNumber("2345-1234-2345-3456"),
            Surname = "Surname",
            FirstName = "First Name"
        };
    }

    private static VirtualCardResponseDto CreateVirtualCardResponseDto(int id = 1)
    {
        return new VirtualCardResponseDto
        {
            Id = id,
            CardNumber = "2345-1234-2345-3456",
            Surname = "Surname",
            FirstName = "First Name",
            FullName = "First Name Surname"
        };
    }
}