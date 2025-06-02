using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        IEmailService manager = null;

        public EmailController(IEmailService manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        public bool SendMail(DetailsEmail details)
        {
            return manager.SendEmail(details);
        }
    }
}
