using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Areas.Identity.Data;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class ContactUsController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly IEmailSender _emailSender;

        public ContactUsController(IEmailSender emailSender, ServeMeContext context)
        {
            _context = context;
            _emailSender = emailSender;

        }

        // GET: AdminControlPanel/ContactUs
        public IActionResult Index()
        {
            var messages = _context.TbContactUs.OrderByDescending(m => m.SentDate).ToList();
            return View(messages);
        }


        // GET: AdminControlPanel/ContactUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactUsModel = await _context.TbContactUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUsModel == null)
            {
                return NotFound();
            }

            return View(contactUsModel);
        }

        // GET: AdminControlPanel/ContactUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Title,Message,SentDate")] ContactUsModel contactUsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactUsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactUsModel);
        }

        // GET: AdminControlPanel/ContactUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactUsModel = await _context.TbContactUs.FindAsync(id);
            if (contactUsModel == null)
            {
                return NotFound();
            }
            return View(contactUsModel);
        }

        // POST: AdminControlPanel/ContactUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Title,Message,SentDate")] ContactUsModel contactUsModel)
        {
            if (id != contactUsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactUsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactUsModelExists(contactUsModel.Id))
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
            return View(contactUsModel);
        }

        // GET: AdminControlPanel/ContactUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactUsModel = await _context.TbContactUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUsModel == null)
            {
                return NotFound();
            }

            return View(contactUsModel);
        }

        // POST: AdminControlPanel/ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactUsModel = await _context.TbContactUs.FindAsync(id);
            if (contactUsModel != null)
            {
                _context.TbContactUs.Remove(contactUsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactUsModelExists(int id)
        {
            return _context.TbContactUs.Any(e => e.Id == id);
        }

        public IActionResult Reply(int id)
        {
            var message = _context.TbContactUs.FirstOrDefault(m => m.Id == id);
            if (message == null) return NotFound();

            var model = new VmReply
            {
                Email = message.Email,
                Subject = "رد على: " + message.Message
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Reply(VmReply model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var message = _context.TbContactUs.FirstOrDefault(m => m.Id == model.MessageId);
            if (message == null) return NotFound();

            // إرسال الإيميل
            await _emailSender.SendEmailAsync(
                model.Email,
                model.Subject,
                model.Body
            );

            message.IsReplied = true;
            _context.Update(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إرسال الرد بنجاح.";
            return RedirectToAction("Index");
        }


    }
}
