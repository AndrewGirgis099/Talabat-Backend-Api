﻿using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserMangerExtention
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = userManager.Users.Include(u=>u.Address).FirstOrDefault(u=>u.NormalizedEmail == email.ToUpper());
            return user;
        }
    }
}
