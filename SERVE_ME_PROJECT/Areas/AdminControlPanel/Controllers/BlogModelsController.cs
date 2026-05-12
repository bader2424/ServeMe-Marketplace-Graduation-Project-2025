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
    public class BlogModelsController : Controller
    {
        private readonly ServeMeContext _context;

        public BlogModelsController(ServeMeContext context)
        {
            _context = context;
        }

         
        
            // GET: AdminControlPanel/BlogModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbBlog.ToListAsync());
        }

        // GET: AdminControlPanel/BlogModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogModel = await _context.TbBlog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogModel == null)
            {
                return NotFound();
            }

            return View(blogModel);
        }

        // GET: AdminControlPanel/BlogModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/BlogModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,ImgUrl,SentDate")] BlogModel @blogModel)
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
                @blogModel.ImgUrl = name;
                _context.Add(@blogModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@blogModel);
        }

        // GET: AdminControlPanel/BlogModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogModel = await _context.TbBlog.FindAsync(id);
            if (blogModel == null)
            {
                return NotFound();
            }
            return View(blogModel);
        }

        // POST: AdminControlPanel/BlogModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,ImgUrl,SentDate")] BlogModel blogModel)
        {
            if (id != blogModel.Id)
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
                    blogModel.ImgUrl = name;
                    _context.Update(blogModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogModelExists(blogModel.Id))
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
            return View(blogModel);
        }

        // GET: AdminControlPanel/BlogModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogModel = await _context.TbBlog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogModel == null)
            {
                return NotFound();
            }

            return View(blogModel);
        }

        // POST: AdminControlPanel/BlogModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogModel = await _context.TbBlog.FindAsync(id);
            if (blogModel != null)
            {
                _context.TbBlog.Remove(blogModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogModelExists(int id)
        {
            return _context.TbBlog.Any(e => e.Id == id);
        }
    }
}
