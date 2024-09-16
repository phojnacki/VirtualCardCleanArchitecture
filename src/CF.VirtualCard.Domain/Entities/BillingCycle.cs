using CF.VirtualCard.Domain.Ddd;
using CF.VirtualCard.Domain.Exceptions;

namespace CF.VirtualCard.Domain.Entities;

public class BillingCycle
{
	private static readonly int WithdrawalsLimit = 10;
	private static readonly decimal CreditLimit = 700;
	private static readonly decimal InterestRate = 0.10m;

	public long Id { get; private set; }
	public DateTime From { get; private set; }
	public DateTime To { get; private set; }
	public long VirtualCardId { get; private set; }
	public decimal CurrentBalance { get; private set; }
	public int WithdrawalsCount { get; private set; }
	public decimal Interest { get; private set; }
	

	public BillingCycle(long virtualCardId)
	{
		From = DateTime.Now;
		To = DateTime.Now.AddMonths(1);
		VirtualCardId = virtualCardId;
		CurrentBalance = 0;
		WithdrawalsCount = 0;
	}

	public void Withdraw(decimal amount)
	{
		if (amount <= 0)
			throw new ValidationException("Withdrawal amount must be greater than zero.");

		if (CurrentBalance - amount < -1 * CreditLimit)
			throw new WithdrawalLimitExceededException("Withdrawal exceeds billing cycle withdrawal limit.");

		if (WithdrawalsCount > WithdrawalsLimit)
			throw new WithdrawalCountExceededException("Withdrawals count exceeds billing cycle withdrawals count limit.");

		CurrentBalance -= amount;
		WithdrawalsCount++;
		Interest += amount * InterestRate;
	}

	public void Deposit(decimal amount)
	{
		if (amount <= 0)
			throw new ValidationException("Withdrawal amount must be greater than zero.");
		
		CurrentBalance += amount;
	}
}