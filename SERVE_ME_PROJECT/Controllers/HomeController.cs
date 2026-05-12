using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Data;
using Microsoft.AspNetCore.Authorization;
using SERVE_ME_PROJECT.ViewModel;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServeMeContext _context;
        // ثابت لعدد المدونات المعروضة في كل صفحة
        private const int PageSize = 3;
        public HomeController(ILogger<HomeController> logger, ServeMeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            VmLandng vmLandng = new VmLandng();
            vmLandng.GeneralSettingsModel = _context.TbGeneralSettings.ToList();
            vmLandng.BlogModel = _context.TbBlog
                                        .OrderByDescending(b => b.SentDate)
                                        .Take(3)
                                        .ToList();
            vmLandng.ServiceProviderModel = _context.TbServiceProvider.ToList();
            return View(vmLandng);
        }

        //جلب
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                _context.TbContactUs.Add(model);
                await _context.SaveChangesAsync();
                ViewBag.Message = "تم إرسال رسالتك بنجاح!";
                return View();
            }
            return View(model);
        }

        /* public IActionResult Save(ContactUsModel contactUsModel)
         {
             if (!ModelState.IsValid)
               return Ok("Contact");
             contactUsModel.SentDate = DateTime.Now;
                 _context.Add(contactUsModel);
                 _context.SaveChanges();

             return Ok("Index");
         } */

        public IActionResult Search(string query)
        {
            var results = _context.TbService
                                  .Where(s => s.Name.Contains(query) || s.Description.Contains(query))
                                  .ToList();

            return View("SearchResults", results);
        }
        public IActionResult Details_Blog(int id)
        {
            var blog = _context.TbBlog.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            VmBlog vmBlog = new VmBlog
            {
                BlogModel = new List<BlogModel> { blog } // تمرير المدونة داخل قائمة
            };

            return View(vmBlog);
        }
        
        public IActionResult Policy()
        {
            return View();
        }
    }
}
