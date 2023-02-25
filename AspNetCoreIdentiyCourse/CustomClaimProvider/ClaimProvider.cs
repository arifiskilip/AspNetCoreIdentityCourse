using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.CustomClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        //Senaryo 1 : Üyelerin Şehir alanına göre sayfa'ya erişimi sağlaması.
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal !=null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity Identity = principal.Identity as ClaimsIdentity;
                AppUser user =await _userManager.FindByNameAsync(Identity.Name);
                if (user !=null)
                {
                    if (user.City!=null)
                    {
                        if (!principal.HasClaim(c=> c.Type=="City"))
                        {
                            Claim CityClaim = new Claim("city", user.City.ToLower(), ClaimValueTypes.String, "Internal");
                            Identity.AddClaim(CityClaim);
                        }
                    }
                }
            }
            return principal;
        }
    }
}
