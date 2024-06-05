using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count()==0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Andrew Girgis",
                    Email = "andrewgirgis099@gmail.com",
                    UserName = "Andrew Girgis",
                    PhoneNumber = "01228817401"
                    
                };

               await _userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
