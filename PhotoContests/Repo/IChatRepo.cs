using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IChatRepo
    {
        void CreateMessage(Message message);
        IQueryable<Message> GetMessages();
        void Update(User user);
    }
}
