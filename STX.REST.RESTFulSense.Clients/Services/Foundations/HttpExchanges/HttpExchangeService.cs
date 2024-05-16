// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService : IHttpExchangeService
    {
        private readonly IHttpBroker httpBroker;

        internal HttpExchangeService(IHttpBroker httpClientBroker)
        {
            this.httpBroker = httpClientBroker;
        }

        public  ValueTask<HttpExchange> GetAsync(
            HttpExchange httpExchange,
            CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            HttpMethod defaultHttpMethod = HttpMethod.Get;
            Version defaultHttpVersion = HttpVersion.Version11;

            ValidateHttpExchange(httpExchange, defaultHttpMethod, defaultHttpVersion);
            
            HttpRequestMessage httpRequestMessage =
                MapToHttpRequest(
                    httpExchangeRequest: httpExchange.Request,
                    defaultHttpMethod: defaultHttpMethod,
                    defaultHttpVersion: defaultHttpVersion);

            HttpResponseMessage httpResponseMessage =
                await httpBroker.SendRequestAsync(
                    httpRequestMessage: httpRequestMessage,
                    cancellationToken: cancellationToken);

            return MapToHttpExchange(
                httpExchange,
                httpResponseMessage);
        });

        private static HttpMethod GetHttpMethod(
            string customHttpMethod,
            HttpMethod defaultHttpMethod)
        {
            if (string.IsNullOrEmpty(customHttpMethod))
            {
                return defaultHttpMethod;
            }

            return HttpMethod.Parse(customHttpMethod);
        }

        private static Version GetHttpVersion(
           string customHttpVersion,
           Version defaultHttpVersion)
        {
            if (string.IsNullOrEmpty(customHttpVersion))
            {
                return defaultHttpVersion;
            }

            return Version.Parse(customHttpVersion);
        }
    }
}