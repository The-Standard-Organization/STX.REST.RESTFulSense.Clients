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

        private static HttpExchangeContentHeaders CreateHttpExchangeContentHeaders(
            HttpContentHeaders httpContentHeaders)
        {
            return new HttpExchangeContentHeaders
            {
                Allow = httpContentHeaders.Allow.ToArray(),
                ContentDisposition =
                    (ContentDispositionHeader)MapToContentDispositionHeader(
                        httpContentHeaders),
                ContentEncoding =
                    httpContentHeaders.ContentEncoding.ToArray(),
                ContentLanguage = httpContentHeaders.ContentLanguage.ToArray(),
                ContentLength = httpContentHeaders.ContentLength,
                ContentLocation = httpContentHeaders.ContentLocation,
                ContentMD5 = httpContentHeaders.ContentMD5,
                ContentRange = MapToContentRangeHeader(httpContentHeaders),
                ContentType = MapToContentType(httpContentHeaders),
                Expires = httpContentHeaders.Expires,
                LastModified = httpContentHeaders.LastModified
            };
        }

        private static MediaTypeHeader MapToContentType(HttpContentHeaders httpContentHeaders)
        {
            return new MediaTypeHeader
            {
                CharSet = httpContentHeaders.ContentType.CharSet,
                MediaType = httpContentHeaders.ContentType.MediaType
            };
        }

        private static ContentRangeHeader MapToContentRangeHeader(
            HttpContentHeaders httpContentHeaders)
        {
            return new ContentRangeHeader
            {
                Unit = httpContentHeaders.ContentRange.Unit,
                From = httpContentHeaders.ContentRange.From,
                To = httpContentHeaders.ContentRange.To,
                Length = httpContentHeaders.ContentRange.Length
            };
        }

        private static ContentDispositionHeader MapToContentDispositionHeader(HttpContentHeaders httpContentHeaders)
        {
            return new ContentDispositionHeader
            {
                DispositionType = httpContentHeaders.ContentDisposition.DispositionType,
                Name = httpContentHeaders.ContentDisposition.Name,
                FileName = httpContentHeaders.ContentDisposition.FileName,
                FileNameStar = httpContentHeaders.ContentDisposition.FileNameStar,
                CreationDate = httpContentHeaders.ContentDisposition.CreationDate,
                ModificationDate = httpContentHeaders.ContentDisposition.ModificationDate,
                ReadDate = httpContentHeaders.ContentDisposition.ReadDate,
                Size = httpContentHeaders.ContentDisposition.Size
            };
        }
    }
}