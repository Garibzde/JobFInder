using JobFind.Enums;
using JobFind.Models;
using JobFind.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace JobFind.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _loginManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> loginManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _roleManager = roleManager;
        }

        

        


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegVM)
        {
            if (!ModelState.IsValid)
            {
                return View(RegVM);
            }
                var user = new AppUser { 
                UserName = RegVM.Username,
                City = RegVM.City,
                Country = RegVM.Country,
                
                Email = RegVM.Email,
                Name= RegVM.Name,
                Surname = RegVM.Surname,
                PhoneNumber = RegVM.PhoneNumber,
                


                };

               
                if (RegVM.Role == "CompanyUser")
                {
                    user.IsApproved = false;
                }
                else
                {
                    user.IsApproved = true; 
                }

                var result = await _userManager.CreateAsync(user, RegVM.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(RegVM.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(RegVM.Role));
                    }
                    await _userManager.AddToRoleAsync(user, RegVM.Role);

                    if (user.IsApproved)
                    {
                        await _loginManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        
                        ViewBag.Message = "Qeydiyyatınız təsdiqlənməlidir. Admin təsdiqlədikdən sonra giriş edə biləcəksiniz.";
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            return RedirectToAction(nameof(Login));
        }


        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginVM LogVM)
        {
            if (!ModelState.IsValid)
            {
                return View(LogVM);
            }

            AppUser user = await _userManager.FindByNameAsync(LogVM.UsernameOrEmail)
                           ?? await _userManager.FindByEmailAsync(LogVM.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
                return View(LogVM);
            }

            if (!user.IsApproved)
            {
                ModelState.AddModelError("", "Hesabınız admin tərəfindən təsdiqlənməyib.");
                return View(LogVM);
            }

            var result = await _loginManager.PasswordSignInAsync(user.UserName, LogVM.Password, LogVM.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesap bloka düşdü, zəhmət olmasa birazdan yenidən cəhd edin.");
                return View(LogVM);
            }

            ModelState.AddModelError("", "Hesaba daxil olunmadı.");
            return View(LogVM);
        }

        
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _loginManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("createroles")]
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString(),
                    });
                }
            }
            return Content("Ok");
        }
    }
}