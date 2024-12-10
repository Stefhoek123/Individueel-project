using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagePostController : ControllerBase
    {
        private readonly IImagePostContainer _imagePostContainer;
        public ImagePostController(IImagePostContainer imagePostContainer)
        {
            _imagePostContainer = imagePostContainer;
        }

        [HttpGet(nameof(GetAllImagePosts))]
        public ActionResult<IEnumerable<ImagePostDto>> GetAllImagePosts()
        {
            return Ok(_imagePostContainer.GetAllImagePosts());
        }

        [HttpGet(nameof(GetImagePostById))]
        public ActionResult<ImagePostDto> GetImagePostById(Guid id)
        {
            ImagePostDto imagePost = _imagePostContainer.GetImagePostById(id);
            return Ok(imagePost);
        }

        [HttpPost(nameof(CreateImagePost))]
        public ActionResult CreateImagePost(ImagePostDto imagePost)
        {
            _imagePostContainer.CreateImagePost(imagePost);
            return Ok();
        }

        [HttpPut(nameof(UpdateImagePost))]
        public ActionResult UpdateImagePost(ImagePostDto imagePost)
        {
            _imagePostContainer.UpdateImagePost(imagePost);
            return Ok();
        }

        [HttpDelete(nameof(DeleteImagePostById))]
        public ActionResult DeleteImagePostById(Guid id)
        {
            _imagePostContainer.DeleteImagePostById(id);
            return Ok();
        }
    }
}
