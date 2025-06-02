using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface INotificationRepo
    {
        void Create(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
        IQueryable<Notification> GetNotificationsIQueryable();
    }
}
