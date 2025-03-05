using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authentication;
        private readonly IValidationService _validation;
        public AuthenticationController(IAuthenticationService authentication , IValidationService validation)
        {
            _authentication = authentication;
            _validation = validation;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var validation = _validation.LoginValidity(model, ModelState);

            var result = await _authentication.Login(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var validation = await _validation.RegistrationValidity(model, ModelState);

            if (!validation.IsAuthenticated)
                return BadRequest(validation);

            var result = await _authentication.Registering(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            Console.WriteLine(User.HasClaim(e => e.Type == JwtRegisteredClaimNames.Sub));
            return Ok(result);
        }
    }
}
