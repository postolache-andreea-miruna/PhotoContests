using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoManager manager;
        public VideoController(IVideoManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> Create([FromBody] VideoCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }
        [HttpDelete("{idVideo}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> Delete([FromRoute] int idVideo)
        {
            manager.Delete(idVideo);
            return Ok();
        }
        [HttpPatch]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> UpdateVideo([FromBody] VideoUpdateModel model)
        {
            manager.Update(model);
            return Ok();
        }

        [HttpGet("allVideoForPhotographer/{emailPh}/{competition}")]
        public async Task<IActionResult> GetVideosReceivedByPhotogr([FromRoute] string emailPh, [FromRoute] string competition)
        {
            var videos = manager.VideosReceivedByPhotogr(emailPh,competition);
            return Ok(videos);
        }

        [HttpGet("videoPart/{emailJuror}/{idPhoto}/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetVideoPartJuror([FromRoute] string emailJuror, [FromRoute]int idPhoto, [FromRoute]int idCompetition, [FromRoute]int idSection)
        {
            var video = manager.GetVideoForPartByJuror(emailJuror,idPhoto,idCompetition,idSection);
            return Ok(video);
        }
    }
}
