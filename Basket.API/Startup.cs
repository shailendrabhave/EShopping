using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
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
            services.AddApiVersioning();
            
            //Redis Settings
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");            
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateShoppingCartHandler).GetTypeInfo().Assembly));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<DiscountService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => {
                options.Address = new Uri(configuration.GetValue<string>("GRPCSettings:DiscountUrl"));
            });
            services.AddSwaggerGen(cfg => 
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "v1" });
            });

            services.AddHealthChecks().AddRedis(configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("/swagger/v1/swagger.json","Basket.API v1"));
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
