using Loan_API.Data.DbContext;
using Loan_API.Data.Modeles;

namespace Loan_API.Data.Repositories;


    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUserName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        
        User user1 = new User
        {
            Id = 1,
            Username = "john_doe",
            PasswordHash = "hashed_password",
            IsBlocked = false,
            Role = "User"
        };
        
    
        User user2 = new User
        {
            Id = 2,
            Username = "jane_smith",
            PasswordHash = "hashed_password",
            IsBlocked = false,
            Role = "Accountant"
        };
    }
