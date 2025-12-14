using System.Text.RegularExpressions;
using Loan_API.Data.Modeles;

namespace Loan_API.Validators;

public class UserValidator
{
    public static ValidationResult Validate(User user, string? password = null)
    {
        var result = new ValidationResult { IsValid = true };
        
        // First Name validation
        if (string.IsNullOrWhiteSpace(user.FirstName))
        {
            result.Errors.Add("FirstName", "First name is required");
            result.IsValid = false;
        }
        else if (user.FirstName.Length < 2 || user.FirstName.Length > 50)
        {
            result.Errors.Add("FirstName", "First name must be between 2 and 50 characters");
            result.IsValid = false;
        }
        
        // Last Name validation
        if (string.IsNullOrWhiteSpace(user.LastName))
        {
            result.Errors.Add("LastName", "Last name is required");
            result.IsValid = false;
        }
        else if (user.LastName.Length < 2 || user.LastName.Length > 50)
        {
            result.Errors.Add("LastName", "Last name must be between 2 and 50 characters");
            result.IsValid = false;
        }
        
        // Username validation
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            result.Errors.Add("Username", "Username is required");
            result.IsValid = false;
        }
        else if (user.Username.Length < 3 || user.Username.Length > 30)
        {
            result.Errors.Add("Username", "Username must be between 3 and 30 characters");
            result.IsValid = false;
        }
        else if (!Regex.IsMatch(user.Username, @"^[a-zA-Z0-9_]+$"))
        {
            result.Errors.Add("Username", "Username can only contain letters, numbers, and underscores");
            result.IsValid = false;
        }
        
        // Email validation
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            result.Errors.Add("Email", "Email is required");
            result.IsValid = false;
        }
        else if (user.Email.Length > 100)
        {
            result.Errors.Add("Email", "Email cannot exceed 100 characters");
            result.IsValid = false;
        }
        else if (!IsValidEmail(user.Email))
        {
            result.Errors.Add("Email", "Invalid email format");
            result.IsValid = false;
        }
        
        // Age validation
        if (user.Age < 18 || user.Age > 120)
        {
            result.Errors.Add("Age", "Age must be between 18 and 120");
            result.IsValid = false;
        }
        
        // Password validation (if provided)
        if (!string.IsNullOrEmpty(password))
        {
            if (password.Length < 6)
            {
                result.Errors.Add("Password", "Password must be at least 6 characters long");
                result.IsValid = false;
            }
        }
        else if (password == null)
        {
            result.Errors.Add("Password", "Password is required");
            result.IsValid = false;
        }
        
        return result;
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }

        
    }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}
