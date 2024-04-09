// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.HttpClients;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.LocalHttpClients
{
    internal partial class LocalHttpClientService : ILocalHttpClientService
    {
        private readonly IHttpClientBroker httpClientBroker;

        internal LocalHttpClientService(IHttpClientBroker httpClientBroker)
        {
            this.httpClientBroker = httpClientBroker;
        }

        public async ValueTask<LocalHttpClient> GetAsync(
            LocalHttpClient localHttpClient,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequestMessage =
                ConvertToHttpRequest(localHttpClient.HttpRequest);

            HttpResponseMessage httpResponseMessage =
               await httpClientBroker.SendRequestAsync(httpRequestMessage, cancellationToken);

            return await ConvertToLocalHttpClient(localHttpClient, httpResponseMessage);
        }
        
        private static HttpRequestMessage ConvertToHttpRequest(
            LocalHttpClientRequest localHttpClientRequest)
        {
            Uri baseUri = new Uri(localHttpClientRequest.BaseAddress);
            string relativeUrl = localHttpClientRequest.RelativeUrl;
            
            return new HttpRequestMessage
            {
                RequestUri = new Uri(baseUri,relativeUrl)
            };
        }

        private static async ValueTask<LocalHttpClient> ConvertToLocalHttpClient(
            LocalHttpClient localHttpClient,
            HttpResponseMessage httpResponseMessage)
        {
            Stream externalStreamContent =
                await httpResponseMessage.Content.ReadAsStreamAsync();
            
            localHttpClient.HttpResponse = new LocalHttpClientResponse
            {
                StreamContent = externalStreamContent
            };

            return localHttpClient;
        }
    }
}