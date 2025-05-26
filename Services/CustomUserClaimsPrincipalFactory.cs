using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NawatechAuthApp.Models;
using System.Security.Claims;

namespace NawatechAuthApp.Services
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                identity.AddClaim(new Claim("ProfilePicture", user.ProfilePicture));
            }
            
            if (!string.IsNullOrEmpty(user.GravatarUrl))
            {
                identity.AddClaim(new Claim("GravatarUrl", user.GravatarUrl));
            }
            
            return identity;
        }
    }
}