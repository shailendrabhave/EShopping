using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot.ApiGateway
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();
            services.AddCacheManager();
        }

        public async void Configure(IApplicationBuilder app, IHostEnvironment env) 
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }
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
