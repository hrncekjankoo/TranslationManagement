using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using TranslationManagement.Api.Repositories;
using TranslationManagement.Api.Repositories.Contracts;
using TranslationManagement.Api.Services;
using TranslationManagement.Api.Services.Contracts;

namespace TranslationManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite("Data Source=TranslationAppDatabase.db"));

            AddRepositories(services);
            AddServices(services);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors();
        }
        
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ITranslationJobService, TranslationJobService>();
            services.AddScoped<ITranslatorManagementService, TranslatorManagementService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ITranslationJobRepository, TranslationJobRepository>();
            services.AddScoped<ITranslatorManagementRepository, TranslatorManagementRepository>();
        }
    }
}
