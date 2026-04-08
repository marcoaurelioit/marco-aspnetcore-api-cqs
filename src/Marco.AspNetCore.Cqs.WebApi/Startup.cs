using AutoMapper;
using Marco.AspNetCore.Cqs.Infra.Data.Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Marco.AspNetCore.Cqs.WebApi
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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Marco AspNetCore CQS API",
                    Version = "v1"
                });
            });

            services.AddAutoMapper(typeof(WebApiAutoMapperProfile));
            services.AddCustomApplicationServices();

            var sqlServerReadOnlySettings = Configuration.GetSection(nameof(SqlServerReadOnlySettings)).Get<SqlServerReadOnlySettings>();
            services.AddDapper(sqlServerReadOnlySettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marco AspNetCore CQS API v1");
                c.RoutePrefix = "swagger";
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
