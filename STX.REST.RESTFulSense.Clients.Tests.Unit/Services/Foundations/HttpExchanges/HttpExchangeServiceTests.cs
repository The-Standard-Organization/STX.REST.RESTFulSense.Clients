// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Force.DeepCloner;
using KellermanSoftware.CompareNetObjects;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges;
using Tynamix.ObjectFiller;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private readonly Mock<IHttpBroker> httpBroker;
        private readonly IHttpExchangeService httpExchangeService;
        private readonly ICompareLogic compareLogic;

        public HttpExchangeServiceTests()
        {
            this.httpBroker = new Mock<IHttpBroker>();
            this.compareLogic = new CompareLogic();

            this.httpExchangeService =
                new HttpExchangeService(httpBroker.Object);
        }

        const int InvalidVersionPolicy = 3;

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static double GetRandomDouble() =>
            new DoubleRange(minValue: 2, maxValue: 10).GetValue();

        private static double GetRandomDoubleBetweenZeroAndOne() =>
            new DoubleRange(minValue: 0, maxValue: 1).GetValue();

        private static long GetRandomLong() =>
            new LongRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static TimeSpan GetRandomTimeSpan() =>
            GetRandomDateTime().TimeOfDay;

        private static DateTimeOffset GetRandomDateTimeWithOutFractions()
        {
            DateTimeOffset randomDateTime = GetRandomDateTime();

            long ticksWithOutFractions =
                (randomDateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond;

            randomDateTime =
                new DateTimeOffset(
                    ticks: ticksWithOutFractions,
                    offset: randomDateTime.Offset);

            return randomDateTime;
        }

        private static bool GetRandomBoolean() =>
            Randomizer<bool>.Create();

        private static string[] CreateRandomStringArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(i => GetRandomString())
                .ToArray();
        }

        private static string CreateRandomQuotedString() =>
            $"\"{GetRandomString()}\"";

        private static string[] CreateRandomQuotedStringArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(i => CreateRandomQuotedString())
                .ToArray();
        }

        private static byte[] CreateRandomByteArray()
        {
            string randomString = GetRandomString();

            return Encoding.UTF8.GetBytes(randomString);
        }

        private static Stream CreateRandomStream()
        {
            string randomContent = GetRandomString();
            byte[] contentBytes = Encoding.ASCII.GetBytes(randomContent);
            var stream = new ReadOnlyMemoryContent(contentBytes)
                .ReadAsStream();

            return stream;
        }

        private static async ValueTask<byte[]> ConvertStreamToByteArray(
            ValueTask<Stream> streamContentTask)
        {
            Stream streamContent = await streamContentTask;
            using MemoryStream memoryStream = new MemoryStream();
            streamContent.CopyTo(memoryStream);

            return memoryStream.ToArray();
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
            IDictionary<string, object> urlParameters =
                CreateRandomUrlParameters();

            randomRelativeURL =
                randomRelativeURL +
                "?" +
                String.Join("&",
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
                .Select(value =>
                    new
                    {
                        Key = GetRandomString(),
                        Value = value
                    })
                .DistinctBy(x => x.Key)
                .ToDictionary(
                    key => key.Key,
                    value => value.Value);
        }

        private static bool CreateSuccessStatusCode(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode >=
                HttpStatusCode.OK && httpStatusCode < HttpStatusCode.MultipleChoices;
        }

        private static dynamic[] CreateRandomNameValueArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
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
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        Name = GetRandomString(),
                        Value = default(string),
                        Parameters = CreateRandomNameValueArray()
                    };
                }).ToArray();
        }

        private static Version GetRandomHttpVersion()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 4);

            return randomNumber switch
            {
                0 => HttpVersion.Version10,
                1 => HttpVersion.Version11,
                2 => HttpVersion.Version20,
                _ => HttpVersion.Version30,
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

        private static long CreateRandomLongToValue(long from) =>
            new LongRange(min: from, max: 10).GetValue();

        private static dynamic CreateRandomHttpContent(bool createContentHeaders)
        {
            dynamic contentHeaders = null;

            if (createContentHeaders)
                contentHeaders = CreateRandomHttpContentHeader();

            return new
            {
                Headers = contentHeaders,
                StreamContent = CreateRandomStream()
            };
        }

        private static dynamic CreateRandomHttpProperties(
            bool sendUrlParameters,
            bool sendRequestHeaders,
            bool sendRequestContent,
            bool sendContentHeaders,
            HttpMethod httpMethod)
        {
            Uri randomUri = CreateRandomUri();

            HttpStatusCode randomStatusCode =
                GetRandomHttpStatusCode();

            IDictionary<string, object> randomUrlParameters = null;
            if (sendUrlParameters)
                randomUrlParameters = CreateRandomUrlParameters();

            dynamic requestHeaders = default;
            if (sendRequestHeaders)
                requestHeaders = CreateRandomHttpRequestHeader();

            dynamic requestContent = default;
            if (sendRequestContent)
                requestContent = CreateRandomHttpContent(sendContentHeaders);

            return new
            {
                BaseAddress =
                    randomUri.GetLeftPart(UriPartial.Authority),

                RelativeUrl = randomUri.PathAndQuery,
                Url = randomUri,
                UrlParameters = randomUrlParameters,
                HttpMethod = httpMethod,
                Version = GetRandomHttpVersion(),
                VersionPolicy = GetRandomHttpVersionPolicy(),
                StatusCode = randomStatusCode,

                IsSuccessStatusCode =
                    CreateSuccessStatusCode(randomStatusCode),

                ReasonPhrase = GetRandomString(),
                ResponseHttpStatus = randomStatusCode,
                RequestHeaders = requestHeaders,
                ResponseHeaders = CreateRandomHttpResponseHeader(),
                RequestContent = requestContent,
                ResponseContent = CreateRandomHttpContent(true)
            };
        }

        private static HttpExchange CreateRandomHttpExchangeRequest(
            dynamic randomProperties)
        {
            return new HttpExchange
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = randomProperties.BaseAddress,
                    RelativeUrl = randomProperties.RelativeUrl,
                    HttpMethod = randomProperties.HttpMethod.Method,
                    Version = randomProperties.Version.ToString(),
                    UrlParameters = randomProperties.UrlParameters,

                    Headers =
                        CreateHttpExchangeRequestHeaders(
                            randomProperties.RequestHeaders),

                    VersionPolicy = (int)randomProperties.VersionPolicy,

                    Content =
                        CreateHttpExchangeContent(
                            randomProperties.RequestContent)
                }
            };
        }

        private static HttpExchange CreateHttpExchangeResponse(
            HttpExchange httpExchange,
            dynamic randomProperties)
        {
            HttpExchange mappedHttpExchange = httpExchange.DeepClone();

            mappedHttpExchange.Response =
                new HttpExchangeResponse
                {
                    Content = CreateHttpExchangeContent(
                        randomProperties.ResponseContent),

                    Headers = CreateHttpExchangeResponseHeaders(
                        randomProperties.ResponseHeaders),

                    Version = randomProperties.Version.ToString(),
                    StatusCode = (int)randomProperties.StatusCode,
                    IsSuccessStatusCode = randomProperties.IsSuccessStatusCode,
                    ReasonPhrase = randomProperties.ReasonPhrase
                };

            return mappedHttpExchange;
        }

        private static HttpExchangeContent CreateHttpExchangeContent(
           dynamic randomContentProperties)
        {
            HttpExchangeContent httpExchangeContent = null;
            if (randomContentProperties is not null)
            {
                httpExchangeContent =
                    CreateHttpExchangeContentFiller(
                        randomContentProperties)
                    .Create();
            }

            return httpExchangeContent;
        }

        private static Filler<HttpExchangeContent> CreateHttpExchangeContentFiller(
            dynamic randomContentProperties)
        {
            var filler = new Filler<HttpExchangeContent>();

            filler.Setup()
                .OnProperty(httpExchangeContent => httpExchangeContent.Headers)
                    .Use((HttpExchangeContentHeaders)CreateHttpExchangeContentHeaders(
                        randomContentProperties.Headers))

                .OnProperty(httpExchangeContent => httpExchangeContent.StreamContent)
                    .Use(new ValueTask<Stream>(
                            ((Stream)randomContentProperties.StreamContent)
                            .DeepClone()));

            return filler;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(
           dynamic randomProperties)
        {
            Uri requestUri = randomProperties.Url;

            if (randomProperties.UrlParameters is not null)
            {
                IEnumerable<string> additionalParameters =
                    (randomProperties.UrlParameters as IDictionary<string, object>)
                        .Select(urlParameter =>
                            $"{urlParameter.Key}={urlParameter.Value}");

                string query =
                    String.Join("&",
                        requestUri.Query
                            .Split("&")
                            .Concat(additionalParameters));


                requestUri =
                    new Uri(
                        new Uri(
                            new Uri(randomProperties.BaseAddress),
                            requestUri.PathAndQuery),
                        query);
            }

            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage()
                {
                    RequestUri = requestUri,
                    Method = randomProperties.HttpMethod,
                    Version = randomProperties.Version,
                    VersionPolicy = randomProperties.VersionPolicy,
                };

            if (randomProperties.RequestHeaders is not null)
            {
                CreateHttpRequestHeaders(
                    randomProperties.RequestHeaders,
                    httpRequestMessage.Headers);
            }

            return httpRequestMessage;
        }

        private static HttpResponseMessage CreateHttpResponseMessage(
            dynamic randomProperties)
        {
            var bogusHttpResponse =
                new Faker<HttpResponseMessage>()
                    .RuleFor(
                        httpResponseMessage =>
                            httpResponseMessage.Content,

                        new StreamContent(
                            randomProperties.ResponseContent.StreamContent))

                    .RuleFor(
                        httpResponseMessage =>
                            httpResponseMessage.IsSuccessStatusCode,

                        (bool)randomProperties.IsSuccessStatusCode)

                    .RuleFor(
                        httpResponseMessage =>
                            httpResponseMessage.ReasonPhrase,

                        (string)randomProperties.ReasonPhrase)

                    .RuleFor(
                        httpResponseMessage =>
                            httpResponseMessage.StatusCode,

                        (HttpStatusCode)randomProperties.StatusCode)

                    .RuleFor(
                        httpResponseMessage =>
                            httpResponseMessage.Version,

                        (Version)randomProperties.Version);

            HttpResponseMessage httpResponseMessage =
                bogusHttpResponse.Generate();

            CreateHttpResponseHeaders(
                randomProperties.ResponseHeaders,
                httpResponseMessage.Headers);

            CreateHttpContentHeaders(
                randomProperties.ResponseContent.Headers,
                httpResponseMessage.Content.Headers);

            return httpResponseMessage;
        }

        private Expression<Func<HttpRequestMessage, bool>> SameHttpRequestMessageAs(
            HttpRequestMessage actualHttpRequestMessage,
            HttpRequestMessage expectedHttpRequestMessage)
        {
            return actualHttpRequestMessage =>
                this.compareLogic.Compare(
                    expectedHttpRequestMessage,
                    actualHttpRequestMessage)
                        .AreEqual;
        }

        public static TheoryData<dynamic> GetRequestValidationExceptions()
        {
            var theoryData = new TheoryData<dynamic>();
            theoryData = CreateRequestHeadersValidationExceptions(theoryData);

            return theoryData;
        }

        public static TheoryData<dynamic> GetContentValidationExceptions()
        {
            var theoryData = new TheoryData<dynamic>();
            theoryData = CreateRequestContentHeadersValidationExceptions(theoryData);

            return theoryData;
        }

        public static TheoryData<dynamic> SendRequestDependencyExceptions()
        {
            var theoryData = new TheoryData<dynamic>();
            theoryData = CreateSendRequestDependencyExceptions(theoryData);

            return theoryData;
        }
    }
}