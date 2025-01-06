using Azure.Core;
using DAL.Interfaces;
using Domain;
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

            var userBool = await _userContainer.AuthenticateUserAsync(request.Email, request.Password);
            if (userBool != null)
            {
                var user = _userContainer.GetUserByEmail(request.Email);
                var userId = user.Id.ToString();
                var userEmail = user.Email;
                // Store userId in the session
                HttpContext.Session.SetString("UserId", userId);
                HttpContext.Session.SetString("Email", userEmail);
                Console.WriteLine($"UserId stored in session: {userId} + {userEmail}");
                return Ok(new { Message = "Logged in successfully" , userIdSession = userId });
            }
            return Ok(new { message = "Invalid credentials"} );
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


        // Endpoint to check if the user is authenticated via session
        [HttpGet("auth/status")]
        public IActionResult CheckAuthStatus()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return Ok(new { isAuthenticated = true , userIdSession = userId });
            }
            return Ok(new { isAuthenticated = false });
        }
    }
}
