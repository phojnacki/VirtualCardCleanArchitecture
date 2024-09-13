namespace CF.VirtualCard.Domain.Exceptions;
public class VirtualCardInactiveException(string message) : ValidationException(message)
{
}

