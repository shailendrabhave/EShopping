using Basket.API.Swagger;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging.Correlation;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        readonly IConfiguration configuration;
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
                    options.Audience = "Basket";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var nginxPath = "/basket";
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
                    foreach(var description in provider.ApiVersionDescriptions)
                    {
                        cfg.SwaggerEndpoint(
                            $"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                            $"Basket API {description.GroupName.ToUpperInvariant()}");
                        cfg.RoutePrefix = string.Empty;
                    }
                    cfg.DocumentTitle = "Basket API Documentation";
                });
            }

            app.UseRouting();
            app.UseAuthentication();    
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
