namespace CF.VirtualCard.Domain.Exceptions;
public class WithdrawalLimitExceededException(string message) : ValidationException(message)
{
}

