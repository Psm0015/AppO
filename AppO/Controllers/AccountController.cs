using AppO.Models;
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

    public async Task<IActionResult> Cadastro()
    {
        return View(new RegisterViewModel());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro(RegisterViewModel registerVM)
    {
        if (ModelState.IsValid)
        {
            if (!(string.Equals(registerVM.Password, registerVM.CPassword))) this.ModelState.AddModelError("Registro", "As senhas devem ser iguais");
            else
            {
                var user = new appUser
                {
                    UserName = registerVM.Username,
                    Name = registerVM.Name,
                    DateOfBirth = registerVM.DateOfBirth,
                    Biography = registerVM.Biography
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Falha ao realizar o registro");
                }
            }
        }
        return View(registerVM);
    }
}
