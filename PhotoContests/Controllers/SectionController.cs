using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionManager manager;
        public SectionController(ISectionManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Create([FromBody] SectionCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Update([FromBody] SectionUpdateModel model)
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
        public async Task<IActionResult> GetSections()
        {
            var sections = manager.GetAllSections();
            return Ok(sections);
        }

        [HttpGet("sections")]
        public async Task<IActionResult> GetSectionsWithId()
        {
            var sections = manager.GetAllSectionsWithId();
            return Ok(sections);
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetSectionById([FromRoute] int id)
        {
            var section = manager.GetSectionById(id);
            return Ok(section);
        }

        [HttpGet("sectionName/{emailPh}/{year_}")]
        public async Task<IActionResult> GetSectionsCompetitionsByPhotographer([FromRoute] string emailPh, [FromRoute] string year_)
        {
            var section = manager.GetSectionsCompetitionsByPhotographer(emailPh,year_);
            return Ok(section);
        }
    }
}
