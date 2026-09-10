using System.Net.Mime;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UrlShortener.Api.Swagger;

public sealed class GenericTestOperationFilter : IOperationFilter
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerName = context.MethodInfo.DeclaringType?.Name?.Replace("Controller", "");
        var actionName = context.MethodInfo.Name;

        if (controllerName is null)
        {
            return;
        }

        var factoryMethod = typeof(TestDataFactory).GetMethod(
            $"{controllerName}{actionName}",
            BindingFlags.Public | BindingFlags.Static);

        if (factoryMethod is null)
        {
            return;
        }

        var sampleObject = factoryMethod.Invoke(null, null);

        if (sampleObject is null)
        {
            return;
        }

        ApplyExample(operation.RequestBody, sampleObject, controllerName, actionName);
    }

    private void ApplyExample(OpenApiRequestBody? requestBody, object sampleObject, string controllerName, string actionName)
    {
        if (requestBody?.Content.TryGetValue(
                MediaTypeNames.Application.Json,
                out var content) != true)
        {
            return;
        }

        var exampleName = $"{actionName} {controllerName.ToLowerInvariant()}";

        content.Examples[exampleName] = new OpenApiExample
        {
            Summary = exampleName,
            Value = OpenApiAnyFactory.CreateFromJson(
                JsonSerializer.Serialize(sampleObject, _jsonOptions))
        };
    }
}