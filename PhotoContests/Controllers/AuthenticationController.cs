using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoContests.Manager;
using PhotoContests.Models;

namespace PhotoContests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationManager authManager;
        private ITokenManager tokenManager;

        public AuthenticationController(IAuthenticationManager authManager, ITokenManager tokenManager)
        {
            this.authManager = authManager;
            this.tokenManager = tokenManager;
        }

        [HttpGet("tokenValid/{token}")]
        public async Task<IActionResult> GetTokenIsValid([FromRoute] string token)
        {
            var isValid = tokenManager.IsTokenExpired(token);
            return Ok(isValid);
        }


        [HttpPost("role")]
        public async Task<IActionResult> Role([FromBody] LogeInUserModel model)
        {
            var role = await authManager.Role(model);
            return Ok(role);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            try
            {
                await authManager.Register(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Register error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogeInUserModel model)
        {
            try
            {
                var token = await authManager.SignIn(model);
                if (token != null)
                    return Ok(token);
                else
                {
                    return BadRequest("Login error");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }
        }
    }
}
