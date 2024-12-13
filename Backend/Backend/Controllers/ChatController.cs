using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatContainer _chatContainer;
        public ChatController(IChatContainer chatContainer)
        {
            _chatContainer = chatContainer;
        }

        [HttpGet(nameof(GetAllChats))]
        public ActionResult<IEnumerable<ChatDto>> GetAllChats()
        {
            return Ok(_chatContainer.GetAllChats());
        }

        [HttpGet(nameof(GetChatById))]
        public ActionResult<ChatDto> GetChatById(Guid id)
        {
            ChatDto chat = _chatContainer.GetChatById(id);
            return Ok(chat);
        }

        [HttpPost(nameof(CreateChat))]
        public ActionResult CreateChat(ChatDto chat)
        {
            _chatContainer.CreateChat(chat);
            return Ok();
        }

        [HttpPut(nameof(UpdateChat))]
        public ActionResult UpdateChat(ChatDto chat)
        {
            _chatContainer.UpdateChat(chat);
            return Ok();
        }

        [HttpDelete(nameof(DeleteChatById))]
        public ActionResult DeleteChatById(Guid id)
        {
            _chatContainer.DeleteChatById(id);
            return Ok();
        }
    }
}
