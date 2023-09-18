using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace COLLATEFINAL.Helpers
{
    public class AppIdentityUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppIdentityUser, IdentityRole>
    {
        public AppIdentityUserClaimsPrincipalFactory(UserManager<AppIdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppIdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserId", user.Id ?? ""));
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("UserProfile", user.ImageUrl ?? ""));

            return identity;
        }

        








    }
}
