using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Loan_API.Data.Enums;

namespace Loan_API.Data.Modeles;

public class Loan 
{ 
    [JsonIgnore]
    public int Id { get; set; }
    
    [JsonIgnore]
    public int UserId { get; set; }
    
    public LoanType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    
    [JsonIgnore]
    public LoanStatus Status { get; set; }
    
    [JsonIgnore]
    public DateTime CreatedAt { get; set; }
    
    [ForeignKey("UserId")]
    [JsonIgnore]
    public User? User { get; set; }
    
    
    
    
    public enum LoanType
    {
        Personal,
        Mortgage,
        Auto,
        Student

    }
    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected,
        PaidOff
    }
    
}