using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, ProvideApiInfo(description));
            }
        }

        private OpenApiInfo ProvideApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Basket API Microservice",
                Version = description.ApiVersion.ToString(),
                Description = "Fetches details about Basket",
                Contact = new OpenApiContact() { Name = "Shailendra Bhave", Email = "shail@xyz.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description = "API version is deprecated";
            }

            return info;
        }
    }
}
