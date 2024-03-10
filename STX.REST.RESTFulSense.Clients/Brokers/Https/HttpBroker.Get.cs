// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial class HttpBroker
    {

        public async ValueTask<T> GetContentAsync<T>(
            string relativeUrl,
            Func<string, ValueTask<T>> deserializationFunction = null)
        {
            HttpResponseMessage responseMessage =
                await this.httpClient.GetAsync(relativeUrl);

            return await DeserializeResponseContent<T>(
                responseMessage, deserializationFunction);
        }

        public async ValueTask<T> GetContentAsync<T>(
            string relativeUrl,
            CancellationToken cancellationToken,
            Func<string, ValueTask<T>> deserializationFunction = null)
        {
            HttpResponseMessage responseMessage =
                await this.httpClient.GetAsync(relativeUrl, cancellationToken);

            return await DeserializeResponseContent<T>(
                responseMessage, deserializationFunction);
        }

        public async ValueTask<string> GetContentStringAsync(string relativeUrl) =>
            await this.httpClient.GetStringAsync(relativeUrl);

        public async ValueTask<Stream> GetContentStreamAsync(string relativeUrl) =>
            await this.httpClient.GetStreamAsync(relativeUrl);
    }
}