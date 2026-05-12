using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Data;
using Microsoft.AspNetCore.Authorization;
using SERVE_ME_PROJECT.ViewModel;
using SERVE_ME_PROJECT.Models;
using System.Drawing.Printing;

namespace SERVE_ME_PROJECT.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly ServeMeContext _context;

        // ثابت لعدد المدونات المعروضة في كل صفحة
        private const int PageSize = 3;
        public BlogController(ILogger<BlogController> logger, ServeMeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Blog(int page = 1)
        {
            // حجم الصفحة (عدد المدونات التي ستظهر في كل صفحة)
            const int PageSize = 3;

            // حساب إجمالي عدد المدونات
            var totalBlogs = _context.TbBlog.Count();

            // حساب المدونات التي سيتم عرضها بناءً على الصفحة الحالية
            var blogs = _context.TbBlog
                .OrderByDescending(b => b.SentDate) // ترتيب المدونات حسب التاريخ
                .Skip((page - 1) * PageSize) // تخطي المدونات السابقة
                .Take(PageSize) // أخذ عدد المدونات المحددة في الصفحة
                .ToList();

            // حساب العدد الإجمالي للصفحات
            var totalPages = (int)Math.Ceiling(totalBlogs / (double)PageSize);

            // إنشاء الـ ViewModel
            VmBlog vmBlog = new VmBlog
            {
                BlogModel = blogs,
                CurrentPage = page,
                TotalPages = totalPages
            };

            // إعادة البيانات للـ View
            return View(vmBlog);
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





    }
}
