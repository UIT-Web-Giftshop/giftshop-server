using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.ServicesExtension
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                var jwtSchema = new OpenApiSecurityScheme()
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter ONLY `JWT token` here",
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSchema.Reference.Id, jwtSchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {jwtSchema, ArraySegment<string>.Empty}
                });
            });
            
            return services;
        }
    }
}