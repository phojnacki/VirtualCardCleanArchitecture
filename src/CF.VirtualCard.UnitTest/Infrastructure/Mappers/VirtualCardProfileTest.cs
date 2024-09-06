using AutoMapper;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Infrastructure.Mappers;
using Xunit;

namespace CF.VirtualCard.UnitTest.Infrastructure.Mappers;

public class VirtualCardProfileTest
{
    public MapperConfiguration MapperConfiguration =
        new(cfg => cfg.AddProfile<VirtualCardProfile>());

    [Fact]
    public void VirtualCardRequestDtoToVirtualCard()
    {
        //Arrange
        var virtualCardRequestDto = new VirtualCardRequestDto
        {
            Surname = "Dickinson",
            FirstName = "Bruce",
            CardNumber = "maiden@metal.com"
        };

        var mapper = MapperConfiguration.CreateMapper();

        //Act
        var virtualCard = mapper.Map<VirtualCard.Domain.Entities.VirtualCard>(virtualCardRequestDto);

        //Assert
        Assert.Equal(virtualCardRequestDto.FirstName, virtualCard.FirstName);
        Assert.Equal(virtualCardRequestDto.Surname, virtualCard.Surname);
        Assert.Equal(virtualCardRequestDto.CardNumber, virtualCard.CardNumber);
    }

    [Fact]
    public void VirtualCardToVirtualCardResponseDto()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Surname = "Dickinson",
            FirstName = "Bruce",
            CardNumber = "maiden@metal.com",
            ExpiryDate = DateTime.Now,
            Id = 1
        };

        var mapper = MapperConfiguration.CreateMapper();

        //Act
        var virtualCardResponseDto = mapper.Map<VirtualCardResponseDto>(virtualCard);

        //Arrange
        Assert.Equal(virtualCard.FirstName, virtualCardResponseDto.FirstName);
        Assert.Equal(virtualCard.Surname, virtualCardResponseDto.Surname);
        Assert.Equal(virtualCard.CardNumber, virtualCardResponseDto.CardNumber);
        Assert.Equal(virtualCard.Id, virtualCardResponseDto.Id);
        Assert.Equal(virtualCard.GetFullName(), virtualCardResponseDto.FullName);
    }

    [Fact]
    public void VirtualCardPaginationToVirtualCardResponseDtoPagination()
    {
        //Arrange
        var virtualCardList = new List<VirtualCard.Domain.Entities.VirtualCard>
        {
            new()
            {
                Surname = "Dickinson",
                FirstName = "Bruce",
                CardNumber = "maiden@metal.com",
                ExpiryDate = DateTime.Now,
                Id = 1
            }
        };

        var virtualCardPagination = new Pagination<VirtualCard.Domain.Entities.VirtualCard>
        {
            Count = 1,
            CurrentPage = 1,
            PageSize = 1,
            Result = virtualCardList
        };

        var mapper = MapperConfiguration.CreateMapper();

        //Act
        var virtualCardResponseDtoPagination = mapper.Map<PaginationDto<VirtualCardResponseDto>>(virtualCardPagination);
        var virtualCardResponseDtoList = mapper.Map<List<VirtualCardResponseDto>>(virtualCardResponseDtoPagination.Result);

        //Assert
        Assert.Equal(virtualCardList.First().FirstName, virtualCardResponseDtoList.First().FirstName);
        Assert.Equal(virtualCardList.First().Surname, virtualCardResponseDtoList.First().Surname);
        Assert.Equal(virtualCardList.First().CardNumber, virtualCardResponseDtoList.First().CardNumber);
        Assert.Equal(virtualCardList.First().Id, virtualCardResponseDtoList.First().Id);
        Assert.Equal(virtualCardList.First().GetFullName(), virtualCardResponseDtoList.First().FullName);
        Assert.Equal(1, virtualCardResponseDtoPagination.TotalPages);
    }

    [Fact]
    public void VirtualCardFilterToVirtualCardFilterDto()
    {
        //Arrange
        var virtualCardFilterDto = new VirtualCardFilterDto
        {
            Surname = "Dickinson",
            FirstName = "Bruce",
            CardNumber = "maiden@metal.com",
            Id = 1,
            CurrentPage = 1,
            PageSize = 1,
            OrderBy = "desc",
            SortBy = "firstName"
        };

        var mapper = MapperConfiguration.CreateMapper();

        //Act
        var virtualCardFilter = mapper.Map<VirtualCardFilter>(virtualCardFilterDto);

        //Assert
        Assert.Equal(virtualCardFilterDto.FirstName, virtualCardFilter.FirstName);
        Assert.Equal(virtualCardFilterDto.Surname, virtualCardFilter.Surname);
        Assert.Equal(virtualCardFilterDto.Id, virtualCardFilter.Id);
        Assert.Equal(virtualCardFilterDto.CardNumber, virtualCardFilter.CardNumber);
        Assert.Equal(virtualCardFilterDto.CurrentPage, virtualCardFilter.CurrentPage);
        Assert.Equal(virtualCardFilterDto.OrderBy, virtualCardFilter.OrderBy);
        Assert.Equal(virtualCardFilterDto.PageSize, virtualCardFilter.PageSize);
        Assert.Equal(virtualCardFilterDto.SortBy, virtualCardFilter.SortBy);
    }
}