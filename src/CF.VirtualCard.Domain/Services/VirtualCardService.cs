using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Exceptions;
using CF.VirtualCard.Domain.Models;
using CF.VirtualCard.Domain.Repositories;
using CF.VirtualCard.Domain.Services.Interfaces;

namespace CF.VirtualCard.Domain.Services;

public class VirtualCardService(IVirtualCardRepository virtualCardRepository)
    : IVirtualCardService
{
    public async Task<Pagination<Entities.VirtualCard>> GetListByFilterAsync(VirtualCardFilter filter,
        CancellationToken cancellationToken)
    {
        if (filter is null)
            throw new ValidationException("Filter is null.");

        if (filter.PageSize > 100)
            throw new ValidationException("Maximum allowed page size is 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await virtualCardRepository.CountByFilterAsync(filter, cancellationToken);

        if (total == 0) return new Pagination<Entities.VirtualCard>();

        var paginateResult = await virtualCardRepository.GetListByFilterAsync(filter, cancellationToken);

        var result = new Pagination<Entities.VirtualCard>
        {
            Count = total,
            CurrentPage = filter.CurrentPage,
            PageSize = filter.PageSize,
            Result = [.. paginateResult]
        };

        return result;
    }

    public async Task<Entities.VirtualCard> GetByFilterAsync(VirtualCardFilter filter, CancellationToken cancellationToken)
    {
        if (filter is null)
            throw new ValidationException("Filter is null.");

        return await virtualCardRepository.GetByFilterAsync(filter, cancellationToken);
    }

    public async Task UpdateAsync(long id, Entities.VirtualCard virtualCard, CancellationToken cancellationToken)
    {
        if (id <= 0) throw new ValidationException("Id is invalid.");

        if (virtualCard is null)
            throw new ValidationException("VirtualCard is null.");

        var entity = await virtualCardRepository.GetByIdAsync(id, cancellationToken) ??
                     throw new EntityNotFoundException(id);

        Validate(virtualCard);

        if (entity.Email != virtualCard.Email && !await IsAvailableEmailAsync(virtualCard.Email, cancellationToken))
            throw new ValidationException("Email is not available.");

        entity.Email = virtualCard.Email;
        entity.FirstName = virtualCard.FirstName;
        entity.Surname = virtualCard.Surname;

        await virtualCardRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<long> CreateAsync(Entities.VirtualCard virtualCard, CancellationToken cancellationToken)
    {
        if (virtualCard is null)
            throw new ValidationException("VirtualCard is null.");

        Validate(virtualCard);

        var isAvailableEmail = await IsAvailableEmailAsync(virtualCard.Email, cancellationToken);
        if (!isAvailableEmail) throw new ValidationException("Email is not available.");

        virtualCard.SetCreatedDate();
        virtualCardRepository.Add(virtualCard);
        await virtualCardRepository.SaveChangesAsync(cancellationToken);

        return virtualCard.Id;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        if (id <= 0) throw new ValidationException("Id is invalid.");

        var entity = await virtualCardRepository.GetByIdAsync(id, cancellationToken) ??
                     throw new EntityNotFoundException(id);
        virtualCardRepository.Remove(entity);
        await virtualCardRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsAvailableEmailAsync(string email, CancellationToken cancellationToken)
    {
        var filter = new VirtualCardFilter { Email = email };
        var existingEmail = await virtualCardRepository.GetByFilterAsync(filter, cancellationToken);
        return existingEmail is null;
    }

    private static void Validate(Entities.VirtualCard virtualCard)
    {
        virtualCard.ValidateFirstName();

        virtualCard.ValidateSurname();

        virtualCard.ValidateEmail();

    }
}