using CF.VirtualCard.Domain.Models;

namespace CF.VirtualCard.Domain.Services.Interfaces;

public interface IVirtualCardService
{
    Task<Pagination<Entities.VirtualCard>>
        GetListByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken);
    Task<Entities.VirtualCard> GetByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken);
    Task UpdateAsync(long id, Entities.VirtualCard virtualCard, CancellationToken cancellationToken);
    Task<long> CreateAsync(Entities.VirtualCard virtualCard, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}