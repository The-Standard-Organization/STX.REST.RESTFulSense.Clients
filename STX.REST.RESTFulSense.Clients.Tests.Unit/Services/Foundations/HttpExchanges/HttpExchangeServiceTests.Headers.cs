// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Force.DeepCloner;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private static dynamic CreateRandomHttpRequestHeader()
        {
            return new
            {
                Accept = CreateRandomMediaTypeHeaderArray(),
                AcceptCharset = CreateRandomStringQualityHeaderArray(),
                AcceptEncoding = CreateRandomStringQualityHeaderArray(),
                AcceptLanguage = CreateRandomStringQualityHeaderArray(),
                AuthenticationHeader = CreateRandomAuthenticationHeader(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = GetRandomString(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDateTime(),
                Expect = CreateRandomNameValueArray(),
                ExpectContinue = GetRandomBoolean(),
                ExpectCore = CreateRandomNameValueWithParametersArray(),
                From = GetRandomString(),
                Host = GetRandomString(),
                IfMatch = GetRandomString(),
                IfModifiedSince = GetRandomDateTime(),
                IfNoneMatch = GetRandomString(),
                IfRange = CreateRandomRangeConditionHeader(),
                IfUnmodifiedSince = GetRandomDateTime(),
                MaxForwards = GetRandomNumber(),
                Pragma = CreateRandomNameValueArray(),
                Protocol = GetRandomString(),
                ProxyAuthorization = CreateRandomAuthenticationHeader(),
                Range = CreateRandomRangeHeader(),
                Referrer = CreateRandomUri(),
                TE = CreateTransferCodingHeaderArray(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateTransferCodingHeaderArray(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateRandomProductHeaderArray(),
                UserAgent = CreateRandomProductInfoHeaderArray(),
                Via = CreateViaHeader(),
                Warning = CreateWarningHeader()
            };
        }

        private static dynamic CreateRandomHttpResponseHeader()
        {
            return new
            {
                AcceptRanges = CreateRandomStringArray(),
                Age = GetRandomTimeSpan(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = CreateRandomStringArray(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDateTime(),
                ETag = $"\"{GetRandomString()}\"",
                Location = new Uri(CreateRandomBaseAddress()),
                Pragma = CreateRandomNameValueArray(),
                ProxyAuthenticate = CreateRandomAuthenticationHeaderArray(),
                RetryAfter = CreateRandomRetryConditionHeader(),
                Server = CreateRandomProductInfoHeaderArray(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateTransferCodingHeaderArray(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateRandomProductHeaderArray(),
                Vary = CreateRandomStringArray(),
                Via = CreateViaHeaderArray(),
                Warning = CreateWarningHeaderArray(),
                WwwAuthenticate = CreateRandomAuthenticationHeaderArray()
            };
        }

        private static dynamic[] CreateRandomMediaTypeHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                    CreateRandomMediaTypeHeader())
                .ToArray();
        }

        private static dynamic CreateRandomMediaTypeHeader()
        {
            return new
            {
                Charset = GetRandomString(),
                MediaType = GetRandomString(),
                Quality = GetRandomDouble()
            };
        }

        private static dynamic[] CreateRandomStringQualityHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Value = GetRandomString(),
                    Quality = GetRandomDouble()
                };
            }).ToArray();
        }

        private static dynamic[] CreateRandomAuthenticationHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                    CreateRandomAuthenticationHeader())
                .ToArray();
        }

        private static dynamic CreateRandomAuthenticationHeader()
        {
            return new
            {
                Schema = GetRandomString(),
                Value = GetRandomString()
            };
        }

        private static dynamic CreateRandomCacheControlHeader()
        {
            return new
            {
                NoCache = GetRandomBoolean(),
                NoCacheHeaders = CreateRandomStringArray(),
                NoStore = GetRandomBoolean(),
                MaxAge = GetRandomTimeSpan(),
                SharedMaxAge = GetRandomTimeSpan(),
                MaxStale = GetRandomBoolean(),
                MaxStaleLimit = GetRandomTimeSpan(),
                MinFresh = GetRandomTimeSpan(),
                NoTransform = GetRandomBoolean(),
                OnlyIfCached = GetRandomBoolean(),
                Public = GetRandomBoolean(),
                Private = GetRandomBoolean(),
                PrivateHeaders = CreateRandomStringArray(),
                MustRevalidate = GetRandomBoolean(),
                ProxyRevalidate = GetRandomBoolean(),
                Extensions = CreateRandomNameValueArray()
            };
        }

        private static dynamic CreateRandomRangeConditionHeader()
        {
            return new
            {
                Date = GetRandomDateTime(),
                EntityTag = GetRandomString()
            };
        }

        private static dynamic CreateRandomRangeHeader()
        {
            return new
            {
                Unit = GetRandomString(),
                Ranges = CreateRandomRangeItemHeaderArray()
            };
        }

        private static dynamic CreateRandomRangeItemHeader()
        {
            return new
            {
                From = GetRandomLong(),
                To = GetRandomLong()
            };
        }

        private static dynamic CreateRandomRangeItemHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomRangeItemHeader())
                .ToArray();
        }

        private static dynamic[] CreateTransferCodingHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Quality = GetRandomDouble(),
                    Value = GetRandomString(),
                    Parameters = CreateRandomNameValueArray()
                };
            }).ToArray();
        }

        private static dynamic CreateRandomProductInfoHeader()
        {
            return new
            {
                Comment = $"({GetRandomString()})",
                ProductHeader = CreateRandomProductHeader()
            };
        }

        private static dynamic CreateRandomProductHeader()
        {
            return new
            {
                Name = GetRandomString(),
                Version = GetRandomString()
            };
        }

        private static dynamic[] CreateRandomProductInfoHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomProductInfoHeader())
                .ToArray();
        }

        private static dynamic[] CreateViaHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateViaHeader())
                .ToArray();
        }

        private static dynamic CreateViaHeader()
        {
            return new
            {
                ProtocolName = GetRandomString(),
                ProtocolVersion = GetRandomString(),
                ReceivedBy = GetRandomString(),
                Comment = $"({GetRandomString()})",
            };
        }

        private static dynamic[] CreateWarningHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateWarningHeader())
                .ToArray();
        }

        private static dynamic CreateWarningHeader()
        {
            return new
            {
                Code = GetRandomNumber(),
                Agent = GetRandomString(),
                Text = $"\"GetRandomString()\"",
                Date = GetRandomDateTime(),
            };
        }

        private static dynamic CreateRandomRetryConditionHeader()
        {
            return new
            {
                Date = GetRandomDateTime(),
                Delta = GetRandomTimeSpan()
            };
        }

        private static dynamic[] CreateRandomProductHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomProductHeader())
                .ToArray();
        }

        private static dynamic CreateRandomHttpContentHeader()
        {
            return new
            {
                Allow = CreateRandomStringArray(),

                ContentDisposition =
                    CreateRandomContentDispositionHeader(),
                
                ContentEncoding = CreateRandomStringArray(),
                ContentLanguage = CreateRandomStringArray(),
                ContentLength = GetRandomNumber(),
                ContentLocation = CreateRandomUri(),
                ContentMD5 = CreateRandomByteArray(),
                ContentRange = CreateRandomContentRangeHeader(),
                ContentType = CreateRandomMediaTypeHeader(),
                Expires = GetRandomDateTime(),
                LastModified = GetRandomDateTime()
            };
        }

        private static dynamic CreateRandomContentDispositionHeader()
        {
            return new
            {
                DispositionType = GetRandomString(),
                Name = GetRandomString(),
                FileName = GetRandomString(),
                FileNameStar = GetRandomString(),
                CreationDate = GetRandomDateTime(),
                ModificationDate = GetRandomDateTime(),
                ReadDate = GetRandomDateTime(),
                Size = GetRandomLong(),
            };
        }

        private static dynamic CreateRandomContentRangeHeader()
        {
            return new
            {
                Unit = GetRandomString(),
                From = GetRandomLong(),
                To = GetRandomLong(),
                Length = GetRandomLong()
            };
        }

        private static ContentDispositionHeader MapHttpExchangeContentDispositionHeader(
            dynamic randomContentDispositionHeaderProperties)
        {
            var contentDispositionHeader = new ContentDispositionHeader
            {
                DispositionType = randomContentDispositionHeaderProperties.DispositionType,
                Name = randomContentDispositionHeaderProperties.Name,
                FileName = randomContentDispositionHeaderProperties.FileName,
                FileNameStar = randomContentDispositionHeaderProperties.FileNameStar,
                CreationDate = randomContentDispositionHeaderProperties.CreationDate,
                ModificationDate = randomContentDispositionHeaderProperties.ModificationDate,
                ReadDate = randomContentDispositionHeaderProperties.ReadDate,
                Size = randomContentDispositionHeaderProperties.Size
            };

            return contentDispositionHeader;
        }

        private static ContentRangeHeader MapHttpExchangeContentRangeHeader(
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

        private static MediaTypeHeader MapHttpExchangeMediaTypeHeader(
            dynamic randomMediaTypeHeaderProperties)
        {
            var mediaTypeHeader = new MediaTypeHeader
            {
                CharSet = randomMediaTypeHeaderProperties.Charset,
                MediaType = randomMediaTypeHeaderProperties.MediaType,
                Quality = randomMediaTypeHeaderProperties.Quality
            };

            return mediaTypeHeader;
        }

        private static CacheControlHeader MapHttpExchangeCacheControlHeader(
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

                Extensions = (randomCacheControlHeaderProperties.Extensions as dynamic[])
                    .Select(extensionHeader =>
                        (NameValueHeader)MapHttpExchangeNameValueHeader(extensionHeader))
                    .ToArray()
            };

            return cacheControlHeader;
        }

        private static NameValueHeader MapHttpExchangeNameValueHeader(
            dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeader
            {
                Name = randomNameValueHeaderProperties.Name,
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static AuthenticationHeader MapHttpExchangeAuthenticateHeader(
            dynamic randomAuthenticationHeaderProperties)
        {
            return new AuthenticationHeader
            {
                Schema = randomAuthenticationHeaderProperties.Schema,
                Value = randomAuthenticationHeaderProperties.Value
            };
        }

        private static ProductInfoHeader MapHttpExchangeProductInfoHeader(
            dynamic randomProductInfoHeaderProperties)
        {
            return new ProductInfoHeader
            {
                Comment = randomProductInfoHeaderProperties.Comment,
                ProductHeader =
                    (ProductHeader)MapHttpExchangeProductHeader(
                        randomProductInfoHeaderProperties.ProductHeader)
            };
        }

        private static ProductHeader MapHttpExchangeProductHeader(
            dynamic randomProductHeaderProperties)
        {
            return new ProductHeader
            {
                Name = randomProductHeaderProperties.Name,
                Version = randomProductHeaderProperties.Version
            };
        }

        private static TransferCodingHeader MapHttpExchangeTransferCodingHeader(
            dynamic randomTransferCodingHeaderProperties)
        {
            return new TransferCodingHeader
            {
                Quality = randomTransferCodingHeaderProperties.Quality,
                Value = randomTransferCodingHeaderProperties.Value,
                Parameters = (randomTransferCodingHeaderProperties.Parameters as dynamic[])
                    .Select(parametersHeader =>
                        (NameValueHeader)MapHttpExchangeNameValueHeader(parametersHeader))
                    .ToArray()
            };
        }

        private static ViaHeader MapHttpExchangeViaHeader(
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

        private static WarningHeader MapHttpExchangeWarningHeader(
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

        private static StringWithQualityHeader MapHttpExchangeStringWithQualityHeader(
            dynamic randomStringWithQualityHeaderProperties)
        {
            var stringWithQualityHeader = new StringWithQualityHeader
            {
                Value = randomStringWithQualityHeaderProperties.Value,
                Quality = randomStringWithQualityHeaderProperties.Quality
            };

            return stringWithQualityHeader;
        }

        private static NameValueHeaderValue MapHttpResponseNameValueHeader(
            dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeaderValue(randomNameValueHeaderProperties.Name)
            {
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static CacheControlHeaderValue MapHttpResponseCacheControlHeader(
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
                            MapHttpResponseNameValueHeader(extensionHeader));

                        return extensionHeader;
                    })
                    .ToArray();

            return cacheControlHeaderValue;
        }

        private static RetryConditionHeaderValue MapHttpResponseRetryConditionHeaderValue(
            dynamic randomRetryConditionHeaderProperties)
        {
            return new RetryConditionHeaderValue(randomRetryConditionHeaderProperties.Date);
        }

        private static NameValueWithParameters MapHttpExchangeNameValueWithParameters(
           dynamic randomNameValueWithParametersProperties)
        {
            var nameValueWithParameters = new NameValueWithParameters
            {
                Name = randomNameValueWithParametersProperties.Name,
                Parameters = (randomNameValueWithParametersProperties.Parameters as dynamic[])
                    .Select(header =>
                        (NameValueHeader)MapHttpExchangeNameValueHeader(header)).ToArray()
            };

            return nameValueWithParameters;
        }

        private static HttpExchangeRequestHeaders CreateHttpExchangeRequestHeaders(
            dynamic randomHeaderProperties) =>
                CreateHttpExchangeRequestHeadersFiller(randomHeaderProperties)
            .Create();

        private static Filler<HttpExchangeRequestHeaders> CreateHttpExchangeRequestHeadersFiller(
            dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeRequestHeaders>();
            
            filler.Setup()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Accept)
                    .Use(() => (randomHeaderProperties.Accept as dynamic[])
                        .Select(header =>
                            (MediaTypeHeader)MapHttpExchangeMediaTypeHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptCharset)
                    .Use(() => (randomHeaderProperties.AcceptCharset as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)MapHttpExchangeStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptEncoding)
                    .Use(() => (randomHeaderProperties.AcceptEncoding as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)MapHttpExchangeStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptLanguage)
                    .Use(() => (randomHeaderProperties.AcceptLanguage as dynamic[])
                        .Select(header =>
                            (StringWithQualityHeader)MapHttpExchangeStringWithQualityHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Authorization)
                    .Use(() => (AuthenticationHeader)MapHttpExchangeAuthenticateHeader(
                        randomHeaderProperties.AuthenticationHeader))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.CacheControl)
                    .Use(() => (CacheControlHeader)MapHttpExchangeCacheControlHeader(
                        randomHeaderProperties.CacheControl))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Connection)
                    .Use((string)randomHeaderProperties.Connection)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ConnectionClose)
                    .Use((bool?)randomHeaderProperties.ConnectionClose)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Date)
                    .Use((DateTimeOffset?)randomHeaderProperties.Date)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Expect)
                    .Use(() => (randomHeaderProperties.Expect as dynamic[])
                        .Select(header => (NameValueHeader)MapHttpExchangeNameValueHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectContinue)
                    .Use((bool?)randomHeaderProperties.ExpectContinue)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectCore)
                    .Use(() => (randomHeaderProperties.ExpectCore as dynamic[])
                        .Select(header =>
                            (NameValueWithParameters)MapHttpExchangeNameValueWithParameters(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.From)
                    .Use((string)randomHeaderProperties.From)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Host)
                    .Use((string)randomHeaderProperties.Host)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfMatch)
                    .Use((string)randomHeaderProperties.IfMatch)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfModifiedSince)
                    .Use((DateTimeOffset?)randomHeaderProperties.IfModifiedSince)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfNoneMatch)
                    .Use((string)randomHeaderProperties.IfNoneMatch)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfRange)
                    .Use(() => new RangeConditionHeader
                    {
                        Date = randomHeaderProperties.IfRange.Date,
                        EntityTag = randomHeaderProperties.IfRange.EntityTag
                    })

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfUnmodifiedSince)
                    .Use((DateTimeOffset?)randomHeaderProperties.IfUnmodifiedSince)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.MaxForwards)
                    .Use((int?)randomHeaderProperties.MaxForwards)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Pragma)
                    .Use(() => (randomHeaderProperties.Pragma as dynamic[])
                        .Select(header =>
                            (NameValueHeader)MapHttpExchangeNameValueHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Protocol)
                    .Use((string)randomHeaderProperties.Protocol)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ProxyAuthorization)
                    .Use(() => (AuthenticationHeader)MapHttpExchangeAuthenticateHeader(
                        randomHeaderProperties.ProxyAuthorization))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Range)
                    .Use(() => new RangeHeader
                    {
                        Unit = randomHeaderProperties.Range.Unit,
                        Ranges = (randomHeaderProperties.Range.Ranges as dynamic[])
                        
                        .Select(header =>
                            new RangeItemHeader
                            {
                                From = (long?)header.From,
                                To = (long?)header.To
                            })
                        .ToArray()
                    })

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Referrer)
                    .Use((Uri)randomHeaderProperties.Referrer)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TE)
                    .Use(() => (randomHeaderProperties.TE as dynamic[])
                        .Select(header =>
                            (TransferCodingHeader)MapHttpExchangeTransferCodingHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Trailer)
                    .Use((string[])randomHeaderProperties.Trailer)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncoding)
                    .Use(() => (randomHeaderProperties.TransferEncoding as dynamic[])
                        .Select(header =>
                            (TransferCodingHeader)MapHttpExchangeTransferCodingHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncodingChunked)
                    .Use((bool?)randomHeaderProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Upgrade)
                    .Use(() => (randomHeaderProperties.Upgrade as dynamic[])
                        .Select(header =>
                            (ProductHeader)MapHttpExchangeProductHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.UserAgent)
                    .Use(() => (randomHeaderProperties.UserAgent as dynamic[])
                        .Select(header =>
                            (ProductInfoHeader)MapHttpExchangeProductInfoHeader(header))
                        .ToArray())

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Via)
                    .Use(() =>
                        (ViaHeader)MapHttpExchangeViaHeader(randomHeaderProperties.Via))

                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Warning)
                    .Use(() =>
                        (WarningHeader)MapHttpExchangeWarningHeader(randomHeaderProperties.Warning));

            return filler;
        }

        private static HttpExchangeContent CreateHttpExchangeContent(
            dynamic randomContentProperties) =>
                CreateHttpExchangeContentFiller(randomContentProperties).Create();

        private static Filler<HttpExchangeContent> CreateHttpExchangeContentFiller(
            dynamic randomContentProperties)
        {
            var filler = new Filler<HttpExchangeContent>();
            
            filler.Setup()
                .OnProperty(httpExchangeContent => httpExchangeContent.Headers)
                    .Use((HttpExchangeContentHeaders)CreateHttpExchangeContentHeaders(
                        randomContentProperties.Headers))

                .OnProperty(httpExchangeContent => httpExchangeContent.StreamContent)
                    .Use(new ValueTask<Stream>(((Stream)randomContentProperties.StreamContent)
                    .DeepClone()));

            return filler;
        }

        private static HttpExchangeContentHeaders CreateHttpExchangeContentHeaders(
            dynamic randomHeaderProperties) =>
                CreateHttpExchangeContentHeadersFiller(randomHeaderProperties).Create();

        private static Filler<HttpExchangeContentHeaders> CreateHttpExchangeContentHeadersFiller(
            dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeContentHeaders>();
            
            filler.Setup()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Allow)
                    .Use((string[])randomHeaderProperties.Allow)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentDisposition)
                    .Use(() => (ContentDispositionHeader)MapHttpExchangeContentDispositionHeader(
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
                    .Use(() => (ContentRangeHeader)MapHttpExchangeContentRangeHeader(
                        randomHeaderProperties.ContentRange))

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentType)
                    .Use(() => (MediaTypeHeader)MapHttpExchangeMediaTypeHeader(
                        randomHeaderProperties.ContentType))

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Expires)
                    .Use((DateTimeOffset)randomHeaderProperties.Expires)

                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.LastModified)
                    .Use((DateTimeOffset)randomHeaderProperties.LastModified);

            return filler;
        }

        private static HttpExchangeResponseHeaders CreateHttpExchangeResponseHeaders(
            dynamic randomHeadersProperties) =>
                CreateHttpExchangeResponseHeadersFiller(randomHeadersProperties).Create();

        private static Filler<HttpExchangeResponseHeaders> CreateHttpExchangeResponseHeadersFiller(
            dynamic randomHeadersProperties)
        {
            var filler = new Filler<HttpExchangeResponseHeaders>();

            filler.Setup()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.AcceptRanges)
                    .Use((string[])randomHeadersProperties.AcceptRanges)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Age)
                    .Use((TimeSpan?)randomHeadersProperties.Age)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.CacheControl)
                    .Use(() => MapHttpExchangeCacheControlHeader(randomHeadersProperties.CacheControl))

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Connection)
                    .Use((string[])randomHeadersProperties.Connection)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ConnectionClose)
                    .Use((bool?)randomHeadersProperties.ConnectionClose)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Date)
                    .Use((DateTimeOffset?)randomHeadersProperties.Date)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ETag)
                    .Use((string)randomHeadersProperties.ETag)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Location)
                    .Use((Uri)randomHeadersProperties.Location)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Pragma)
                    .Use(() =>
                        (randomHeadersProperties.Pragma as dynamic[])
                        .Select(pragmaHeader => 
                            (NameValueHeader)MapHttpExchangeNameValueHeader(pragmaHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ProxyAuthenticate)
                    .Use(() =>
                        (randomHeadersProperties.ProxyAuthenticate as dynamic[])
                        .Select(proxyAuthenticateHeader =>
                            (AuthenticationHeader)MapHttpExchangeAuthenticateHeader(proxyAuthenticateHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.RetryAfter)
                    .Use(() =>
                        new RetryConditionHeader
                        {
                            Date = (DateTimeOffset?)randomHeadersProperties.RetryAfter.Date,
                            Delta = (TimeSpan?)randomHeadersProperties.RetryAfter.Delta
                        })

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Server)
                    .Use(() => (randomHeadersProperties.Server as dynamic[])
                        .Select(serverHeader =>
                            (ProductInfoHeader)MapHttpExchangeProductInfoHeader(serverHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Trailer)
                    .Use((string[])randomHeadersProperties.Trailer)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncoding)
                    .Use(() => (randomHeadersProperties.TransferEncoding as dynamic[])
                        .Select(transferEncodingHeader =>
                            (TransferCodingHeader)MapHttpExchangeTransferCodingHeader(transferEncodingHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncodingChunked)
                    .Use((bool?)randomHeadersProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Upgrade)
                    .Use(() => (randomHeadersProperties.Upgrade as dynamic[])
                        .Select(upgradeHeader =>
                            (ProductHeader)MapHttpExchangeProductHeader(upgradeHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Vary)
                    .Use((string[])randomHeadersProperties.Vary)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Via)
                    .Use(() => (randomHeadersProperties.Via as dynamic[])
                        .Select(viaHeader => (ViaHeader)MapHttpExchangeViaHeader(viaHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Warning)
                    .Use(() => (randomHeadersProperties.Warning as dynamic[])
                        .Select(warningHeader =>
                            (WarningHeader)MapHttpExchangeWarningHeader(warningHeader))
                        .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.WwwAuthenticate)
                    .Use(() =>
                        (randomHeadersProperties.WwwAuthenticate as dynamic[])
                        .Select(wwwAuthenticateHeader =>
                            (AuthenticationHeader)MapHttpExchangeAuthenticateHeader(wwwAuthenticateHeader))
                        .ToArray());

            return filler;
        }
    }
}
