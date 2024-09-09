namespace CF.VirtualCard.Domain.Exceptions;
public class WithdrawalCountExceededException(string message) : ValidationException(message)
{
}

