using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class CityModelsController : Controller
    {
        private readonly ServeMeContext _context;

        public CityModelsController(ServeMeContext context)
        {
            _context = context;
        }

        // GET: AdminControlPanel/CityModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbCity.ToListAsync());
        }

        // GET: AdminControlPanel/CityModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityModel = await _context.TbCity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cityModel == null)
            {
                return NotFound();
            }

            return View(cityModel);
        }

        // GET: AdminControlPanel/CityModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/CityModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityModel cityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cityModel);
        }

        // GET: AdminControlPanel/CityModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityModel = await _context.TbCity.FindAsync(id);
            if (cityModel == null)
            {
                return NotFound();
            }
            return View(cityModel);
        }

        // POST: AdminControlPanel/CityModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CityModel cityModel)
        {
            if (id != cityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityModelExists(cityModel.Id))
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
            return View(cityModel);
        }

        // GET: AdminControlPanel/CityModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cityModel = await _context.TbCity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cityModel == null)
            {
                return NotFound();
            }

            return View(cityModel);
        }

        // POST: AdminControlPanel/CityModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cityModel = await _context.TbCity.FindAsync(id);
            if (cityModel != null)
            {
                _context.TbCity.Remove(cityModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityModelExists(int id)
        {
            return _context.TbCity.Any(e => e.Id == id);
        }
    }
}
