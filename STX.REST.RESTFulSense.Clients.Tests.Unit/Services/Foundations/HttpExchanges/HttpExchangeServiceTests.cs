// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Bogus;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        private static object[] CreateRandomStringArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(i => GetRandomString())
                    .ToArray();
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
            (int)httpStatusCode >= 200 && (int)httpStatusCode <= 299;

        private static dynamic[] CreateRandomNameValueProperties()
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

        private static dynamic[] CreateRandomNameValueWithParametersProperties()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Name = GetRandomString(),
                    Parameters = CreateRandomNameValueProperties()
                };
            }).ToArray();
        }

        private static dynamic[] CreateRandomAuthenticationHeaderProperties()
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

        private static dynamic CreateRandomCacheControlHeaderProperties()
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
                Extensions = CreateRandomNameValueProperties()
            };
        }

        private static dynamic CreateRandomProductHeaderProperties()
        {
            return new
            {
                Name = GetRandomString(),
                Version = GetRandomString()
            };
        }

        private static dynamic[] CreateArrayProductHeaderProperties()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                CreateRandomProductHeaderProperties())
                .ToArray();
        }

        private static dynamic CreateRandomRetryConditionHeaderProperties()
        {
            return new
            {
                Date = GetRandomDate(),
                Delta = GetRandomTimeSpan()
            };
        }

        private static dynamic CreateProductInfoHeaderProperties()
        {
            return new
            {
                Comment = GetRandomString(),
                ProductHeader = CreateRandomProductHeaderProperties()
            };
        }

        private static dynamic[] CreateArrayProductInfoHeaderProperties()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
                CreateProductInfoHeaderProperties())
                .ToArray();
        }

        private static dynamic[] CreateArrayTransferCodingHeaderProperties()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Quantity = GetRandomDouble(),
                    Value = GetRandomString(),
                    Parameters = CreateRandomNameValueProperties()
                };
            }).ToArray();
        }

        private static dynamic[] CreateArrayViaHeaderProperties()
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

        private static dynamic[] CreateArrayWarningHeaderProperties()
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

        private static dynamic[] CreateRandomMediaTypeHeaderArrayPropeties()
        {
            return Enumerable.Range(0, GetRandomNumber()).Select(item =>
            {
                return new
                {
                    Charset = GetRandomString(),
                    MediaType = GetRandomString(),
                    Quality = GetRandomDouble()
                };
            }).ToArray();
        }

        private static dynamic CreateRandomRangeConditionHeaderProperties()
        {
            return new
            {
                Date = GetRandomDate(),
                EntityTag = GetRandomString()
            };
        }

        private static dynamic CreateRandomRangeItemHeaderProperties()
        {
            return new
            {
                From = GetRandomNumber(),
                To = GetRandomNumber()
            };
        }

        private static dynamic CreateRandomRangeHeaderProperties()
        {
            return new
            {
                Unit = GetRandomString(),
                Ranges = CreateRandomRangeItemHeaderProperties()
            };
        }

        private static dynamic[] CreateRandomStringQualityHeaderArrayProperties()
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

        private static dynamic CreateRandomHttpRequestHeaderProperties()
        {
            return new
            {
                Accept = CreateRandomMediaTypeHeaderArrayPropeties(),
                AcceptCharset = CreateRandomStringQualityHeaderArrayProperties(),
                AcceptEncoding = CreateRandomStringQualityHeaderArrayProperties(),
                AcceptLanguage = CreateRandomStringQualityHeaderArrayProperties(),
                AuthenticationHeader = CreateRandomAuthenticationHeaderProperties(),
                CacheControl = CreateRandomCacheControlHeaderProperties(),
                Connection = GetRandomString(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDate(),
                Expect = CreateRandomNameValueProperties(),
                ExpectContinue = GetRandomBoolean(),
                ExpectCore = CreateRandomNameValueWithParametersProperties(),
                From = GetRandomString(),
                Host = GetRandomString(),
                IfMatch = GetRandomString(),
                IfModifiedSince = GetRandomDate(),
                IfNoneMatch = GetRandomString(),
                IfRange = CreateRandomRangeConditionHeaderProperties(),
                IfUnmodifiedSince = GetRandomDate(),
                MaxForwards = GetRandomNumber(),
                Pragma = CreateRandomNameValueProperties(),
                Protocol = GetRandomString(),
                ProxyAuthorization = CreateRandomAuthenticationHeaderProperties(),
                Range = CreateRandomRangeHeaderProperties(),
                Referrer = CreateRandomUri(),
                TE = CreateArrayTransferCodingHeaderProperties(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateArrayTransferCodingHeaderProperties(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateRandomProductHeaderProperties(),
                UserAgent = CreateArrayProductInfoHeaderProperties(),
                Via = CreateArrayViaHeaderProperties(),
                Warning = CreateArrayWarningHeaderProperties()
            };
        }

        private static dynamic CreateRandomHttpResponseHeaderProperties()
        {
            return new
            {
                AcceptRanges = CreateRandomStringArray(),
                Age = GetRandomTimeSpan(),
                CacheControl = CreateRandomCacheControlHeaderProperties(),
                Connection = CreateRandomStringArray(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDate(),
                ETag = GetRandomString(),
                Location = new Uri(CreateRandomBaseAddress()),
                Pragma = CreateRandomNameValueProperties(),
                ProxyAuthenticate = CreateRandomAuthenticationHeaderProperties(),
                RetryAfter = CreateRandomRetryConditionHeaderProperties(),
                Server = CreateArrayProductInfoHeaderProperties(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateArrayTransferCodingHeaderProperties(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateArrayProductHeaderProperties(),
                Vary = CreateRandomStringArray(),
                Via = CreateArrayViaHeaderProperties(),
                Warning = CreateArrayWarningHeaderProperties(),
                WwwAuthenticate = CreateRandomAuthenticationHeaderProperties()
            };
        }

        private static Stream CreateStream(string content)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            var stream = new ReadOnlyMemoryContent(contentBytes)
                .ReadAsStream();

            return stream;
        }

        private static dynamic CreateRandomHttpProperties()
        {
            Uri randomUri = CreateRandomUri();
            Version randomVersion = GetRandomHttpVersion();
            HttpVersionPolicy randomVersionPolicy = GetRandomHttpVersionPolicy();
            HttpStatusCode randomStatusCode = GetRandomHttpStatusCode();
            string randomReasonPhrase = GetRandomString();
            string randomContent = GetRandomString();

            return new
            {
                BaseAddress = randomUri.GetLeftPart(UriPartial.Authority),
                RelativeUrl = randomUri.LocalPath,
                Url = randomUri,
                UrlParameters = CreateRandomUrlParameters(),
                HttpMethod = HttpMethod.Get,
                ResponseStream = CreateStream(randomContent),
                ResponseHttpStatus = randomStatusCode,
                Version = randomVersion,
                VersionPolicy = randomVersionPolicy,
                StatusCode = randomStatusCode,
                IsSuccessStatusCode = IsSuccessStatusCode(randomStatusCode),
                ReasonPhrase = randomReasonPhrase,
                RequestHeaders = CreateRandomHttpRequestHeaderProperties(),
                ResponseHeaders = CreateRandomHttpResponseHeaderProperties()
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
                    //Content = randomProperties.Content
                }
            };
        }

        private static HttpExchangeResponseHeaders CreateHttpExchangeResponseHeaders(dynamic randomHeaderProperties) =>
            CreateHttpExchangeResponseHeadersFiller(randomHeaderProperties).Create();

        private static Filler<HttpExchangeResponseHeaders> CreateHttpExchangeResponseHeadersFiller(dynamic randomHeaderProperties)
        {
            var filler = new Filler<HttpExchangeResponseHeaders>();
            filler.Setup()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.AcceptRanges).Use((string[])randomHeaderProperties.AcceptRanges)
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Age).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.CacheControl).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Connection).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ConnectionClose).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Date).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ETag).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Location).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Pragma).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.ProxyAuthenticate).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.RetryAfter).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Server).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Trailer).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncoding).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.TransferEncodingChunked).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Upgrade).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Vary).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Via).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.Warning).IgnoreIt()
                .OnProperty(httpExchangeResponseHeaders => httpExchangeResponseHeaders.WwwAuthenticate).IgnoreIt();

            return filler;
        }

        private static HttpExchange CreateHttpExchangeResponse(
           HttpExchange httpExchange,
           dynamic randomProperties)
        {
            HttpExchange mappedHttpExchange = httpExchange.DeepClone();
            Stream responseStream = randomProperties.ResponseStream;

            mappedHttpExchange.Response = new HttpExchangeResponse
            {
                Content = new HttpExchangeContent
                {
                    StreamContent = new ValueTask<Stream>(
                        responseStream.DeepClone())
                },
                Version = randomProperties.Version.ToString(),
                StatusCode = (int)randomProperties.StatusCode,
                IsSuccessStatusCode = randomProperties.IsSuccessStatusCode,
                ReasonPhrase = randomProperties.ReasonPhrase,
                Headers = CreateHttpExchangeResponseHeaders(randomProperties.ResponseHeaders)
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

        private static HttpResponseMessage CreateHttpResponseMessage(dynamic randomProperties)
        {
            var bogusHttpResponse =
                new Faker<HttpResponseMessage>()
                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.Content,
                        new StreamContent(randomProperties.ResponseStream))

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
                        (Version)randomProperties.Version)

                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.Content,
                        new StreamContent(randomProperties.ResponseStream));

            HttpResponseMessage httpResponseMessage =
                bogusHttpResponse.Generate();

            (randomProperties.ResponseHeaders.AcceptRanges as string[]).Select(header =>
            {

                httpResponseMessage.Headers.AcceptRanges.Add(header);
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