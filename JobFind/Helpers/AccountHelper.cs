using JobFind.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JobFind.Helpers
{
    public class AccountHelper
    {
        public static async Task<bool> IsUserInRoleAsync(UserManager<AppUser> userManager, ClaimsPrincipal user, string role)
        {
            var appUser = await userManager.GetUserAsync(user);
            return appUser != null && await userManager.IsInRoleAsync(appUser, role);
        }
    }
}
