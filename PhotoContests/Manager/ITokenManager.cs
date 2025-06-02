using PhotoContests.Entities;

namespace PhotoContests.Manager
{
    public interface ITokenManager
    {
        bool IsTokenExpired(string token);
        Task<string> CreateToken(User user);
    }
}
