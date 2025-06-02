using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotographerController : ControllerBase
    {
        private readonly IPhotographerManager photographerManager;
        public PhotographerController(IPhotographerManager photographerManager)
        {
            this.photographerManager = photographerManager;
        }

        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetPhotographerDetail([FromRoute] string email)
        {
            var photographer = photographerManager.GetPhotographerInfo(email);
            return Ok(photographer);
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetPhotographerDetailId([FromRoute] string id)
        {
            var photographer = photographerManager.GetPhotographerInfoById(id);
            return Ok(photographer);
        }

        [HttpPut]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> Update([FromBody] PhotographerUpdateModel model)
        {
            photographerManager.Update(model);
            return Ok();
        }
    }
}
