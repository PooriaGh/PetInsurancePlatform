using Microsoft.AspNetCore.Identity;
using PetInsurancePlatform.IdentityApp.Data;

namespace PetInsurancePlatform.IdentityApp.Identity;

internal static class IdentityInstaller
{
    public static void AddIdentityProvider(this IServiceCollection services)
    {
        services
            .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedPhoneNumber = true;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.Cookie.MaxAge = options.ExpireTimeSpan;
        });
    }
}
