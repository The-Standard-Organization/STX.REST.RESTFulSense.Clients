// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
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

        private static HttpExchange CreateHttpExchangeRequest(
            dynamic randomProperties)
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

                    Headers = CreateHttpExchangeRequestHeaders(
                        randomProperties.RequestHeaders),

                    VersionPolicy = (int)randomProperties.VersionPolicy,

                    Content = CreateHttpExchangeContent(
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

        private static HttpRequestMessage CreateHttpRequestMessage(
           dynamic randomProperties)
        {
            return new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod,
            };
        }

        private static HttpResponseMessage CreateHttpResponseMessage(
            dynamic randomProperties)
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

            CreateHttpResponseHeaders(randomProperties, httpResponseMessage);
            CreateHttpResponseContentHeaders(randomProperties, httpResponseMessage);

            return httpResponseMessage;
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

            HttpStatusCode randomStatusCode =
                GetRandomHttpStatusCode();

            return new
            {
                BaseAddress =
                    randomUri.GetLeftPart(UriPartial.Authority),

                RelativeUrl = randomUri.LocalPath,
                Url = randomUri,
                UrlParameters = CreateRandomUrlParameters(),
                HttpMethod = HttpMethod.Get,
                Version = GetRandomHttpVersion(),
                VersionPolicy = GetRandomHttpVersionPolicy(),
                StatusCode = randomStatusCode,

                IsSuccessStatusCode =
                    CreateSuccessStatusCode(randomStatusCode),

                ReasonPhrase = GetRandomString(),
                ResponseHttpStatus = randomStatusCode,
                RequestHeaders = CreateRandomHttpRequestHeader(),
                ResponseHeaders = CreateRandomHttpResponseHeader(),
                RequestContent = CreateRandomHttpContent(),
                ResponseContent = CreateRandomHttpContent()
            };
        }

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
                        Parameters = CreateRandomNameValueArray()
                    };
                }).ToArray();
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static double GetRandomDouble() =>
            new DoubleRange(minValue: 2, maxValue: 10).GetValue();

        private static long GetRandomLong() =>
            new LongRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static TimeSpan GetRandomTimeSpan() =>
            GetRandomDateTime().TimeOfDay;

        private static DateTimeOffset GetRandomDateTimeWithOutFractions()
        {
            DateTimeOffset randomDateTime = GetRandomDateTime();
            long ticksWithOutFractions = (randomDateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond;

            randomDateTime =
                new DateTimeOffset(
                    ticks: ticksWithOutFractions,
                    offset: randomDateTime.Offset);

            return randomDateTime;
        }

        private static bool GetRandomBoolean() =>
            Randomizer<bool>.Create();

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
    }
}