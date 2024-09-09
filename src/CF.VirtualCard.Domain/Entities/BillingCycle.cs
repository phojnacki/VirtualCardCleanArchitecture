using CF.VirtualCard.Domain.Ddd;

namespace CF.VirtualCard.Domain.Entities;

public class BillingCycle : IAggregateRoot
{
	public long Id { get; private set; }
	public DateTime From { get; private set; }
	public DateTime To { get; private set; }
	public long VirtualCardId { get; private set; }
	public decimal CreditLimit { get; set; }
	public decimal CurrentBalance { get; set; }
	public int WithdrawalsLimit { get; set; }
	public int WithdrawalsCount { get; set; }

	public void Withdraw(decimal amount)
	{
		if (amount <= 0)
			throw new InvalidOperationException("Withdrawal amount must be greater than zero.");

		if (CurrentBalance - amount < -1 * CreditLimit)
			throw new InvalidOperationException("Withdrawal exceeds billing cycle limit.");

		if (WithdrawalsCount > WithdrawalsLimit)
			throw new InvalidOperationException("Withdrawals count exceeds billing cycle withdrawals count limit.");

		CurrentBalance -= amount;
		WithdrawalsCount++;
	}

	public void Deposit(decimal amount)
	{
		if (amount <= 0)
			throw new InvalidOperationException("Withdrawal amount must be greater than zero.");
		
		CurrentBalance += amount;
	}
}