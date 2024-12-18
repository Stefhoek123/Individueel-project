using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostContainer _postContainer;
        public PostController(IPostContainer postContainer)
        {
            _postContainer = postContainer;
        }

        [HttpGet(nameof(GetAllPosts))]
        public ActionResult<IEnumerable<PostDto>> GetAllPosts()
        {
            return Ok(_postContainer.GetAllPosts());
        }

        [HttpGet(nameof(GetPostById))]
        public ActionResult<PostDto> GetPostById(Guid id)
        {
            PostDto post = _postContainer.GetPostById(id);
            return Ok(post);
        }

        [HttpPost(nameof(CreatePost))]
        public ActionResult CreatePost(PostDto post)
        {
            _postContainer.CreatePost(post);
            return Ok();
        }

        [HttpPut(nameof(UpdatePost))]
        public ActionResult UpdatePost(PostDto post)
        {
            _postContainer.UpdatePost(post);
            return Ok();
        }

        [HttpDelete(nameof(DeletePostById))]
        public ActionResult DeletePostById(Guid id)
        {
            _postContainer.DeletePostById(id);
            return Ok();
        }

        [HttpPost(nameof(GetImageUrl))]
        public IActionResult GetImageUrl(IFormFile file)
        {
            List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file extension");
            }

            long size = file.Length;
            if (size > (5 * 1024 * 1024))
            {
                return BadRequest("Maximum size can be 5mb");
            }

            string fileName = Guid.NewGuid().ToString() + extension;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(filePath);

            using FileStream fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
            file.CopyTo(fileStream);

            return Ok(new { fileName = $"uploads/{fileName}" });
        }
    }
}
