using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportManager manager;
        public ReportController(IReportManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpGet("summaryReportsNonJuror/{idPhoto}/{idSection}/{idCompetition}")]
        public async Task<IActionResult> GetSummaryReports([FromRoute] int idPhoto, [FromRoute] int idSection, [FromRoute] int idCompetition)
        {
            var summaryReports = manager.GetAllParticipationPhReports(idCompetition, idSection, idPhoto);
            return Ok(summaryReports);
        }

        [HttpGet("summaryReportsJustJuror/{idPhoto}/{idSection}/{idCompetition}")]
        public async Task<IActionResult> GetSummaryReportsJuror([FromRoute] int idPhoto, [FromRoute] int idSection, [FromRoute] int idCompetition)
        {
            var existSummaryReportsJuror = manager.GetExistParticipationReportsJuror(idCompetition, idSection, idPhoto);
            return Ok(existSummaryReportsJuror);
        }

        [HttpGet("reportMessage/{idPhoto}/{idSection}/{idCompetition}/{emailUser}")]
        public async Task<IActionResult> GetReportMessageUserPart([FromRoute] int idPhoto, [FromRoute] int idSection, [FromRoute] int idCompetition, [FromRoute] string emailUser)
        {
            var message = manager.GetReportMessage(idCompetition,idSection,idPhoto,emailUser);
            return Ok(message);
        }

        [HttpGet("reportValuesJuror/{idPhoto}/{idSection}/{idCompetition}/{email}")]
        public async Task<IActionResult> GetAcceptDeclineReportForJuror([FromRoute] int idPhoto, [FromRoute] int idSection, [FromRoute] int idCompetition, [FromRoute] string email)
        {
            var reportValues = manager.GetAcceptDeclineReportJuror(idCompetition, idSection, idPhoto, email);
            return Ok(reportValues);
        }

        [HttpGet("{idCompetition}")]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> GetReportResult([FromRoute] int idCompetition)
        {
            manager.ReportResult(idCompetition);
            return Ok();
        }

    }
}
