using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<User> userManager;

        public UserProfileController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }


        [HttpGet]
        [Authorize]
        public async Task<Object> GetUserProfile()
        {
            string userID = User.Claims.First(c => c.Type == "UserID").Value;

            var user = await userManager.FindByIdAsync(userID);

            return new { 
                user.UserName,
                user.Email,
                user.mobileNumber,

            };

        }


    }
}
