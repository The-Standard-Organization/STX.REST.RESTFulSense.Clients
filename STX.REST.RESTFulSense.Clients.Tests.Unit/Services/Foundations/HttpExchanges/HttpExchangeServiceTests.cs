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

        private static HttpRequestMessage CreateHttpRequestMessage(
            dynamic randomProperties)
        {
            return new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod,
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

        private static HttpResponseMessage CreateHttpResponseMessage(
            dynamic randomProperties)
        {
            var bogusHttpResponse =
                new Faker<HttpResponseMessage>()
                    .RuleFor(
                        httpResponseMessage => httpResponseMessage.Content,
                        new StreamContent(randomProperties.ResponseContent.StreamContent)
                        {
                            Headers = {  },
                        })

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
            
            CreateHttpResponseHeader(randomProperties, httpResponseMessage);
            (randomProperties.ResponseContent.Headers.Allow as string[])
                .Select(header => 
                    {
                        httpResponseMessage.Content.Headers.Allow.Add(header);
                        return header;
                    }).ToArray();

            httpResponseMessage.Content.Headers.ContentDisposition =
                (ContentDispositionHeaderValue?)CreateContentDispositionHeaderValue(
                    randomProperties.ResponseContent.Headers.ContentDisposition);
            
            (randomProperties.ResponseContent.Headers.ContentEncoding as string[])
                .Select(header => 
                {
                    httpResponseMessage.Content.Headers.ContentEncoding.Add(header);
                    return header;
                }).ToArray();

            httpResponseMessage.Content.Headers.ContentLength =
                (long?)randomProperties.ResponseContent.Headers.ContentLength;
            
            httpResponseMessage.Content.Headers.ContentLocation =
                (Uri?)randomProperties.ResponseContent.Headers.ContentLocation;

            
            httpResponseMessage.Content.Headers.ContentMD5 =
                        (byte[])randomProperties.ResponseContent.Headers.ContentMD5;

            httpResponseMessage.Content.Headers.ContentRange =
                (ContentRangeHeaderValue?)CreateContentRangeHeaderValue(
                    randomProperties.ResponseContent.Headers.ContentRange);

            httpResponseMessage.Content.Headers.ContentType =
                (MediaTypeHeaderValue)CreateMediaTypeHeaderValue(
                    randomProperties.ResponseContent.Headers.ContentType,
                    httpResponseMessage);

            httpResponseMessage.Content.Headers.Expires =
                (DateTimeOffset?)randomProperties.ResponseContent.Headers.Expires;
            
            httpResponseMessage.Content.Headers.LastModified =
                (DateTimeOffset?)randomProperties.ResponseContent.Headers.LastModified;

            return httpResponseMessage;
        }

        

        private static MediaTypeHeaderValue CreateMediaTypeHeaderValue(
            dynamic randomMediaTypeHeaderValueProperties,
            HttpResponseMessage httpResponseMessage)
        {
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(
                randomMediaTypeHeaderValueProperties.MediaType,
                randomMediaTypeHeaderValueProperties.Charset);
            
            foreach (dynamic parameter in randomMediaTypeHeaderValueProperties.Parameters)
            {
                var nameValueHeaderValue = new NameValueHeaderValue(
                    parameter.Name,
                    parameter.Value);

                mediaTypeHeaderValue.Parameters.Add(nameValueHeaderValue);
            }
            
            httpResponseMessage.Content.Headers.ContentType = mediaTypeHeaderValue;

            return mediaTypeHeaderValue;
        }
        
        private static ContentRangeHeaderValue CreateContentRangeHeaderValue(
            dynamic randomContentRangeHeaderProperties)
        {
            return new ContentRangeHeaderValue(
                randomContentRangeHeaderProperties.From,
                randomContentRangeHeaderProperties.From,
                randomContentRangeHeaderProperties.Length)
            {
                Unit = randomContentRangeHeaderProperties.Unit,
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
                CreationDate = randomContentDispositionHeaderProperties.CreationDate,
                ModificationDate = randomContentDispositionHeaderProperties.ModificationDate,
                ReadDate = randomContentDispositionHeaderProperties.ReadDate,
                Size = randomContentDispositionHeaderProperties.Size
            };
        }

        private static void CreateHttpResponseHeader(dynamic randomProperties, HttpResponseMessage httpResponseMessage)
        {
              (randomProperties.ResponseHeaders.AcceptRanges as string[]).Select(header =>
            {
                httpResponseMessage.Headers.AcceptRanges.Add(header);

                return header;
            }).ToArray();

            httpResponseMessage.Headers.Age = (TimeSpan?)randomProperties.ResponseHeaders.Age;

            httpResponseMessage.Headers.CacheControl =
                MapHttpResponseCacheControlHeader(randomProperties.ResponseHeaders.CacheControl);

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
                (RetryConditionHeaderValue?)MapHttpResponseRetryConditionHeaderValue(
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

        private static TimeSpan GetRandomTimeSpan()
        {
            return new DateTimeRange(
                earliestDate: new DateTime())
                .GetValue().TimeOfDay;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static bool GetRandomBoolean() =>
            Randomizer<bool>.Create();

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
    }
}