using System;
using System.Text.Json;
using API.ServicesExtension;
using Application.Features.Products.Queries;
using Application.Mapping;
using Application.Middlewares;
using Application.PipelineBehaviors;
using Domain.Settings;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

            services.AddDistributedMemoryCache();
            services.AddSession(opts =>
            {
                opts.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            
            services.AddSwaggerService();
            services.AddAuthenticationService(Configuration);

            services.AddMediatR(typeof(GetPagingProductsHandler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetPagingProductQueryValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ExceptionHandlingMiddleware>();
            
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaveFlagRepository, SaveFlagRepository>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IMailService, MailService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            var appSettingsSection = Configuration.GetSection("ServicesSettings");
            services.Configure<CloudinarySettings>(appSettingsSection.GetSection("CloudinarySettings"));
            services.Configure<AuthenticationSettings>(appSettingsSection.GetSection("AuthenticationSettings"));
            services.Configure<MailSettings>(appSettingsSection.GetSection("MailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseSession();
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}