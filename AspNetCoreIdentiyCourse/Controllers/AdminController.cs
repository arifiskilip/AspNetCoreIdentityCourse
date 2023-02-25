using AspNetCoreIdentiyCourse.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> 
            signInManager, RoleManager<AppRole> roleManager):base(userManager,signInManager,roleManager)
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUsers()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRole(RoleViewModel model)
        {
            AppRole role = new AppRole();
            role.Name = model.Name;
            var result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelErrors(result);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role!=null)
            {
                _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Roles");
        }
        public IActionResult UpdateRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role!=null)
            {
                return View(role.Adapt<RoleViewModel>());
            }
            return RedirectToAction("Roles");
        }
        [HttpPost]
        public IActionResult UpdateRole(RoleViewModel model)
        {
            var role = _roleManager.FindByIdAsync(model.Id).Result;
            if (role != null)
            {
                role.Name = model.Name;
                var result = _roleManager.UpdateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelErrors(result);
                }
            }
            return View(model);
;        }
        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            AppUser user = _userManager.FindByIdAsync(id).Result;
            ViewBag.userName = user.UserName;
            List<AppRole> roles = _roleManager.Roles.ToList();
            List<string> userRoles = _userManager.GetRolesAsync(user).Result as List<string>;
            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
            foreach (var item in roles)
            {
                RoleAssignViewModel roleAssign = new RoleAssignViewModel();
                roleAssign.Id = item.Id;
                roleAssign.Name = item.Name;
                if (userRoles.Contains(item.Name))
                {             
                    roleAssign.Exist = true;
                }
                else
                {
                    roleAssign.Exist = false;
                }
                roleAssignViewModels.Add(roleAssign);
            }
            return View(roleAssignViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;
            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.Name); 
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }
            return RedirectToAction("GetUsers");
        } 

        //Claims
        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }

        
    }
}
