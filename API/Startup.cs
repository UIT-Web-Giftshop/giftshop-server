using System.Text.Json;
using Amazon.S3;
using API.ServicesExtension;
using Application.Features.Products.Queries;
using Application.Features.Products.Queries.GetPagingProducts;
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
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => 
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

            services.AddSwaggerService();
            services.AddAuthenticationService(Configuration);

            services.AddMediatR(typeof(GetPagingProductsHandler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetPagingProductsQueryValidator).Assembly);
            services.AddAWSService<IAmazonS3>();
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ExceptionHandlingMiddleware>();
            
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaveFlagRepository, SaveFlagRepository>();
            services.AddScoped<IAWSS3BucketService, AWSS3BucketService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            var appSettingsSection = Configuration.GetSection("ServicesSettings");
            services.Configure<AWSS3Settings>(appSettingsSection.GetSection("AWSS3Settings"));
            services.Configure<AuthenticationSettings>(appSettingsSection.GetSection("AuthenticationSettings"));
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}