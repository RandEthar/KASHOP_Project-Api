using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {  private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public  AuthenticationService(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<RegistorResponse> Registor(RegistorRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?token={token}&userId={user.Id}";

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Welcome to KASHOP",
                    $@"<h1>Welcome to KASHOP</h1>
                    <p>Your account has been created successfully.</p>
                     <p>Please confirm your email:</p>
                     <a href='{emailUrl}'>Confirm Email</a>"
                );

            
                return new RegistorResponse
                {
                    Message = "User created successfully",
                    Success = true,
              
                };
            }
            else
            {

                return new RegistorResponse
                {
                    Message = "Error",
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            var result =await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }

        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //معلومات عن اليوزر بدنا نخزنها داخل التوكن payload
            var userClaims =new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier,user.Id),
              new Claim(ClaimTypes.Email,user.Email),
              new Claim(ClaimTypes.Name,user.UserName)
          };
                   var token = new JwtSecurityToken(
                  issuer: _config["Jwt:Issuer"],
                  audience: _config["Jwt:Audience"],
                 claims: userClaims,
                 expires: DateTime.Now.AddDays(5),
                 signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginResponse { Message = "Invalid email", Success = false };
            }

            if (!await _userManager.IsEmailConfirmedAsync(user) )
            {
                return new LoginResponse { Message = "Please confirm your email", Success = false };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new LoginResponse { Message = "Invalid password", Success = false };
            }

            return new LoginResponse { Message = "success", Success = true ,AccessToken=await GenerateAccessToken(user) };
        }
        public async Task<ForgetPasswordResponse> RequestPasswordResetAsync
            (ForgetPasswordRequest forgetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequest.Email);
            if (user is null)
            {
                return new ForgetPasswordResponse
                {
                    Message = "email not found",
                    Success = false
                };
            }
            var random= new Random();
            var code= random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.CodeResetPasswordExpiration = DateTime.Now.AddMinutes(15);

            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset Request",
                $@"<h1>Password Reset Request</h1>
                    <p>Your password reset code is: <strong>{code}</strong></p>
                     <p>This code will expire in 15 minutes.</p>"
            );
            return new ForgetPasswordResponse
            {
               Message = "Password reset code sent to your email",
                Success = true
            };

        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Message = "No account found with this email address.",
                    Success = false
                };
            }
            else if (user.CodeResetPassword != resetPasswordRequest.Code)
            {
                return new ResetPasswordResponse
                {
                    Message = "The verification code is invalid.",
                    Success = false
                };
            }
            else if (user.CodeResetPasswordExpiration < DateTime.Now)
            {
                return new ResetPasswordResponse
                {
                    Message = "The verification code has expired. Please request a new one.",
                    Success = false
                };
            }

            var isSamePassword = await _userManager.CheckPasswordAsync(user, resetPasswordRequest.NewPassword);
            if (isSamePassword)
            {
                return new ResetPasswordResponse
                {
                    Message = "New password must be different from the current password.",
                    Success = false
                };
            }

          
            var result = await _userManager.ResetPasswordAsync(user,await _userManager.GeneratePasswordResetTokenAsync(user) , resetPasswordRequest.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Message = "Failed to reset password. Please ensure the password meets all requirements.",
                    Success = false
                };
            }

            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset Successful",
                @"<h1>Password Reset Successful</h1>
          <p>Your password has been reset successfully.</p>"
            );

            return new ResetPasswordResponse
            {
                Message = "Your password has been reset successfully.",
                Success = true
            };
        }
    }
}
