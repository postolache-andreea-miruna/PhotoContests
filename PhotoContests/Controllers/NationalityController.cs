using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalityController : ControllerBase
    {
        private readonly INationalityManager manager;
        public NationalityController(INationalityManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Create([FromBody] NationalityCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Update([FromBody] NationalityUpdateModel model)
        {
            manager.Update(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            manager.Delete(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetNationalities()
        {
            var nationalities = manager.GetAllNationalities();
            return Ok(nationalities);
        }

        [HttpGet("nationalities")]
        public async Task<IActionResult> GetNationalitiesId()
        {
            var nationalities = manager.GetAllNationalitiesWithId();
            return Ok(nationalities);
        }

        [HttpGet("nationalitiesRank/{idCompetition}/{idSection}")]
        public async Task<IActionResult> GetNationalitiesRanking([FromRoute] int idCompetition, [FromRoute] int idSection)
        {
            var nationalities = manager.GetNationalitiesForCompSection(idCompetition,idSection);
            return Ok(nationalities);
        }
    }
}
