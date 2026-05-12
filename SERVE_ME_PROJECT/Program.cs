using SERVE_ME_PROJECT.Models;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SERVE_ME_PROJECT.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using SERVE_ME_PROJECT.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using SERVE_ME_PROJECT.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// استرجاع سلسلة الاتصال من الإعدادات
string conStr = builder.Configuration.GetConnectionString("DefaultConnetion") ?? "";

// إضافة DbContext وتوصيله بقاعدة البيانات
builder.Services.AddDbContext<ServeMeContext>(options => options.UseSqlServer(conStr));

// إضافة Identity (التحقق من المستخدمين والأدوار)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ServeMeContext>()
.AddRoles<IdentityRole>() // لإضافة دعم الأدوار
.AddDefaultTokenProviders();

builder.Services.Configure<AuthorizationOptions>(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// إضافة باقي الخدمات الأخرى
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// تخصيص إعدادات ملفات تعريف الارتباط (Cookies)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

//builder.Services.AddTransient<IEmailSender, DummyEmailSender>();

// بناء التطبيق
var app = builder.Build();

await ApplyDatabaseMigrationsAsync(app.Services);
await IdentitySeedData.SeedRolesAsync(app.Services);

// إعداد خط الأنابيب HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // إضافة مصادقة المستخدمين
app.UseAuthorization();   // إضافة التفويض (الأذونات)

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 404)
    {
        response.Redirect("/Error/NotFound");
    }
    else if (response.StatusCode == 403)
    {
        response.Redirect("/Error/AccessDenied");
    }
});

// مسارات التحكم الخاصة بـ Admin و Provider
app.MapControllerRoute(
    name: "AdminControlPanel",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Provider",
    pattern: "{area:exists}/{controller=Home}/{action=Staticts}/{id?}");

app.MapControllerRoute(
    name: "Identity",
    pattern: "{area:exists}/{controller=Home}/{action=Staticts}/{id?}");

// المسار الافتراضي للتطبيق
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

static async Task ApplyDatabaseMigrationsAsync(IServiceProvider services)
{
    const int maxAttempts = 12;

    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ServeMeContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            await context.Database.MigrateAsync();
            return;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            logger.LogWarning(ex, "Database is not ready. Retrying migration attempt {Attempt}/{MaxAttempts}.", attempt, maxAttempts);
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    await context.Database.MigrateAsync();
}
