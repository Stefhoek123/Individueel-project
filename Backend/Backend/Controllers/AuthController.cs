using Azure.Core;
using Backend.Services;
using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserContainer _userContainer;

        public AuthController(JwtService jwtService, IUserContainer userContainer)
        {
            _jwtService = jwtService;
            _userContainer = userContainer;
        } 

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login( LoginRequestDto request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result is null)
            {
                return Ok(new { message = "Does not exist" });
            }

            return result;
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

        [HttpGet("current")]
        public IActionResult GetCurrentUser([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "JWT is missing." });
            }

            var jwt = authorizationHeader.Substring("Bearer ".Length);
 
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new { message = "E-mail not found in token." });
            }

            var user = _userContainer.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

    }
}
