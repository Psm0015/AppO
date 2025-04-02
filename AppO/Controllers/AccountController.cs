using AppO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppO.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid)
        {
            TempData["Errors"] = string.Join(", ", ModelState.Values
                                              .SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage));
            return RedirectToAction("Index", "Home");
        } 

        var user = await _userManager.FindByNameAsync(loginVM.UserName);

        if(user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if(result.Succeeded)
            {
                if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(loginVM.ReturnUrl);
            }
        }
        ModelState.AddModelError("", "Usuário ou Senha Inválidos!");
        TempData["Errors"] = "Usuário ou Senha Inválidos!";
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.User = null;
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
