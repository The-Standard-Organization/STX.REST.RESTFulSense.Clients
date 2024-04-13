// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService : IHttpExchangeService
    {
        private readonly IHttpBroker httpBroker;

        internal HttpExchangeService(IHttpBroker httpClientBroker)
        {
            this.httpBroker = httpClientBroker;
        }

        public async ValueTask<HttpExchange> GetAsync(
            HttpExchange httpExchange,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequestMessage =
                ConvertToHttpRequest(httpExchange.Request);

            HttpResponseMessage httpResponseMessage =
               await httpBroker.SendRequestAsync(
                   httpRequestMessage,
                   cancellationToken);

            return await ConvertToLocalHttpClient(
                httpExchange,
                httpResponseMessage);
        }

        private static HttpRequestMessage ConvertToHttpRequest(
            HttpExchangeRequest httpExchangeRequest)
        {
            Uri baseAddress = new Uri(httpExchangeRequest.BaseAddress);
            string relativeUrl = httpExchangeRequest.RelativeUrl;

            return new HttpRequestMessage
            {
                RequestUri = new Uri(baseAddress, relativeUrl)
            };
        }

        private static async ValueTask<HttpExchange> ConvertToLocalHttpClient(
            HttpExchange httpExchange,
            HttpResponseMessage httpResponseMessage)
        {
            Stream externalStreamContent =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            httpExchange.Response = new HttpExchangeResponse
            {
                StreamContent = externalStreamContent
            };

            return httpExchange;
        }
    }
}