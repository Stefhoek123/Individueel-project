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

        [HttpGet(nameof(GetPostsByFamilyId))]
        public ActionResult<List<PostDto>> GetPostsByFamilyId(Guid id)
        {
            List<PostDto> posts = _postContainer.GetPostsByFamilyId(id);
            if (posts == null || posts.Count == 0)
            {
                return NotFound();
            }
            return Ok(posts);
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
            // Extensies en bijbehorende MIME-types
            var allowedMimeTypes = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" }
            };

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedMimeTypes.ContainsKey(extension))
            {
                return Ok(new { message = "Ongeldige bestandsextensie." });
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return Ok(new { message = "Maximale bestandsgrootte is 5MB."});
            }

            if (file.ContentType != allowedMimeTypes[extension])
            {
                return Ok(new { message = "Mismatch tussen bestandsextensie en MIME-type."});
            }

            // Optioneel: controleer of het bestand echt een afbeelding is (bijvoorbeeld via ImageSharp of System.Drawing)
            try
            {
                using var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                // Indien dit mislukt, is het bestand geen echte afbeelding
            }
            catch
            {
                return Ok(new { message = "Het bestand is geen geldige afbeelding."});
            }

            string fileName = Guid.NewGuid() + extension;
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadFolder);
            string fullPath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok(new { fileName = $"Uploads/{fileName}" });
        }

    }
}
