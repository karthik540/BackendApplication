using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Data
{
    public class DbInit
    {
        public static void InitializeWithFakeData(AuthenticationContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
