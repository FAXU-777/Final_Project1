using Loan_API.Data.Modeles;
using Loan_API.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validators;

[TestClass]
public class LoanValidatorTests
{
    [TestMethod]
    public void Validate_ValidLoan_ReturnsValid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void Validate_ValidLoanWithoutCurrency_ReturnsValid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = null
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_InvalidLoanType_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = (Loan.LoanType)99, // Invalid enum value
            Amount = 50000,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Type"));
    }

    [TestMethod]
    public void Validate_ValidLoanTypes_ReturnsValid()
    {
        // Arrange & Act & Assert
        var types = new[] 
        { 
            Loan.LoanType.Personal, 
            Loan.LoanType.Mortgage, 
            Loan.LoanType.Auto, 
            Loan.LoanType.Student 
        };

        foreach (var type in types)
        {
            var loan = new Loan
            {
                Type = type,
                Amount = 50000,
                Currency = "USD"
            };

            var result = LoanValidator.Validate(loan);
            Assert.IsTrue(result.IsValid, $"Loan type {type} should be valid");
        }
    }

    [TestMethod]
    public void Validate_NegativeAmount_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = -1000,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Amount"));
        Assert.IsTrue(result.Errors["Amount"].Contains("greater than 0"));
    }

    [TestMethod]
    public void Validate_ZeroAmount_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 0,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Amount"));
    }

    [TestMethod]
    public void Validate_AmountTooSmall_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 0.001m,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Amount"));
    }

    [TestMethod]
    public void Validate_AmountTooLarge_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 1000000000m,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Amount"));
    }

    [TestMethod]
    public void Validate_ValidAmountBoundary_Minimum_ReturnsValid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 0.01m,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_ValidAmountBoundary_Maximum_ReturnsValid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 999999999.99m,
            Currency = "USD"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_CurrencyLowercase_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "usd"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Currency"));
        Assert.IsTrue(result.Errors["Currency"].Contains("uppercase"));
    }

    [TestMethod]
    public void Validate_CurrencyWrongLength_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "US"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Currency"));
        Assert.IsTrue(result.Errors["Currency"].Contains("exactly 3 characters"));
    }

    [TestMethod]
    public void Validate_CurrencyWithNumbers_ReturnsInvalid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "123"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Currency"));
    }

    [TestMethod]
    public void Validate_ValidCurrencies_ReturnsValid()
    {
        // Arrange & Act & Assert
        var currencies = new[] { "USD", "EUR", "GBP", "JPY", "CAD" };

        foreach (var currency in currencies)
        {
            var loan = new Loan
            {
                Type = Loan.LoanType.Personal,
                Amount = 50000,
                Currency = currency
            };

            var result = LoanValidator.Validate(loan);
            Assert.IsTrue(result.IsValid, $"Currency {currency} should be valid");
        }
    }

    [TestMethod]
    public void Validate_EmptyCurrency_ReturnsValid()
    {
        // Arrange
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = ""
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var loan = new Loan
        {
            Type = (Loan.LoanType)99,
            Amount = -1000,
            Currency = "usd"
        };

        // Act
        var result = LoanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Count > 1);
    }
}



