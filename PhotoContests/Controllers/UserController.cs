using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserMyManager manager;
        public UserController(IUserMyManager manager)
        {
            this.manager = manager;
        }

        [HttpGet("byEmailPh/{email}")]
        public async Task<IActionResult> GetPhotoForEmailUser([FromRoute] string email)
        {
            var photoUrl = manager.GetPhotoForEmail(email);
            return Ok(photoUrl);
        }
    }
}
