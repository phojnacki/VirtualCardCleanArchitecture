using System.Text.RegularExpressions;
using CF.VirtualCard.Domain.Exceptions;

namespace CF.VirtualCard.Domain.Entities;

public static partial class VirtualCardExtensions
{
    public static string GetFullName(this VirtualCard virtualCard)
    {
        return $"{virtualCard.FirstName} {virtualCard.Surname}";
    }

    public static void SetExpiryDate(this VirtualCard virtualCard)
    {
        virtualCard.ExpiryDate = DateTime.Now.AddYears(1);
    }

    public static void ValidateCardNumber(this VirtualCard virtualCard)
    {
        if (string.IsNullOrEmpty(virtualCard.CardNumber))
            throw new ValidationException("The CardNumber is required.");

        if (!CardNumberValidatorRegex().IsMatch(virtualCard.CardNumber))
            throw new ValidationException("The CardNumber contains not valid digits separated by spaces and dashes.");
    }

    public static void ValidateSurname(this VirtualCard virtualCard)
    {
        if (string.IsNullOrEmpty(virtualCard.Surname))
            throw new ValidationException("The Surname is required.");

        if (virtualCard.Surname.Length is < 2 or > 100)
            throw new ValidationException(
                "The Surname must be a string with a minimum length of 2 and a maximum length of 100.");
    }

    public static void ValidateFirstName(this VirtualCard virtualCard)
    {
        if (string.IsNullOrEmpty(virtualCard.FirstName))
            throw new ValidationException("The First Name is required.");

        if (virtualCard.FirstName.Length is < 2 or > 100)
            throw new ValidationException(
                "The First Name must be a string with a minimum length of 2 and a maximum length of 100.");
    }

    [GeneratedRegex(@"^(?:\d[ -]*?){13,19}$")]
    private static partial Regex CardNumberValidatorRegex();
}