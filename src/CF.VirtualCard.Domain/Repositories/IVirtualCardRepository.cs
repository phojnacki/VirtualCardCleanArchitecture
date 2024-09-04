using CF.VirtualCard.Domain.Models;

namespace CF.VirtualCard.Domain.Repositories;

public interface IVirtualCardRepository : IRepositoryBase<Entities.VirtualCard>
{
    Task<int> CountByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken);
    Task<Entities.VirtualCard> GetByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken);
    Task<List<Entities.VirtualCard>> GetListByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken);
}