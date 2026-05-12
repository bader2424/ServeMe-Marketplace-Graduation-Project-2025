using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Controllers
{
    [AllowAnonymous]
    public class ContactUsModelsController : Controller
    {
        private readonly ServeMeContext _context;

        public ContactUsModelsController(ServeMeContext context)
        {
            _context = context;
        }


        // GET: ContactUsModels/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // POST: ContactUsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,Name,Email,Title,Message,SentDate")] ContactUsModel contactUsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactUsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Contact));
            }
            return View(contactUsModel);
        }

        private bool ContactUsModelExists(int id)
        {
            return _context.TbContactUs.Any(e => e.Id == id);
        }
    }
}
