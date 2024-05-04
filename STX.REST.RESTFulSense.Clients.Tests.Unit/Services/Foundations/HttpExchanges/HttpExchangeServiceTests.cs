// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Bogus;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
                Comment = GetRandomString(),
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
                    Quantity = GetRandomDouble(),
                    Value = GetRandomString(),
                    Parameters = CreateRandomNameValueArray()
                };
            }).ToArray();
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
                    Comment = GetRandomString(),
                };
            }).ToArray();
        }

        private static dynamic[] CreateWarningHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Code = GetRandomNumber(),
                    Agent = GetRandomString(),
                    Text = GetRandomString(),
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
                Ranges = CreateRandomRangeItemHeader()
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
                AuthenticationHeader = CreateRandomAuthenticationHeaderArray(),
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
                ProxyAuthorization = CreateRandomAuthenticationHeaderArray(),
                Range = CreateRandomRangeHeader(),
                Referrer = CreateRandomUri(),
                TE = CreateTransferCodingHeaderArray(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateTransferCodingHeaderArray(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateRandomProductHeader(),
                UserAgent = CreateRandomProductInfoHeaderArray(),
                Via = CreateViaHeaderArray(),
                Warning = CreateWarningHeaderArray()
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
                ETag = GetRandomString(),
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
                Alow = CreateRandomStringArray(),
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

        private static Filler<HttpExchangeRequestHeaders> CreateHttpExchangeRequestHeadersFiller(dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeRequestHeaders>();
            filler.Setup()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Accept).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptCharset).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptEncoding).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.AcceptLanguage).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Authorization).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.CacheControl).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Connection).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ConnectionClose).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Date).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Expect).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectContinue).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ExpectCore).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.From).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Host).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfMatch).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfModifiedSince).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfNoneMatch).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfRange).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.IfUnmodifiedSince).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.MaxForwards).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Pragma).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Protocol).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.ProxyAuthorization).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Range).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Referrer).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TE).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Trailer).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncoding).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.TransferEncodingChunked).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Upgrade).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.UserAgent).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Via).IgnoreIt()
                .OnProperty(httpExchangeRequestHeaders => httpExchangeRequestHeaders.Warning).IgnoreIt();

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

        private static AuthenticationHeader CreateHttpExchangeAuthenticateHeader(dynamic randomAuthenticationHeaderProperties)
        {
            return new AuthenticationHeader
            {
                Schema = randomAuthenticationHeaderProperties.Schema,
                Value = randomAuthenticationHeaderProperties.Value
            };
        }

        private static CacheControlHeader CreateHttpExchangeCacheControlHeader(dynamic randomCacheControlHeaderProperties)
        {
            var cacheControl = new CacheControlHeader
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

            return cacheControl;
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
                    .Select(proxyAuthenticateHeader => (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(proxyAuthenticateHeader))
                    .ToArray())

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.RetryAfter)
                .Use(() =>
                    new RetryConditionHeader
                    {
                        Date = randomHeadersProperties.RetryAfter.Date,
                        Delta = randomHeadersProperties.RetryAfter.Delta
                    })

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Server)
                .IgnoreIt()

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Trailer)
                .Use((string[])randomHeadersProperties.Trailer)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncoding)
                .IgnoreIt()

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncodingChunked)
                .Use((bool?)randomHeadersProperties.TransferEncodingChunked)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Upgrade)
                .IgnoreIt()

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Vary)
                .Use((string[])randomHeadersProperties.Vary)

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Via)
                .IgnoreIt()

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Warning)
                .IgnoreIt()

                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.WwwAuthenticate)
                .Use(() =>
                    (randomHeadersProperties.WwwAuthenticate as dynamic[])
                    .Select(wwwAuthenticateHeader => (AuthenticationHeader)CreateHttpExchangeAuthenticateHeader(wwwAuthenticateHeader))
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
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Allow).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentDisposition).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentEncoding).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLanguage).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLength).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentLocation).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentMD5).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentRange).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.ContentType).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.Expires).IgnoreIt()
                .OnProperty(httpExchangeContentHeader => httpExchangeContentHeader.LastModified).IgnoreIt();

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

            (randomProperties.ResponseHeaders.Pragma as NameValueHeaderValue[]).Select(header =>
            {
                httpResponseMessage.Headers.Pragma.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.ProxyAuthenticate as AuthenticationHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.ProxyAuthenticate.Add(header);

                return header;
            }).ToArray();

            httpResponseMessage.Headers.RetryAfter =
                (RetryConditionHeaderValue)randomProperties.ResponseHeaders.RetryAfter;

            (randomProperties.ResponseHeaders.Server as ProductInfoHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.Server.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Trailer as string[]).Select(header =>
            {
                httpResponseMessage.Headers.Trailer.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.TransferEncoding as TransferCodingHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.TransferEncoding.Add(header);

                return header;
            }).ToArray();

            httpResponseMessage.Headers.TransferEncodingChunked =
                (bool?)randomProperties.ResponseHeaders.TransferEncodingChunked;

            (randomProperties.ResponseHeaders.Upgrade as ProductHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.Upgrade.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Vary as string[]).Select(header =>
            {
                httpResponseMessage.Headers.Vary.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Via as ViaHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.Via.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.Warning as WarningHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.Warning.Add(header);

                return header;
            }).ToArray();

            (randomProperties.ResponseHeaders.WwwAuthenticate as AuthenticationHeaderValue[])
                .Select(header =>
            {
                httpResponseMessage.Headers.WwwAuthenticate.Add(header);

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