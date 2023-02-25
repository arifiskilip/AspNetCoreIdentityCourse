using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.ViewModels;
using Mapster;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using WebUI.Enums;

using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebUI.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager):base(userManager,signInManager)
        {
            
        }
        public IActionResult Index()
        {
            UserViewModel userViewModel = CurrentUser.Adapt<UserViewModel>();
            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel model)
        {
            try
            {

                bool exist = _userManager.CheckPasswordAsync(CurrentUser, model.PasswordOld).Result;
                if (exist)
                {
                    var result = _userManager.ChangePasswordAsync(CurrentUser, model.PasswordOld, model.PasswordNew).Result;
                    if (result.Succeeded)
                    {
                        _userManager.UpdateSecurityStampAsync(CurrentUser);
                        _signInManager.SignOutAsync();
                        _signInManager.PasswordSignInAsync(CurrentUser, model.PasswordNew, true, false);
                        ViewBag.success = "true";
                    }
                    else
                    {
                        AddModelErrors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış.");
                }
            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        public IActionResult EditUser()
        {
            UserViewModel model = CurrentUser.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model, IFormFile userPicture)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        CurrentUser.Picture = "/Images/" + fileName;
                    }
                }

                CurrentUser.UserName = model.UserName;
                CurrentUser.Email = model.Email;
                CurrentUser.PhoneNumber = model.PhoneNumber;
                CurrentUser.FirstName = model.FirstName;
                CurrentUser.LastName = model.LastName;
                CurrentUser.City = model.City;
                CurrentUser.Gender = (int)model.Gender;
                var result = await _userManager.UpdateAsync(CurrentUser);
                if (result.Succeeded)
                {
                    ViewBag.success = "true";
                    await _userManager.UpdateSecurityStampAsync(CurrentUser);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(CurrentUser, true);
                }
                else
                {
                    AddModelErrors(result);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View ();
        }

        //Policy Test
        [Authorize(Policy ="CityPolicy")]
        public IActionResult AnkaraPolicy()
        {
            return View();
        }

    }
}
