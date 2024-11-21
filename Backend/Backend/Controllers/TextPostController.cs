using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextPostController : ControllerBase
    {
        private readonly ITextPostContainer _textPostContainer;
        public TextPostController(ITextPostContainer textPostContainer)
        {
            _textPostContainer = textPostContainer;
        }

        [HttpGet(nameof(GetAllTextPosts))]
        public ActionResult<IEnumerable<TextPostDto>> GetAllTextPosts()
        {
            return Ok(_textPostContainer.GetAllTextPosts());
        }

        [HttpGet(nameof(GetTextPostById))]
        public ActionResult<TextPostDto> GetTextPostById(Guid id)
        {
            TextPostDto textPost = _textPostContainer.GetTextPostById(id);
            return Ok(textPost);
        }

        [HttpPost(nameof(CreateTextPost))]
        public ActionResult CreateTextPost(TextPostDto textPost)
        {
            _textPostContainer.CreateTextPost(textPost);
            return Ok();
        }

        [HttpPut(nameof(UpdateTextPost))]
        public ActionResult UpdateTextPost(TextPostDto textPost)
        {
            _textPostContainer.UpdateTextPost(textPost);
            return Ok();
        }

        [HttpDelete(nameof(DeleteTextPostById))]
        public ActionResult DeleteTextPostById(Guid id)
        {
            _textPostContainer.DeleteTextPostById(id);
            return Ok();
        }
    }
}
