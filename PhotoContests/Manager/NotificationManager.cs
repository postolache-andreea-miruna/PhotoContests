using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class NotificationManager: INotificationManager
    {
        private readonly INotificationRepo repo;
        private readonly UserManager<User> userManager;
        private PhotoContestsContext db;

        public NotificationManager(INotificationRepo repo, UserManager<User> userManager, PhotoContestsContext db)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.db = db;
        }

        public void Create (NotificationCreateModel model)
        {
            var users = userManager.Users;
            var userSender = users.Where(u => u.Email.Equals(model.emailSender)).FirstOrDefault();
            var userReceiver = users.Where(u => u.Email.Equals(model.emailReceiver)).FirstOrDefault();

            var newNotification = new Notification
            {
                idUser2 = userReceiver.Id,
                message = model.message,
                title = model.title,
                sendDate = DateTime.UtcNow,
                status = false,
                idUser = userSender.Id
            };
            repo.Create(newNotification);
        }

        public void Delete(int idNotification)
        {
            var notification = repo.GetNotificationsIQueryable()
                .FirstOrDefault(n => n.idNotification == idNotification);
            if (notification == null) return;
            repo.Delete(notification);
        }

        public void Update(int idNotification)
        {
            var notification = repo.GetNotificationsIQueryable()
                .FirstOrDefault(n => n.idNotification == idNotification);
            notification.status = true; //notification was read
            repo.Update(notification);
        }

        public List<NotificationModel> GetAllNotifications(string emailReceiver)
        {
            var users = userManager.Users;
            var userReceiver = users.Where(u => u.Email.Equals(emailReceiver)).FirstOrDefault();
            var notif = repo.GetNotificationsIQueryable();
            if(notif == null)
            {
                return new List<NotificationModel>();
            }
            var notifications = notif
                .Where(n => n.idUser2 == userReceiver.Id)
                .Select(n => new NotificationModel
                {
                    idNotification = n.idNotification,
                    title = n.title,
                    sendDate = n.sendDate,
                    status = n.status,
                    senderName = userManager.Users.Where(u => u.Id.Equals(n.idUser)).Select(u => u.firstName).FirstOrDefault() + " " + userManager.Users.Where(u => u.Id.Equals(n.idUser)).Select(u => u.lastName).FirstOrDefault(),
                    profilePicture = userManager.Users.Where(u => u.Id.Equals(n.idUser)).Select(u => u.profilePicture).FirstOrDefault(),
                    senderRole = (from ur in db.UserRoles
                                  join r in db.Roles on ur.RoleId equals r.Id
                                  where ur.UserId == n.idUser
                                  select r.Name).FirstOrDefault()//userManager.GetRolesAsync(userManager.Users.FirstOrDefault(u => u.Id.Equals(n.idUser))).Result.FirstOrDefault()
                })
                .OrderByDescending(n => n.sendDate)
                .ToList();
            return notifications;

        }

        public NotificationByIdModel GetNotificationById(int idNotification)
        {
            var notification = repo.GetNotificationsIQueryable().Where(n => n.idNotification == idNotification)
                .Select(n => new NotificationByIdModel
                {
                    title = n.title,
                    message = n.message,
                    sendDate = n.sendDate
                })
                .FirstOrDefault();
            return notification;
        }

        public int GetNoNotificationsUnreadIdUser(string email)
        {
            var user = userManager.FindByEmailAsync(email);
            var idUser = user.Result.Id;
            var noNotif = repo.GetNotificationsIQueryable()
                .Where(n => n.idUser2 == idUser && n.status == false)
                .Count();
            return noNotif;

        }

    }
}
