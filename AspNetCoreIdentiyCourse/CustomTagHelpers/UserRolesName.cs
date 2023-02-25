using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.CustomTagHelpers
{
    [HtmlTargetElement("td",Attributes ="user-rolles")]
    public class UserRolesName : TagHelper
    {
        private UserManager<AppUser> _userManager { get;}
        [HtmlAttributeName("user-rolles")]
        public string UserId { get; set; }

        public UserRolesName(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user =await _userManager.FindByIdAsync(UserId);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            string html = string.Empty;
            foreach (var item in userRoles)
            {
                html += $"<div><span class='badge badge-success'> {item} </span></div>";
               
            }
            output.Content.SetHtmlContent(html);
        }
    }
}
