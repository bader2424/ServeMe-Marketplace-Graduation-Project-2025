using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Data
{
    public static class IdentitySeedData
    {
        private const string AdminEmail = "admin@serveme.local";
        private const string ProviderEmail = "provider@serveme.local";
        private const string CustomerEmail = "customer@serveme.local";

        private static readonly string[] RequiredRoles = ["admin", "provider", "customer "];

        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ServeMeContext>();

            foreach (var roleName in RequiredRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var admin = await EnsureUserAsync(userManager, AdminEmail, "مدير النظام", "المكلا", "Admin@12345", "admin");
            var provider = await EnsureUserAsync(userManager, ProviderEmail, "مزود خدمة تجريبي", "الشحر", "Provider@12345", "provider");
            var customer = await EnsureUserAsync(userManager, CustomerEmail, "عميل تجريبي", "المكلا", "Customer@12345", "customer ");

            await EnsureReferenceDataAsync(context);
            await EnsureWalletAsync(context, admin.Id, 0);
            await EnsureWalletAsync(context, provider.Id, 0);
            await EnsureWalletAsync(context, customer.Id, 25000);
            await EnsureSiteContentAsync(context, provider.Id, customer.Id);
            await EnsureExpandedCatalogAsync(context, provider.Id, customer.Id);
        }

        private static async Task<ApplicationUser> EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string fullName,
            string city,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName,
                    City = city,
                    PhoneNumber = "777000000",
                    ProfileImagePath = string.Empty,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Failed to create seed user {email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }

            return user;
        }

        private static async Task EnsureReferenceDataAsync(ServeMeContext context)
        {
            await context.Database.ExecuteSqlRawAsync("""
                SET IDENTITY_INSERT [TbTypeservice] ON;
                IF NOT EXISTS (SELECT 1 FROM [TbTypeservice] WHERE [Id] = 1)
                    INSERT INTO [TbTypeservice] ([Id], [Name]) VALUES (1, N'بيع');
                IF NOT EXISTS (SELECT 1 FROM [TbTypeservice] WHERE [Id] = 2)
                    INSERT INTO [TbTypeservice] ([Id], [Name]) VALUES (2, N'تأجير');
                SET IDENTITY_INSERT [TbTypeservice] OFF;

                SET IDENTITY_INSERT [TbOrderStatModel] ON;
                IF NOT EXISTS (SELECT 1 FROM [TbOrderStatModel] WHERE [Id] = 1)
                    INSERT INTO [TbOrderStatModel] ([Id], [Name]) VALUES (1, N'قيد الانتظار');
                IF NOT EXISTS (SELECT 1 FROM [TbOrderStatModel] WHERE [Id] = 2)
                    INSERT INTO [TbOrderStatModel] ([Id], [Name]) VALUES (2, N'مقبول');
                IF NOT EXISTS (SELECT 1 FROM [TbOrderStatModel] WHERE [Id] = 3)
                    INSERT INTO [TbOrderStatModel] ([Id], [Name]) VALUES (3, N'مكتمل');
                SET IDENTITY_INSERT [TbOrderStatModel] OFF;

                SET IDENTITY_INSERT [TbCategory] ON;
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 45)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (45, N'مطاعم');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 46)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (46, N'فنادق');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 47)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (47, N'فنانين');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 48)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (48, N'قاعات');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 49)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (49, N'مصورين');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 50)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (50, N'هدايا');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 51)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (51, N'فرق تنظيم');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 52)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (52, N'مشاغل خياطة');
                IF NOT EXISTS (SELECT 1 FROM [TbCategory] WHERE [Id] = 53)
                    INSERT INTO [TbCategory] ([Id], [Name]) VALUES (53, N'تنسيق حفلات');
                SET IDENTITY_INSERT [TbCategory] OFF;
                """);

            var cityNames = new[] { "المكلا", "الشحر", "الغيل" };
            foreach (var cityName in cityNames)
            {
                if (!await context.TbCity.AnyAsync(city => city.Name == cityName))
                {
                    context.TbCity.Add(new CityModel { Name = cityName });
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task EnsureSiteContentAsync(ServeMeContext context, string providerUserId, string customerUserId)
        {
            if (!await context.TbGeneralSettings.AnyAsync())
            {
                context.TbGeneralSettings.Add(new GeneralSettingsModel
                {
                    SiteName = "أخدمني",
                    SiteLogo = "35f16b7f-827d-4df2-87f9-67696de3ea1e.png",
                    SiteDescription = "منصة خدمات اجتماعية تربط العملاء بمزودي الخدمات لتنظيم المناسبات والرحلات بسهولة.",
                    ContactEmail = "info@serveme.local",
                    ContactPhone = "777000000",
                    Address = "حضرموت - المكلا",
                    FacebookLink = "https://example.com/facebook",
                    TwitterLink = "https://example.com/twitter",
                    InstagramLink = "https://example.com/instagram",
                    LinkedInLink = "https://example.com/linkedin"
                });
            }

            if (!await context.TbBanner.AnyAsync())
            {
                context.TbBanner.AddRange(
                    new BannerModel { Title = "خدمات المناسبات", UrlImage = "88fe525f-1101-4b5b-9351-1342240c1e15.jpg" },
                    new BannerModel { Title = "مزودون موثوقون", UrlImage = "67ff4716-868f-4282-af83-5a33fb496b46.jpg" },
                    new BannerModel { Title = "تنظيم متكامل", UrlImage = "8e02bf23-12da-479b-a974-8dde989ee185.jpg" });
            }

            if (!await context.TbBlog.AnyAsync())
            {
                context.TbBlog.AddRange(
                    new BlogModel
                    {
                        Title = "كيف تختار مزود الخدمة المناسب لمناسبتك؟",
                        Content = "ابدأ بتحديد الميزانية، ثم قارن بين التقييمات والصور والخدمات المتاحة قبل تأكيد الطلب.",
                        ImgUrl = "02b13a0e-e1f3-41e2-abe2-ce96638198d9.jpg",
                        SentDate = DateTime.UtcNow.AddDays(-2)
                    },
                    new BlogModel
                    {
                        Title = "خطوات بسيطة لتنظيم حفل تخرج ناجح",
                        Content = "اختر القاعة، حدد المصور، وجهز الهدايا والتنسيق مسبقا حتى تكون التجربة مريحة.",
                        ImgUrl = "2db0d59c-1ff3-4dc4-b329-9d72e0d5285f.jpg",
                        SentDate = DateTime.UtcNow.AddDays(-1)
                    },
                    new BlogModel
                    {
                        Title = "لماذا المحفظة الإلكترونية مفيدة داخل المنصة؟",
                        Content = "المحفظة تساعد على متابعة المدفوعات والطلبات وتحفظ سجل العمليات بشكل واضح.",
                        ImgUrl = "70ddce86-8355-4519-b8cd-6acdd65fb388.jpg",
                        SentDate = DateTime.UtcNow
                    });
            }

            var provider = await context.TbServiceProvider.FirstOrDefaultAsync(item => item.UserId == providerUserId);
            if (provider == null)
            {
                provider = new ServiceProviderModel
                {
                    NameShop = "متجر مزود الخدمة التجريبي",
                    Description = "مزود خدمة تجريبي لعرض كيفية عمل المنصة مع الخدمات والطلبات.",
                    Logo = "755672cf-3549-42a7-8c09-589c9fc4e587.jpg",
                    PhotoRecord = "9f6446b1-2cdd-43a3-ade3-c077a1bdca61.jpg",
                    JoinDate = DateTime.UtcNow,
                    UserId = providerUserId,
                    Status = "مقبول"
                };

                context.TbServiceProvider.Add(provider);
                await context.SaveChangesAsync();
            }

            await EnsureServicesAsync(context, providerUserId, customerUserId);
        }

        private static async Task EnsureServicesAsync(ServeMeContext context, string providerUserId, string customerUserId)
        {
            if (await context.TbService.AnyAsync())
            {
                return;
            }

            var mukallaId = await context.TbCity.Where(city => city.Name == "المكلا").Select(city => city.Id).FirstAsync();
            var shihrId = await context.TbCity.Where(city => city.Name == "الشحر").Select(city => city.Id).FirstAsync();
            var ghailId = await context.TbCity.Where(city => city.Name == "الغيل").Select(city => city.Id).FirstAsync();

            var seedServices = new[]
            {
                new { Name = "بوفيه مناسبات متكامل", CategoryId = 45, CityId = mukallaId, TypeId = 1, Price = 1800m, Image = "/img/88fe525f-1101-4b5b-9351-1342240c1e15.jpg" },
                new { Name = "قاعة فندقية للضيافة", CategoryId = 46, CityId = shihrId, TypeId = 2, Price = 3500m, Image = "/img/67ff4716-868f-4282-af83-5a33fb496b46.jpg" },
                new { Name = "فرقة فنية للمناسبات", CategoryId = 47, CityId = ghailId, TypeId = 1, Price = 2200m, Image = "/img/8e02bf23-12da-479b-a974-8dde989ee185.jpg" },
                new { Name = "قاعة احتفالات مزينة", CategoryId = 48, CityId = mukallaId, TypeId = 2, Price = 5000m, Image = "/img/02b13a0e-e1f3-41e2-abe2-ce96638198d9.jpg" },
                new { Name = "جلسة تصوير احترافية", CategoryId = 49, CityId = shihrId, TypeId = 1, Price = 1200m, Image = "/img/2db0d59c-1ff3-4dc4-b329-9d72e0d5285f.jpg" },
                new { Name = "هدايا تخرج فاخرة", CategoryId = 50, CityId = ghailId, TypeId = 1, Price = 650m, Image = "/img/70ddce86-8355-4519-b8cd-6acdd65fb388.jpg" },
                new { Name = "فريق تنظيم فعالية", CategoryId = 51, CityId = mukallaId, TypeId = 1, Price = 2800m, Image = "/img/38b7f7f7-534f-41c6-b4b7-32cff8a32962.jpg" },
                new { Name = "تفصيل فساتين مناسبات", CategoryId = 52, CityId = shihrId, TypeId = 1, Price = 1500m, Image = "/img/495fa17e-e07d-428e-b5c9-75c52add494e.jpg" },
                new { Name = "تنسيق حفل كامل", CategoryId = 53, CityId = mukallaId, TypeId = 1, Price = 4200m, Image = "/img/5a944294-6884-4fc5-b72c-2aab8e3d8827.jpg" }
            };

            foreach (var item in seedServices)
            {
                var service = new ServiceModel
                {
                    Name = item.Name,
                    Description = "خدمة تجريبية كاملة التفاصيل لاختبار عرض الخدمات والطلب والتقييم داخل المنصة.",
                    CreateDate = DateTime.UtcNow,
                    Price = item.Price,
                    TypeServiceId = item.TypeId,
                    CategoryId = item.CategoryId,
                    CityId = item.CityId,
                    UserId = providerUserId,
                    IsApproved = true
                };

                service.Images.Add(new ServiceImgeModel { ImagePath = item.Image });
                service.Comments.Add(new CommentModel
                {
                    Content = "خدمة ممتازة وتجربة واضحة داخل المنصة.",
                    RatingValue = 5,
                    CommentDate = DateTime.UtcNow,
                    UserId = customerUserId
                });

                context.TbService.Add(service);
            }

            await context.SaveChangesAsync();
        }

        private static async Task EnsureWalletAsync(ServeMeContext context, string userId, decimal balance)
        {
            if (await context.Wallets.AnyAsync(wallet => wallet.UserId == userId))
            {
                return;
            }

            context.Wallets.Add(new Wallet
            {
                UserId = userId,
                CurrentBalance = balance,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Status = "Active",
                Notes = "Seeded demo wallet"
            });

            await context.SaveChangesAsync();
        }

        private static async Task EnsureExpandedCatalogAsync(ServeMeContext context, string providerUserId, string customerUserId)
        {
            var cityId = await context.TbCity.Select(city => city.Id).FirstAsync();

            var catalog = new[]
            {
                new DemoService("Wedding buffet silver package", 45, 1, 1450m, "/img/88fe525f-1101-4b5b-9351-1342240c1e15.jpg"),
                new DemoService("Wedding buffet gold package", 45, 1, 2400m, "/img/02b13a0e-e1f3-41e2-abe2-ce96638198d9.jpg"),
                new DemoService("Graduation dinner service", 45, 1, 1750m, "/img/2db0d59c-1ff3-4dc4-b329-9d72e0d5285f.jpg"),
                new DemoService("Outdoor catering station", 45, 1, 1950m, "/img/70ddce86-8355-4519-b8cd-6acdd65fb388.jpg"),

                new DemoService("Beach hotel hall rental", 46, 2, 3800m, "/img/67ff4716-868f-4282-af83-5a33fb496b46.jpg"),
                new DemoService("Luxury suite for bridal prep", 46, 2, 2600m, "/img/8e02bf23-12da-479b-a974-8dde989ee185.jpg"),
                new DemoService("Graduation guest rooms", 46, 2, 2100m, "/img/38b7f7f7-534f-41c6-b4b7-32cff8a32962.jpg"),
                new DemoService("Family resort day package", 46, 2, 2900m, "/img/495fa17e-e07d-428e-b5c9-75c52add494e.jpg"),

                new DemoService("Traditional music band", 47, 1, 1800m, "/img/5a944294-6884-4fc5-b72c-2aab8e3d8827.jpg"),
                new DemoService("Graduation ceremony host", 47, 1, 950m, "/img/9f6446b1-2cdd-43a3-ade3-c077a1bdca61.jpg"),
                new DemoService("Wedding DJ and sound system", 47, 2, 2300m, "/img/1c925a50-a55c-4a55-a8b7-4ae51f75bdaf.jpg"),
                new DemoService("Live oud performance", 47, 1, 1600m, "/img/2414300f-3624-46b0-9a62-d32fb048f16f.jpg"),

                new DemoService("Classic wedding hall", 48, 2, 5200m, "/img/476340ee-71a5-4408-a678-bd37aa7b8a2d.jpg"),
                new DemoService("Small engagement hall", 48, 2, 2800m, "/img/30c1df44-5e0e-4159-b0c9-c0e7e2082bb2.webp"),
                new DemoService("Graduation stage hall", 48, 2, 3400m, "/img/29188c2c-793c-412e-9c52-ea12163aa423.webp"),
                new DemoService("Outdoor celebration venue", 48, 2, 4500m, "/img/39eb4e28-80ff-450d-a293-96a26067a662.webp"),

                new DemoService("Wedding photo session", 49, 1, 1500m, "/img/677bed45-5189-4448-9630-fda588c785ed.jpg"),
                new DemoService("Graduation portrait package", 49, 1, 900m, "/img/8603bc92-2bf0-4683-8e5b-d111dc1ec830.jpg"),
                new DemoService("Event video coverage", 49, 1, 2500m, "/img/8ac811fc-398d-4a50-992f-6e5f8fa80783.jpg"),
                new DemoService("Drone photography add-on", 49, 1, 1300m, "/img/909147da-24aa-4471-8260-68eb074c0f54.jpg"),

                new DemoService("Graduation gift boxes", 50, 1, 450m, "/img/1f49027f-11fe-47cb-b2a4-6fbd7aed089e.jpg"),
                new DemoService("Wedding guest favors", 50, 1, 700m, "/img/47f098bd-dba1-4fb6-bac6-7cc87d413f22.jpg"),
                new DemoService("Luxury flower bouquet", 50, 1, 350m, "/img/5d77da18-e330-4e4c-b417-8d7a604e0d81.jpg"),
                new DemoService("Custom invitation cards", 50, 1, 550m, "/img/9dc416d8-f563-4576-bc55-a0fdd09d0a18.jpg"),

                new DemoService("Full event organizer team", 51, 1, 3900m, "/img/1ba89a2f-83ca-45ee-be53-1fe4cf7a922c.jpg"),
                new DemoService("Reception coordination team", 51, 1, 1800m, "/img/39223ccd-97d1-424a-8f12-2cdfaba9708f.JPG"),
                new DemoService("Graduation ceremony planners", 51, 1, 2200m, "/img/5a44b50a-e786-410c-b932-bc5f8e284609.jpg"),
                new DemoService("Bride entrance coordination", 51, 1, 1300m, "/img/64009a98-18ee-462c-8c8d-b09dfde76fbe.jpg"),

                new DemoService("Custom evening dress tailoring", 52, 1, 1700m, "/img/115e6dac-1c65-4dd6-a174-12a625ac581c.webp"),
                new DemoService("Men suit rental", 52, 2, 850m, "/img/839cb76f-8129-4d6d-a1a3-04204efc3745.webp"),
                new DemoService("Graduation gown tailoring", 52, 1, 650m, "/img/8630c33b-8835-418e-92e2-1751c54cb3d0.webp"),
                new DemoService("Bridal dress fitting service", 52, 1, 2400m, "/img/5df6be17-c6b4-4c33-bf73-1c1ea855f3a7.webp"),

                new DemoService("Complete wedding decoration", 53, 1, 5200m, "/img/6334e004-0d76-4aff-b988-68fdf9491b9c.png"),
                new DemoService("Graduation table styling", 53, 1, 1200m, "/img/35f16b7f-827d-4df2-87f9-67696de3ea1e.png"),
                new DemoService("Flower arch installation", 53, 1, 1750m, "/img/6a9d4c78-c748-4a38-9b0f-254d07f1e804.png"),
                new DemoService("Stage backdrop design", 53, 1, 2100m, "/img/8806a409-9995-45bb-bba9-956283359e07.png"),

                new DemoService("Tourism seafood dinner package", 45, 1, 1250m, "/img/88fe525f-1101-4b5b-9351-1342240c1e15.jpg"),
                new DemoService("Tourism mountain picnic catering", 45, 1, 980m, "/img/70ddce86-8355-4519-b8cd-6acdd65fb388.jpg"),
                new DemoService("Hotel weekend tourism package", 46, 2, 3100m, "/img/67ff4716-868f-4282-af83-5a33fb496b46.jpg"),
                new DemoService("Family beach resort booking", 46, 2, 3600m, "/img/8e02bf23-12da-479b-a974-8dde989ee185.jpg"),
                new DemoService("Heritage village music night", 47, 1, 1400m, "/img/5a944294-6884-4fc5-b72c-2aab8e3d8827.jpg"),
                new DemoService("Tour guide and local host", 47, 1, 750m, "/img/9f6446b1-2cdd-43a3-ade3-c077a1bdca61.jpg"),
                new DemoService("Conference hall for tourism groups", 48, 2, 2400m, "/img/476340ee-71a5-4408-a678-bd37aa7b8a2d.jpg"),
                new DemoService("Desert camp evening venue", 48, 2, 3300m, "/img/39eb4e28-80ff-450d-a293-96a26067a662.webp"),
                new DemoService("Travel photography walk", 49, 1, 850m, "/img/677bed45-5189-4448-9630-fda588c785ed.jpg"),
                new DemoService("Tourism video story package", 49, 1, 1900m, "/img/8ac811fc-398d-4a50-992f-6e5f8fa80783.jpg"),
                new DemoService("Local souvenir gift basket", 50, 1, 300m, "/img/1f49027f-11fe-47cb-b2a4-6fbd7aed089e.jpg"),
                new DemoService("Custom travel welcome boxes", 50, 1, 520m, "/img/47f098bd-dba1-4fb6-bac6-7cc87d413f22.jpg"),
                new DemoService("Tour group coordination team", 51, 1, 2100m, "/img/1ba89a2f-83ca-45ee-be53-1fe4cf7a922c.jpg"),
                new DemoService("Airport reception team", 51, 1, 1150m, "/img/39223ccd-97d1-424a-8f12-2cdfaba9708f.JPG"),
                new DemoService("Traditional outfit rental", 52, 2, 650m, "/img/839cb76f-8129-4d6d-a1a3-04204efc3745.webp"),
                new DemoService("Custom cultural costume tailoring", 52, 1, 1100m, "/img/115e6dac-1c65-4dd6-a174-12a625ac581c.webp"),
                new DemoService("Tourism welcome event decoration", 53, 1, 1600m, "/img/6334e004-0d76-4aff-b988-68fdf9491b9c.png"),
                new DemoService("Outdoor tourism booth styling", 53, 1, 1350m, "/img/35f16b7f-827d-4df2-87f9-67696de3ea1e.png")
            };

            foreach (var item in catalog)
            {
                if (await context.TbService.AnyAsync(service => service.Name == item.Name))
                {
                    continue;
                }

                var service = new ServiceModel
                {
                    Name = item.Name,
                    Description = "Demo service with image, price, category, city, provider, and comments so the website feels populated while browsing.",
                    CreateDate = DateTime.UtcNow,
                    Price = item.Price,
                    TypeServiceId = item.TypeServiceId,
                    CategoryId = item.CategoryId,
                    CityId = cityId,
                    UserId = providerUserId,
                    IsApproved = true
                };

                service.Images.Add(new ServiceImgeModel { ImagePath = item.ImagePath });
                service.Comments.Add(new CommentModel
                {
                    Content = "Useful demo service for browsing and recording the website.",
                    RatingValue = 5,
                    CommentDate = DateTime.UtcNow,
                    UserId = customerUserId
                });

                context.TbService.Add(service);
            }

            await context.SaveChangesAsync();
        }

        private sealed record DemoService(string Name, int CategoryId, int TypeServiceId, decimal Price, string ImagePath);
    }
}
