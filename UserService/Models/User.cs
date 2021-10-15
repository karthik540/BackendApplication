using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace UserService.Models
{
    public enum UserType
    {
        Admin,
        User
    }

    public class User : IdentityUser
    {
        //  ID , Username , Email Exist in IndentityUser Parent table...
        public string password { get; set; }
        public UserType userType { get; set; }
        public string mobileNumber { get; set; }
        public bool Confirmed { get; set; }

    }
}
