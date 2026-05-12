using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SERVE_ME_PROJECT.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using SERVE_ME_PROJECT.Areas.AdminControlPanel.ViewModels;

namespace SERVE_ME_PROJECT.Areas.Admin.Controllers
{
    [Area("AdminControlPanel")]
//    [Authorize(Roles = "Admin")] // إن أردت تقييد الوصول للمشرف فقط
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // عرض كل المستخدمين مع أدوارهم
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var usersWithRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(usersWithRoles);
        }

        // تفاصيل المستخدم
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserWithRolesViewModel
            {
                User = user,
                Roles = roles
            };

            return View(model);
        }

        // تفعيل أو تعطيل المستخدم (تعطيل بدلاً من حذف)
        public async Task<IActionResult> ToggleActive(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // يمكنك التعامل مع الأخطاء هنا
            }

            return RedirectToAction(nameof(Index));
        }

        // عرض صفحة تغيير نوع المستخدم (الأدوار)
        public async Task<IActionResult> ChangeUserType(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new ChangeUserTypeViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                CurrentRoles = userRoles,
                AllRoles = _roleManager.Roles.Select(r => r.Name).ToList() // ⬅️ تأكد من وجود هذا السطر

            };

            return View(model);
        }

        // استقبال POST لتغيير نوع المستخدم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserType(ChangeUserTypeViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                // ✅ إعادة تعبئة البيانات الناقصة
                model.FullName = user.FullName;
                model.CurrentRoles = await _userManager.GetRolesAsync(user);
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // إزالة كل الأدوار القديمة
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء إزالة الأدوار القديمة.");

                // ✅ إعادة تعبئة البيانات الناقصة
                model.FullName = user.FullName;
                model.CurrentRoles = await _userManager.GetRolesAsync(user);
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                return View(model);
            }

            // إضافة الدور الجديد
            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء إضافة الدور الجديد.");

                    // ✅ إعادة تعبئة البيانات الناقصة
                    model.FullName = user.FullName;
                    model.CurrentRoles = await _userManager.GetRolesAsync(user);
                    model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
