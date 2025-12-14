using Loan_API.Data.DbContext;
using Loan_API.Data.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Loan_API.Data.Repositories;

public class LoanRepository : ILoanRepository
{ 
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    // Retrieves a loan by its ID, including associated user information
    public Loan? GetById(int id)
    {
        return _context.Loans
            .Include(l => l.User)
            .FirstOrDefault(l => l.Id == id);
    }

    public IEnumerable<Loan> GetByUserId(int userId)
    {
        return _context.Loans
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .ToList();
    }

    // Retrieves all loans from the database, including associated user information
    public IEnumerable<Loan> GetAll()
    {
        return _context.Loans
            .Include(l => l.User)
            .ToList();
    }


    public void Add(Loan loan)
    {
        try
        {
            _context.Loans.Add(loan);
            _context.SaveChanges();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            throw new Exception($"Database error: {ex.Message}. Inner: {ex.InnerException?.Message}", ex);
        }
    }


    public void Update(Loan loan)
    {
        _context.Loans.Update(loan);
        _context.SaveChanges();
    }

    

    public void Delete(Loan loan)
    {
        _context.Loans.Remove(loan);
        _context.SaveChanges();
    }
}