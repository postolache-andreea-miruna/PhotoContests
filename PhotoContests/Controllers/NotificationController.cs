using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;
using System.Runtime.CompilerServices;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationManager notificationManager;
        public NotificationController(INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationCreateModel model)
        {
            notificationManager.Create(model);
            return Ok();
        }

        [HttpPatch("readNotif/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            notificationManager.Update(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int idNotification)
        {
            notificationManager.Delete(idNotification);
            return Ok();
        }

        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetNotificationsByEmail([FromRoute]string email)
        {
            var notifications = notificationManager.GetAllNotifications(email);
            return Ok(notifications);
        }

        [HttpGet("byNotification/{id}")]
        public async Task<IActionResult> GetNotificationById([FromRoute] int id)
        {
            var notification = notificationManager.GetNotificationById(id);
            return Ok(notification);
        }

        [HttpGet("notificationsNo/{email}")]
        public async Task<IActionResult> GetNoNotificationsLoggedUser([FromRoute] string email)
        {
            var notif = notificationManager.GetNoNotificationsUnreadIdUser(email);
            return Ok(notif);
        }
    }
}
