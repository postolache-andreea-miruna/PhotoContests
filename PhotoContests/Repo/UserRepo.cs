using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class UserRepo:IUserRepo
    {
        private PhotoContestsContext db;
        public UserRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public IQueryable<User> GetUsersIQueryable()
        {
            var users = db.Users;
            return users;
        }
    }
}
