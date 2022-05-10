using Microsoft.Extensions.DependencyInjection;

namespace API.ServicesExtension
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });
            
            return services;
        }
    }
}