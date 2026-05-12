using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class GeneralSettingsModelsController : Controller
    {
        private readonly ServeMeContext _context;

        public GeneralSettingsModelsController(ServeMeContext context)
        {
            _context = context;
        }

        // GET: AdminControlPanel/GeneralSettingsModels
        public async Task<IActionResult> Index()
        {
            var GeneralSettings = await _context.TbGeneralSettings.ToListAsync();
            return View(GeneralSettings);
        }

        // GET: AdminControlPanel/GeneralSettingsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalSettingsModel = await _context.TbGeneralSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (generalSettingsModel == null)
            {
                return NotFound();
            }

            return View(generalSettingsModel);
        }

        // GET: AdminControlPanel/GeneralSettingsModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/GeneralSettingsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SiteName,SiteLogo,SiteDescription,ContactEmail,ContactPhone,Address,FacebookLink,TwitterLink,InstagramLink,LinkedInLink")] GeneralSettingsModel @generalSettingsModel)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files[0];
                string ext = Path.GetExtension(file.FileName);
                string name = Guid.NewGuid().ToString() + ext;
                var path = Path.Combine("wwwroot","img", name);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                @generalSettingsModel.SiteLogo = name;
                _context.Add(@generalSettingsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@generalSettingsModel);
        }

        // GET: AdminControlPanel/GeneralSettingsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalSettingsModel = await _context.TbGeneralSettings.FindAsync(id);
            if (generalSettingsModel == null)
            {
                return NotFound();
            }
            return View(generalSettingsModel);
        }

        // POST: AdminControlPanel/GeneralSettingsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SiteName,SiteLogo,SiteDescription,ContactEmail,ContactPhone,Address,FacebookLink,TwitterLink,InstagramLink,LinkedInLink")] GeneralSettingsModel generalSettingsModel)
        {
            if (id != generalSettingsModel.Id)
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
                    @generalSettingsModel.SiteLogo = name;
                    _context.Update(generalSettingsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneralSettingsModelExists(generalSettingsModel.Id))
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
            return View(generalSettingsModel);
        }

        // GET: AdminControlPanel/GeneralSettingsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalSettingsModel = await _context.TbGeneralSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (generalSettingsModel == null)
            {
                return NotFound();
            }

            return View(generalSettingsModel);
        }

        // POST: AdminControlPanel/GeneralSettingsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var generalSettingsModel = await _context.TbGeneralSettings.FindAsync(id);
            if (generalSettingsModel != null)
            {
                _context.TbGeneralSettings.Remove(generalSettingsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeneralSettingsModelExists(int id)
        {
            return _context.TbGeneralSettings.Any(e => e.Id == id);
        }
    }
}
