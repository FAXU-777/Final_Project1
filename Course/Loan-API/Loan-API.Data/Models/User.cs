using System.Text.Json.Serialization;

namespace Loan_API.Data.Modeles;

public class User
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? Username { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    
    [JsonIgnore]
    public string? PasswordHash { get; set; }
    
    [JsonIgnore]
    public bool IsBlocked { get; set; }
    [JsonIgnore]
    public string? Role { get; set; }
    [JsonIgnore]
    
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    
}