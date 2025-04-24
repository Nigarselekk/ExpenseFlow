using System; 
using Microsoft.AspNetCore.Identity;
namespace ExpenseFlow.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{

        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 

