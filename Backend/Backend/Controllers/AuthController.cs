using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserContainer _userContainer;
        public AuthController(IUserContainer userContainer)
        {
            _userContainer = userContainer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }
            // Replace this with actual user authentication logic
            var userBool = await _userContainer.AuthenticateUserAsync(request.Email, request.Password);
            if (userBool != null)
            {
                var user = _userContainer.GetUserByEmail(request.Email);
                var userId = user.Id.ToString();
                // Store userId in the session
                HttpContext.Session.SetString("UserId", userId);
                Console.WriteLine($"UserId stored in session: {userId}");
                return Ok(new { Message = "Logged in successfully" });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out" });
        }
     
        [HttpPost("check-account")]
        public async Task<IActionResult> CheckAccount([FromBody] LoginRequest request)
        {
            if (await _userContainer.IsAccountAvailableAsync(request.Email))
            {
                return Ok(new { accountExists = false, message = "Account not found. Please register." });
            }
            return Ok(new { message = "Account exists" });
        }

        [HttpGet("auth/check")]
        public IActionResult CheckAuth()
        {
            // Check if user is authenticated by verifying session
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                return Ok(new { isAuthenticated = true, userId });
            }
            return Unauthorized(new { isAuthenticated = false });
        }

        // Endpoint to check if the user is authenticated via session
        [HttpGet("auth/status")]
        public IActionResult CheckAuthStatus()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return Ok(new { isAuthenticated = true });
            }
            return Ok(new { isAuthenticated = false });
        }

        // Endpoint to fetch userId from the session
        [HttpGet("auth/userid")]
        public IActionResult GetUserIdFromSession()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return Ok(new { userId });
            }
            return Unauthorized(new { message = "User is not authenticated" });
        }
    }
}
