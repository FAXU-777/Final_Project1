using System.Text.RegularExpressions;
using Loan_API.Data.Modeles;

namespace Loan_API.Validators;

public class LoanValidator
{
    public static ValidationResult Validate(Loan loan)
    {
        var result = new ValidationResult { IsValid = true }; 
        
        // Type validation
        if (!Enum.IsDefined(typeof(Loan.LoanStatus), loan.Type))
        {
            result.Errors.Add("Type", "Invalid loan type");
            result.IsValid = false;
        }
        
        // Amount validation
        if (loan.Amount <= 0)
        {
            result.Errors.Add("Amount", "Amount must be greater than 0");
            result.IsValid = false;
        }
        else if (loan.Amount < 0.01m || loan.Amount > 999999999.99m)
        {
            result.Errors.Add("Amount", "Amount must be between 0.01 and 999,999,999.99");
            result.IsValid = false;
        }
        
        // Currency validation 
        if (!string.IsNullOrWhiteSpace(loan.Currency))
        {
            if (loan.Currency.Length != 3)
            {
                result.Errors.Add("Currency", "Currency must be exactly 3 characters (e.g., USD, EUR)");
                result.IsValid = false;
            }
            else if (!Regex.IsMatch(loan.Currency, @"^[A-Z]{3}$"))
            {
                result.Errors.Add("Currency", "Currency must be 3 uppercase letters (e.g., USD, EUR, GBP)");
                result.IsValid = false;
            }
        }
        
        return result;
    }
}
