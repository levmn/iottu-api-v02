using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Shared.Iottu.Contracts.DTOs;

namespace Web.Iottu.Api.Catalog.Helpers
{
    public class ExamplesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CreateMotoDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["placa"] = new OpenApiString("HOQ-6915"),
                    ["modelo"] = new OpenApiString("Honda CG 160 | Mottu-E"),
                    ["chassi"] = new OpenApiString("9C2KC1670JR123456"),
                    ["numeroMotor"] = new OpenApiString("KC16E-1234567"),
                    ["statusId"] = new OpenApiInteger(1),
                    ["tagId"] = new OpenApiString("e5e0e2ff-749d-41c1-aaa2-e6eb33df66d7"),
                    ["patioId"] = new OpenApiString("40dc9fbe-91ef-4528-bf30-6a9913cabffb")
                };
            }
            else if (context.Type == typeof(CreateTagDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["codigoRFID"] = new OpenApiString("RFID-0001"),
                    ["ssidWifi"] = new OpenApiString("MOTTU_WIFI_01"),
                    ["latitude"] = new OpenApiDouble(-23.56168),
                    ["longitude"] = new OpenApiDouble(-46.65598),
                    ["dataHora"] = new OpenApiString("2025-09-29T20:30:00Z"),
                    ["antenaId"] = new OpenApiString("115a9544-531c-4a52-879e-dae8b0efcb0c")
                };
            }
            else if (context.Type == typeof(CreateAntenaDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["localizacao"] = new OpenApiString("Avenida Paulista, SP"),
                    ["identificador"] = new OpenApiString("ANT-01")
                };
            }
            else if (context.Type == typeof(CreatePatioDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["cep"] = new OpenApiString("01310-100"),
                    ["numero"] = new OpenApiString("1000"),
                    ["cidade"] = new OpenApiString("São Paulo"),
                    ["estado"] = new OpenApiString("SP"),
                    ["capacidade"] = new OpenApiInteger(200)
                };
            }
        }
    }
}
