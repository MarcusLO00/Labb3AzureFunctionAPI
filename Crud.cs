using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppLabb3.Models;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace FunctionAppLabb3;

//Post
public class CreateItem
{
    private readonly ILogger<CreateItem> _logger;
    private readonly string _connectionstring;

    public CreateItem(ILogger<CreateItem> logger)
    {
        _logger = logger;
        _connectionstring = Environment.GetEnvironmentVariable("SqlConnectionString");
    }

    [Function("CreateItem")]

    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        //Omvandlar till json, datan bli i en json fil
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //Konverterar json datan till ett objekt av typen product
        Snus? item = JsonSerializer.Deserialize<Snus>(requestBody);

        // kontroll av inmating
        if (item == null ||  string.IsNullOrEmpty(item.Name)) // name ??, bör kolla allt ?
        {
        
            var badresponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badresponse.WriteAsJsonAsync("Bad request");
            return badresponse;
        }

        using (SqlConnection conn = new SqlConnection(_connectionstring))
        {
            conn.Open();
            var query = "INSERT INTO Snus (Name, Amount) VALUES (@Name, @Amount)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);

                await cmd.ExecuteNonQueryAsync();
            }
        }

            _logger.LogInformation($"Created Item: {item.Name} with amount: {item.Amount}");

        var reponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
        return reponse;
    }
}


