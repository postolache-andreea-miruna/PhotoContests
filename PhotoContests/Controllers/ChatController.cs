using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Entities;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatManager manager;
        private readonly UserManager<User> userManager;

        public ChatController(IChatManager manager, UserManager<User> userManager)
        {
            this.manager = manager;
            this.userManager = userManager;
        }

        [HttpPatch("connection/{idUser}")]
        public async Task<IActionResult> UpdateConnectionCode(string idUser, string connection)
        {
            manager.UpdateConnectionCode(idUser, connection);
            return Ok();
        }

        [HttpPost("createmessage")]
        public async Task<IActionResult> CreateMessage([FromBody] ChatCreateModel model)
        {
            manager.CreateMessage(model);
            return Ok();
        }

        [HttpGet("messages/{emailUser}/{emailUser2}")]
        public async Task<IActionResult> GetAllMessages(string emailUser, string emailUser2)
        {
            var idUser = userManager.Users.Where(u => u.Email == emailUser).Select(u => u.Id).FirstOrDefault();
            var idUser2 = userManager.Users.Where(u => u.Email == emailUser2).Select(u => u.Id).FirstOrDefault();

            var messages = manager.GetAllMessagesConv(idUser, idUser2);
            return Ok(messages);
        }

        [HttpPut("availability/{emailUser},{availability}")]
        public async Task<IActionResult> UpdateTheAvailability([FromRoute] string emailUser, bool availability)
        {
            var idUser = userManager.Users.Where(u => u.Email == emailUser).Select(u => u.Id).FirstOrDefault();
            manager.UpdateAvailability(idUser, availability);
            return Ok();
        }

        [HttpGet("fullname/{emailUser}")]
        public async Task<IActionResult> GetFullNameUser(string emailUser)
        {
            var idUser = userManager.Users.Where(u => u.Email == emailUser).Select(u => u.Id).FirstOrDefault();
            var nume = manager.GetFullName(idUser);
            return Ok(nume);
        }

        [HttpGet("unreadMesssages/{emailUser}")]
        public async Task<IActionResult> GetUnreadMessagesNumber(string emailUser)
        {
            var noMessages = manager.NumberUnreadMessages(emailUser);
            return Ok(noMessages);
        }
    }
}
