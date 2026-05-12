using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Areas.Provider.Controllers
{
    [Area("Provider")]
    [Authorize(Roles = "provider")]
    public class CommentController : Controller

    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Provider/Comment
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);

            var servicesWithComments = await _context.TbService
                .Where(s => s.UserId == currentUserId && s.Comments.Any())
                .Include(s => s.Comments)
                .ToListAsync();

            return View(servicesWithComments);
        }
        public async Task<IActionResult> ServiceDetailsWithComments(int id)
        {
            var currentUserId = _userManager.GetUserId(User);

            var service = await _context.TbService
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == currentUserId);

            if (service == null)
                return NotFound();

            return View(service);
        }


    }
}
