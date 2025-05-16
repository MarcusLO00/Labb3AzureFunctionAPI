using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppLabb3.Models;
using System.Text.Json;

namespace FunctionAppLabb3;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

//Post
public class CreateItem
{
    private readonly ILogger<CreateItem> _logger;

    public CreateItem(ILogger<CreateItem> logger)
    {
        _logger = logger;
    }

    [Function("CreateItem")]

    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        //Omvandlar till json, datan bli i en json fil
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //Konverterar json datan till ett objekt av typen product
        Product? item = JsonSerializer.Deserialize<Product>(requestBody);

        // kontroll av inmating
        if (item == null ||  string.IsNullOrEmpty(item.Name)) // name ??
        {
        
            var badresponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badresponse.WriteAsJsonAsync("Bad request");
            return badresponse;
        }

        _logger.LogInformation($"Created Item: {item.Name} with amount: {item.Amount}");
        var reponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
        return reponse;
    }
}

//Azure123!
//marcus123
