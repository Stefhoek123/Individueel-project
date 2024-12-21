using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate user credentials (replace with your own logic)
            if (request.Email == "email" && request.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim("CustomClaimType", "CustomClaimValue")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Ok(new { username = HttpContext.User.Identity.Name });
            }

            return Unauthorized();
        }
    }
}
