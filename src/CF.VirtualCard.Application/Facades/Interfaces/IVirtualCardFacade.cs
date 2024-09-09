using CF.VirtualCard.Application.Dtos;

namespace CF.VirtualCard.Application.Facades.Interfaces;

public interface IVirtualCardFacade
{
    Task<VirtualCardResponseDto> GetByFilterAsync(VirtualCardFilterDto filterDto, CancellationToken cancellationToken);
    Task<PaginationDto<VirtualCardResponseDto>> GetListByFilterAsync(VirtualCardFilterDto filterDto,
        CancellationToken cancellationToken);
    Task<long> CreateAsync(VirtualCardRequestDto virtualCardRequestDto, CancellationToken cancellationToken);
    Task UpdateAsync(long id, VirtualCardRequestDto virtualCardRequestDto, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    Task WithdrawAsync(long id, decimal amount, CancellationToken cancellationToken);
}