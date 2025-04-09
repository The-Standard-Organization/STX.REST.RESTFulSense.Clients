// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private static ContentDispositionHeader CreateContentDispositionHeader(
            dynamic randomContentDispositionHeaderProperties)
        {
            var contentDispositionHeader = new ContentDispositionHeader
            {
                DispositionType =
                    randomContentDispositionHeaderProperties.DispositionType,

                Name = randomContentDispositionHeaderProperties.Name,
                FileName = randomContentDispositionHeaderProperties.FileName,
                FileNameStar = randomContentDispositionHeaderProperties.FileNameStar,
                CreationDate = randomContentDispositionHeaderProperties.CreationDate,

                ModificationDate =
                    randomContentDispositionHeaderProperties.ModificationDate,

                ReadDate = randomContentDispositionHeaderProperties.ReadDate,
                Size = randomContentDispositionHeaderProperties.Size
            };

            return contentDispositionHeader;
        }

        private static ContentRangeHeader CreateContentRangeHeader(
            dynamic randomContentRangeHeaderProperties)
        {
            var contentRangeHeader = new ContentRangeHeader
            {
                Unit = randomContentRangeHeaderProperties.Unit,
                From = randomContentRangeHeaderProperties.From,
                To = randomContentRangeHeaderProperties.To,
                Length = randomContentRangeHeaderProperties.Length
            };

            return contentRangeHeader;
        }

        private static MediaTypeHeader CreateMediaTypeHeader(
            dynamic randomMediaTypeHeaderProperties)
        {
            var mediaTypeHeader = new MediaTypeHeader
            {
                CharSet = randomMediaTypeHeaderProperties.CharSet,
                MediaType = randomMediaTypeHeaderProperties.MediaType,
                Quality = randomMediaTypeHeaderProperties.Quality
            };

            return mediaTypeHeader;
        }

        private static CacheControlHeader CreateCacheControlHeader(
            dynamic randomCacheControlHeaderProperties)
        {
            var cacheControlHeader = new CacheControlHeader
            {
                NoCache = randomCacheControlHeaderProperties.NoCache,
                NoCacheHeaders = randomCacheControlHeaderProperties.NoCacheHeaders,
                NoStore = randomCacheControlHeaderProperties.NoStore,
                MaxAge = randomCacheControlHeaderProperties.MaxAge,
                SharedMaxAge = randomCacheControlHeaderProperties.SharedMaxAge,
                MaxStale = randomCacheControlHeaderProperties.MaxStale,
                MaxStaleLimit = randomCacheControlHeaderProperties.MaxStaleLimit,
                MinFresh = randomCacheControlHeaderProperties.MinFresh,
                NoTransform = randomCacheControlHeaderProperties.NoTransform,
                OnlyIfCached = randomCacheControlHeaderProperties.OnlyIfCached,
                Public = randomCacheControlHeaderProperties.Public,
                Private = randomCacheControlHeaderProperties.Private,
                PrivateHeaders = randomCacheControlHeaderProperties.PrivateHeaders,
                MustRevalidate = randomCacheControlHeaderProperties.MustRevalidate,
                ProxyRevalidate = randomCacheControlHeaderProperties.ProxyRevalidate,

                Extensions =
                    (randomCacheControlHeaderProperties.Extensions as dynamic[])
                        .Select(extensionHeader =>
                            (NameValueHeader)CreateNameValueHeader(extensionHeader))
                        .ToArray()
            };

            return cacheControlHeader;
        }

        private static NameValueHeader CreateNameValueHeader(
            dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeader
            {
                Name = randomNameValueHeaderProperties.Name,
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static NameValueWithParametersHeader CreateNameValueWithParameters(
            dynamic randomNameValueWithParametersProperties)
        {
            var nameValueWithParameters = new NameValueWithParametersHeader
            {
                Name = randomNameValueWithParametersProperties.Name,

                Parameters =
                    (randomNameValueWithParametersProperties.Parameters as dynamic[])
                        .Select(header =>
                            (NameValueHeader)CreateNameValueHeader(header))
                        .ToArray()
            };

            return nameValueWithParameters;
        }

        private static AuthenticationHeader CreateAuthorizationHeader(
            dynamic randomAuthenticationHeaderProperties)
        {
            return new AuthenticationHeader
            {
                Schema = randomAuthenticationHeaderProperties.Schema,
                Value = randomAuthenticationHeaderProperties.Value
            };
        }

        private static RangeConditionHeader CreateRangeConditionHeader(
            dynamic randomRangeConditionHeaderProperities)
        {
            return new RangeConditionHeader
            {
                Date = randomRangeConditionHeaderProperities.Date,
                EntityTag = randomRangeConditionHeaderProperities.EntityTag
            };
        }

        private static RangeItemHeader CreateRangeItemHeader(
            dynamic randomRangeItemHeader)
        {
            return new RangeItemHeader()
            {
                From = randomRangeItemHeader.From,
                To = randomRangeItemHeader.To
            };
        }

        private static RangeHeader CreateRangeHeader(
            dynamic randomRangeHeaderProperties)
        {
            return new RangeHeader()
            {
                Unit = randomRangeHeaderProperties.Unit,

                Ranges =
                    (randomRangeHeaderProperties.Ranges as dynamic[])
                        .Select(header =>
                            (RangeItemHeader)CreateRangeItemHeader(header))
                        .ToArray()
            };
        }

        private static RetryConditionHeader CreateRetryConditionHeader(
            dynamic randomRetryConditionHeaderProperities)
        {
            return new RetryConditionHeader
            {
                Delta = randomRetryConditionHeaderProperities.Delta,
                Date = randomRetryConditionHeaderProperities.Date
            };
        }

        private static ProductHeader CreateProductHeader(
            dynamic randomProductHeaderProperties)
        {
            return randomProductHeaderProperties is not null
                ? new ProductHeader
                {
                    Name = randomProductHeaderProperties.Name,
                    Version = randomProductHeaderProperties.Version
                }
                : null;
        }

        private static ProductInfoHeader CreateProductInfoHeader(
            dynamic randomProductInfoHeaderProperties)
        {
            return new ProductInfoHeader
            {
                Comment = randomProductInfoHeaderProperties.Comment,

                Product =
                    CreateProductHeader(
                        randomProductInfoHeaderProperties.Product)
            };
        }

        private static TransferCodingHeader CreateTransferCodingHeader(
            dynamic randomTransferCodingHeaderProperties)
        {
            return new TransferCodingHeader
            {
                Quality = randomTransferCodingHeaderProperties.Quality,
                Value = randomTransferCodingHeaderProperties.Value,

                Parameters =
                    (randomTransferCodingHeaderProperties.Parameters as dynamic[])
                        .Select(parametersHeader =>
                            (NameValueHeader)CreateNameValueHeader(parametersHeader))
                        .ToArray()
            };
        }

        private static ViaHeader CreateViaHeader(
            dynamic randomViaHeaderProperties)
        {
            return new ViaHeader
            {
                ProtocolName = randomViaHeaderProperties.ProtocolName,
                ProtocolVersion = randomViaHeaderProperties.ProtocolVersion,
                ReceivedBy = randomViaHeaderProperties.ReceivedBy,
                Comment = randomViaHeaderProperties.Comment
            };
        }

        private static WarningHeader CreateWarningHeader(
            dynamic randomWarningHeaderProperties)
        {
            return new WarningHeader
            {
                Code = randomWarningHeaderProperties.Code,
                Agent = randomWarningHeaderProperties.Agent,
                Text = randomWarningHeaderProperties.Text,
                Date = (DateTimeOffset?)randomWarningHeaderProperties.Date
            };
        }

        private static StringWithQualityHeader CreateStringWithQualityHeader(
            dynamic randomStringWithQualityHeaderProperties)
        {
            return new StringWithQualityHeader
            {
                Value = randomStringWithQualityHeaderProperties.Value,
                Quality = randomStringWithQualityHeaderProperties.Quality
            };
        }

        private static HttpExchangeRequestHeaders CreateHttpExchangeRequestHeaders(
            dynamic randomHeaderProperties)
        {
            HttpExchangeRequestHeaders httpExchangeRequestHeaders = null;

            if (randomHeaderProperties is not null)
            {
                httpExchangeRequestHeaders =
                    CreateHttpExchangeRequestHeadersFiller(randomHeaderProperties)
                        .Create();
            }

            return httpExchangeRequestHeaders;
        }

        private static Filler<HttpExchangeRequestHeaders> CreateHttpExchangeRequestHeadersFiller(
            dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeRequestHeaders>();

            filler.Setup()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Accept)
                    .Use((randomHeaderProperties.Accept as dynamic[])
                        .Select(header =>
                            (MediaTypeHeader)CreateMediaTypeHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptCharset)
                    .Use((randomHeaderProperties.AcceptCharset as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)CreateStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptEncoding)
                    .Use((randomHeaderProperties.AcceptEncoding as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)CreateStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptLanguage)
                    .Use((randomHeaderProperties.AcceptLanguage as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)CreateStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Authorization)
                    .Use((AuthenticationHeader)CreateAuthorizationHeader(
                            randomHeaderProperties.Authorization))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.CacheControl)
                    .Use((CacheControlHeader)CreateCacheControlHeader(
                            randomHeaderProperties.CacheControl))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Connection)
                    .Use((string[])randomHeaderProperties.Connection)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ConnectionClose)
                    .Use((bool?)randomHeaderProperties.ConnectionClose)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Date)
                    .Use((DateTimeOffset?)randomHeaderProperties.Date)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Expect)
                    .Use((randomHeaderProperties.Expect as dynamic[])
                        .Select(header =>
                            (NameValueWithParametersHeader)CreateNameValueWithParameters(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectContinue)
                    .Use((bool?)randomHeaderProperties.ExpectContinue)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.From)
                    .Use((string)randomHeaderProperties.From)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Host)
                    .Use((string)randomHeaderProperties.Host)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfMatch)
                    .Use((string[])randomHeaderProperties.IfMatch)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfModifiedSince)
                    .Use((DateTimeOffset?)randomHeaderProperties.IfModifiedSince)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfNoneMatch)
                    .Use((string[])randomHeaderProperties.IfNoneMatch)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfRange)
                    .Use((RangeConditionHeader)CreateRangeConditionHeader(randomHeaderProperties.IfRange))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfUnmodifiedSince)
                    .Use((DateTimeOffset?)randomHeaderProperties.IfUnmodifiedSince)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.MaxForwards)
                    .Use((int?)randomHeaderProperties.MaxForwards)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Pragma)
                    .Use((randomHeaderProperties.Pragma as dynamic[])
                        .Select(header =>
                            (NameValueHeader)CreateNameValueHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Protocol)
                    .Use((string)randomHeaderProperties.Protocol)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ProxyAuthorization)
                    .Use((AuthenticationHeader)CreateAuthorizationHeader(
                        randomHeaderProperties.ProxyAuthorization))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Range)
                    .Use((RangeHeader)CreateRangeHeader(randomHeaderProperties.Range))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Referrer)
                    .Use((Uri)randomHeaderProperties.Referrer)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TE)
                    .Use((randomHeaderProperties.TE as dynamic[])
                        .Select(header =>
                            (TransferCodingHeader)CreateTransferCodingHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Trailer)
                    .Use((string[])randomHeaderProperties.Trailer)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncoding)
                    .Use((randomHeaderProperties.TransferEncoding as dynamic[])
                        .Select(header =>
                            (TransferCodingHeader)CreateTransferCodingHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncodingChunked)
                    .Use((bool?)randomHeaderProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Upgrade)
                    .Use((randomHeaderProperties.Upgrade as dynamic[])
                        .Select(header =>
                            (ProductHeader)CreateProductHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.UserAgent)
                    .Use((randomHeaderProperties.UserAgent as dynamic[])
                        .Select(header =>
                            (ProductInfoHeader)CreateProductInfoHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Via)
                    .Use((randomHeaderProperties.Via as dynamic[])
                        .Select(header =>
                            (ViaHeader)CreateViaHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Warning)
                    .Use((randomHeaderProperties.Warning as dynamic[])
                        .Select(header =>
                            (WarningHeader)CreateWarningHeader(header))
                        .ToArray());

            return filler;
        }

        private static HttpExchangeResponseHeaders CreateHttpExchangeResponseHeaders(
            dynamic randomResponseHeadersProperties) =>
                CreateHttpExchangeResponseHeadersFiller(randomResponseHeadersProperties).Create();

        private static Filler<HttpExchangeResponseHeaders> CreateHttpExchangeResponseHeadersFiller(
            dynamic randomResponseHeadersProperties)
        {
            var filler = new Filler<HttpExchangeResponseHeaders>();

            filler.Setup()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.AcceptRanges)
                    .Use((string[])randomResponseHeadersProperties.AcceptRanges)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Age)
                    .Use((TimeSpan?)randomResponseHeadersProperties.Age)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.CacheControl)
                    .Use((CacheControlHeader)CreateCacheControlHeader(
                        randomResponseHeadersProperties.CacheControl))

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Connection)
                    .Use((string[])randomResponseHeadersProperties.Connection)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ConnectionClose)
                    .Use((bool?)randomResponseHeadersProperties.ConnectionClose)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Date)
                    .Use((DateTimeOffset?)randomResponseHeadersProperties.Date)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ETag)
                    .Use((string)randomResponseHeadersProperties.ETag)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Location)
                    .Use((Uri)randomResponseHeadersProperties.Location)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Pragma)
                    .Use((randomResponseHeadersProperties.Pragma as dynamic[])
                        .Select(pragmaHeader =>
                            (NameValueHeader)CreateNameValueHeader(pragmaHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ProxyAuthenticate)
                    .Use((randomResponseHeadersProperties.ProxyAuthenticate as dynamic[])
                        .Select(proxyAuthenticateHeader =>
                            (AuthenticationHeader)CreateAuthorizationHeader(proxyAuthenticateHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.RetryAfter)
                    .Use((RetryConditionHeader)CreateRetryConditionHeader(randomResponseHeadersProperties.RetryAfter))

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Server)
                    .Use((randomResponseHeadersProperties.Server as dynamic[])
                            .Select(serverHeader =>
                                (ProductInfoHeader)CreateProductInfoHeader(serverHeader))
                            .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Trailer)
                    .Use((string[])randomResponseHeadersProperties.Trailer)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncoding)
                    .Use((randomResponseHeadersProperties.TransferEncoding as dynamic[])
                        .Select(transferEncodingHeader =>
                            (TransferCodingHeader)CreateTransferCodingHeader(transferEncodingHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncodingChunked)
                    .Use((bool?)randomResponseHeadersProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Upgrade)
                    .Use((randomResponseHeadersProperties.Upgrade as dynamic[])
                        .Select(upgradeHeader =>
                            (ProductHeader)CreateProductHeader(upgradeHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Vary)
                    .Use((string[])randomResponseHeadersProperties.Vary)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Via)
                    .Use((randomResponseHeadersProperties.Via as dynamic[])
                        .Select(viaHeader =>
                            (ViaHeader)CreateViaHeader(viaHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Warning)
                    .Use((randomResponseHeadersProperties.Warning as dynamic[])
                        .Select(warningHeader =>
                            (WarningHeader)CreateWarningHeader(warningHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.WwwAuthenticate)
                    .Use((randomResponseHeadersProperties.WwwAuthenticate as dynamic[])
                        .Select(wwwAuthenticateHeader =>
                            (AuthenticationHeader)CreateAuthorizationHeader(wwwAuthenticateHeader))
                        .ToArray());

            return filler;
        }

        private static HttpExchangeContentHeaders CreateHttpExchangeContentHeaders(
            dynamic randomHeaderProperties)
        {
            HttpExchangeContentHeaders httpExchangeContentHeaders = null;

            if (randomHeaderProperties is not null)
            {
                httpExchangeContentHeaders =
                    CreateHttpExchangeContentHeadersFiller(randomHeaderProperties).Create();
            }

            return httpExchangeContentHeaders;
        }

        private static Filler<HttpExchangeContentHeaders> CreateHttpExchangeContentHeadersFiller(
            dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeContentHeaders>();

            filler.Setup()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Allow)
                    .Use((string[])randomHeaderProperties.Allow)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentDisposition)
                    .Use((ContentDispositionHeader)CreateContentDispositionHeader(
                        randomHeaderProperties.ContentDisposition))

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentEncoding)
                    .Use((string[])randomHeaderProperties.ContentEncoding)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLanguage)
                    .Use((string[])randomHeaderProperties.ContentLanguage)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLength)
                    .Use((long?)randomHeaderProperties.ContentLength)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLocation)
                    .Use((Uri)randomHeaderProperties.ContentLocation)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentMD5)
                    .Use((byte[])randomHeaderProperties.ContentMD5)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentRange)
                    .Use((ContentRangeHeader)CreateContentRangeHeader(
                            randomHeaderProperties.ContentRange))

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentType)
                    .Use((MediaTypeHeader)CreateMediaTypeHeader(
                            randomHeaderProperties.ContentType))

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Expires)
                    .Use((DateTimeOffset?)randomHeaderProperties.Expires)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.LastModified)
                    .Use((DateTimeOffset?)randomHeaderProperties.LastModified);

            return filler;
        }
    }
}
