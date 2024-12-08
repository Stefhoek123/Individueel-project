using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetSessionInfo()
        {
            List<string> sessionInfo = new List<string>();

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariables.SessionKeyUsername)))
            {
                HttpContext.Session.SetString(SessionKey.SessionKeyUsername.ToString(), "Current user");
                HttpContext.Session.SetString(SessionKey.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());
            }
           
            var username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
            var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            sessionInfo.Add(username?? string.Empty);
            sessionInfo.Add(sessionId ?? string.Empty);

            return sessionInfo;
        }
    }
}
