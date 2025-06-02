using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionManager manager;
        public CompetitionController(ICompetitionManager manager)
        {
            this.manager = manager;
        }
        [HttpPost]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Create([FromBody] CompetitionCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Update([FromBody] CompetitionUpdateModel model)
        {
            manager.Update(model);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCompetitionsAllInfo()
        {
            var competitions = manager.GetAllInfoCompetitions();
            return Ok(competitions);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompetitions()
        {
            var competitions = manager.GetAllCompetitions();
            return Ok(competitions);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCompetitions()
        {
            var competitions = manager.GetAllActiveCompetitions();
            return Ok(competitions);
        }

        [HttpGet("currentCompetitions")]
        public async Task<IActionResult> GetCurrentCompetitions()
        {
            var competitions = manager.GetAllCompetitionsCurrentYear();
            return Ok(competitions);
        }

        [HttpGet("byYear/{year}")]
        public async Task<IActionResult> GetCompetitionsByYear([FromRoute]int year)
        {
            var competitions = manager.GetAllCompetitionsByYear(year);
            return Ok(competitions);
        }

        [HttpGet("competition/{id}")]
        public async Task<IActionResult> GetCompetitionById([FromRoute] int id)
        {
            var competition = manager.GetCompetitionById(id);
            return Ok(competition);
        }

        [HttpGet("byPhotographer/{email}")]
        public async Task<IActionResult> GetCompetitionsByPhotographer([FromRoute] string email)
        {
            var competitions = manager.GetAllCompetitionsByPhotographer(email);
            return Ok(competitions);
        }

        //
        [HttpGet("byPhotographerNameComp/{email}")]
        public async Task<IActionResult> GetCompetitionsNamesByPhotographer([FromRoute] string email)
        {
            var competitions = manager.GetAllCompetitionsNamesByPhotographer(email);
            return Ok(competitions);
        }
        //new
        [HttpGet("competitionsActive/{email}")]
        public async Task<IActionResult> GetCompetitionsActivePhotographer([FromRoute] string email)
        {
            var competitions = manager.GetAllCompetitionsActive(email);
            return Ok(competitions);
        }

        [HttpGet("allPresentCompetitions")]
        public async Task<IActionResult> GetAllActivePresentCompetitions()
        {
            var competitions = manager.GetAllActiveCurrentCompetitions();
            return Ok(competitions);
        }


        [HttpGet("competitionsComplete/{email}")]
        public async Task<IActionResult> GetCompetitionsCompletePhotographer([FromRoute] string email)
        {
            var competitions = manager.GetAllCompetitionsCompleted(email);
            return Ok(competitions);
        }
        [HttpGet("competitionsFinished")]
        public async Task<IActionResult> GetCompetitionsFinished()
        {
            var competitions = manager.GetAllCompetitionsFinished();
            return Ok(competitions);
        }

        [HttpGet("competitionsInJudging/{email}")]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> GetCompetitionsInJudging([FromRoute] string email)
        {
            var competitions = manager.GetAllUserCompetitionsInJudging(email);
            return Ok(competitions);
        }

        [HttpGet("competitionsCompleteUser/{email}")] //the used one
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> GetCompetitionsCompleteUserPhotographer([FromRoute] string email)
        {
            var competitions = manager.GetUserAllCompletedCompetitions(email);
            return Ok(competitions);
        }

        [HttpGet("competitionsActivePhUser/{email}")] //the used one
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> GetUserAllActiveCompetitionsPhotosPhotographer([FromRoute] string email)
        {
            var competitions = manager.GetUserAllActiveCompetitionsPhotos(email);
            return Ok(competitions);
        }


        [HttpGet("competitionsFuture")]
        public async Task<IActionResult> GetCompetitionsFuture()
        {
            var competitions = manager.GetAllCompetitionsFuture();
            return Ok(competitions);
        }

        [HttpGet("jurorComp/{email}")]
        public async Task<IActionResult> GetAllCompetitionsByJuror([FromRoute] string email, string type = "active")
        {
            var participations = manager.GetAllCompetitionsJuror(email, type);
            return Ok(participations);
        }

        //new
        [HttpGet("jurorActiveComp/{email}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> GetAllActiveCompetitionsForJuror([FromRoute] string email)
        {
            var participations = manager.GetAllActiveCompetitionsJuror(email);
            return Ok(participations);
        }

        [HttpGet("jurorEndedComp/{email}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> GetAllEndedCompetitionsForJuror([FromRoute] string email)
        {
            var participations = manager.GetAllEndedCompetitionsJuror(email);
            return Ok(participations);
        }

        [HttpGet("jurorFinishComp/{email}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> GetAllFinishedCompetitionsForJuror([FromRoute] string email)
        {
            var participations = manager.GetAllFinishedCompetitionsJuror(email);
            return Ok(participations);
        }

        [HttpGet("photographersPhReported/{id}")]
        public async Task<IActionResult> GetPhotographersPhReported([FromRoute] int id)
        {
            var competition = manager.GetPhEmail(id);
            return Ok(competition);
        }

        [HttpGet("compYearPhotographer/{email}")]
        public async Task<IActionResult> GetYearsCompParticipate([FromRoute] string email)
        {
            var years = manager.GetYearsCompetitionsByPhotographer(email);
            return Ok(years);
        }

        [HttpGet("compAssignJuror/{emailJ}")]
        public async Task<IActionResult> GetCompAssignedJuror([FromRoute] string emailJ)
        {
            var years = manager.GetNameCompJurors(emailJ);
            return Ok(years);
        }
    }
}
