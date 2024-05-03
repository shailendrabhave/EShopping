using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
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
            services.AddApiVersioning();
            services.AddCors(options =>
            {
                options.AddPolicy("", pollicy =>
                {
                    pollicy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            var dbConnectionString = Configuration["DatabaseSettings:ConnectionString"];
            if(!String.IsNullOrEmpty(dbConnectionString))
            {
                services.AddHealthChecks()
                .AddMongoDb(dbConnectionString, "Catalog Mongodb Health Check", HealthStatus.Degraded);
            }            

            services.AddSwaggerGen(swagger => swagger.SwaggerDoc(name:"v1", new OpenApiInfo(){Title = "Catalog.API", Version = "v1" }));
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).GetTypeInfo().Assembly));
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();

            //Identity Server Changes
            var userPolicy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser().Build();

            services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter(userPolicy));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://id-local.eshopping.com:44344";
                    options.Audience = "Catalog";
                });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanRead", policy =>
            //    policy.RequireClaim("scope", "catalogapi.read"));

            //    options.AddPolicy("CanWrite", policy =>
            //    policy.RequireClaim("scope", "catalogapi.write"));
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var nginxPath = "/catalog";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var forwardedHeaderOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                };
                forwardedHeaderOptions.KnownNetworks.Clear();
                app.UseForwardedHeaders(forwardedHeaderOptions);
                app.UseSwagger();
                app.UseSwaggerUI(cfg =>
                 {
                     foreach (var description in provider.ApiVersionDescriptions)
                     {
                         cfg.SwaggerEndpoint(
                             $"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                             $"Catalog API {description.GroupName.ToUpperInvariant()}");
                         cfg.RoutePrefix = string.Empty;
                     }
                     cfg.DocumentTitle = "Catalog API Documentation";
                 });

                app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => 
            { 
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
