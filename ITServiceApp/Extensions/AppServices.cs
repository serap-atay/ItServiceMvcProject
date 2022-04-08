using ITServiceApp.MapperProfile;
using ITServiceApp.Models.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITServiceApp.Extensions
{
    public static class AppServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(AccountProfile));
            });

            return services;
        }
    }
}
