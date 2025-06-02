using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Entities;
using PhotoContests.Manager;
using PhotoContests.Models;
using System.Runtime.CompilerServices;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoManager photoManager;

        public PhotoController(IPhotoManager photoManager)
        {
            this.photoManager = photoManager;
        }

        [HttpPost]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> Create([FromBody] PhotoCreateModel model)
        {
            var id = photoManager.Create(model);
            return Ok(id);
        }

        [HttpPut]
        [Authorize(Policy = "PhotographerUser")]
        public async Task<IActionResult> Update([FromBody] PhotoUpdateModel model)
        {
            photoManager.Update(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            photoManager.Delete(id);
            return Ok();
        }

        [HttpGet("photos/{email}")]
        public async Task<IActionResult> GetAllPhotosByPhotographer([FromRoute] string email)
        {
            var photos = photoManager.GetAllPhotosByPhotographer(email);
            return Ok(photos);
        }

        [HttpGet("photosNoPart/{email}/{idComp}/{idSect}")]
        public async Task<IActionResult> GetNoPartPhotosByPhotographer([FromRoute] string email, [FromRoute] int idComp, [FromRoute] int idSect)
        {
            var photos = photoManager.GetAllPhotosDontPartByPhotographer(email,idComp,idSect);
            return Ok(photos);
        }

        [HttpGet("photosVisible/{email}")]
        public async Task<IActionResult> GetAllVisiblePhotosByPhotographe([FromRoute] string email)
        {
            var photos = photoManager.GetAllPhotosByPhotographerVisible(email);
            return Ok(photos);
        }

        [HttpGet("photoDetail/{id}")]
        public async Task<IActionResult> GetPhotoById([FromRoute] int id)
        {
            var photo = photoManager.GetPhotoInfo(id);
            return Ok(photo);
        }

        [HttpGet("portofolio/{email}")]
        public async Task<IActionResult> GetPortofolioPhotographer([FromRoute] string email)
        {
            var photos = photoManager.GetPortofolioPhotographer(email);
            return Ok(photos);
        }


        [HttpGet("portofolioViewOther/{emailPh}")]
        public async Task<IActionResult> GetPortofolioPhotographerViewedByOthers([FromRoute] string emailPh)
        {
            var photos = photoManager.GetPortofolioPhotographerOtherView(emailPh);
            return Ok(photos);
        }
    }

}
