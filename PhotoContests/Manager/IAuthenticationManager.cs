using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IAuthenticationManager
    {
        Task<IList<string>> Role(LogeInUserModel model);
        Task<TokenModel> SignIn(LogeInUserModel model);
        Task Register(RegisterUserModel model);
    }
}
