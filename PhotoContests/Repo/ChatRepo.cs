using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class ChatRepo: IChatRepo
    {
        private PhotoContestsContext db;
        public ChatRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void CreateMessage(Message message)
        {
            db.Messages.Add(message);
            db.SaveChanges();
        }

        public IQueryable<Message> GetMessages()
        {
            var messages = db.Messages;
            return messages;
        }

        public void Update(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
        }

    }
}
