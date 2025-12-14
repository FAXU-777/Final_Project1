using Loan_API.Data.Modeles;

namespace Loan_API.Data.Repositories;

public interface ILoanRepository
{
    Loan? GetById(int id);
    IEnumerable<Loan> GetByUserId(int userId);
    void Add(Loan loan);
    void Update(Loan loan);
    void Delete(Loan loan);
    IEnumerable<Loan> GetAll();

    
}