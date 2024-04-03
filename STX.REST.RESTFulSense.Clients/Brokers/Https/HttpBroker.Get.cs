// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial class HttpBroker
    {
        public async ValueTask<HttpResponseMessage> GetContentAsync(
            string relativeUrl) =>
                await this.httpClient.GetAsync(relativeUrl);

        public async ValueTask<HttpResponseMessage> GetContentAsync(
            string relativeUrl,
            CancellationToken cancellationToken) =>
                await this.httpClient.GetAsync(relativeUrl, cancellationToken);
    }
}