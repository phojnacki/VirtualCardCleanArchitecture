using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Domain.Repositories;
using CF.VirtualCard.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CF.VirtualCard.Infrastructure.Repositories;

public class VirtualCardRepository(VirtualCardContext context)
    : RepositoryBase<Domain.Entities.VirtualCard>(context), IVirtualCardRepository
{
    public async Task<int> CountByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken)
    {
        var query = DbContext.VirtualCards.AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.CountAsync(cancellationToken);
    }

	public override async Task<Domain.Entities.VirtualCard> GetByIdAsync(long id, CancellationToken cancellationToken)
	{
		return await DbContext.VirtualCards.Include(vc => vc.CurrentBillingCycle)
			.FirstOrDefaultAsync(vc => vc.Id == id);
	}

	public async Task<Domain.Entities.VirtualCard> GetByFilterAsync(VirtualCardFilter filter,
        CancellationToken cancellationToken)
    {
        var query = DbContext.VirtualCards.Include(vc => vc.CurrentBillingCycle).AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Domain.Entities.VirtualCard>> GetListByFilterAsync(VirtualCardFilter filter,
        CancellationToken cancellationToken)
    {
        var query = DbContext.VirtualCards.AsQueryable();

        query = ApplyFilter(filter, query);

        query = ApplySorting(filter, query);

        if (filter.CurrentPage > 0)
            query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

        return await query.ToListAsync(cancellationToken);
    }

    private static IQueryable<Domain.Entities.VirtualCard> ApplySorting(VirtualCardFilter filter,
        IQueryable<Domain.Entities.VirtualCard> query)
    {
        query = filter?.OrderBy.ToLower() switch
        {
            "firstname" => filter.SortBy.Equals("asc", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderBy(x => x.FirstName)
                : query.OrderByDescending(x => x.FirstName),
            "surname" => filter.SortBy.Equals("asc", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderBy(x => x.Surname)
                : query.OrderByDescending(x => x.Surname),
            "cardnumber" => filter.SortBy.Equals("asc", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderBy(x => x.CardNumber)
                : query.OrderByDescending(x => x.CardNumber),
            _ => query
        };

        return query;
    }

    private static IQueryable<Domain.Entities.VirtualCard> ApplyFilter(VirtualCardFilter filter,
        IQueryable<Domain.Entities.VirtualCard> query)
    {
        if (filter.Id > 0)
            query = query.Where(x => x.Id == filter.Id);

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{filter.FirstName}%"));

        if (!string.IsNullOrWhiteSpace(filter.Surname))
            query = query.Where(x => EF.Functions.Like(x.Surname, $"%{filter.Surname}%"));

        if (!string.IsNullOrWhiteSpace(filter.CardNumber))
            query = query.Where(x => x.CardNumber.Value == filter.CardNumber);

        return query;
    }
}