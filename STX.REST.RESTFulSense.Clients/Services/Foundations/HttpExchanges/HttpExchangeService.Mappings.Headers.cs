// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

            if (httpExchangeRequestHeaders.Accept is not null)
            {
                httpExchangeRequestHeaders.Accept.Select(header =>
                {
                    var mediaTypeWithQualityHeaderValue =
                        MapToMediaTypeWithQualityHeaderValue(header);

                    httpRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);

                    return header;
                }
                ).ToArray();
            }

            if (httpExchangeRequestHeaders.AcceptCharset is not null)
            {
                httpExchangeRequestHeaders.AcceptCharset.Select(header =>
                {
                    var stringWithQualityHeaderValue =
                        MapToStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptCharset.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.AcceptEncoding is not null)
            {
                httpExchangeRequestHeaders.AcceptEncoding.Select(header =>
                {
                    var stringWithQualityHeaderValue =
                        MapToStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptEncoding.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.AcceptLanguage is not null)
            {
                httpExchangeRequestHeaders.AcceptLanguage.Select(header =>
                {
                    var stringWithQualityHeaderValue =
                        MapToStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptLanguage.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.Authorization =
                MapToAuthenticationHeaderValue(
                    httpExchangeRequestHeaders.Authorization);

            httpRequestHeaders.CacheControl =
               MapToCacheControlHeaderValue(
                   httpExchangeRequestHeaders.CacheControl);

            if (httpExchangeRequestHeaders.Connection is not null)
            {
                httpExchangeRequestHeaders.Connection.Select(header =>
                {
                    httpRequestHeaders.Connection.Add(header);

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.ConnectionClose =
                httpExchangeRequestHeaders.ConnectionClose;

            httpRequestHeaders.Date =
                httpExchangeRequestHeaders.Date;

            if (httpExchangeRequestHeaders.Expect is not null)
            {
                httpExchangeRequestHeaders.Expect.Select(header =>
                {
                    var nameValueWithParametersHeaderValue =
                        MapToNameValueWithParametersHeaderValue(header);

                    httpRequestHeaders.Expect.Add(nameValueWithParametersHeaderValue);

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.ExpectContinue =
                httpExchangeRequestHeaders.ExpectContinue;

            httpRequestHeaders.From =
                httpExchangeRequestHeaders.From;

            httpRequestHeaders.Host =
                httpExchangeRequestHeaders.Host;

            if (httpExchangeRequestHeaders.IfMatch is not null)
            {
                httpExchangeRequestHeaders.IfMatch
                    .Select(header =>
                {
                    httpRequestHeaders.IfMatch.Add(new EntityTagHeaderValue(header));

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.IfModifiedSince =
                httpExchangeRequestHeaders.IfModifiedSince;

            if (httpExchangeRequestHeaders.IfNoneMatch is not null)
            {
                httpExchangeRequestHeaders.IfNoneMatch
                    .Select(header =>
                    {
                        httpRequestHeaders.IfNoneMatch.Add(new EntityTagHeaderValue(header));

                        return header;
                    }).ToArray();
            }

            httpRequestHeaders.IfRange =
                MapToRangeConditionHeaderValue(
                    httpExchangeRequestHeaders.IfRange);

            httpRequestHeaders.IfUnmodifiedSince =
                httpExchangeRequestHeaders.IfUnmodifiedSince;

            httpRequestHeaders.MaxForwards =
                httpExchangeRequestHeaders.MaxForwards;

            if (httpExchangeRequestHeaders.Pragma is not null)
            {
                httpExchangeRequestHeaders.Pragma.Select(header =>
                {
                    var nameValueHeaderValue =
                        MapToNameValueHeaderValue(header);

                    httpRequestHeaders.Pragma.Add(nameValueHeaderValue);

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.Protocol =
                httpExchangeRequestHeaders.Protocol;

            httpRequestHeaders.ProxyAuthorization =
                MapToAuthenticationHeaderValue(
                    httpExchangeRequestHeaders.ProxyAuthorization);

            httpRequestHeaders.Range =
                MapToRangeHeaderValue(httpExchangeRequestHeaders.Range);

            httpRequestHeaders.Referrer =
                httpExchangeRequestHeaders.Referrer;

            if (httpExchangeRequestHeaders.TE is not null)
            {
                httpExchangeRequestHeaders.TE.Select(header =>
                {
                    var transferCodingWithQualityHeaderValue =
                        MapToTransferCodingWithQualityHeaderValue(
                            header);

                    httpRequestHeaders.TE.Add(transferCodingWithQualityHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.TE is not null)
            {
                httpExchangeRequestHeaders.Trailer.Select(header =>
                {
                    httpRequestHeaders.Trailer.Add(header);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.TransferEncoding is not null)
            {
                httpExchangeRequestHeaders.TransferEncoding.Select(header =>
                {
                    var transferCodingHeaderValue =
                        MapToTransferCodingHeaderValue(header);

                    httpRequestHeaders.TransferEncoding.Add(transferCodingHeaderValue);

                    return header;
                }).ToArray();
            }

            httpRequestHeaders.TransferEncodingChunked =
                httpExchangeRequestHeaders.TransferEncodingChunked;

            if (httpExchangeRequestHeaders.Upgrade is not null)
            {
                httpExchangeRequestHeaders.Upgrade.Select(header =>
                {
                    var productHeaderValue = MapToProductHeaderValue(header);
                    httpRequestHeaders.Upgrade.Add(productHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.UserAgent is not null)
            {
                httpExchangeRequestHeaders.UserAgent.Select(header =>
                {
                    var productInfoHeaderValue =
                        MapToProductInfoHeaderValue(header);

                    httpRequestHeaders.UserAgent.Add(productInfoHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.Via is not null)
            {
                httpExchangeRequestHeaders.Via.Select(header =>
                {
                    ViaHeaderValue viaHeaderValue = MapToViaHeaderValue(header);
                    httpRequestHeaders.Via.Add(viaHeaderValue);

                    return header;
                }).ToArray();
            }

            if (httpExchangeRequestHeaders.Warning is not null)
            {
                httpExchangeRequestHeaders.Warning.Select(header =>
                {
                    WarningHeaderValue warningHeaderValue =
                        MapToWarningHeaderValue(header);

                    httpRequestHeaders.Warning.Add(warningHeaderValue);

                    return header;
                }).ToArray();
            }
        }

        private static async ValueTask<HttpExchangeResponseHeaders> MapHttpExchangeResponseHeaders(
            HttpResponseHeaders httpResponseHeaders)
        {
            if (httpResponseHeaders is null)
                return null;

            return new HttpExchangeResponseHeaders
            {
                AcceptRanges =
                    await MapArrayAsync(
                        httpResponseHeaders.AcceptRanges,
                        @string => @string),

                Age = httpResponseHeaders.Age,

                CacheControl =
                    MapToCacheControlHeader(
                        httpResponseHeaders.CacheControl),

                Connection =
                    await MapArrayAsync(
                        httpResponseHeaders.Connection,
                        @string => @string),

                ConnectionClose = httpResponseHeaders.ConnectionClose,
                Date = httpResponseHeaders.Date,
                ETag = httpResponseHeaders.ETag.ToString(),
                Location = httpResponseHeaders.Location,

                Pragma =
                    await MapArrayAsync(
                        httpResponseHeaders.Pragma,
                        MapToNameValueHeader),

                ProxyAuthenticate =
                    await MapArrayAsync(
                        httpResponseHeaders.ProxyAuthenticate,
                        MapToAuthenticationHeader),

                RetryAfter =
                    MapToRetryConditionHeader(
                        httpResponseHeaders.RetryAfter),

                Server =
                    await MapArrayAsync(
                        httpResponseHeaders.Server,
                        MapToProductInfoHeader),

                Trailer =
                    await MapArrayAsync(
                        httpResponseHeaders.Trailer,
                        @string => @string),

                TransferEncoding =
                    await MapArrayAsync(
                        httpResponseHeaders.TransferEncoding,
                        MapToTransferCodingHeader),

                TransferEncodingChunked =
                        httpResponseHeaders.TransferEncodingChunked,

                Upgrade =
                    await MapArrayAsync(
                        httpResponseHeaders.Upgrade,
                        MapToProductHeader),

                Vary =
                    await MapArrayAsync(
                        httpResponseHeaders.Vary,
                        @string => @string),

                Via =
                    await MapArrayAsync(
                        httpResponseHeaders.Via,
                        MapToViaHeader),

                Warning =
                    await MapArrayAsync(
                        httpResponseHeaders.Warning,
                        MapToWarningHeader),

                WwwAuthenticate =
                    await MapArrayAsync(
                        httpResponseHeaders.WwwAuthenticate,
                        MapToAuthenticationHeader)
            };
        }

        private static async ValueTask<HttpExchangeContentHeaders> MapHttpExchangeContentHeaders(
            HttpContentHeaders httpContentHeaders)
        {
            if (httpContentHeaders is null)
                return null;

            return new HttpExchangeContentHeaders
            {
                Allow =
                    await MapArrayAsync(
                        httpContentHeaders.Allow,
                        @string => @string),

                ContentDisposition =
                    MapToContentDispositionHeader(
                        httpContentHeaders.ContentDisposition),

                ContentEncoding =
                    await MapArrayAsync(
                        httpContentHeaders.ContentEncoding,
                        @string => @string),

                ContentLanguage =
                    await MapArrayAsync(
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
