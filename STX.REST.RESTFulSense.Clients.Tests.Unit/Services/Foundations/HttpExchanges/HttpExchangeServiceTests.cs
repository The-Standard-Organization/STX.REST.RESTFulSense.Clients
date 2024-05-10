// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private readonly Mock<IHttpBroker> httpBroker;
        private readonly IHttpExchangeService httpExchangeService;

        public HttpExchangeServiceTests()
        {
            this.httpBroker = new Mock<IHttpBroker>();

            this.httpExchangeService =
                new HttpExchangeService(httpBroker.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static double GetRandomDouble() =>
            new DoubleRange(minValue: 2, maxValue: 10).GetValue();

        private static TimeSpan GetRandomTimeSpan() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue().TimeOfDay;

        private static DateTimeOffset GetRandomDate() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static bool GetRandomBoolean() =>
            Randomizer<bool>.Create();

        private static string[] CreateRandomStringArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(i => GetRandomString())
                    .ToArray();
        }

        private static byte[] CreateRandomByteArray()
        {
            string randomString = GetRandomString();

            return Encoding.UTF8.GetBytes(randomString);
        }

        private static string CreateRandomBaseAddress()
        {
            string randomHost = GetRandomString();

            return $"{Uri.UriSchemeHttps}://{randomHost}";
        }

        private static Uri CreateRandomUri()
        {
            string baseAddress = CreateRandomBaseAddress();
            string randomRelativeURL = GetRandomString();
            IDictionary<string, object> urlParameters = CreateRandomUrlParameters();
            randomRelativeURL =
                randomRelativeURL +
                string.Join("&",
                    urlParameters.Select(item =>
                        $"{item.Key}={item.Value}"));

            return new Uri(
                    new Uri(baseAddress),
                    randomRelativeURL);
        }

        private static IDictionary<string, object> CreateRandomUrlParameters()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(index => GetRandomString() as object)
                .Concat(
                    Enumerable.Range(0, GetRandomNumber())
                    .Select(index => GetRandomNumber() as object))
                .ToDictionary(
                    key => GetRandomString(),
                    value => value);
        }

        private static Version GetRandomHttpVersion()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 5);

            return randomNumber switch
            {
                0 => HttpVersion.Version10,
                1 => HttpVersion.Version11,
                2 => HttpVersion.Version20,
                3 => HttpVersion.Version30,
                _ => HttpVersion.Unknown,
            };
        }

        private static HttpVersionPolicy GetRandomHttpVersionPolicy()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 2);

            return randomNumber switch
            {
                0 => HttpVersionPolicy.RequestVersionOrLower,
                1 => HttpVersionPolicy.RequestVersionOrHigher,
                _ => HttpVersionPolicy.RequestVersionExact,
            };
        }

        private static HttpStatusCode GetRandomHttpStatusCode()
        {
            int min = (int)HttpStatusCode.Continue;
            int max = (int)HttpStatusCode.NetworkAuthenticationRequired;
            Random random = new Random();

            HttpStatusCode randomHttpStatusCode =
                (HttpStatusCode)random.Next(min, max + 1);

            return randomHttpStatusCode;
        }

        private static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode) =>
            httpStatusCode >= HttpStatusCode.OK && httpStatusCode < HttpStatusCode.MultipleChoices;

        private static dynamic[] CreateRandomNameValueArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Name = GetRandomString(),
                    Value = GetRandomString()
                };
            }).ToArray();
        }

        private static dynamic[] CreateRandomNameValueWithParametersArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Name = GetRandomString(),
                    Parameters = CreateRandomNameValueArray()
                };
            }).ToArray();
        }

        private static dynamic CreateRandomAuthenticationHeader()
        {
            return new
            {
                Schema = GetRandomString(),
                Value = GetRandomString()
            };
        }

        private static dynamic[] CreateRandomAuthenticationHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Schema = GetRandomString(),
                    Value = GetRandomString()
                };
            }).ToArray();
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

        private static dynamic CreateRandomProductHeader()
        {
            return new
            {
                Name = GetRandomString(),
                Version = GetRandomString()
            };
        }

        private static dynamic[] CreateRandomProductHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                CreateRandomProductHeader())
                .ToArray();
        }

        private static dynamic CreateRandomRetryConditionHeader()
        {
            return new
            {
                Date = GetRandomDate(),
                Delta = GetRandomTimeSpan()
            };
        }

        private static dynamic CreateRandomProductInfoHeader()
        {
            return new
            {
                Comment = $"({GetRandomString()})",
                ProductHeader = CreateRandomProductHeader()
            };
        }

        private static dynamic[] CreateRandomProductInfoHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                CreateRandomProductInfoHeader())
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
        private static dynamic[] CreateViaHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    ProtocolName = GetRandomString(),
                    ProtocolVersion = GetRandomString(),
                    ReceivedBy = GetRandomString(),
                    Comment = $"({GetRandomString()})",
                };
            }).ToArray();
        }
        
        private static dynamic CreateWarningHeader()
        {
            return new
            {
                Code = GetRandomNumber(),
                Agent = GetRandomString(),
                Text = $"\"GetRandomString()\"",
                Date = GetRandomDate(),
            };
        }

        private static dynamic[] CreateWarningHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Code = GetRandomNumber(),
                    Agent = GetRandomString(),
                    Text = $"\"GetRandomString()\"",
                    Date = GetRandomDate(),
                };
            }).ToArray();
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

        private static dynamic[] CreateRandomMediaTypeHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                CreateRandomMediaTypeHeader())
                .ToArray();
        }

        private static dynamic CreateRandomRangeConditionHeader()
        {
            return new
            {
                Date = GetRandomDate(),
                EntityTag = GetRandomString()
            };
        }

        private static dynamic CreateRandomRangeItemHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                    CreateRandomRangeItemHeader())
                .ToArray();
        }
        
        private static dynamic CreateRandomRangeItemHeader()
        {
            return new
            {
                From = GetRandomNumber(),
                To = GetRandomNumber()
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

        private static dynamic CreateRandomContentDispositionHeader()
        {
            return new
            {
                DispositionType = GetRandomString(),
                Name = GetRandomString(),
                FileName = GetRandomString(),
                FileNameStar = GetRandomString(),
                CreationDate = GetRandomDate(),
                ModificationDate = GetRandomDate(),
                ReadDate = GetRandomDate(),
                Size = GetRandomNumber(),
            };
        }

        private static dynamic CreateRandomContentRangeHeader()
        {
            return new
            {
                Unit = GetRandomString(),
                From = GetRandomNumber(),
                To = GetRandomNumber(),
                Length = GetRandomNumber()
            };
        }

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
                Date = GetRandomDate(),
                Expect = CreateRandomNameValueArray(),
                ExpectContinue = GetRandomBoolean(),
                ExpectCore = CreateRandomNameValueWithParametersArray(),
                From = GetRandomString(),
                Host = GetRandomString(),
                IfMatch = GetRandomString(),
                IfModifiedSince = GetRandomDate(),
                IfNoneMatch = GetRandomString(),
                IfRange = CreateRandomRangeConditionHeader(),
                IfUnmodifiedSince = GetRandomDate(),
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
                Date = GetRandomDate(),
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

        private static dynamic CreateRandomHttpContentHeader()
        {
            return new
            {
                Allow = CreateRandomStringArray(),
                ContentDisposition = CreateRandomContentDispositionHeader(),
                ContentEncoding = CreateRandomStringArray(),
                ContentLanguage = CreateRandomStringArray(),
                ContentLength = GetRandomNumber(),
                ContentLocation = CreateRandomUri(),
                ContentMD5 = CreateRandomByteArray(),
                ContentRange = CreateRandomContentRangeHeader(),
                ContentType = CreateRandomMediaTypeHeader(),
                Expires = GetRandomDate(),
                LastModified = GetRandomDate()
            };
        }

        private static Stream CreateRandomStream()
        {
            string randomContent = GetRandomString();
            byte[] contentBytes = Encoding.ASCII.GetBytes(randomContent);
            var stream = new ReadOnlyMemoryContent(contentBytes)
                .ReadAsStream();

            return stream;
        }

        private static dynamic CreateRandomHttpContent()
        {
            return new
            {
                Headers = CreateRandomHttpContentHeader(),
                StreamContent = CreateRandomStream()
            };
        }

        private static dynamic CreateRandomHttpProperties()
        {
            Uri randomUri = CreateRandomUri();
            HttpStatusCode randomStatusCode = GetRandomHttpStatusCode();

            return new
            {
                BaseAddress = randomUri.GetLeftPart(UriPartial.Authority),
                RelativeUrl = randomUri.LocalPath,
                Url = randomUri,
                UrlParameters = CreateRandomUrlParameters(),
                HttpMethod = HttpMethod.Get,
                Version = GetRandomHttpVersion(),
                VersionPolicy = GetRandomHttpVersionPolicy(),
                StatusCode = randomStatusCode,
                IsSuccessStatusCode = IsSuccessStatusCode(randomStatusCode),
                ReasonPhrase = GetRandomString(),
                ResponseHttpStatus = randomStatusCode,
                RequestHeaders = CreateRandomHttpRequestHeader(),
                ResponseHeaders = CreateRandomHttpResponseHeader(),
                RequestContent = CreateRandomHttpContent(),
                ResponseContent = CreateRandomHttpContent()
            };
        }

        private static HttpExchangeRequestHeaders CreateHttpExchangeRequestHeaders(dynamic randomHeaderProperties) =>
            CreateHttpExchangeRequestHeadersFiller(randomHeaderProperties).Create();

        private static Filler<HttpExchangeRequestHeaders> CreateHttpExchangeRequestHeadersFiller(
            dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeRequestHeaders>();
            filler.Setup()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Accept)
                .Use(() => (randomHeaderProperties.Accept as dynamic[])
                    .Select(header =>
                   (MediaTypeHeader)CreateHttpExchangeMediaTypeHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptCharset)
                .Use(() => (randomHeaderProperties.AcceptCharset as dynamic[])
                    .Select(header =>
                        (StringWithQualityHeader)CreateHttpExchangeStringWithQualityHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptEncoding)
                .Use(() => (randomHeaderProperties.AcceptEncoding as dynamic[])
                    .Select(header =>
                        (StringWithQualityHeader)CreateHttpExchangeStringWithQualityHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptLanguage)
                .Use(() => (randomHeaderProperties.AcceptLanguage as dynamic[])
                    .Select(header =>
                        (StringWithQualityHeader)CreateHttpExchangeStringWithQualityHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Authorization)
                .Use(() => (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(
                    randomHeaderProperties.AuthenticationHeader))
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.CacheControl)
                .Use(() => (CacheControlHeader)CreateHttpExchangeCacheControlHeader(
                    randomHeaderProperties.CacheControl))
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Connection)
                .Use((string)randomHeaderProperties.Connection)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ConnectionClose)
                .Use((bool?)randomHeaderProperties.ConnectionClose)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Date)
                .Use((DateTimeOffset?)randomHeaderProperties.Date)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Expect)
                .Use(() => (randomHeaderProperties.Expect as dynamic[])
                    .Select(header => (NameValueHeader)CreateHttpExchangeNameValueHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectContinue)
                .Use((bool?)randomHeaderProperties.ExpectContinue)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectCore)
                .Use(() => (randomHeaderProperties.ExpectCore as dynamic[])
                    .Select(header =>
                        (NameValueWithParameters)CreateHttpExchangeNameValueWithParameters(header)).ToArray())
                
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
                        (NameValueHeader)CreateHttpExchangeNameValueHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Protocol)
                .Use((string)randomHeaderProperties.Protocol)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ProxyAuthorization)
                .Use(() => (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(
                    randomHeaderProperties.ProxyAuthorization))
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Range)
                .Use(() => new RangeHeader
                {
                    Unit = randomHeaderProperties.Range.Unit,
                    Ranges = (randomHeaderProperties.Range.Ranges as dynamic[]).Select(header =>
                        new RangeItemHeader
                        {
                            From = (long?)header.From,
                            To = (long?)header.To
                        }).ToArray()
                })
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Referrer)
                .Use((Uri)randomHeaderProperties.Referrer)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TE)
                .Use(() => (randomHeaderProperties.TE as dynamic[])
                    .Select(header =>
                        (TransferCodingHeader)CreateHttpExchangeTransferCodingHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Trailer)
                .Use((string[])randomHeaderProperties.Trailer)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncoding)
                .Use(() => (randomHeaderProperties.TransferEncoding as dynamic[])
                    .Select(header =>
                        (TransferCodingHeader)CreateHttpExchangeTransferCodingHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncodingChunked)
                .Use((bool?)randomHeaderProperties.TransferEncodingChunked)
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Upgrade)
                .Use(() => (randomHeaderProperties.Upgrade as dynamic[])
                    .Select(header =>
                        (ProductHeader)CreateHttpExchangeProductHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.UserAgent)
                .Use(() => (randomHeaderProperties.UserAgent as dynamic[])
                    .Select(header =>
                        (ProductInfoHeader)CreateHttpExchangeProductInfoHeader(header)).ToArray())
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Via)
                .Use(() =>
                    (ViaHeader)CreateHttpExchangeViaHeader(randomHeaderProperties.Via))
                
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Warning)
                .Use(() =>
                    (WarningHeader)CreateHttpExchangeWarningHeader(randomHeaderProperties.Warning));
            

            return filler;
        }

        private static HttpExchange CreateHttpExchangeRequest(dynamic randomProperties)
        {
            return new HttpExchange
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = randomProperties.BaseAddress,
                    RelativeUrl = randomProperties.RelativeUrl,
                    HttpMethod = HttpMethod.Get.Method,
                    Version = randomProperties.Version.ToString(),
                    UrlParameters = randomProperties.UrlParameters,
                    Headers = CreateHttpExchangeRequestHeaders(randomProperties.RequestHeaders),
                    VersionPolicy = (int)randomProperties.VersionPolicy,
                    Content = CreateHttpExchangeContent(randomProperties.RequestContent)
                }
            };
        }

        private static NameValueHeader CreateHttpExchangeNameValueHeader(dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeader
            {
                Name = randomNameValueHeaderProperties.Name,
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static NameValueWithParameters CreateHttpExchangeNameValueWithParameters(
            dynamic randomNameValueWithParametersProperties)
        {
            var nameValueWithParameters = new NameValueWithParameters
            {
                Name = randomNameValueWithParametersProperties.Name,
                Parameters = (randomNameValueWithParametersProperties.Parameters as dynamic[])
                    .Select(header =>
                        (NameValueHeader)CreateHttpExchangeNameValueHeader(header)).ToArray()
            };

            return nameValueWithParameters;
        }

        private static AuthenticationHeader CreateHttpExchangeAuthenticateHeader(dynamic randomAuthenticationHeaderProperties)
        {
            return new AuthenticationHeader
            {
                Schema = randomAuthenticationHeaderProperties.Schema,
                Value = randomAuthenticationHeaderProperties.Value
            };
        }

        private static ProductInfoHeader CreateHttpExchangeProductInfoHeader(dynamic randomProductInfoHeaderProperties)
        {
            return new ProductInfoHeader
            {
                Comment = randomProductInfoHeaderProperties.Comment,
                ProductHeader =
                    (ProductHeader)CreateHttpExchangeProductHeader(randomProductInfoHeaderProperties.ProductHeader)
            };
        }
        
        private static TransferCodingHeader CreateHttpExchangeTransferCodingHeader(
            dynamic randomTransferCodingHeaderProperties)
        {
            return new TransferCodingHeader
            {
                Quality = randomTransferCodingHeaderProperties.Quality,
                Value = randomTransferCodingHeaderProperties.Value,
                Parameters = (randomTransferCodingHeaderProperties.Parameters as dynamic[])
                    .Select(parametersHeader => (NameValueHeader)CreateHttpExchangeNameValueHeader(parametersHeader))
                    .ToArray()
            };
        }
        
        private static ProductHeader  CreateHttpExchangeProductHeader(dynamic randomProductHeaderProperties)
        {
            return new ProductHeader
            {
                Name = randomProductHeaderProperties.Name,
                Version = randomProductHeaderProperties.Version
            };
        }

        private static ViaHeader CreateHttpExchangeViaHeader(dynamic randomViaHeaderProperties)
        {
            return new ViaHeader
            {
                ProtocolName = randomViaHeaderProperties.ProtocolName,
                ProtocolVersion = randomViaHeaderProperties.ProtocolVersion,
                ReceivedBy = randomViaHeaderProperties.ReceivedBy,
                Comment = randomViaHeaderProperties.Comment
            };
        }

        private static WarningHeader CreateHttpExchangeWarningHeader(dynamic randomWarningHeaderProperties)
        {
            return new WarningHeader
            {
                Code = randomWarningHeaderProperties.Code,
                Agent = randomWarningHeaderProperties.Agent,
                Text = randomWarningHeaderProperties.Text,
                Date = (DateTimeOffset?)randomWarningHeaderProperties.Date
            };
        }
        
        private static CacheControlHeader CreateHttpExchangeCacheControlHeader(dynamic randomCacheControlHeaderProperties)
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
                    .Select(extensionHeader => (NameValueHeader)CreateHttpExchangeNameValueHeader(extensionHeader))
                    .ToArray()
            };

            return cacheControlHeader;
        }

        private static ContentDispositionHeader CreateHttpExchangeContentDispositionHeader(
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

        private static ContentRangeHeader CreateHttpExchangeContentRangeHeader(
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

        private static MediaTypeHeader CreateHttpExchangeMediaTypeHeader(
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

        private static StringWithQualityHeader CreateHttpExchangeStringWithQualityHeader(
            dynamic randomStringWithQualityHeaderProperties)
        {
            var stringWithQualityHeader = new StringWithQualityHeader
            {
                Value = randomStringWithQualityHeaderProperties.Value,
                Quality = randomStringWithQualityHeaderProperties.Quality
            };

            return stringWithQualityHeader;
        }

        private static HttpExchangeResponseHeaders CreateHttpExchangeResponseHeaders(dynamic randomHeadersProperties) =>
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
                .Use(() => CreateHttpExchangeCacheControlHeader(randomHeadersProperties.CacheControl))

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
                    .Select(pragmaHeader => (NameValueHeader)CreateHttpExchangeNameValueHeader(pragmaHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ProxyAuthenticate)
                .Use(() =>
                    (randomHeadersProperties.ProxyAuthenticate as dynamic[])
                    .Select(proxyAuthenticateHeader =>
                        (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(proxyAuthenticateHeader))
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
                    .Select(serverHeader => (ProductInfoHeader)CreateHttpExchangeProductInfoHeader(serverHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Trailer)
                .Use((string[])randomHeadersProperties.Trailer)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncoding)
                .Use(() => (randomHeadersProperties.TransferEncoding as dynamic[])
                    .Select(transferEncodingHeader =>
                        (TransferCodingHeader)CreateHttpExchangeTransferCodingHeader(transferEncodingHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncodingChunked)
                .Use((bool?)randomHeadersProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Upgrade)
                .Use(() => (randomHeadersProperties.Upgrade as dynamic[])
                    .Select(upgradeHeader =>
                        (ProductHeader)CreateHttpExchangeProductHeader(upgradeHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Vary)
                .Use((string[])randomHeadersProperties.Vary)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Via)
                .Use(() => (randomHeadersProperties.Via as dynamic[])
                    .Select(viaHeader => (ViaHeader)CreateHttpExchangeViaHeader(viaHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Warning)
                .Use(() => (randomHeadersProperties.Warning as dynamic[])
                    .Select(warningHeader => (WarningHeader)CreateHttpExchangeWarningHeader(warningHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.WwwAuthenticate)
                .Use(() =>
                    (randomHeadersProperties.WwwAuthenticate as dynamic[])
                    .Select(wwwAuthenticateHeader =>
                        (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(wwwAuthenticateHeader))
                    .ToArray());

            return filler;
        }

        private static HttpExchangeContent CreateHttpExchangeContent(dynamic randomContentProperties) =>
            CreateHttpExchangeContentFiller(randomContentProperties).Create();

        private static Filler<HttpExchangeContent> CreateHttpExchangeContentFiller(dynamic randomContentProperties)
        {
            var filler = new Filler<HttpExchangeContent>();
            filler.Setup()
                .OnProperty(httpExchangeContent => httpExchangeContent.Headers)
                    .Use((HttpExchangeContentHeaders)CreateHttpExchangeContentHeaders(randomContentProperties.Headers))

                .OnProperty(httpExchangeContent => httpExchangeContent.StreamContent)
                    .Use(new ValueTask<Stream>(((Stream)randomContentProperties.StreamContent).DeepClone()));

            return filler;
        }

        private static HttpExchangeContentHeaders CreateHttpExchangeContentHeaders(dynamic randomHeaderProperties) =>
           CreateHttpExchangeContentHeadersFiller(randomHeaderProperties).Create();

        private static Filler<HttpExchangeContentHeaders> CreateHttpExchangeContentHeadersFiller(dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeContentHeaders>();
            filler.Setup()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Allow)
                .Use((string[])randomHeaderProperties.Allow)
                
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentDisposition)
                .Use(() => (ContentDispositionHeader)CreateHttpExchangeContentDispositionHeader(randomHeaderProperties.ContentDisposition))
                
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
                .Use(() => (ContentRangeHeader)CreateHttpExchangeContentRangeHeader(randomHeaderProperties.ContentRange))
                
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentType)
                .Use(() => (MediaTypeHeader)CreateHttpExchangeMediaTypeHeader(randomHeaderProperties.ContentType))
                
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Expires)
                .Use((DateTimeOffset)randomHeaderProperties.Expires)
                
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.LastModified)
                .Use((DateTimeOffset)randomHeaderProperties.LastModified);

            return filler;
        }

        private static HttpExchange CreateHttpExchangeResponse(
           HttpExchange httpExchange,
           dynamic randomProperties)
        {
            HttpExchange mappedHttpExchange = httpExchange.DeepClone();

            mappedHttpExchange.Response =
                new HttpExchangeResponse
                {
                    Content = CreateHttpExchangeContent(randomProperties.ResponseContent),
                    Headers = CreateHttpExchangeResponseHeaders(randomProperties.ResponseHeaders),
                    Version = randomProperties.Version.ToString(),
                    StatusCode = (int)randomProperties.StatusCode,
                    IsSuccessStatusCode = randomProperties.IsSuccessStatusCode,
                    ReasonPhrase = randomProperties.ReasonPhrase
                };

            return mappedHttpExchange;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(dynamic randomProperties)
        {
            return new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod,
            };
        }

        private static NameValueHeaderValue CreateHttpResponseNameValueHeader(dynamic randomNameValueHeaderProperties)
        {
            return new NameValueHeaderValue(randomNameValueHeaderProperties.Name)
            {
                Value = randomNameValueHeaderProperties.Value
            };
        }

        private static CacheControlHeaderValue CreateHttpResponseCacheControlHeader(dynamic randomCacheControlHeaderProperties)
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
                        cacheControlHeaderValue.Extensions.Add(CreateHttpResponseNameValueHeader(extensionHeader));
                        return extensionHeader;
                    })
                    .ToArray();

            return cacheControlHeaderValue;
        }

        private static RetryConditionHeaderValue CreateHttpResponseRetryConditionHeaderValue(
            dynamic randomRetryConditionHeaderProperties)
        {
            return new RetryConditionHeaderValue(randomRetryConditionHeaderProperties.Date){};
        }
        
        private static HttpResponseMessage CreateHttpResponseMessage(dynamic randomProperties)
        {
            var bogusHttpResponse =
                new Faker<HttpResponseMessage>()
                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.Content,
                        new StreamContent(randomProperties.ResponseContent.StreamContent))

                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.IsSuccessStatusCode,
                        (bool)randomProperties.IsSuccessStatusCode)

                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.ReasonPhrase,
                        (string)randomProperties.ReasonPhrase)

                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.StatusCode,
                        (HttpStatusCode)randomProperties.StatusCode)

                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.Version,
                        (Version)randomProperties.Version);

            HttpResponseMessage httpResponseMessage =
                bogusHttpResponse.Generate();

            (randomProperties.ResponseHeaders.AcceptRanges as string[]).Select(header =>
            {
                httpResponseMessage.Headers.AcceptRanges.Add(header);

                return header;
            }).ToArray();

            httpResponseMessage.Headers.Age = (TimeSpan?)randomProperties.ResponseHeaders.Age;
            
            httpResponseMessage.Headers.CacheControl =
                CreateHttpResponseCacheControlHeader(randomProperties.ResponseHeaders.CacheControl);

            (randomProperties.ResponseHeaders.Connection as string[]).Select(header =>
            {
                httpResponseMessage.Headers.Connection.Add(header);

                return header;
            }).ToArray();


            httpResponseMessage.Headers.ConnectionClose =
                (bool?)randomProperties.ResponseHeaders.ConnectionClose;

            httpResponseMessage.Headers.Date =
                (DateTimeOffset?)randomProperties.ResponseHeaders.Date;

            httpResponseMessage.Headers.ETag =
                new EntityTagHeaderValue((string)randomProperties.ResponseHeaders.ETag);

            httpResponseMessage.Headers.Location =
                (Uri)randomProperties.ResponseHeaders.Location;

            (randomProperties.ResponseHeaders.Pragma as dynamic[]).Select(header =>
            {
                var pragmaHeader = new NameValueHeaderValue(header.Name)
                {
                    Value = header.Value
                };
                httpResponseMessage.Headers.Pragma.Add(pragmaHeader);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.ProxyAuthenticate as dynamic[])
                .Select(header =>
                {
                    var proxyAuthenticateHeader = new AuthenticationHeaderValue(
                        header.Schema, header.Value);
                    
                httpResponseMessage.Headers.ProxyAuthenticate.Add(proxyAuthenticateHeader);

                return header;
            }).ToArray();

            httpResponseMessage.Headers.RetryAfter =
                (RetryConditionHeaderValue?)CreateHttpResponseRetryConditionHeaderValue(
                    randomProperties.ResponseHeaders.RetryAfter);

            (randomProperties.ResponseHeaders.Server as dynamic[])
                .Select(header =>
                {
                    var productInfoHeaderValueNameVersion =
                        new ProductInfoHeaderValue(header.ProductHeader.Name, header.ProductHeader.Version);
                    
                    httpResponseMessage.Headers.Server.Add(productInfoHeaderValueNameVersion);
                    
                    var productInfoHeaderValueComment =
                        new ProductInfoHeaderValue(header.Comment); 
                    
                    httpResponseMessage.Headers.Server.Add(productInfoHeaderValueComment);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Trailer as string[]).Select(header =>
            {
                httpResponseMessage.Headers.Trailer.Add(header);
                
                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.TransferEncoding as dynamic[])
                .Select(header =>
                {
                    var transferEncodingHeader = new TransferCodingHeaderValue(header.Value);
                    var parameters = header.Parameters as dynamic[];
                    foreach (var param in parameters)
                    {
                        transferEncodingHeader.Parameters.Add(
                            new NameValueHeaderValue(param.Name, param.Value));
                    };
                    
                    httpResponseMessage.Headers.TransferEncoding.Add(transferEncodingHeader);
                    
                return header;
            }).ToArray();

            httpResponseMessage.Headers.TransferEncodingChunked =
                (bool?)randomProperties.ResponseHeaders.TransferEncodingChunked;

            (randomProperties.ResponseHeaders.Upgrade as dynamic[])
                .Select(header =>
                {
                    var productHeaderValue = new ProductHeaderValue(header.Name, header.Version);
                    
                httpResponseMessage.Headers.Upgrade.Add(productHeaderValue);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Vary as string[]).Select(header =>
            {
                httpResponseMessage.Headers.Vary.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Via as dynamic[]).Select(header =>
                {
                    var viaHeader = new ViaHeaderValue(
                        header.ProtocolVersion,
                        header.ReceivedBy,
                        header.ProtocolName,
                        header.Comment);
                    
                httpResponseMessage.Headers.Via.Add(viaHeader);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Warning as dynamic[]).Select(header =>
            {
                var warningHeader = new WarningHeaderValue(
                    header.Code,
                    header.Agent,
                    header.Text,
                    header.Date);
                
                httpResponseMessage.Headers.Warning.Add(warningHeader);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.WwwAuthenticate as dynamic[]).Select(header =>
            {
                var authenticateHeader = new AuthenticationHeaderValue(
                    header.Schema, header.Value);
                
                httpResponseMessage.Headers.WwwAuthenticate.Add(authenticateHeader);

                return header;
            }).ToArray();

            return httpResponseMessage;
        }

        private static async ValueTask<byte[]> ConvertStreamToByteArray(ValueTask<Stream> streamContentTask)
        {
            Stream streamContent = await streamContentTask;
            using MemoryStream memoryStream = new MemoryStream();
            streamContent.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}