using AppO.Data;
using AppO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace AppO.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminUsersController : Controller
{

    private readonly UserManager<appUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminUsersController(UserManager<appUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "UserName")
    {
        var resultado = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            resultado = resultado.Where(p => p.UserName.Contains(filter));
        }

        var model = await PagingList.CreateAsync(resultado, 10, pageindex, sort, "UserName");

        model.RouteValue = new RouteValueDictionary { { "filter", filter } };

        return View(model);
    }
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();

        var user = await _context.Users.FindAsync(id);

        if(user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("Id,UserName, Name, DateOfBirth, Biography, UserImage, BannerImage")] appUser user)
    {
        if (id != user.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var userInDb = await _context.Users.FindAsync(id);

                if (userInDb == null)
                    return NotFound();

                userInDb.UserName = user.UserName;
                userInDb.Name = user.Name;
                userInDb.DateOfBirth = user.DateOfBirth;
                userInDb.Biography = user.Biography;
                userInDb.UserImage = user.UserImage;
                userInDb.BannerImage = user.BannerImage;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Opcional: mostrar uma mensagem mais amigável ou tentar resolver conflito
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

}
