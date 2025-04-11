using AppO.Models;
using AppO.Repositories.Interfaces;
using AppO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppO.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<appUser> _userManager;
    private readonly SignInManager<appUser> _signInManager;
    private readonly IUserRepository _userRepository;

    public AccountController(UserManager<appUser> userManager, SignInManager<appUser> signInManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
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
        if (user.LockoutEnabled)
        {
            ModelState.AddModelError("", "Usuário Inativo");
            TempData["Errors"] = "Usuário Inativo";
            return RedirectToAction("Index", "Home");
        }
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
    [Authorize]
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
                    DateOfBirth = registerVM.DateOfBirth
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
        TempData["Errors"] = string.Join(", ", ModelState.Values
                                              .SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage));
        return View(registerVM);
    }
    [Authorize]
    public async Task<IActionResult> Perfil()
    {
        var userId = _userManager.GetUserId(User);
        appUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        ProfileViewModel profileVM = new ProfileViewModel
        {
            Name = user.Name,
            UserName = user.UserName,
            Biography = user.Biography ,
            UserImage = user.UserImage,
            BannerImage = user.BannerImage,
            Followers = await _userRepository.FollowerCounter(user.Id),
            Following = await _userRepository.FollowingCounter(user.Id)
        };

        return View(profileVM);
    }
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        appUser user = _userManager.GetUserAsync(User).Result;
        var model = new EditProfileViewModel
        {
            Name = user.Name,
            Username = user.UserName,
            DateOfBirth = user.DateOfBirth,
            Biography = user.Biography,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            // Atualizar os dados do usuário
            user.Name = model.Name;
            user.UserName = model.Username;
            user.DateOfBirth = model.DateOfBirth;
            user.Biography = model.Biography;

            // Processar a imagem de perfil
            if (model.UserImage != null && model.UserImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.UserImage.CopyToAsync(memoryStream);
                    user.UserImage = memoryStream.ToArray();
                }
            }

            // Processar a imagem do banner
            if (model.BannerImage != null && model.BannerImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.BannerImage.CopyToAsync(memoryStream);
                    user.BannerImage = memoryStream.ToArray();
                }
            }

            // Atualizar o usuário no banco de dados
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Perfil", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(model);
    }
}
