// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static TOutput[] MapArray<TInput, TOutput>(
            IEnumerable<TInput> elements,
            Func<TInput, TOutput> functionMapping)
        {
            TOutput[] outputElements = null;
            if (elements != null)
            {
                outputElements =
                    elements
                        .Select(element =>
                                functionMapping(element))
                        .ToArray();
            }

            return outputElements;
        }

        private static HttpRequestMessage MapToHttpRequest(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion)
        {
            var baseAddress = new Uri(httpExchangeRequest.BaseAddress);
            string relativeUrl = httpExchangeRequest.RelativeUrl;

            HttpMethod httpMethod =
                GetHttpMethod(
                    customHttpMethod: httpExchangeRequest.HttpMethod,
                    defaultHttpMethod);

            Version httpVersion =
                GetHttpVersion(
                    customHttpVersion: httpExchangeRequest.Version,
                    defaultHttpVersion: defaultHttpVersion);

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(baseAddress, relativeUrl),
                Version = httpVersion,
                Method = httpMethod,
                
            };

            if (httpExchangeRequest.Headers is not null)
            {
                CreateHttpRequestHeaders(
                    httpExchangeRequest.Headers,
                    httpRequestMessage.Headers);
            }

            return httpRequestMessage;
        }

        private static void CreateHttpRequestHeaders(
            HttpExchangeRequestHeaders httpExchangeRequestHeaders,
            HttpRequestHeaders httpRequestHeaders)
        {
            httpRequestHeaders.Authorization = MapToAuthenticationHeaderValue(httpExchangeRequestHeaders);

        }

        private static AuthenticationHeaderValue MapToAuthenticationHeaderValue(HttpExchangeRequestHeaders httpExchangeRequestHeaders)
        {
            return new AuthenticationHeaderValue(
                httpExchangeRequestHeaders.Authorization.Schema,
                httpExchangeRequestHeaders.Authorization.Value
            );
        }

        private static HttpExchange MapToHttpExchange(
            HttpExchange httpExchange,
            HttpResponseMessage httpResponseMessage)
        {
            httpExchange.Response =
                new HttpExchangeResponse
                {
                    Headers =
                        MapHttpExchangeResponseHeaders(httpResponseMessage.Headers),

                    Content =
                        MapToHttpExchangeContent(httpResponseMessage.Content),

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
            if (httpContent.Headers != null)
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
