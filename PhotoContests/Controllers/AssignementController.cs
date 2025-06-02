using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignementController : ControllerBase
    {
        private readonly IAssignementManager manager;
        public AssignementController(IAssignementManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Create([FromBody] AssignementCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpDelete("{emailJuror}/{idCompetition}")]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Delete([FromRoute] string emailJuror, [FromRoute] int idCompetition)
        {
            manager.Delete(emailJuror,idCompetition);
            return Ok();
        }
    }
}
