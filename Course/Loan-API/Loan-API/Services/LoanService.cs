using Loan_API.Data.Modeles;
using Loan_API.Data.Repositories;

namespace Loan_API.Services;

public class LoanService
{ 
    // Repositories
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;
    
    public LoanService (ILoanRepository loanRepository, IUserRepository userRepository)
    {
        _loanRepository = loanRepository;
        _userRepository = userRepository;
    }
    
    // Create a new loan
    public Loan CreateLoan(int userId, Loan loan)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (user.IsBlocked)
        {
            throw new Exception("User is blocked and cannot create loans");
        }
        
        loan.UserId = userId;
        loan.Status = Loan.LoanStatus.Pending;
        loan.CreatedAt = DateTime.UtcNow;
        loan.User = null; 
        
        _loanRepository.Add(loan);
        return loan;
    }

    public void UpdateLoan(int UserId, Loan loan, bool isAccauntant)
    {
        Loan existingLoan = _loanRepository.GetById(loan.Id);
        if (existingLoan == null)
        {
            throw new Exception("Loan not found");
        }
        if (existingLoan.UserId != UserId && !isAccauntant)
        {
            throw new Exception("Unauthorized to update this loan");
        }
        existingLoan.Amount = loan.Amount;
        existingLoan.Currency = loan.Currency;
        existingLoan.Type = loan.Type;
        
        _loanRepository.Update(existingLoan);
        
    }
    
    public void DeleteLoan(int loanId)
    {
        Loan existingLoan = _loanRepository.GetById(loanId);
        if (existingLoan == null)
        {
            throw new Exception("Loan not found");
        }
        
        _loanRepository.Delete(existingLoan);
    }
    
    public IEnumerable<Loan> GetLoans(int userId, bool isAccountant)
    {
        if (isAccountant)
        {
            return _loanRepository.GetAll();
        }
        return _loanRepository.GetByUserId(userId);
    }
    
    public Loan GetLoanById(int loanId, int userId, bool isAccountant)
    {
        Loan loan = _loanRepository.GetById(loanId);
        if (loan == null)
        {
            throw new Exception("Loan not found");
        }
        
        if (!isAccountant && loan.UserId != userId)
        {
            throw new Exception("Unauthorized to view this loan");
        }
        
        return loan;
    }
    
    
    public IEnumerable<Loan> GetLoansByUserId(int userId)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        return _loanRepository.GetByUserId(userId);
    }
    
}