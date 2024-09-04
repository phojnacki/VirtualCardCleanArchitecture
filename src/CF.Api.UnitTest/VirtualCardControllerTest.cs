using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CF.Api.Controllers;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Application.Facades.Interfaces;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CF.Api.UnitTest;

public class VirtualCardControllerTest
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Mock<ICorrelationContextAccessor> _correlationContext = new();
    private readonly Mock<IVirtualCardFacade> _virtualCardFacade = new();
    private readonly Mock<ILogger<VirtualCardController>> _logger = new();

    public VirtualCardControllerTest()
    {
        _correlationContext.Setup(x => x.CorrelationContext)
            .Returns(new CorrelationContext(Guid.NewGuid().ToString(), "HeaderValue"));
    }

    [Fact]
    public async Task GetListTestAsync()
    {
        //Arrange
        var facadeResult = new PaginationDto<VirtualCardResponseDto>
        {
            Count = 2,
            Result =
            [
                new VirtualCardResponseDto
                {
                    Email = "tarnished@test.com",
                    FirstName = "Elden",
                    Surname = "Ring",
                    FullName = "Elden Ring",
                    Id = 1
                },
                new VirtualCardResponseDto
                {
                    Email = "nameless_king@test.com",
                    FirstName = "Elden",
                    Surname = "King",
                    FullName = "Elden King",
                    Id = 2
                }
            ]
        };

        _virtualCardFacade
            .Setup(x => x.GetListByFilterAsync(It.IsAny<VirtualCardFilterDto>(), _cancellationTokenSource.Token))
            .ReturnsAsync(facadeResult);

        var controller = new VirtualCardController(_correlationContext.Object, _logger.Object, _virtualCardFacade.Object);

        var requestDto = new VirtualCardFilterDto
        {
            FirstName = "Elden"
        };

        //Act
        var actionResult = await controller.Get(requestDto, _cancellationTokenSource.Token);

        //Assert
        Assert.NotNull(actionResult);
        Assert.Equal(2, actionResult.Value?.Count);
        Assert.Equal(2, actionResult.Value?.Result.Count(x => x.FirstName == "Elden"));
    }

    [Fact]
    public async Task GetByIdTestAsync()
    {
        //Arrange
        var facadeResult = new VirtualCardResponseDto
        {
            Email = "tarnished@test.com",
            FirstName = "Elden",
            Surname = "Ring",
            FullName = "Elden Ring",
            Id = 1
        };

        _virtualCardFacade.Setup(x => x.GetByFilterAsync(It.IsAny<VirtualCardFilterDto>(), _cancellationTokenSource.Token))
            .ReturnsAsync(facadeResult);

        var controller = new VirtualCardController(_correlationContext.Object, _logger.Object, _virtualCardFacade.Object);

        //Act
        var actionResult = await controller.Get(1, _cancellationTokenSource.Token);

        //Assert
        Assert.NotNull(actionResult);
        Assert.Equal(1, actionResult.Value?.Id);
        Assert.Equal("tarnished@test.com", actionResult.Value?.Email);
    }

    [Fact]
    public async Task PostTestAsync()
    {
        //Arrange
        _virtualCardFacade.Setup(x => x.CreateAsync(It.IsAny<VirtualCardRequestDto>(), _cancellationTokenSource.Token))
            .ReturnsAsync(1);

        var controller = new VirtualCardController(_correlationContext.Object, _logger.Object, _virtualCardFacade.Object);

        var requestDto = new VirtualCardRequestDto
        {
            ConfirmPassword = "123DarkSouls!",
            Password = "123DarkSouls!",
            Email = "chosen_one@test.com",
            FirstName = "Dark",
            Surname = "Souls"
        };

        //Act
        var actionResult = await controller.Post(requestDto, _cancellationTokenSource.Token);

        //Assert
        Assert.NotNull(actionResult);
    }

    [Fact]
    public async Task PutTestAsync()
    {
        //Arrange
        _virtualCardFacade.Setup(x =>
            x.UpdateAsync(It.IsAny<long>(), It.IsAny<VirtualCardRequestDto>(), _cancellationTokenSource.Token));

        var controller = new VirtualCardController(_correlationContext.Object, _logger.Object, _virtualCardFacade.Object);

        var requestDto = new VirtualCardRequestDto
        {
            ConfirmPassword = "123DarkSouls!",
            Password = "123DarkSouls!",
            Email = "chosen_one@test.com",
            FirstName = "Dark",
            Surname = "Souls"
        };

        //Act
        var actionResult = await controller.Put(1, requestDto, _cancellationTokenSource.Token);

        //Assert
        Assert.NotNull(actionResult);
    }

    [Fact]
    public async Task DeleteTestAsync()
    {
        //Arrange
        _virtualCardFacade.Setup(x => x.DeleteAsync(It.IsAny<long>(), _cancellationTokenSource.Token));

        var controller = new VirtualCardController(_correlationContext.Object, _logger.Object, _virtualCardFacade.Object);

        //Act
        var actionResult = await controller.Delete(1, _cancellationTokenSource.Token);

        //Assert
        Assert.NotNull(actionResult);
    }
}