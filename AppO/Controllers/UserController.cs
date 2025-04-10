using AppO.Data;
using AppO.Models;
using AppO.Repositories.Interfaces;
using AppO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppO.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<appUser> _userManager;
        private readonly IUserRepository userRepository;
        public UserController(UserManager<appUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Index","Feed");
            
            appUser? userInfo = await _userManager.FindByNameAsync(username);
            appUser myUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (userInfo == null)
            {
                TempData["Error"] = "Usuário não encontrado!";
                return RedirectToAction("Index", "Feed");
            }

            UserViewModel user = new UserViewModel
            {
                User = new ProfileViewModel
                {
                    Id = userInfo.Id,
                    Name = userInfo.Name,
                    UserName = userInfo.UserName,
                    Biography = userInfo.Biography,
                    UserImage = userInfo.UserImage,
                    BannerImage = userInfo.BannerImage,
                    Followers = await userRepository.FollowerCounter(userInfo.Id),
                    Following = await userRepository.FollowingCounter(userInfo.Id)
                },
                Following = await userRepository.IsFollowingAsync(userInfo.Id, myUser.Id)
            };

            return View(user);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(string UserID)
        {
            appUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (await userRepository.IsFollowingAsync(UserID, user.Id))
            {
                await userRepository.RemoveFollow(UserID, user.Id);
            }
            else
            {
                await userRepository.AddFollow(UserID, user.Id);
            }

            appUser followedUser = await _userManager.FindByIdAsync(UserID);
            return RedirectToAction("Index", "User", new { username = followedUser.UserName });
        }

    }
}
