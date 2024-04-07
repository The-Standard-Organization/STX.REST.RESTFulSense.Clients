// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.HttpClients;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpClients
{
    internal class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientBroker httpClientBroker;

        internal HttpClientService(IHttpClientBroker httpClientBroker)
        {
            this.httpClientBroker = httpClientBroker;
        }

        public ValueTask<HttpClient> GetAsync(HttpClient httpClient, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}