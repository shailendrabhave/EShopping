using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            services.AddApplicationServices();
            services.AddInfraServices(configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(cfg => {
                cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });
            services.AddHealthChecks().Services.AddDbContext<OrderContext>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
