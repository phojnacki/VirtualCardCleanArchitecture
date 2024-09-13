using CF.VirtualCard.Domain.Ddd;
using CF.VirtualCard.Domain.Exceptions;

namespace CF.VirtualCard.Domain.Entities;

public class VirtualCard : IAggregateRoot
{
    public long Id { get; set; }
    public DateTime ExpiryDate { get; set; } 
    public CardNumber CardNumber { get; set; } 
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public BillingCycle CurrentBillingCycle { get; set; }
    public bool IsActive { get; set; }

	public VirtualCard()
	{
		ExpiryDate = DateTime.Now.AddYears(1);
        IsActive = true;
	}

    public void OpenBillingCycle()
    {
        CurrentBillingCycle = new BillingCycle(Id);
    }

    public void Withdraw(decimal amount)
    {
        if (IsActive)
        {
            CurrentBillingCycle.Withdraw(amount);
        }
        else
        {
            throw new VirtualCardInactiveException("Virtual card is not active, cannot withdraw amount of money.");
        }
    }
}