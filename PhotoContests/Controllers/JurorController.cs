using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JurorController : ControllerBase
    {
        private readonly IJurorManager jurorManager;
        public JurorController(IJurorManager jurorManager)
        {
            this.jurorManager = jurorManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetJurors()
        {
            var jurors = jurorManager.GetAllJurors();
            return Ok(jurors);
        }

        [HttpGet("byCompetition/{id}")]
        public async Task<IActionResult> GetJurorsComp([FromRoute] int id)
        {
            var jurors = jurorManager.GetAllJurorsByCompetitionId(id);
            return Ok(jurors);
        }

        [HttpGet("notInCompetition/{id}")]
        public async Task<IActionResult> GetJurorsNotInComp([FromRoute] int id)
        {
            var jurors = jurorManager.GetAllJurorsNotInCompById(id);
            return Ok(jurors);
        }

        [HttpGet("jurorsInfo/{email}")]
        public async Task<IActionResult> GetJurorInfo([FromRoute] string email)
        {
            var juror = jurorManager.GetJurorInfo(email);
            return Ok(juror);
        }


        [HttpGet("jurorsInfoId/{id}")]
        public async Task<IActionResult> GetJurorInfoId([FromRoute] string id)
        {
            var juror = jurorManager.GetJurorInfoByID(id);
            return Ok(juror);
        }

        [HttpPut]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> Update([FromBody] JurorUpdateModel model)
        {
            jurorManager.Update(model);
            return Ok();
        }
    }
}
