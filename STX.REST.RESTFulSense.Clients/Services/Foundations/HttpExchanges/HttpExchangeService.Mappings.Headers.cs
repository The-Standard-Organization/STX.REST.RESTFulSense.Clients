// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static void MapToHttpRequestHeaders(
            HttpExchangeRequestHeaders httpExchangeRequestHeaders,
            HttpRequestHeaders httpRequestHeaders)
        {
            if (httpExchangeRequestHeaders is null)
                return;

            /*httpRequestHeaders.Accept

            httpRequestHeaders.AcceptCharset

            httpRequestHeaders.AcceptEncoding

            httpRequestHeaders.AcceptLanguage

            httpRequestHeaders.Authorization =
                MapToAuthenticationHeaderValue(
                    httpExchangeRequestHeaders.Authorization);

            httpRequestHeaders.CacheControl*/
        }


        private static HttpExchangeResponseHeaders MapHttpExchangeResponseHeaders(
            HttpResponseHeaders httpResponseHeaders)
        {
            if (httpResponseHeaders is null)
                return null;

            return new HttpExchangeResponseHeaders
            {
                AcceptRanges =
                    MapArray(
                        httpResponseHeaders.AcceptRanges,
                        @string => @string),

                Age = httpResponseHeaders.Age,

                CacheControl =
                    MapToCacheControlHeader(
                        httpResponseHeaders.CacheControl),

                Connection =
                    MapArray(
                        httpResponseHeaders.Connection,
                        @string => @string),

                ConnectionClose = httpResponseHeaders.ConnectionClose,
                Date = httpResponseHeaders.Date,
                ETag = httpResponseHeaders.ETag.ToString(),
                Location = httpResponseHeaders.Location,

                Pragma =
                    MapArray(
                        httpResponseHeaders.Pragma,
                        MapToNameValueHeader),

                ProxyAuthenticate =
                    MapArray(
                        httpResponseHeaders.ProxyAuthenticate,
                        MapToAuthenticationHeader),

                RetryAfter =
                    MapToRetryConditionHeader(
                        httpResponseHeaders.RetryAfter),

                Server =
                    MapArray(
                        httpResponseHeaders.Server,
                        MapToProductInfoHeader),

                Trailer =
                    MapArray(
                        httpResponseHeaders.Trailer,
                        @string => @string),

                TransferEncoding =
                    MapArray(
                        httpResponseHeaders.TransferEncoding,
                        MapToTransferCodingHeader),

                TransferEncodingChunked =
                            httpResponseHeaders.TransferEncodingChunked,

                Upgrade =
                    MapArray(
                        httpResponseHeaders.Upgrade,
                        MapToProductHeader),

                Vary =
                    MapArray(
                        httpResponseHeaders.Vary,
                        @string => @string),

                Via =
                    MapArray(
                        httpResponseHeaders.Via,
                        MapToViaHeader),

                Warning =
                    MapArray(
                        httpResponseHeaders.Warning,
                        MapToWarningHeader),

                WwwAuthenticate =
                    MapArray(
                        httpResponseHeaders.WwwAuthenticate,
                        MapToAuthenticationHeader)
            };
        }

        private static HttpExchangeContentHeaders MapHttpExchangeContentHeaders(
            HttpContentHeaders httpContentHeaders)
        {
            if (httpContentHeaders is null)
                return null;

            return new HttpExchangeContentHeaders
            {
                Allow =
                    MapArray(
                        httpContentHeaders.Allow,
                        @string => @string),

                ContentDisposition =
                    MapToContentDispositionHeader(
                        httpContentHeaders.ContentDisposition),

                ContentEncoding =
                    MapArray(
                        httpContentHeaders.ContentEncoding,
                        @string => @string),

                ContentLanguage =
                    MapArray(
                        httpContentHeaders.ContentLanguage,
                        @string => @string),

                ContentLength = httpContentHeaders.ContentLength,
                ContentLocation = httpContentHeaders.ContentLocation,
                ContentMD5 = httpContentHeaders.ContentMD5,

                ContentRange =
                    MapToContentRangeHeader(
                        httpContentHeaders.ContentRange),

                ContentType =
                    MapToMediaTypeHeader(
                        httpContentHeaders.ContentType),

                Expires = httpContentHeaders.Expires,
                LastModified = httpContentHeaders.LastModified
            };
        }
    }
}
