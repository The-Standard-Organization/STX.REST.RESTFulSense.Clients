// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial interface IHttpBroker
    {
        ValueTask<HttpResponseMessage> GetStringAsync(
            string relativeUrl);

        ValueTask<HttpResponseMessage> GetStringAsync(
            string relativeUrl,
            CancellationToken cancellationToken);
    }
}