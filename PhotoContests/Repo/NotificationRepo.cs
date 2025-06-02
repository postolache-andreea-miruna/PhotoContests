using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class NotificationRepo:INotificationRepo
    {
        private PhotoContestsContext db;
        public NotificationRepo(PhotoContestsContext db)
        {
            this.db = db;
        }
        public void Create(Notification notification)
        {
            db.Notifications.Add(notification);
            db.SaveChanges();
        }
        public void Update(Notification notification)
        {
            db.Notifications.Update(notification);
            db.SaveChanges();
        }
        public void Delete(Notification notification)
        {
            db.Notifications.Remove(notification);
            db.SaveChanges();
        }

        public IQueryable<Notification> GetNotificationsIQueryable()
        {
            var notifications = db.Notifications;
            return notifications;
        }

    }
}
