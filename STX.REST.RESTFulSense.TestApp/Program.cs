// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;

using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges;

namespace STX.REST.RESTFulSense.TestApp
{
    internal class Program
    {
        public async static Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var httpBroker = new HttpBroker(httpClient);
            var httpExchangeService = new HttpExchangeService(httpBroker);

            var httpExchange = new HttpExchange
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = @"https://github.com/",
                    RelativeUrl = @"The-Standard-Organization/STX.REST.RESTFulSense.Clients"
                }
            };

            httpExchange = await httpExchangeService.GetAsync(httpExchange);
            Console.WriteLine($"Status code: {httpExchange.Response.StatusCode}");
        }
    }
}
