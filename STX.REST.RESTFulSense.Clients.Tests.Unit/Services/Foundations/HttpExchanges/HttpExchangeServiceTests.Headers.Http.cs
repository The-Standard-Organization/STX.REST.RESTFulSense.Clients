// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http.Headers;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private static NameValueHeaderValue CreateNameValueHeaderValue(
            dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeaderValue(randomNameValueHeaderProperties.Name)
            {
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static CacheControlHeaderValue CreateCacheControlHeaderValue(
            dynamic randomCacheControlHeaderProperties)
        {
            var cacheControlHeaderValue = new CacheControlHeaderValue
            {
                NoCache = (bool)randomCacheControlHeaderProperties.NoCache,
                NoStore = (bool)randomCacheControlHeaderProperties.NoStore,
                MaxAge = (TimeSpan?)randomCacheControlHeaderProperties.MaxAge,
                SharedMaxAge = (TimeSpan?)randomCacheControlHeaderProperties.SharedMaxAge,
                MaxStale = (bool)randomCacheControlHeaderProperties.MaxStale,
                MaxStaleLimit = (TimeSpan?)randomCacheControlHeaderProperties.MaxStaleLimit,
                MinFresh = (TimeSpan?)randomCacheControlHeaderProperties.MinFresh,
                NoTransform = (bool)randomCacheControlHeaderProperties.NoTransform,
                OnlyIfCached = (bool)randomCacheControlHeaderProperties.OnlyIfCached,
                Public = (bool)randomCacheControlHeaderProperties.Public,
                Private = (bool)randomCacheControlHeaderProperties.Private,
                MustRevalidate = (bool)randomCacheControlHeaderProperties.MustRevalidate,
                ProxyRevalidate = (bool)randomCacheControlHeaderProperties.ProxyRevalidate,
            };

            string[] _ =
                (randomCacheControlHeaderProperties.NoCacheHeaders is string[] noCacheHeaders)
                    ? noCacheHeaders.Select(header =>
                    {
                        cacheControlHeaderValue.NoCacheHeaders.Add(header);

                        return header;
                    }).ToArray()
                    : null;

            string[] __ =
                (randomCacheControlHeaderProperties.PrivateHeaders is string[] privateHeaders)
                    ? privateHeaders.Select(header =>
                    {
                        cacheControlHeaderValue.PrivateHeaders.Add(header);

                        return header;
                    }).ToArray()
                    : null;

            (randomCacheControlHeaderProperties.Extensions as dynamic[])
                    .Select(extensionHeader =>
                    {
                        cacheControlHeaderValue.Extensions.Add(
                            CreateNameValueHeaderValue(extensionHeader));

                        return extensionHeader;
                    })
                    .ToArray();

            return cacheControlHeaderValue;
        }

        private static EntityTagHeaderValue CreateEntityTagHeaderValue(
            dynamic randomEntityTagHeaderProperties)
        {
            return new EntityTagHeaderValue((string)randomEntityTagHeaderProperties);
        }

        private static RangeItemHeaderValue CreateRangeItemHeaderValue(dynamic randomRangeItemHeaderProperties)
        {
            return new RangeItemHeaderValue(
                from: randomRangeItemHeaderProperties.From,
                to: randomRangeItemHeaderProperties.To);
        }

        private static RangeHeaderValue CreateRangeHeaderValue(dynamic randomRangeHeaderProperities)
        {
            RangeHeaderValue rangeHeaderValue =
                new RangeHeaderValue()
                {
                    Unit = randomRangeHeaderProperities.Unit,
                };

            (randomRangeHeaderProperities.Ranges as dynamic[])
                .Select(header =>
                    {
                        RangeItemHeaderValue rangeItemHeaderValue =
                            CreateRangeItemHeaderValue(header);

                        rangeHeaderValue.Ranges.Add(rangeItemHeaderValue);

                        return header;
                    })
                .ToArray();

            return rangeHeaderValue;
        }

        private static RangeConditionHeaderValue CreateRangeConditionHeaderValue(
            dynamic randomRangeConditionHeaderProperities)
        {
            RangeConditionHeaderValue rangeConditionHeaderValue =
                randomRangeConditionHeaderProperities.Date != null
                    ? new RangeConditionHeaderValue(randomRangeConditionHeaderProperities.Date)
                    : new RangeConditionHeaderValue(randomRangeConditionHeaderProperities.EntityTag);

            return rangeConditionHeaderValue;
        }

        private static TransferCodingWithQualityHeaderValue CreateTransferCodingWithQualityHeaderValue(
            dynamic randomTransferCodingWithQualityHeaderProperities)
        {
            TransferCodingWithQualityHeaderValue transferCodingWithQualityHeaderValue =
                new TransferCodingWithQualityHeaderValue(
                    value: randomTransferCodingWithQualityHeaderProperities.Value,
                    quality: randomTransferCodingWithQualityHeaderProperities.Quality);

            (randomTransferCodingWithQualityHeaderProperities.Parameters as dynamic[])
                .Select(header =>
                {
                    NameValueHeaderValue nameValueHeaderValue =
                        CreateNameValueHeaderValue(header);

                    transferCodingWithQualityHeaderValue.Parameters.Add(nameValueHeaderValue);

                    return header;
                })
                .ToArray();

            return transferCodingWithQualityHeaderValue;
        }

        private static TransferCodingHeaderValue CreateTransferCodingHeaderValue(
            dynamic randomTransferCodingHeaderProperities)
        {
            TransferCodingHeaderValue transferCodingHeaderValue =
                new TransferCodingHeaderValue(randomTransferCodingHeaderProperities.Value);

            (randomTransferCodingHeaderProperities.Parameters as dynamic[])
                .Select(header =>
                {
                    NameValueHeaderValue nameValueHeaderValue =
                        CreateNameValueHeaderValue(header);

                    transferCodingHeaderValue.Parameters.Add(nameValueHeaderValue);

                    return header;
                })
                .ToArray();

            return transferCodingHeaderValue;
        }

        private static ProductHeaderValue CreateProductHeaderValue(
            dynamic randomProductHeaderProperities)
        {
            return new ProductHeaderValue(
                name: randomProductHeaderProperities.Name,
                version: randomProductHeaderProperities.Version);
        }

        private static ProductInfoHeaderValue CreateProductInforHeaderValue(string comment)
        {
            return new ProductInfoHeaderValue(
                comment: comment);
        }

        private static ProductInfoHeaderValue CreateProductInforHeaderValue(dynamic product)
        {
            return new ProductInfoHeaderValue(
                productName: product.Name,
                productVersion: product.Version);
        }

        private static ProductInfoHeaderValue CreateProductInfoHeaderValue(
            dynamic randomProductInfoHeaderProperities)
        {
            ProductInfoHeaderValue productInfoHeaderValue =
                string.IsNullOrEmpty(randomProductInfoHeaderProperities.Comment)
                    ? CreateProductInforHeaderValue(randomProductInfoHeaderProperities.Product)
                    : CreateProductInforHeaderValue(randomProductInfoHeaderProperities.Comment);

            return productInfoHeaderValue;
        }

        private static ViaHeaderValue CreateViaHeaderValue(
            dynamic randomViaHeaderProperities)
        {
            return new ViaHeaderValue(
                protocolVersion: randomViaHeaderProperities.ProtocolVersion,
                receivedBy: randomViaHeaderProperities.ReceivedBy,
                protocolName: randomViaHeaderProperities.ProtocolName,
                comment: randomViaHeaderProperities.Comment);
        }

        private static WarningHeaderValue CreateWarningHeaderValue(
            dynamic randomWarningHeaderProperities)
        {
            return new WarningHeaderValue(
                code: (int)randomWarningHeaderProperities.Code,
                agent: randomWarningHeaderProperities.Agent,
                text: randomWarningHeaderProperities.Text,
                date: (DateTimeOffset)randomWarningHeaderProperities.Date);
        }

        private static RetryConditionHeaderValue CreateRetryConditionHeaderValue(
            dynamic randomRetryConditionHeaderProperties)
        {
            return new RetryConditionHeaderValue(randomRetryConditionHeaderProperties.Date);
        }

        private static MediaTypeHeaderValue CreateMediaTypeHeaderValue(
           dynamic randomMediaTypeHeaderValueProperties)
        {
            return new MediaTypeHeaderValue(
                mediaType: randomMediaTypeHeaderValueProperties.MediaType,
                charSet: randomMediaTypeHeaderValueProperties.Charset);
        }

        private static MediaTypeWithQualityHeaderValue CreateMediaTypeWithQualityHeaderValue(
           dynamic randomMediaTypeHeaderValueProperties)
        {
            return new MediaTypeWithQualityHeaderValue(
                mediaType: randomMediaTypeHeaderValueProperties.MediaType,
                quality: randomMediaTypeHeaderValueProperties.Quality);
        }

        private static StringWithQualityHeaderValue CreateStringWithQualityHeaderValue(
            dynamic randomStringWithQualityHeaderValue)
        {
            return new StringWithQualityHeaderValue(
                value: randomStringWithQualityHeaderValue.Value,
                quality: randomStringWithQualityHeaderValue.Quality);
        }

        private static AuthenticationHeaderValue CreateAuthenticationHeaderValue(
            dynamic randomAuthenticationHeaderValue)
        {
            return new AuthenticationHeaderValue(
                scheme: randomAuthenticationHeaderValue.Schema,
                parameter: randomAuthenticationHeaderValue.Value);
        }

        private static ContentRangeHeaderValue CreateContentRangeHeaderValue(
            dynamic randomContentRangeHeaderProperties)
        {
            return new ContentRangeHeaderValue(
                from: randomContentRangeHeaderProperties.From,
                to: randomContentRangeHeaderProperties.To,
                length: randomContentRangeHeaderProperties.Length)
            {
                Unit = randomContentRangeHeaderProperties.Unit
            };
        }

        private static ContentDispositionHeaderValue CreateContentDispositionHeaderValue(
            dynamic randomContentDispositionHeaderProperties)
        {
            return new ContentDispositionHeaderValue(
                randomContentDispositionHeaderProperties.DispositionType)
            {
                Name = randomContentDispositionHeaderProperties.Name,
                FileName = randomContentDispositionHeaderProperties.FileName,
                FileNameStar = randomContentDispositionHeaderProperties.FileNameStar,
                CreationDate = (DateTimeOffset?)randomContentDispositionHeaderProperties.CreationDate,
                ModificationDate = (DateTimeOffset?)randomContentDispositionHeaderProperties.ModificationDate,
                ReadDate = (DateTimeOffset?)randomContentDispositionHeaderProperties.ReadDate,
                Size = randomContentDispositionHeaderProperties.Size
            };
        }

        private static void CreateHttpRequestHeaders(
            dynamic randomRequestHeadersProperties,
            HttpRequestHeaders httpRequestHeaders)
        {
            (randomRequestHeadersProperties.Accept as dynamic[])
                .Select(header =>
                {
                    MediaTypeWithQualityHeaderValue mediaTypeWithQualityHeaderValue =
                        CreateMediaTypeWithQualityHeaderValue(header);

                    httpRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.AcceptCharset as dynamic[])
                .Select(header =>
                {
                    StringWithQualityHeaderValue stringWithQualityHeaderValue =
                        CreateStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptCharset.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.AcceptEncoding as dynamic[])
                .Select(header =>
                {
                    StringWithQualityHeaderValue stringWithQualityHeaderValue =
                        CreateStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptEncoding.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.AcceptLanguage as dynamic[])
                .Select(header =>
                {
                    StringWithQualityHeaderValue stringWithQualityHeaderValue =
                        CreateStringWithQualityHeaderValue(header);

                    httpRequestHeaders.AcceptLanguage.Add(stringWithQualityHeaderValue);

                    return header;
                }).ToArray();

            httpRequestHeaders.Authorization =
                CreateAuthenticationHeaderValue(randomRequestHeadersProperties.Authorization);

            httpRequestHeaders.CacheControl =
                CreateCacheControlHeaderValue(randomRequestHeadersProperties.CacheControl);

            (randomRequestHeadersProperties.Connection as string[])
               .Select(header =>
               {
                   httpRequestHeaders.Connection.Add(header);

                   return header;
               }).ToArray();

            httpRequestHeaders.ConnectionClose =
                (bool?)randomRequestHeadersProperties.ConnectionClose;

            httpRequestHeaders.Date =
                (DateTimeOffset?)randomRequestHeadersProperties.Date;

            httpRequestHeaders.ExpectContinue =
                (bool?)randomRequestHeadersProperties.ExpectContinue;

            httpRequestHeaders.From = randomRequestHeadersProperties.From;
            httpRequestHeaders.Host = randomRequestHeadersProperties.Host;

            (randomRequestHeadersProperties.IfMatch as string[])
               .Select(header =>
               {
                   EntityTagHeaderValue entityTagHeaderValue =
                       CreateEntityTagHeaderValue(header);

                   httpRequestHeaders.IfMatch.Add(entityTagHeaderValue);

                   return header;
               }).ToArray();

            (randomRequestHeadersProperties.IfNoneMatch as string[])
                .Select(header =>
                {
                    EntityTagHeaderValue entityTagHeaderValue =
                        CreateEntityTagHeaderValue(header);

                    httpRequestHeaders.IfNoneMatch.Add(entityTagHeaderValue);

                    return header;
                }).ToArray();

            httpRequestHeaders.IfRange =
                CreateRangeConditionHeaderValue(randomRequestHeadersProperties.IfRange);

            httpRequestHeaders.IfUnmodifiedSince =
                (DateTimeOffset?)randomRequestHeadersProperties.IfUnmodifiedSince;

            httpRequestHeaders.MaxForwards =
                (int?)randomRequestHeadersProperties.MaxForwards;

            (randomRequestHeadersProperties.Pragma as dynamic[])
                .Select(header =>
                {
                    NameValueHeaderValue nameValueHeaderValue =
                        CreateNameValueHeaderValue(header);

                    httpRequestHeaders.Pragma.Add(nameValueHeaderValue);

                    return header;
                }).ToArray();

            httpRequestHeaders.Protocol =
                randomRequestHeadersProperties.Protocol;

            httpRequestHeaders.ProxyAuthorization =
                CreateAuthenticationHeaderValue(randomRequestHeadersProperties.ProxyAuthorization);

            httpRequestHeaders.Range =
                CreateRangeHeaderValue(randomRequestHeadersProperties.Range);

            httpRequestHeaders.Referrer = (Uri)randomRequestHeadersProperties.Referrer;

            (randomRequestHeadersProperties.TE as dynamic[])
                .Select(header =>
                {
                    TransferCodingWithQualityHeaderValue transferCodingWithQualityHeaderValue =
                        CreateTransferCodingWithQualityHeaderValue(header);

                    httpRequestHeaders.TE.Add(transferCodingWithQualityHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.Trailer as string[])
               .Select(header =>
               {
                   httpRequestHeaders.Trailer.Add(header);

                   return header;
               }).ToArray();

            (randomRequestHeadersProperties.TransferEncoding as dynamic[])
                .Select(header =>
                {
                    TransferCodingHeaderValue transferCodingHeaderValue =
                        CreateTransferCodingHeaderValue(header);

                    httpRequestHeaders.TransferEncoding.Add(transferCodingHeaderValue);

                    return header;
                }).ToArray();

            httpRequestHeaders.TransferEncodingChunked =
                (bool?)randomRequestHeadersProperties.TransferEncodingChunked;

            (randomRequestHeadersProperties.Upgrade as dynamic[])
                .Select(header =>
                {
                    ProductHeaderValue productHeaderValue =
                        CreateProductHeaderValue(header);

                    httpRequestHeaders.Upgrade.Add(productHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.UserAgent as dynamic[])
                .Select(header =>
                {
                    ProductInfoHeaderValue productInfoHeaderValue =
                        CreateProductInfoHeaderValue(header);

                    httpRequestHeaders.UserAgent.Add(productInfoHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.Via as dynamic[])
                .Select(header =>
                {
                    ViaHeaderValue viaHeaderValue =
                        CreateViaHeaderValue(header);

                    httpRequestHeaders.Via.Add(viaHeaderValue);

                    return header;
                }).ToArray();

            (randomRequestHeadersProperties.Warning as dynamic[])
               .Select(header =>
               {
                   WarningHeaderValue warningHeaderValue =
                       CreateWarningHeaderValue(header);

                   httpRequestHeaders.Warning.Add(warningHeaderValue);

                   return header;
               }).ToArray();
        }

        private static void CreateHttpResponseHeaders(
            dynamic randomResponseHeadersProperties,
            HttpResponseHeaders httpResponseHeaders)
        {
            (randomResponseHeadersProperties.AcceptRanges as string[])
                .Select(header =>
                {
                    httpResponseHeaders.AcceptRanges.Add(header);

                    return header;
                }).ToArray();

            httpResponseHeaders.Age =
                (TimeSpan?)randomResponseHeadersProperties.Age;

            httpResponseHeaders.CacheControl =
                CreateCacheControlHeaderValue(randomResponseHeadersProperties.CacheControl);

            (randomResponseHeadersProperties.Connection as string[])
                .Select(header =>
                {
                    httpResponseHeaders.Connection.Add(header);

                    return header;
                }).ToArray();

            httpResponseHeaders.ConnectionClose =
                (bool?)randomResponseHeadersProperties.ConnectionClose;

            httpResponseHeaders.Date =
                (DateTimeOffset?)randomResponseHeadersProperties.Date;

            httpResponseHeaders.ETag =
                CreateEntityTagHeaderValue(randomResponseHeadersProperties.ETag);

            httpResponseHeaders.Location =
                (Uri)randomResponseHeadersProperties.Location;

            (randomResponseHeadersProperties.Pragma as dynamic[])
                .Select(header =>
                {
                    var pragmaHeader =
                        CreateNameValueHeaderValue(header);

                    httpResponseHeaders.Pragma.Add(pragmaHeader);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.ProxyAuthenticate as dynamic[])
                .Select(header =>
                {
                    AuthenticationHeaderValue authenticationHeaderValue =
                        CreateAuthenticationHeaderValue(header);

                    httpResponseHeaders.ProxyAuthenticate.Add(authenticationHeaderValue);

                    return header;
                }).ToArray();

            httpResponseHeaders.RetryAfter =
                CreateRetryConditionHeaderValue(
                    randomResponseHeadersProperties.RetryAfter);

            (randomResponseHeadersProperties.Server as dynamic[])
                .Select(header =>
                {
                    ProductInfoHeaderValue productInfoHeaderValue =
                        CreateProductInfoHeaderValue(header);

                    httpResponseHeaders.Server.Add(productInfoHeaderValue);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.Trailer as string[])
                .Select(header =>
                {
                    httpResponseHeaders.Trailer.Add(header);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.TransferEncoding as dynamic[])
                .Select(header =>
                {
                    TransferCodingHeaderValue transferCodingHeaderValue =
                        CreateTransferCodingHeaderValue(header);

                    httpResponseHeaders.TransferEncoding.Add(transferCodingHeaderValue);

                    return header;
                }).ToArray();

            httpResponseHeaders.TransferEncodingChunked =
                (bool?)randomResponseHeadersProperties.TransferEncodingChunked;

            (randomResponseHeadersProperties.Upgrade as dynamic[])
                .Select(header =>
                {
                    ProductHeaderValue productHeaderValue =
                        CreateProductHeaderValue(header);

                    httpResponseHeaders.Upgrade.Add(productHeaderValue);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.Vary as string[])
                .Select(header =>
                {
                    httpResponseHeaders.Vary.Add(header);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.Via as dynamic[])
                .Select(header =>
                {
                    ViaHeaderValue viaHeaderValue =
                        CreateViaHeaderValue(header);

                    httpResponseHeaders.Via.Add(viaHeaderValue);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.Warning as dynamic[])
                .Select(header =>
                {
                    WarningHeaderValue warningHeaderValue =
                        CreateWarningHeaderValue(header);

                    httpResponseHeaders.Warning.Add(warningHeaderValue);

                    return header;
                }).ToArray();

            (randomResponseHeadersProperties.WwwAuthenticate as dynamic[])
                .Select(header =>
                {
                    AuthenticationHeaderValue authenticationHeaderValue =
                        CreateAuthenticationHeaderValue(header);

                    httpResponseHeaders.WwwAuthenticate.Add(authenticationHeaderValue);

                    return header;
                }).ToArray();
        }

        private static void CreateHttpContentHeaders(
            dynamic randomResponseContentHeadersProperties,
            HttpContentHeaders httpContentHeaders)
        {
            (randomResponseContentHeadersProperties.Allow as string[])
                .Select(header =>
                {
                    httpContentHeaders.Allow.Add(header);

                    return header;
                }).ToArray();

            httpContentHeaders.ContentDisposition =
                CreateContentDispositionHeaderValue(
                    randomResponseContentHeadersProperties.ContentDisposition);

            (randomResponseContentHeadersProperties.ContentEncoding as string[])
                .Select(header =>
                {
                    httpContentHeaders.ContentEncoding.Add(header);

                    return header;
                }).ToArray();

            (randomResponseContentHeadersProperties.ContentLanguage as string[])
                .Select(header =>
                {
                    httpContentHeaders.ContentLanguage.Add(header);

                    return header;
                }).ToArray();

            httpContentHeaders.ContentLength =
                (long?)randomResponseContentHeadersProperties.ContentLength;

            httpContentHeaders.ContentLocation =
                (Uri)randomResponseContentHeadersProperties.ContentLocation;

            httpContentHeaders.ContentMD5 =
                (byte[])randomResponseContentHeadersProperties.ContentMD5;

            httpContentHeaders.ContentRange =
                CreateContentRangeHeaderValue(
                    randomResponseContentHeadersProperties.ContentRange);

            httpContentHeaders.ContentType =
                CreateMediaTypeHeaderValue(
                    randomResponseContentHeadersProperties.ContentType);

            httpContentHeaders.Expires =
                (DateTimeOffset?)randomResponseContentHeadersProperties.Expires;

            httpContentHeaders.LastModified =
                (DateTimeOffset?)randomResponseContentHeadersProperties.LastModified;
        }
    }
}
