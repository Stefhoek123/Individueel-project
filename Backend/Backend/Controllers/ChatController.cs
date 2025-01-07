using Backend.Hubs;
using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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

        [HttpGet(nameof(GetChatsById))]
        public ActionResult<List<ChatDto>> GetChatsById(Guid id)
        {
            List<ChatDto> chats = _chatContainer.GetChatsById(id);
            if (chats == null || chats.Count == 0)
            {
                return NotFound();
            }
            return Ok(chats);
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
        public async Task<ActionResult> DeleteChatById(Guid id)
        {
            var chat = _chatContainer.GetChatById(id);
            if (chat != null)
            {
                _chatContainer.DeleteChatById(id);
                var hubContext = (IHubContext<ChatHub, IChatClient>)HttpContext.RequestServices.GetService(typeof(IHubContext<ChatHub, IChatClient>));
                if (hubContext != null)
                {
                    await hubContext.Clients.All.DeleteMessage(id);
                }
                return Ok();
            }
            return NotFound();
        }
    }
}
