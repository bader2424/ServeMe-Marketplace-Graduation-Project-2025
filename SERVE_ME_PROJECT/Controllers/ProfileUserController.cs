using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ProfileUserController : Controller
{
    private readonly ServeMeContext _context;
    private readonly UserManager<ApplicationUser> _userManager; // تغيير IdentityUser إلى ApplicationUser

    public ProfileUserController(ServeMeContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.TbOrder
            .Include(o => o.Service)
            .Include(o => o.OrderStat)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return View(orders);
    }
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model, IFormFile ProfileImage, string OldImagePath)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null) return NotFound();

        if (ModelState.IsValid)
        {
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;

            // معالجة الصورة الجديدة
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgusers");
                Directory.CreateDirectory(uploadsFolder); // يتأكد من وجود المجلد
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImagePath = uniqueFileName;
            }
            else
            {
                // إذا لم يتم رفع صورة جديدة، احتفظ بالصورة القديمة
                user.ProfileImagePath = OldImagePath;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }


}
