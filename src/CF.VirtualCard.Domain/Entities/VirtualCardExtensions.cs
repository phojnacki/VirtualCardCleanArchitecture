using System.Text.RegularExpressions;
using CF.VirtualCard.Domain.Exceptions;

namespace CF.VirtualCard.Domain.Entities;

public static partial class VirtualCardExtensions
{
    public static string GetFullName(this VirtualCard virtualCard)
    {
        return $"{virtualCard.FirstName} {virtualCard.Surname}";
    }

    public static void SetCreatedDate(this VirtualCard virtualCard)
    {
        virtualCard.Created = DateTime.Now;
    }

    public static void SetUpdatedDate(this VirtualCard virtualCard)
    {
        virtualCard.Updated = DateTime.Now;
    }

    public static void ValidatePassword(this VirtualCard virtualCard)
    {
        if (string.IsNullOrEmpty(virtualCard.Password))
            throw new ValidationException("The Password is required.");

        const string regex =
            @"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$";

        if (!Regex.IsMatch(virtualCard.Password, regex))
            throw new ValidationException(
                "Password must be at least 8 characters and contain at 3 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*).");
    }

    public static void ValidateEmail(this VirtualCard virtualCard)
    {
        if (string.IsNullOrEmpty(virtualCard.Email))
            throw new ValidationException("The Email is required.");

        if (!EmailValidatorRegex().IsMatch(virtualCard.Email))
            throw new ValidationException("The Email is not a valid e-mail address.");
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

    [GeneratedRegex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmailValidatorRegex();
}