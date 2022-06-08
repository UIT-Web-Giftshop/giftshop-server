using System;
using System.Text.Json;
using Amazon.S3;
using API.Commons;
using API.ServicesExtension;
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
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

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
                opts.IdleTimeout = TimeSpan.FromMinutes(double.Parse(Configuration["ServicesSettings:AuthenticationSettings:ExpirationMinutes"]));
            });
            
            services.AddSwaggerService();
            services.AddAuthenticationService(Configuration);
            services.AddCorsService();
            services.AddAWSService<IAmazonS3>();

            services.AddMediatR(typeof(GetPagingProductsQueryHandler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetPagingProductsQueryValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ExceptionHandlingMiddleware>();
            
            services.AddSingleton<IMongoContext, MongoContext>();
            
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICounterRepository, CounterRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IWishlistRepository, WishlistRepository>();
            services.AddTransient<IVerifyTokenRepository, VerifyTokenRepository>();
            
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAWSS3BucketService, AWSS3BucketService>();
            
            var appSettingsSection = Configuration.GetSection("ServicesSettings");
            services.Configure<CloudinarySettings>(appSettingsSection.GetSection("CloudinarySettings"));
            services.Configure<AuthenticationSettings>(appSettingsSection.GetSection("AuthenticationSettings"));
            services.Configure<MailSettings>(appSettingsSection.GetSection("MailSettings"));
            services.Configure<AWSS3Settings>(appSettingsSection.GetSection("S3Bucket"));
            services.Configure<DomainSettings>(appSettingsSection.GetSection("AppDomain"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseCors(Constants.CORS_ANY_ORIGIN_POLICY);
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseSerilogRequestLogging(opts =>
            {
                opts.MessageTemplate =
                    "{IpAddress} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                opts.EnrichDiagnosticContext = (context, httpContext) =>
                {
                    context.Set("IpAddress", httpContext.Connection.RemoteIpAddress);
                };
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}