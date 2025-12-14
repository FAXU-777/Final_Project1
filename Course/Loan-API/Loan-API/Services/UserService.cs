using Loan_API.Data.Modeles;
using Loan_API.Data.Repositories;

namespace Loan_API.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    // Registers a new user with the given password
    public User Register(User user, string password)
    {
        User existingUser = _userRepository.GetByUserName(user.Username);
        if(existingUser != null)
        {
            throw new Exception("Username already exists");
        }
        
        user.PasswordHash= BCrypt.Net.BCrypt.HashPassword(password);
        user.IsBlocked = false;
        user.Role = "User";
        _userRepository.Add(user);
        return user;
        
    }
    
    public User Authenticate(string username, string password)
    {
        User user = _userRepository.GetByUserName(username);
        
        if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }
        
        if(user.IsBlocked)
        {
            throw new Exception("User is blocked");
        }
        
        return user;
    }

    public void BlockUser(int userId, bool block)
    {
        User user = _userRepository.GetById(userId);
        if(user == null)
        {
            throw new Exception("User not found");
        }
        
        user.IsBlocked = block;
        _userRepository.Update(user);
    }
    
    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }
    
    public User CreateAccountant(User user, string password)
    {
        User? existingUser = _userRepository.GetByUserName(user.Username);
        if(existingUser != null)
        {
            throw new Exception("Username already exists");
        }
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.IsBlocked = false;
        user.Role = "Accountant";
        _userRepository.Add(user);
        return user;
    }
    

    
}