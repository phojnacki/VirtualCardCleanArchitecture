using CF.VirtualCard.Domain.Ddd;

namespace CF.VirtualCard.Domain.Entities;

public class VirtualCard : IAggregateRoot
{
    public long Id { get; set; }
    public DateTime ExpiryDate { get; set; } 
    public CardNumber CardNumber { get; set; } 
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public BillingCycle CurrentBillingCycle { get; set; }

	public VirtualCard()
	{
		ExpiryDate = DateTime.Now.AddYears(1);
	}

    public void OpenBillingCycle()
    {
        CurrentBillingCycle = new BillingCycle(Id);
    }
}