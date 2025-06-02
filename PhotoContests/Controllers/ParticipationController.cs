using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Entities;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationManager manager;
        public ParticipationController(IParticipationManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> Create([FromBody] ParticipationCreateModel model)
        {
            var message = manager.Create(model);
            return Ok(message);
        }

        /*    [HttpPut]
            public async Task<IActionResult> Update([FromBody] ParticipationUpdateModel model)
            {
                manager.Update(model);
                return Ok();
            }*/
        [HttpPut]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Update([FromBody] Participation2UpdateModel model)
        {
            manager.UpdateNew(model);
            return Ok();
        }

        [HttpDelete("{idCompetition}/{idSection}/{idPhoto}")]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> Delete([FromRoute] int idCompetition, [FromRoute] int idSection, [FromRoute] int idPhoto)
        {
            manager.Delete(idPhoto, idSection, idCompetition);
            return Ok();
        }

        [HttpGet("forVote/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetParticipationsVoting([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var participations = manager.GetParticipationsForVoting(idSection,idCompetition);
            return Ok(participations);
        }


        //ranking bifat
        [HttpGet("participations/{idCompetition}/{idSection}")] //ranking bifat participations order by totalPoints (public) untill result day, at result day order by finalResult
        public async Task<IActionResult> GetParticipationPerPhotoCompSection([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var participations = manager.GetAllParticipationPhotosByCompSect(idCompetition,idSection);
            return Ok(participations);
        }

        //ranking bifat
        [HttpGet("myParticipations/{email}/{idCompetition}/{idSection}")] //participations order by totalPoints (public) untill result day, at result day order by finalResult
        public async Task<IActionResult> GetUserMyParticipationPerPhotoCompSection([FromRoute] int idCompetition, [FromRoute] int idSection, [FromRoute] string email)
        {
            var participations = manager.GetAllUserParticipationPhotosByCompSect(idCompetition, idSection,email);
            return Ok(participations);
        }

        [HttpGet("participate/{idCompetition}/{idSection}")] //participations per user ordered by total points and grouped by sections
        public async Task<IActionResult> GetParticipationGroupedCompSection([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var participations = manager.GetAllParticipationPhotosByCompSectGrouped(idCompetition,idSection);
            return Ok(participations);
        }

        [HttpGet("participateUser/{idCompetition}/{idSection}/{email}")] //participations per user ordered by total points and grouped by sections
        public async Task<IActionResult> GetParticipationGroupedCompSectionUser([FromRoute] int idCompetition, [FromRoute] int idSection, [FromRoute] string email)
        {
            var participations = manager.GetAllMyParticipationPhotosByCompSectUser(idCompetition, idSection,email);
            return Ok(participations);
        }

        //ranking bifat
        [HttpGet("myStatus/{email}/{idCompetition}")] //all user participations for the given competition
        public async Task<IActionResult> GetUserParticipationStatus([FromRoute] string email,[FromRoute] int idCompetition)
        {
            var participations = manager.GetAllParticipationForUserComp(email,idCompetition);
            return Ok(participations);  
        }

        [HttpGet("history/{email}")]
        public async Task<IActionResult> GetUserParticipationHistory([FromRoute] string email)
        {
            var participations = manager.GetParticipationHistoryUser(email);
            return Ok(participations);
        }


        [HttpGet("historyGrouped/{email}")]
        public async Task<IActionResult> GetUserParticipationGroupedHistory([FromRoute] string email)
        {
            var participations = manager.GetParticipationHistoryUserGrouped(email);
            return Ok(participations);
        }

        [HttpGet("bestOf/{email}")]
        public async Task<IActionResult> GetUserParticipationBestOf([FromRoute] string email)
        {
            var participations = manager.GetBestOfSectionByPh(email);
            return Ok(participations);
        }

        //GetPhParticipationsCompNationSection(int idComp, string section, string nation="all nations")
        [HttpGet("phParticipation/{idCompetition}/{idSection}/{nation}")] //all participations via nationality for a given section in a competition
        public async Task<IActionResult> GetParticipationsPhCompNatSect([FromRoute] int idCompetition, [FromRoute] int idSection, [FromRoute] string nation = "all nations")
        {
            var participations = manager.GetPhParticipationsCompNationSection(idCompetition, idSection, nation);
            return Ok(participations);
        }

        //GetParticipationByComp(int idComp, string firstName, string lastName)
        [HttpGet("byIdParticipations/{idCompetition}/{firstName}/{lastName}")] //find participations by users given names. ordered by totalPoints before final results, ordered by totalPoints at results day
        public async Task<IActionResult> GetParticipationByCompUserSearch([FromRoute] int idCompetition, [FromRoute] string firstName = "null", [FromRoute] string lastName = "null")
        {
            var participations = manager.GetParticipationByComp(idCompetition, firstName, lastName);
            return Ok(participations);
        }
        //NEW
        [HttpGet("activeCompSectPh/{email}")]
        public async Task<IActionResult> GetAllActiveCompetitionsByUser([FromRoute] string email)
        {
            var participations = manager.GetAllActiveCompetitionsUser(email);
            return Ok(participations);
        }

        [HttpGet("lastWinner/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetLastWinnerCompSect([FromRoute]int idCompetition, int idSection)
        {
            var winner = manager.GetPhotoSectionWinnerPreviousComp(idSection,idCompetition);
            return Ok(winner);
        }

        //all sections that have participants
        [HttpGet("activeSections/{idCompetition}")]
        public async Task<IActionResult> GetAllSectionsActiveCompId([FromRoute]int idCompetition)
        {
            var sections = manager.GetActiveSectionsCompetition(idCompetition);
            return Ok(sections);
        }

        //
        [HttpGet("winners/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetAllWinners([FromRoute]int idCompetition, [FromRoute]int idSection)
        {
            var winners = manager.GetWinners(idCompetition,idSection);
            return Ok(winners);
        }


        //pe asta nu il voi folosi doar l-am facut
        [HttpGet("beforeWinners/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetBeforeWinners([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var winners = manager.GetWinnersBeforeResults(idCompetition, idSection);
            return Ok(winners);
        }

        [HttpGet("participationsActiveJur/{idCompetition}/{idSection}")] 
        public async Task<IActionResult> GetAllParticipationActiveJuror([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var participations = manager.GetParticipationActiveJuror(idCompetition, idSection);
            return Ok(participations);
        }

        [HttpGet("statisticPhPodiumPart/{emailPh}")]
        public async Task<IActionResult> GetNoPodiumCompStatistic([FromRoute] string emailPh)
        {
            var result = manager.GetNoPodiumCompStatistic(emailPh);
            return Ok(result);
        }

        [HttpGet("statisticPhPart/{emailPh}/{year_}/{section}")]
        public async Task<IActionResult> GetCompYearSectPhStatistic([FromRoute] string emailPh, [FromRoute]string year_="allYears", [FromRoute]string section="allSections")
        {
            var result = manager.GetCompYearSectPhStatistic(emailPh,year_,section);
            return Ok(result);
        }

        [HttpGet("statisticJComp/{emailJ}/{comp}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> GetDeniedCompJStatistic([FromRoute] string emailJ, [FromRoute] string comp)
        {
            var result = manager.GetDeniedCompJStatistic(emailJ, comp);
            return Ok(result);
        }
    }
}
