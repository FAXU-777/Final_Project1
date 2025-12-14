using Loan_API.Data.Modeles;
using Loan_API.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Validators;

[TestClass]
public class UserValidatorTests
{
    [TestMethod]
    public void Validate_ValidUser_ReturnsValid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void Validate_MissingFirstName_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("FirstName"));
        Assert.AreEqual("First name is required", result.Errors["FirstName"]);
    }

    [TestMethod]
    public void Validate_FirstNameTooShort_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "J",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("FirstName"));
    }

    [TestMethod]
    public void Validate_FirstNameTooLong_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = new string('A', 51),
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("FirstName"));
    }

    [TestMethod]
    public void Validate_MissingLastName_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("LastName"));
    }

    [TestMethod]
    public void Validate_MissingUsername_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Username"));
    }

    [TestMethod]
    public void Validate_UsernameTooShort_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "ab",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Username"));
    }

    [TestMethod]
    public void Validate_UsernameWithInvalidCharacters_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "john-doe!",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Username"));
        Assert.IsTrue(result.Errors["Username"].Contains("letters, numbers, and underscores"));
    }

    [TestMethod]
    public void Validate_ValidUsernameWithUnderscore_ReturnsValid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "john_doe123",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_MissingEmail_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Email"));
    }

    [TestMethod]
    public void Validate_InvalidEmailFormat_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "invalid-email",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Email"));
        Assert.IsTrue(result.Errors["Email"].Contains("Invalid email format"));
    }

    [TestMethod]
    public void Validate_ValidEmail_ReturnsValid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john.doe@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_AgeTooYoung_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 17
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Age"));
    }

    [TestMethod]
    public void Validate_AgeTooOld_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 121
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Age"));
    }

    [TestMethod]
    public void Validate_ValidAgeBoundary_ReturnsValid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 18
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_MissingPassword_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        string? password = null;

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Password"));
    }

    [TestMethod]
    public void Validate_PasswordTooShort_ReturnsInvalid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "12345";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.ContainsKey("Password"));
        Assert.IsTrue(result.Errors["Password"].Contains("at least 6 characters"));
    }

    [TestMethod]
    public void Validate_ValidPassword_ReturnsValid()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john@example.com",
            Age = 25
        };
        var password = "password123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var user = new User
        {
            FirstName = "J",
            LastName = "D",
            Username = "ab",
            Email = "invalid",
            Age = 15
        };
        var password = "123";

        // Act
        var result = UserValidator.Validate(user, password);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Count > 1);
    }
    
    
}



