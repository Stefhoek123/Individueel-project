using System.Security.Claims;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("auth")]
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
            var isAuthenticated = await _userContainer.AuthenticateUserAsync(request.Email, request.Password);

            if (isAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, request.Email)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identity));

                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                // Extract user information from claims
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                return Ok(new
                {
                    email
                });
            }

            return Unauthorized(new { message = "User is not authenticated" });
        }

        [HttpPost("check-account")]
        public async Task<IActionResult> CheckAccount([FromBody] LoginRequest request)
        {
            if (await _userContainer.IsAccountAvailableAsync(request.Email))
            {
                return NotFound(new { message = "Account not found. Please register." });
            }

            return Ok(new { message = "Account exists" });
        }
    }
}
