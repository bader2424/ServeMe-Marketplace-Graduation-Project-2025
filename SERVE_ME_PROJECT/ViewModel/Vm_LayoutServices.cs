using SERVE_ME_PROJECT.Models;
namespace SERVE_ME_PROJECT.ViewModel
{
    public class Vm_LayoutServices
    {
        public List<BannerModel> @BannerModel { get; set; }
        public List<GeneralSettingsModel> @GeneralSettingsModel { get; set; }
        public List<ServiceProviderModel> @ServiceProviderModel { get; set; }
        public List<BlogModel> @BlogModel { get; set; }
        public List<ContactUsModel> @ContactUsModel { get; set; }

    }
}
