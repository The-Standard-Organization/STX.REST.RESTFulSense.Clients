// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static HttpRequestMessage MapToHttpRequest(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion)
        {
            var baseAddress = new Uri(httpExchangeRequest.BaseAddress);
            string relativeUrl = httpExchangeRequest.RelativeUrl;
            HttpMethod httpMethod = GetHttpMethod(
                customHttpMethod: httpExchangeRequest.HttpMethod,
                defaultHttpMethod);

            Version httpVersion = GetHttpVersion(
                customHttpVersion: httpExchangeRequest.Version,
                defaultHttpVersion: defaultHttpVersion);

            return new HttpRequestMessage
            {
                RequestUri = new Uri(baseAddress, relativeUrl),
                Version = httpVersion,
                Method = httpMethod
            };
        }

        private static HttpExchange MapToHttpExchange(
            HttpExchange httpExchange,
            HttpResponseMessage httpResponseMessage)
        {
            httpExchange.Response = new HttpExchangeResponse
            {
                Content = new HttpExchangeContent
                {
                    StreamContent = new ValueTask<Stream>(
                         httpResponseMessage.Content.ReadAsStreamAsync()),

                    Headers = CreateHttpExchangeContentHeaders(
                         httpResponseMessage.Content.Headers)
                },
                ReasonPhrase = httpResponseMessage.ReasonPhrase,
                Version = httpResponseMessage.Version.ToString(),
                StatusCode = (int)httpResponseMessage.StatusCode,
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode
            };

            return httpExchange;
        }
    }
}
