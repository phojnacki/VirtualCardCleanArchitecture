namespace CF.VirtualCard.Domain.Entities;

public class BillingCycle
{
	public long Id { get; set; }
	public DateTime From { get; set; }
	public DateTime To { get; set; }
	public long VirtualCardId { get; set; }
	public decimal CreditLimit { get; set; }
	public decimal CreditBalance { get; set; }
	public int WithdrawalsLimit { get; set; }
	public int WithdrawalsCount { get; set; }

	public void Withdraw(decimal amount)
	{
		if (amount <= 0)
			throw new InvalidOperationException("Withdrawal amount must be greater than zero.");

		if (CreditBalance + amount > CreditLimit)
			throw new InvalidOperationException("Withdrawal exceeds billing cycle limit.");

		if (WithdrawalsCount > WithdrawalsLimit)
			throw new InvalidOperationException("Withdrawals count exceeds billing cycle withdrawals count limit.");

		CreditBalance += amount;
		WithdrawalsCount++;
	}

}