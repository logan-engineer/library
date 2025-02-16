using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using library.Migrations;
using library.Models;
using library.ViewModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager,RoleManager<IdentityRole>roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result=await signInManager.PasswordSignInAsync(model.Email,model.Password, model.RememberMe,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Email or Password is incorrect");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };
                var result =await userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    var roleExit = await roleManager.RoleExistsAsync("User");
                    if (!roleExit)
                    {
                        var role = new IdentityRole("User");
                        await roleManager.CreateAsync(role);
                    }
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    return View(model);
                }
                
            }
            return View(model);
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel {Email=username});
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result=await userManager.AddPasswordAsync(user,model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is not valid");
                    return View(model);
                }
            }
             else
                {
                    ModelState.AddModelError("", "Something went Wrong.try again...");
                    return View(model);
                }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
