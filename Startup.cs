using BistHub.Api.Common;
using BistHub.Api.Data;
using BistHub.Api.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BistHub.Api
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
            services
                .AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(fv => {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BistHub.Api", Version = "v1" });
                //c.AddSwaggerSecurityDefinition(Configuration);
                c.AddSwaggerSecurityDefinition();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            // Configure configs
            services.Configure<FirebaseConfig>(Configuration.GetSection("Firebase"));
            services.Configure<GoogleSheetsConfig>(Configuration.GetSection("GoogleSheet"));

            // Configure logger
            services.AddSerilog(Configuration);

            // Configure authentication
            services.AddFirebaseAuthentication(Configuration["Firebase:AppName"]);

            // Configure EF Core
            services.AddDbContext<BistHubContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString"]).EnableSensitiveDataLogging());

            // Configure background jobs
            services.AddHostedService<Jobs.StockPriceCollectionJob>();
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BistHub.Api v1");
                    c.AddOAuthUi(Configuration["Firebase:ClientId"]);
                });
            }

            app.UseMiddleware<Middlewares.TraceLoggerMiddleware>();
            app.UseMiddleware<Middlewares.ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
