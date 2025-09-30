using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Iottu.Api.Catalog.Helpers
{
    public class QueryParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            foreach (var param in context.MethodInfo.GetParameters())
            {
                if (param.Name == "page" || param.Name == "pageSize")
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = param.Name,
                        In = ParameterLocation.Query,
                        Required = false,
                        Schema = new OpenApiSchema { Type = "integer", Default = new Microsoft.OpenApi.Any.OpenApiInteger(param.Name == "page" ? 1 : 10) },
                        Description = param.Name == "page" ? "Número da página" : "Itens por página"
                    });
                }
            }
        }
    }
}
