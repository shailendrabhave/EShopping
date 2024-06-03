using Basket.API.Swagger;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging.Correlation;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Basket.API
{
    public class Startup
    {
        IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                options.ApiVersionReader = new MediaTypeApiVersionReader("ver");
                options.ApiVersionReader = new QueryStringApiVersionReader("api-version", "ver");
            });

            services.AddVersionedApiExplorer(options => 
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            //Redis Settings
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");            
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartHandler).GetTypeInfo().Assembly));
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<DiscountService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => {
                options.Address = new Uri(configuration.GetValue<string>("GRPCSettings:DiscountUrl"));
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options => 
            {
                options.OperationFilter<SwaggerDefaultValues>();
            });
            

            services.AddHealthChecks().AddRedis(configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
            services.AddMassTransit(config => 
            {
                config.UsingRabbitMq((ctx, cfg) => 
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => 
                {
                    foreach(var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions() 
                {
                    Predicate = _=> true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
