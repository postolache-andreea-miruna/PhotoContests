using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewManager reviewManager;
        public ReviewController(IReviewManager reviewManager)
        {
            this.reviewManager = reviewManager;
        }

        [HttpPost]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> Create([FromBody] ReviewCreateModel model)
        {
            reviewManager.Create(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            reviewManager.Delete(id);
            return Ok();
        }
        [HttpPatch]
        [Authorize(Policy = "JurorUser")]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewJurorUpdateModel model)
        {
            reviewManager.Update(model);
            return Ok();
        }

        [HttpGet("reviews/{idPhoto}")]
        public async Task<IActionResult> GetReviewsForPhoto([FromRoute] int idPhoto)
        {
            var reviews = reviewManager.GetAllReviewsForPhoto(idPhoto);
            return Ok(reviews);
        }

        [HttpGet("reviews/{idPhoto}/{idComp}/{idSection}")]
        public async Task<IActionResult> GetReviewsForPhotoCompSect([FromRoute] int idPhoto, [FromRoute] int idComp, [FromRoute] int idSection)
        {
            var reviews = reviewManager.GetAllReviewsPhotoCompSect(idPhoto,idComp,idSection);
            return Ok(reviews);
        }

        [HttpGet("reviewsByJuror/{email}/{idPhoto}/{idComp}/{idSection}")] //return for the juror for the participation the reviews
        public async Task<IActionResult> GetReviewPartByTheJuror([FromRoute] string email, [FromRoute] int idPhoto, [FromRoute] int idComp, [FromRoute] int idSection)
        {
            var reviews = reviewManager.GetReviewPartByJuror(email,idPhoto, idComp, idSection);
            return Ok(reviews);
        }
    }
}
