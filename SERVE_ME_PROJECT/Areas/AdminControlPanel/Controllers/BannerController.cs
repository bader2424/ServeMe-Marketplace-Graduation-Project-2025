using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class BannerController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BannerController(ServeMeContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminControlPanel/Banner
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var provider = await _userManager.FindByIdAsync(userId);

            ViewBag.ProviderFullName = provider.FullName;
            ViewBag.ProviderImage = string.IsNullOrEmpty(provider.ProfileImagePath)
                ? "~/provider/images/pofl.jpg"
                : provider.ProfileImagePath;
            return View(await _context.TbBanner.ToListAsync());
        }

        // GET: AdminControlPanel/Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerModel = await _context.TbBanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerModel == null)
            {
                return NotFound();
            }

            return View(bannerModel);
        }

        // GET: AdminControlPanel/Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/Banner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create( BannerModel bannerModel)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.FirstOrDefault(); // الحصول على أول ملف مرفوع
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string name = Guid.NewGuid().ToString() + ext;
                    var path = Path.Combine("wwwroot", "img", name);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    bannerModel.UrlImage = name;
                }
                else
                {
                    // إذا لم يتم رفع صورة، يمكن تعيين مسار افتراضي أو ترك القيمة كما هي
                    bannerModel.UrlImage = "default.jpg"; // يمكنك تخصيص هذا حسب الحاجة
                }

                _context.Add(bannerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bannerModel);
        }

        /* public async Task<IActionResult> Create([Bind("Id,UrlImage")] BannerModel bannerModel)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files[0];
                string ext = Path.GetExtension(file.FileName);
                string name = Guid.NewGuid().ToString() + ext;
                var path = Path.Combine("wwwroot", "img", name);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                bannerModel.UrlImage = name;
                _context.Add(bannerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bannerModel);
        }
        */
        // GET: AdminControlPanel/Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerModel = await _context.TbBanner.FindAsync(id);
            if (bannerModel == null)
            {
                return NotFound();
            }
            return View(bannerModel);
        }

        // POST: AdminControlPanel/Banner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,UrlImage")] BannerModel bannerModel)
        {
            if (id != bannerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Form.Files.FirstOrDefault(); // الحصول على أول ملف مرفوع
                    if (file != null)
                    {
                        string ext = Path.GetExtension(file.FileName);
                        string name = Guid.NewGuid().ToString() + ext;
                        var path = Path.Combine("wwwroot", "img", name);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        bannerModel.UrlImage = name;
                    }
                    else
                    {
                        // إذا لم يتم رفع صورة، يمكن ترك القيمة كما هي (أي عدم تغيير الصورة)
                        // bannerModel.UrlImage تبقى كما هي من قاعدة البيانات.
                    }

                    _context.Update(bannerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerModelExists(bannerModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bannerModel);
        }

        /*  public async Task<IActionResult> Edit(int id, [Bind("Id,UrlImage")] BannerModel bannerModel)
          {
              if (id != bannerModel.Id)
              {
                  return NotFound();
              }

              if (ModelState.IsValid)
              {
                  try
                  {
                      var file = Request.Form.Files[0];
                      string ext = Path.GetExtension(file.FileName);
                      string name = Guid.NewGuid().ToString() + ext;
                      var path = Path.Combine("wwwroot", "img", name);
                      using (var stream = new FileStream(path, FileMode.Create))
                      {
                          file.CopyTo(stream);
                      }
                      bannerModel.UrlImage = name;
                      _context.Update(bannerModel);
                      await _context.SaveChangesAsync();
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!BannerModelExists(bannerModel.Id))
                      {
                          return NotFound();
                      }
                      else
                      {
                          throw;
                      }
                  }
                  return RedirectToAction(nameof(Index));
              }
              return View(bannerModel);
          }
        */


        // GET: AdminControlPanel/Banner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerModel = await _context.TbBanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bannerModel == null)
            {
                return NotFound();
            }

            return View(bannerModel);
        }

        // POST: AdminControlPanel/Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bannerModel = await _context.TbBanner.FindAsync(id);
            if (bannerModel != null)
            {
                _context.TbBanner.Remove(bannerModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerModelExists(int id)
        {
            return _context.TbBanner.Any(e => e.Id == id);
        }
    }
}
