using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;


namespace SERVE_ME_PROJECT.Controllers
{
    public class ServiceProviderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ServeMeContext _context;

        public ServiceProviderController(UserManager<ApplicationUser> userManager, ServeMeContext context) 
        { 
            _userManager = userManager;
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public IActionResult JoinAsProvider()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinAsProviderPost(ServiceProviderModel serviceProviderModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
                return View("JoinAsProvider", serviceProviderModel);

            var exists = _context.TbServiceProvider.Any(x => x.UserId == user.Id && x.Status == "قيد المراجعة");
            if (exists)
            {
                ModelState.AddModelError("", "تم إرسال الطلب مسبقًا.");
                return View("JoinAsProvider", serviceProviderModel);
            }

            var request = new ServiceProviderModel
            {
                UserId = user.Id,
                NameShop = serviceProviderModel.NameShop,
                Description = serviceProviderModel.Description,
                Status = "قيد المراجعة"
            };

            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            if (!Directory.Exists(wwwRootPath)) Directory.CreateDirectory(wwwRootPath);

            if (serviceProviderModel.Imagf1 != null)
            {
                string fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(serviceProviderModel.Imagf1.FileName);
                string filePath1 = Path.Combine(wwwRootPath, fileName1);
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await serviceProviderModel.Imagf1.CopyToAsync(stream);
                }
                request.Logo = fileName1;
            }

            if (serviceProviderModel.Imagf2 != null)
            {
                string fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(serviceProviderModel.Imagf2.FileName);
                string filePath2 = Path.Combine(wwwRootPath, fileName2);
                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await serviceProviderModel.Imagf2.CopyToAsync(stream);
                }
                request.PhotoRecord = fileName2;
            }

            _context.TbServiceProvider.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
