namespace Loan_API.Domain.Log;


public class Log
{
    
    public int Id { get; set; }
    public string Message { get; set; } = null!;
    public string Level { get; set; } = null!;   // Info, Warning, Error
    public string Endpoint { get; set; }        // /api/loan/create
    public string Method { get; set; }          // GET, POST...
    public int? UserId { get; set; }             // optional – ვინ გააკეთა
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}