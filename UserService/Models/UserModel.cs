using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class UserModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string userType { get; set; }
        public string mobileNumber { get; set; }

    }
}
