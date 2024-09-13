using CF.VirtualCard.Domain.Entities;
using CF.VirtualCard.Domain.Exceptions;
using Xunit;

namespace CF.VirtualCard.UnitTest.Domain.Entities;

public class VirtualCardTest
{

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("123 123 123 123")]
    [InlineData("1234 1234 1234")]
    [InlineData("2345 2354 3456 456")]
    [InlineData("3456345223413456-")]
    [InlineData(" 3456345223413456 ")]
    [InlineData("1234-2345-354-2341")]
    public void InvalidCardNumberFormatTest(string cardnumber)
    {
        //Arrange
        
        //Act
        var exception = Assert.Throws<CardNumberInvalidException>(() => 
            new VirtualCard.Domain.Entities.VirtualCard
		    {
			    CardNumber = new CardNumber(cardnumber)
		    });

        //Assert
    }

    [Fact]
    public void ValidCardNumberTest()
    {
        //Arrange


        //Act
        var exception = Record.Exception(() => new VirtualCard.Domain.Entities.VirtualCard
        {
            CardNumber = new CardNumber("3425 3642 7853 4536")
        });

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

        const string invalidCardNumberFormatErrorMessage = "The First Name is required.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateFirstName);

        //Assert
        Assert.Equal(invalidCardNumberFormatErrorMessage, exception.Message);
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

        const string invalidCardNumberFormatErrorMessage = "The Surname is required.";

        //Act
        var exception = Assert.Throws<ValidationException>(virtualCard.ValidateSurname);

        //Assert
        Assert.Equal(invalidCardNumberFormatErrorMessage, exception.Message);
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

}