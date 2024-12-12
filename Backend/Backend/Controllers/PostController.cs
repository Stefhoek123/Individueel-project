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
    }
}
