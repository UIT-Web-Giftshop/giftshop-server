using API.Commons;
using Microsoft.Extensions.DependencyInjection;

namespace API.ServicesExtension
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(Constants.CORS_ANY_ORIGIN_POLICY, builder =>
                {
                    builder.AllowAnyOrigin()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            return services;
        }
    }
}