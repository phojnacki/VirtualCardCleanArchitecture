using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Exceptions;
using Xunit;

namespace CF.VirtualCard.UnitTest.Domain.Entities;

public class VirtualCardTest
{

    [Theory]
    [InlineData("1894@")]
    [InlineData("aaa@com.   ")]
    [InlineData("aaa@@.com   ")]
    [InlineData("aaa   @gmail.com")]
    [InlineData("aaa   @gmail")]
    [InlineData("aaa@gmail.com   ")]
    [InlineData("@gmail.com")]
    public void InvalidEmailFormatTest(string email)
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Email = email
        };

        const string invalidEmailFormatErrorMessage = "The Email is not a valid e-mail address.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateEmail);

        //Assert
        Assert.Equal(invalidEmailFormatErrorMessage, exception.Message);
    }

    [Fact]
    public void InvalidEmailRequiredTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Email = string.Empty
        };

        const string invalidEmailFormatErrorMessage = "The Email is required.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateEmail);

        //Assert
        Assert.Equal(invalidEmailFormatErrorMessage, exception.Message);
    }

    [Fact]
    public void ValidEmailTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Email = "valdivia@gmail.com"
        };

        //Act
        var exception = Record.Exception(virtualCard.ValidateEmail);

        //Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaawwwwwwwwwwwwwwwwwwwwwwwwwwwwwwewewe")]
    public void InvalidFirstNameTest(string firstName)
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            FirstName = firstName
        };

        const string invalidFirstName =
            "The First Name must be a string with a minimum length of 2 and a maximum length of 100.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateFirstName);

        //Assert
        Assert.Equal(invalidFirstName, exception.Message);
    }

    [Fact]
    public void ValidFirstNameTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            FirstName = "Valdivia"
        };

        //Act
        var exception = Record.Exception(virtualCard.ValidateFirstName);

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void InvalidFirstNameRequiredTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            FirstName = string.Empty
        };

        const string invalidEmailFormatErrorMessage = "The First Name is required.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateFirstName);

        //Assert
        Assert.Equal(invalidEmailFormatErrorMessage, exception.Message);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaawwwwwwwwwwwwwwwwwwwwwwwwwwwwwwewewe")]
    public void InvalidSurnameTest(string surname)
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Surname = surname
        };

        const string invalidSurname =
            "The Surname must be a string with a minimum length of 2 and a maximum length of 100.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateSurname);

        //Assert
        Assert.Equal(invalidSurname, exception.Message);
    }

    [Fact]
    public void ValidSurnameTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Surname = "Valdivia"
        };

        //Act
        var exception = Record.Exception(virtualCard.ValidateSurname);

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void InvalidSurnameRequiredTest()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            Surname = string.Empty
        };

        const string invalidEmailFormatErrorMessage = "The Surname is required.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateSurname);

        //Assert
        Assert.Equal(invalidEmailFormatErrorMessage, exception.Message);
    }

    [Fact]
    public void GetFullName()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard
        {
            FirstName = "Valdivia",
            Surname = "El Mago"
        };

        //Act
        var result = virtualCard.GetFullName();

        //Assert
        Assert.Equal("Valdivia El Mago", result);
    }

    [Fact]
    public void SetCreatedDate()
    {
        //Arrange
        var virtualCard = new VirtualCard.Domain.Entities.VirtualCard();
        var actualDate = DateTime.Now;

        //Act
        virtualCard.SetCreatedDate();

        //Assert
        Assert.True(virtualCard.Created >= actualDate);
    }

}