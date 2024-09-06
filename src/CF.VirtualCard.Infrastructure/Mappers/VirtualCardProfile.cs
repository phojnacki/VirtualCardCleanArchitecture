using AutoMapper;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Models;

namespace CF.VirtualCard.Infrastructure.Mappers;

public class VirtualCardProfile : Profile
{
    public VirtualCardProfile()
    {
        CreateVirtualCardProfile();
    }

    private void CreateVirtualCardProfile()
    {
        CreateMap<VirtualCardRequestDto, Domain.Entities.VirtualCard>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiryDate, opt => opt.Ignore());

        CreateMap<Domain.Entities.VirtualCard, VirtualCardResponseDto>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .AfterMap((source, destination) => { destination.FullName = source.GetFullName(); });

        CreateMap<VirtualCardFilterDto, VirtualCardFilter>();

        CreateMap<Pagination<Domain.Entities.VirtualCard>, PaginationDto<VirtualCardResponseDto>>()
            .AfterMap((source, converted, context) =>
            {
                converted.Result = context.Mapper.Map<List<VirtualCardResponseDto>>(source.Result);
            });
    }
}