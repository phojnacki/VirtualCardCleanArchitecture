namespace CF.VirtualCard.Domain.Exceptions;
public class CardNumberInvalidException(string message) : ValidationException(message)
{
}

