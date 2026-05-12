using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Controllers
{
    public class SettingController : Controller
    {
        private readonly ServeMeContext _context;

        public SettingController(ServeMeContext context)
        {
            _context = context;
        }

        
        // GET:  
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Loginnew()
        {
            return View();
        }

        public IActionResult JoingProvider()
        {
            return View();
        }

        // POST:  

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Name,Email,Password,Phone")] ApplicationUser userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(userModel);
        }

        private bool UserExists(int id)
        {
            return _context.TbContactUs.Any(e => e.Id == id);
        }
    }
}

  
 