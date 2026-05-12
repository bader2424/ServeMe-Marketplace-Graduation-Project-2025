using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.ViewModels
{
    public class ChangeUserTypeViewModel
    {
        public string UserId { get; set; }
        [BindNever]
        public string? FullName { get; set; }

        [BindNever]
        public IList<string>? CurrentRoles { get; set; }


        [Required(ErrorMessage = "يجب اختيار نوع المستخدم")]
        public string SelectedRole { get; set; }

        [BindNever]

        public List<string> AllRoles { get; set; } = new List<string>();
    }
}
