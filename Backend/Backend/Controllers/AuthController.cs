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
            // Authenticate user using the provided IUserContainer
            if (await _userContainer.AuthenticateUserAsync(request.Email, request.Password))
            {
                // Create claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, request.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in using CookieAuth
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true, // Set this to true for persistent cookies
                        ExpiresUtc = DateTime.UtcNow.AddHours(1) // Adjust expiration as needed
                    });

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
