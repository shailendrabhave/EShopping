using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging.Correlation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            var dbConnectionString = Configuration["DatabaseSettings:ConnectionString"];
            if(!String.IsNullOrEmpty(dbConnectionString))
            {
                services.AddHealthChecks()
                .AddMongoDb(dbConnectionString, "Catalog Mongodb Health Check", HealthStatus.Degraded);
            }            

            services.AddSwaggerGen(swagger => swagger.SwaggerDoc(name:"v1", new OpenApiInfo(){Title = "Catalog.API", Version = "v1" }));
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).GetTypeInfo().Assembly));
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHealthChecks(pattern: "/health", new HealthCheckOptions()
                {
                    Predicate =_ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
