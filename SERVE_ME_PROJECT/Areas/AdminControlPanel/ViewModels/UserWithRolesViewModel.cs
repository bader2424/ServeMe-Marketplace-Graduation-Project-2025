using SERVE_ME_PROJECT.Models;
using System.Collections.Generic;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.ViewModels
{
    public class UserWithRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
