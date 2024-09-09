using AutoMapper;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Application.Facades.Interfaces;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Domain.Services.Interfaces;

namespace CF.VirtualCard.Application.Facades;

public class VirtualCardFacade(IVirtualCardService virtualCardService, IMapper mapper) : IVirtualCardFacade
{
    public async Task<PaginationDto<VirtualCardResponseDto>> GetListByFilterAsync(VirtualCardFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var filter = mapper.Map<VirtualCardFilter>(filterDto);

        var result = await virtualCardService.GetListByFilterAsync(filter, cancellationToken);

        var paginationDto = mapper.Map<PaginationDto<VirtualCardResponseDto>>(result);

        return paginationDto;
    }

    public async Task<VirtualCardResponseDto> GetByFilterAsync(VirtualCardFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var filter = mapper.Map<VirtualCardFilter>(filterDto);

        var result = await virtualCardService.GetByFilterAsync(filter, cancellationToken);

        var resultDto = mapper.Map<VirtualCardResponseDto>(result);

        return resultDto;
    }

    public async Task UpdateAsync(long id, VirtualCardRequestDto virtualCardRequestDto, CancellationToken cancellationToken)
    {
        var virtualCard = mapper.Map<Domain.Entities.VirtualCard>(virtualCardRequestDto);

        await virtualCardService.UpdateAsync(id, virtualCard, cancellationToken);
    }

    public async Task<long> CreateAsync(VirtualCardRequestDto virtualCardRequestDto, CancellationToken cancellationToken)
    {
        var virtualCard = mapper.Map<Domain.Entities.VirtualCard>(virtualCardRequestDto);

        var id = await virtualCardService.CreateAsync(virtualCard, cancellationToken);

        return id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await virtualCardService.DeleteAsync(id, cancellationToken);
    }

	public async Task WithdrawAsync(long id, decimal amount, CancellationToken cancellationToken)
	{
		await virtualCardService.WithdrawAsync(id, amount, cancellationToken);
	}
}