// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace STX.REST.RESTFulSense.Clients.Brokers.LocalHttpClientBrokers
{
    internal class LocalHttpClientBroker : ILocalHttpClientBroker
    {
        private readonly HttpClient httpClient;

        public LocalHttpClientBroker(HttpClient httpClient) =>
            this.httpClient = httpClient;

        public async ValueTask<HttpResponseMessage> SendRequestAsync(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken = default) =>
            await this.httpClient.SendAsync(httpRequestMessage, cancellationToken);
    }
}