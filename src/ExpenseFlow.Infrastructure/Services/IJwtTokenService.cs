namespace ExpenseFlow.Application.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(string userId, string email, IEnumerable<string> roles);
    }
}