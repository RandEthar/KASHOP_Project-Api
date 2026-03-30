using KASHOP.BLL.Service;
using KASHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public   AccountController(IAuthenticationService _authenticationService)
        {
            this._authenticationService = _authenticationService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistorRequest request)
        {
            var result =await _authenticationService.Registor(request);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.Login(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(String token,String userId)
        {
       
           var result = await _authenticationService.ConfirmEmailAsync(token, userId);
            if (result)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgetPasswordRequest fororgetPasswordRequest)
        {

            var result = await _authenticationService.RequestPasswordResetAsync(fororgetPasswordRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> PasswordReset(ResetPasswordRequest resetPasswordRequest)
        {

            var result = await _authenticationService.ResetPasswordAsync(resetPasswordRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



    }
}
