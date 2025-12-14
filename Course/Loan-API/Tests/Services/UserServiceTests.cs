using Loan_API.Data.Modeles;
using Loan_API.Data.Repositories;
using Loan_API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Services;

[TestClass]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private UserService _userService;

    [TestInitialize]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUserRepository.Object);
    }

    [TestMethod]
    public void Register_ValidUser_ReturnsUser()
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

        _mockUserRepository.Setup(r => r.GetByUserName("johndoe")).Returns((User?)null);
        _mockUserRepository.Setup(r => r.Add(It.IsAny<User>()));

        // Act
        var result = _userService.Register(user, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("User", result.Role);
        Assert.IsFalse(result.IsBlocked);
        Assert.IsNotNull(result.PasswordHash);
        _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Register_DuplicateUsername_ThrowsException()
    {
        // Arrange
        var user = new User
        {
            Username = "johndoe"
        };
        var existingUser = new User { Username = "johndoe" };

        _mockUserRepository.Setup(r => r.GetByUserName("johndoe")).Returns(existingUser);

        // Act
        _userService.Register(user, "password123");
    }

    [TestMethod]
    public void Authenticate_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var username = "johndoe";
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Id = 1,
            Username = username,
            PasswordHash = hashedPassword,
            IsBlocked = false
        };

        _mockUserRepository.Setup(r => r.GetByUserName(username)).Returns(user);

        // Act
        var result = _userService.Authenticate(username, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(username, result.Username);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Authenticate_InvalidUsername_ThrowsException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetByUserName("wronguser")).Returns((User?)null);

        // Act
        _userService.Authenticate("wronguser", "password123");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Authenticate_InvalidPassword_ThrowsException()
    {
        // Arrange
        var username = "johndoe";
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            IsBlocked = false
        };

        _mockUserRepository.Setup(r => r.GetByUserName(username)).Returns(user);

        // Act
        _userService.Authenticate(username, "wrongpassword");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Authenticate_BlockedUser_ThrowsException()
    {
        // Arrange
        var username = "johndoe";
        var password = "password123";
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsBlocked = true
        };

        _mockUserRepository.Setup(r => r.GetByUserName(username)).Returns(user);

        // Act
        _userService.Authenticate(username, password);
    }

    [TestMethod]
    public void BlockUser_ValidUser_BlocksUser()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Username = "johndoe",
            IsBlocked = false
        };

        _mockUserRepository.Setup(r => r.GetById(userId)).Returns(user);
        _mockUserRepository.Setup(r => r.Update(It.IsAny<User>()));

        // Act
        _userService.BlockUser(userId, true);

        // Assert
        Assert.IsTrue(user.IsBlocked);
        _mockUserRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void BlockUser_UserNotFound_ThrowsException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetById(999)).Returns((User?)null);

        // Act
        _userService.BlockUser(999, true);
    }

    [TestMethod]
    public void GetAllUsers_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "user1" },
            new User { Id = 2, Username = "user2" }
        };

        _mockUserRepository.Setup(r => r.GetAll()).Returns(users);

        // Act
        var result = _userService.GetAllUsers();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void CreateAccountant_ValidUser_ReturnsAccountant()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Jane",
            LastName = "Accountant",
            Username = "jane_accountant",
            Email = "jane@example.com",
            Age = 30
        };
        var password = "password123";

        _mockUserRepository.Setup(r => r.GetByUserName("jane_accountant")).Returns((User?)null);
        _mockUserRepository.Setup(r => r.Add(It.IsAny<User>()));

        // Act
        var result = _userService.CreateAccountant(user, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Accountant", result.Role);
        Assert.IsFalse(result.IsBlocked);
        Assert.IsNotNull(result.PasswordHash);
        _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    }
}



