using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Models;

public class UsersController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }
}
