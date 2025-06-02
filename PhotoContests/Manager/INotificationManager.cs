using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface INotificationManager
    {
        void Create(NotificationCreateModel model);
        void Delete(int idNotification);
        void Update(int idNotification);
        List<NotificationModel> GetAllNotifications(string emailReceiver);
        //NotificationByIdModel GetNotificareById(int idNotification);
        NotificationByIdModel GetNotificationById(int idNotification);
        int GetNoNotificationsUnreadIdUser(string email);
    }
}
