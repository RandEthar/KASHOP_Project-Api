using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IAuthenticationService
    {
            Task<RegistorResponse> Registor(RegistorRequest request);
        Task<LoginResponse> Login(LoginRequest request);


        Task<bool> ConfirmEmailAsync(String token, String userId);
        Task<String> GenerateAccessToken(ApplicationUser user);
        Task<ForgetPasswordResponse> RequestPasswordResetAsync
        (ForgetPasswordRequest forgetPasswordRequest);
        Task<ResetPasswordResponse> ResetPasswordAsync
    (ResetPasswordRequest resetPasswordRequest);
    }
}
