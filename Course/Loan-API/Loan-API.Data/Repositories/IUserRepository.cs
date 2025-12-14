using Loan_API.Data.Modeles;

namespace Loan_API.Data.Repositories;

public interface IUserRepository
{
    User? GetById(int id);
    User? GetByUserName(string username);
    public void Add (User user);
    public void Update (User user);
    public void Delete (User user);


    IEnumerable<User> GetAll();
}