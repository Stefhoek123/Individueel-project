using Azure.Core;
using Backend.Services;
using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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

    }
}
