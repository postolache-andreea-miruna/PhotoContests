using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly IVotingManager manager;
        public VotingController(IVotingManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VotingCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpDelete("{email}/{idCompetition}/{idSection}/{idPhoto}")]
        public async Task<IActionResult> Delete([FromRoute] string email, [FromRoute] int idCompetition, [FromRoute] int idSection, [FromRoute] int idPhoto)
        {
            manager.Delete(email, idPhoto, idSection, idCompetition);
            return Ok();
        }

        [HttpGet("voted/{email}/{idPh}/{idComp}/{idSect}")]
        public async Task<IActionResult> GetParticipationVotedByUser([FromRoute] string email, [FromRoute] int idPh, [FromRoute] int idComp, [FromRoute] int idSect)
        {
            var model = new VotedParticipationModel
            {
                emailUser = email,
                idPhoto = idPh,
                idCompetition = idComp,
                idSection = idSect
            };
            var votedResponse = manager.VotedParticipation(model);
            return Ok(votedResponse);
        }

        [HttpGet("jurorVote/{email}/{idPh}/{idComp}/{idSect}")]
        public async Task<IActionResult> GetJurorVoteForParticipationByJ([FromRoute] string email, [FromRoute] int idPh, [FromRoute] int idComp, [FromRoute] int idSect)
        {
            var points = manager.GetJurorVoteForParticipation(email, idPh, idSect, idComp);
            return Ok(points);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] VotingCreateModel model)
        {
            manager.Update(model);
            return Ok();
        }
    }
}
