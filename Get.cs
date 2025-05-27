using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppLabb3.Models;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Net;

namespace FunctionAppLabb3
{
    public class GetItems
    {
        private readonly ILogger<GetItems> _logger;
        private readonly string _connectionstring;

        public GetItems(ILogger<GetItems> logger)
        {
            _logger = logger;
            _connectionstring = Environment.GetEnvironmentVariable("SqlConnectionString");
        }

        [Function("GetItems")]

        public async Task <HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "items")] HttpRequestData req)
        {
            var items = new List<Snus>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                var query = "SELECT * FROM Snus";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while ( await reader.ReadAsync())
                    {
                        items.Add(new Snus
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Amount = reader.GetInt32(2)
                        });
                    }
                }
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(items);
            return response;
        }
            


    }
}
