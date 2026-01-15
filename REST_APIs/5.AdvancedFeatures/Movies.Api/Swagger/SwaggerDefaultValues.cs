using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            foreach (var responseType in apiDescription.SupportedResponseTypes)
            {
                var statusCode = responseType.IsDefaultResponse
                    ? "default"
                    : responseType.StatusCode.ToString();

                if (operation.Responses.ContainsKey(statusCode))
                {
                    var response = operation.Responses[statusCode];
                    foreach (var contentType in response.Content.Keys)
                    {
                        if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                        {
                            response.Content.Remove(contentType);
                        }
                    }
                }

                if (operation.Parameters is null)
                {
                    return;
                }

                foreach (var parameter in operation.Parameters)
                {
                    var description = apiDescription.ParameterDescriptions
                        .First(p => p.Name == parameter.Name);
                    
                    parameter.Description ??= description.ModelMetadata?.Description;

                    if (parameter.Schema.Default is null && description.DefaultValue is not null)
                    {
                        var json = JsonSerializer.Serialize(
                            description.DefaultValue,
                            description.ModelMetadata!.ModelType);

                        parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                    }

                    parameter.Required |= description.IsRequired;
                }
            }
        }
    }
}
