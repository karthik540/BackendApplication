using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;

namespace UserService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly ApplicationSettings appSettings;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            IOptions<ApplicationSettings> appSettings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostUser(UserModel userModel)
        {
            User newUser = new User()
            {
                UserName = userModel.username,
                Email = userModel.email,
                mobileNumber = userModel.mobileNumber,
            };

            if (userModel.userType == "Admin")
                newUser.userType = UserType.Admin;
            else if (userModel.userType == "User")
                newUser.userType = UserType.User;

            try
            {
                var result = await userManager.CreateAsync(newUser, userModel.password);

                if(result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var baseUrl = "https://localhost:44347/api/user/ConfirmEmail";
                    var parameter = "?uid={0}&token={1}";

                    string verifyLink = baseUrl + string.Format(parameter, newUser.Id, token);

                    SendEmailConfirmation(newUser.Email , verifyLink);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.username);

            if(user.userType == UserType.Admin)
            {
                return BadRequest(new
                {
                    message = "Wrong User Type !"
                });
            }

            if(user != null && await userManager.CheckPasswordAsync(user , loginModel.password))
            {
                if(!user.EmailConfirmed)
                {
                    return BadRequest(new
                    {
                        message = "Email Not Confirmed !"
                    });
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);                

                return Ok(new { token });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Username and Password Incorrect !"
                });
            }

        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.username);

            if (user.userType == UserType.User)
            {
                return BadRequest(new
                {
                    message = "Wrong User Type !"
                });
            }

            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.password))
            {
                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Username and Password Incorrect !"
                });
            }

        }


        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<string> ConfirmEmail(string uid , string token)
        {
            var user = await userManager.FindByIdAsync(uid);
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await userManager.ConfirmEmailAsync(user, token);

                if(result.Succeeded)
                {
                    return "<h1>Email Has been Verified !</h1>";
                }
                return "<h1>Token Verification Failed !</h1>";
            }
            else
            {
                return "Invlid Token Details !";
            }
        }


        private async Task SendEmailConfirmation(string uEmail , string link) //User user , string token
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("dorris.weimann@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(uEmail));
            email.Subject = "Account Verification Link";
            var bodyContent = string.Format("<h1>Verify your Account Registration</h1> <br> <a href='{0}' target='_blank'>Verify</a>", link);
            email.Body = new TextPart(TextFormat.Html) { Text = bodyContent };

            var name = "Dorris Weimann";
            var username = "dorris.weimann@ethereal.email";
            var password = "jz26uKrC73wJaWpece";

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            
            smtp.Authenticate(username, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
