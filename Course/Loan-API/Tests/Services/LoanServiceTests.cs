using Loan_API.Data.Modeles;
using Loan_API.Data.Repositories;
using Loan_API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Services;

[TestClass]
public class LoanServiceTests
{
    private Mock<ILoanRepository> _mockLoanRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private LoanService _loanService;

    [TestInitialize]
    public void Setup()
    {
        _mockLoanRepository = new Mock<ILoanRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _loanService = new LoanService(_mockLoanRepository.Object, _mockUserRepository.Object);
    }

    [TestMethod]
    public void CreateLoan_ValidLoan_ReturnsLoan()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Username = "johndoe",
            IsBlocked = false
        };
        var loan = new Loan
        {
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "USD"
        };

        _mockUserRepository.Setup(r => r.GetById(userId)).Returns(user);
        _mockLoanRepository.Setup(r => r.Add(It.IsAny<Loan>()));

        // Act
        var result = _loanService.CreateLoan(userId, loan);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
        Assert.AreEqual(Loan.LoanStatus.Pending, result.Status);
        Assert.IsTrue(result.CreatedAt <= DateTime.UtcNow);
        _mockLoanRepository.Verify(r => r.Add(It.IsAny<Loan>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateLoan_UserNotFound_ThrowsException()
    {
        // Arrange
        var userId = 999;
        var loan = new Loan { Type = Loan.LoanType.Personal, Amount = 50000 };

        _mockUserRepository.Setup(r => r.GetById(userId)).Returns((User?)null);

        // Act
        _loanService.CreateLoan(userId, loan);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateLoan_BlockedUser_ThrowsException()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            IsBlocked = true
        };
        var loan = new Loan { Type = Loan.LoanType.Personal, Amount = 50000 };

        _mockUserRepository.Setup(r => r.GetById(userId)).Returns(user);

        // Act
        _loanService.CreateLoan(userId, loan);
    }

    [TestMethod]
    public void UpdateLoan_OwnLoan_UpdatesLoan()
    {
        // Arrange
        var userId = 1;
        var loanId = 1;
        var existingLoan = new Loan
        {
            Id = loanId,
            UserId = userId,
            Type = Loan.LoanType.Personal,
            Amount = 50000,
            Currency = "USD"
        };
        var updatedLoan = new Loan
        {
            Id = loanId,
            Type = Loan.LoanType.Mortgage,
            Amount = 150000,
            Currency = "EUR"
        };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(existingLoan);
        _mockLoanRepository.Setup(r => r.Update(It.IsAny<Loan>()));

        // Act
        _loanService.UpdateLoan(userId, updatedLoan, false);

        // Assert
        Assert.AreEqual(Loan.LoanType.Mortgage, existingLoan.Type);
        Assert.AreEqual(150000, existingLoan.Amount);
        Assert.AreEqual("EUR", existingLoan.Currency);
        _mockLoanRepository.Verify(r => r.Update(It.IsAny<Loan>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void UpdateLoan_SomeoneElseLoan_ThrowsException()
    {
        // Arrange
        var userId = 1;
        var loanId = 1;
        var existingLoan = new Loan
        {
            Id = loanId,
            UserId = 2, // Different user
            Type = Loan.LoanType.Personal,
            Amount = 50000
        };
        var updatedLoan = new Loan { Id = loanId, Type = Loan.LoanType.Mortgage, Amount = 150000 };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(existingLoan);

        // Act
        _loanService.UpdateLoan(userId, updatedLoan, false);
    }

    [TestMethod]
    public void UpdateLoan_AccountantCanUpdateAnyLoan_UpdatesLoan()
    {
        // Arrange
        var userId = 1;
        var loanId = 1;
        var existingLoan = new Loan
        {
            Id = loanId,
            UserId = 2, // Different user
            Type = Loan.LoanType.Personal,
            Amount = 50000
        };
        var updatedLoan = new Loan { Id = loanId, Type = Loan.LoanType.Mortgage, Amount = 150000 };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(existingLoan);
        _mockLoanRepository.Setup(r => r.Update(It.IsAny<Loan>()));

        // Act
        _loanService.UpdateLoan(userId, updatedLoan, true); // isAccountant = true

        // Assert
        _mockLoanRepository.Verify(r => r.Update(It.IsAny<Loan>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void UpdateLoan_LoanNotFound_ThrowsException()
    {
        // Arrange
        var userId = 1;
        var loan = new Loan { Id = 999, Type = Loan.LoanType.Personal, Amount = 50000 };

        _mockLoanRepository.Setup(r => r.GetById(999)).Returns((Loan?)null);

        // Act
        _loanService.UpdateLoan(userId, loan, false);
    }

    [TestMethod]
    public void DeleteLoan_ValidLoan_DeletesLoan()
    {
        // Arrange
        var loanId = 1;
        var loan = new Loan { Id = loanId, UserId = 1, Amount = 50000 };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(loan);
        _mockLoanRepository.Setup(r => r.Delete(It.IsAny<Loan>()));

        // Act
        _loanService.DeleteLoan(loanId);

        // Assert
        _mockLoanRepository.Verify(r => r.Delete(It.IsAny<Loan>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteLoan_LoanNotFound_ThrowsException()
    {
        // Arrange
        _mockLoanRepository.Setup(r => r.GetById(999)).Returns((Loan?)null);

        // Act
        _loanService.DeleteLoan(999);
    }

    [TestMethod]
    public void GetLoans_UserGetsOwnLoans_ReturnsUserLoans()
    {
        // Arrange
        var userId = 1;
        var loans = new List<Loan>
        {
            new Loan { Id = 1, UserId = userId, Amount = 50000 },
            new Loan { Id = 2, UserId = userId, Amount = 100000 }
        };

        _mockLoanRepository.Setup(r => r.GetByUserId(userId)).Returns(loans);

        // Act
        var result = _loanService.GetLoans(userId, false);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(l => l.UserId == userId));
    }

    [TestMethod]
    public void GetLoans_AccountantGetsAllLoans_ReturnsAllLoans()
    {
        // Arrange
        var userId = 1;
        var allLoans = new List<Loan>
        {
            new Loan { Id = 1, UserId = 1, Amount = 50000 },
            new Loan { Id = 2, UserId = 2, Amount = 100000 },
            new Loan { Id = 3, UserId = 3, Amount = 75000 }
        };

        _mockLoanRepository.Setup(r => r.GetAll()).Returns(allLoans);

        // Act
        var result = _loanService.GetLoans(userId, true); // isAccountant = true

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public void GetLoanById_OwnLoan_ReturnsLoan()
    {
        // Arrange
        var loanId = 1;
        var userId = 1;
        var loan = new Loan { Id = loanId, UserId = userId, Amount = 50000 };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(loan);

        // Act
        var result = _loanService.GetLoanById(loanId, userId, false);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(loanId, result.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetLoanById_SomeoneElseLoan_ThrowsException()
    {
        // Arrange
        var loanId = 1;
        var userId = 1;
        var loan = new Loan { Id = loanId, UserId = 2, Amount = 50000 }; // Different user

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(loan);

        // Act
        _loanService.GetLoanById(loanId, userId, false);
    }

    [TestMethod]
    public void GetLoanById_AccountantCanGetAnyLoan_ReturnsLoan()
    {
        // Arrange
        var loanId = 1;
        var userId = 1;
        var loan = new Loan { Id = loanId, UserId = 2, Amount = 50000 };

        _mockLoanRepository.Setup(r => r.GetById(loanId)).Returns(loan);

        // Act
        var result = _loanService.GetLoanById(loanId, userId, true); // isAccountant = true

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(loanId, result.Id);
    }

    [TestMethod]
    public void GetLoansByUserId_ValidUserId_ReturnsUserLoans()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, Username = "johndoe" };
        var loans = new List<Loan>
        {
            new Loan { Id = 1, UserId = userId, Amount = 50000 },
            new Loan { Id = 2, UserId = userId, Amount = 100000 }
        };

        _mockUserRepository.Setup(r => r.GetById(userId)).Returns(user);
        _mockLoanRepository.Setup(r => r.GetByUserId(userId)).Returns(loans);

        // Act
        var result = _loanService.GetLoansByUserId(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetLoansByUserId_UserNotFound_ThrowsException()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetById(999)).Returns((User?)null);

        // Act
        _loanService.GetLoansByUserId(999);
    }
}



