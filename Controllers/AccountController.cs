using BZLAND.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BZLAND.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) { return View(register); }
            IdentityUser user = new IdentityUser()
            {
                UserName = register.UserName,
                Email = register.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(register);


            }
            return RedirectToAction(nameof(Login));

        }



        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login,string? ReturnUrl)
        {
            if (!ModelState.IsValid) { return View(login); }
            IdentityUser user =await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(login);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password,true);
            if (result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(login);

            }
            _signInManager.SignInAsync(user, login.RemmeberMe);
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index","Home");

        }






    }
}
