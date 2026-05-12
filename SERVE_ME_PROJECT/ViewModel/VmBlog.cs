using SERVE_ME_PROJECT.Models;
namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmBlog
    {
        public List<BlogModel> @BlogModel { get; set; }
        public int CurrentPage { get; set; } // الصفحة الحالية
        public int TotalPages { get; set; } // إجمالي الصفحات

    }
}
