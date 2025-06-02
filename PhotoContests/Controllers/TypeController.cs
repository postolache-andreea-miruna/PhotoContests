using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeManager manager;
        public TypeController(ITypeManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Create([FromBody] TypeCreateModel model)
        {
            manager.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> Update([FromBody] TypeUpdateModel model)
        {
            manager.Update(model);
            return Ok();
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminUser")]
        public async Task<IActionResult> DeleteType([FromRoute] int id)
        {
            manager.Delete(id);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            var types = manager.GetAllTypes();
            return Ok(types);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypesId()
        {
            var types = manager.GetAllTypesWithId();
            return Ok(types);
        }

    }
}
