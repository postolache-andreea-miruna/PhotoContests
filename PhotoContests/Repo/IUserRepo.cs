using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IUserRepo
    {
        IQueryable<User> GetUsersIQueryable();
    }
}
