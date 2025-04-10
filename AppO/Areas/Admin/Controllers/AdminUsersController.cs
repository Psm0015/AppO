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
}
