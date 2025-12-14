using System.Security.Claims;
using Loan_API.Data.Modeles;
using Loan_API.Services;
using Loan_API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan_API.Controllers;

[ApiController]
[Route("api/[controller]")]

    public class LoanController : ControllerBase
    {
        private readonly LoanService _loanService;
        
        public LoanController(LoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        [Authorize] // Only authenticated users can create loans
        public IActionResult CreateLoan([FromBody] Loan loan)
        {
            var validationResult = LoanValidator.Validate(loan);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var createdLoan = _loanService.CreateLoan(userId, loan);
                return Ok(createdLoan);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner exception: {ex.InnerException.Message}";
                }
                return BadRequest(errorMessage);
            }
        }
        
        [HttpPut("{id}")] // Update loan by ID
        [Authorize] 
        public IActionResult UpdateLoan(int id, [FromBody] Loan loan)
        {
            var validationResult = LoanValidator.Validate(loan);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
                bool isAccountant = userRole == "Accountant";
                
                loan.Id = id;
                _loanService.UpdateLoan(userId, loan, isAccountant);
                return Ok(new { message = "Loan updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")] // Delete loan by ID
        [Authorize(Roles = "Accountant")] 
        public IActionResult DeleteLoan(int id)
        {
            try
            {
                _loanService.DeleteLoan(id);
                return Ok(new { message = "Loan deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet] // Get all loans for the authenticated user
        [Authorize]
        public IActionResult GetLoans()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
                bool isAccountant = userRole == "Accountant";
                
                var loans = _loanService.GetLoans(userId, isAccountant);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]  // Get loan by ID
        [Authorize]
        public IActionResult GetLoanById(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
                bool isAccountant = userRole == "Accountant";
                
                var loan = _loanService.GetLoanById(id, userId, isAccountant);
                return Ok(loan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("user/{userId}")] // Get loans by user ID Accountant only
        [Authorize(Roles = "Accountant")]
        public IActionResult GetLoansByUserId(int userId)
        {
            try
            {
                var loans = _loanService.GetLoansByUserId(userId);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }