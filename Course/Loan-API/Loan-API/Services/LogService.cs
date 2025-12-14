using Loan_API.Data.DbContext;
using Loan_API.Domain.Log;

namespace Loan_API.Services;

    // public interface ILogService
    // {
    //     Task LogInfoAsync(string message, string? endpoint = null, string method = null, int? userId = null);
    //     Task LogErrorAsync(string message, string? endpoint = null, string method = null, int? userId = null);
    // }
    //
    //
    //     public class LogService : ILogService
    //     {
    //         private readonly AppDbContext _context;
    //
    //         public LogService(AppDbContext context)
    //         {
    //             _context = context;
    //         }
    //         public async Task LogInfoAsync(string message, string endpoint = null, string? method = null, int? userId = null)
    //         {
    //             await SaveLogAsync(message, "Info", endpoint, method, userId);
    //         }
    //
    //         public async Task LogErrorAsync(string message, string endpoint = null, string? method = null, int? userId = null)
    //         {
    //             await SaveLogAsync(message, "Error", endpoint, method, userId);
    //         }
    //
    //         private async Task SaveLogAsync(string message, string level, string? endpoint, string? method, int? userId)
    //         {
    //             var log = new Log
    //             {
    //                 Message = message,
    //                 Level = level,
    //                 Endpoint = endpoint,
    //                 Method = method,
    //                 UserId = userId
    //             };
    //
    //             _context.Logs.Add(log);
    //             await _context.SaveChangesAsync();
    //         
    // }
//}