using Common.Logging.Correlation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot.ApiGateway
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
            var authScheme = "EShoppingGatewayAuthScheme";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(authScheme,
                options =>
                {
                    options.Authority = "https://localhost:8009";
                    options.Audience = "EShoppingGateway";
                });
            services.AddOcelot();
            services.AddCacheManager();
        }

        public async void Configure(IApplicationBuilder app, IHostEnvironment env) 
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CORSPolicy");
            app.AddCorrelationMiddleware();
            app.UseRouting();
            app.UseEndpoints(e => {
                e.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello Ocelot");
                });
            });
            await app.UseOcelot();
        }
    }
}
