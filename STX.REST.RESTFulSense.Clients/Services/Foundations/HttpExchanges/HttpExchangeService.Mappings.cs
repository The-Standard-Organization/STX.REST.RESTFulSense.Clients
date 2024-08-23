// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static async ValueTask<TOutput[]> MapArrayAsync<TInput, TOutput>(
            IEnumerable<TInput> elements,
            Func<TInput, TOutput> functionMapping)
        {
            TOutput[] outputElements = null;
            if (elements is not null)
            {
                outputElements =
                    elements
                        .Select(element =>
                                functionMapping(element))
                        .ToArray();
            }

            return outputElements;
        }

        private static string MapUrlParameters(
            IDictionary<string, object> urlParameters,
            string relativeUrl)
        {
            string fullRelativeUrl = relativeUrl;

            if (urlParameters is not null)
            {
                IEnumerable<string> additionalParameters =
                    urlParameters
                        .Select(urlParameter =>
                            $"{urlParameter.Key}={urlParameter.Value}");

                fullRelativeUrl =
                    String.Join("&",
                        relativeUrl
                            .Split("&")
                            .Concat(additionalParameters));
            }

            return fullRelativeUrl;
        }

        private static async ValueTask<HttpRequestMessage> MapToHttpRequestAsync(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion,
            HttpVersionPolicy defaultHttpVersionPolicy)
        {
            var baseAddress = new Uri(httpExchangeRequest.BaseAddress);

            string relativeUrl =
                MapUrlParameters(
                    httpExchangeRequest.UrlParameters,
                    httpExchangeRequest.RelativeUrl);

            Uri requestUri =
                new Uri(
                    baseAddress,
                    relativeUrl);

            HttpMethod httpMethod =
                await GetHttpMethodAsync(
                    customHttpMethod: httpExchangeRequest.HttpMethod,
                    defaultHttpMethod);

            Version httpVersion =
                await GetHttpVersionAsync(
                    customHttpVersion: httpExchangeRequest.Version,
                    defaultHttpVersion: defaultHttpVersion);

            HttpVersionPolicy httpVersionPolicy =
                (HttpVersionPolicy)httpExchangeRequest.VersionPolicy;

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Version = httpVersion,
                Method = httpMethod,
                VersionPolicy = httpVersionPolicy
            };

            if (httpExchangeRequest.Headers is not null)
            {
                MapToHttpRequestHeaders(
                    httpExchangeRequest.Headers,
                    httpRequestMessage.Headers);
            }

            return httpRequestMessage;
        }

        private static async ValueTask<HttpExchange> MapToHttpExchange(
            HttpExchange httpExchange,
            HttpResponseMessage httpResponseMessage)
        {
            httpExchange.Response =
                new HttpExchangeResponse
                {
                    Headers =
                        await MapHttpExchangeResponseHeaders(
                            httpResponseMessage.Headers),

                    Content =
                        MapToHttpExchangeContent(
                            httpResponseMessage.Content),

                    ReasonPhrase = httpResponseMessage.ReasonPhrase,
                    Version = httpResponseMessage.Version.ToString(),
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode
                };

            return httpExchange;
        }

        private static HttpExchangeContent MapToHttpExchangeContent(HttpContent httpContent)
        {
            HttpExchangeContentHeaders httpExchangeContentHeaders = null;
            if (httpContent.Headers is not null)
            {
                httpExchangeContentHeaders =
                     MapHttpExchangeContentHeaders(
                         httpContent.Headers);
            }

            return new HttpExchangeContent
            {
                StreamContent = new ValueTask<Stream>(
                         httpContent.ReadAsStreamAsync()),

                Headers = httpExchangeContentHeaders
            };
        }
    }
}
