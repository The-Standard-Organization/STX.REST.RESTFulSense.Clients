// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Stream CreateStream(string content)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            var stream = new ReadOnlyMemoryContent(contentBytes)
                .ReadAsStream();

            return stream;
        }

        private static string GetRandomBaseAddressUri()
        {
            string randomHost = GetRandomString();

            return $"{Uri.UriSchemeHttps}://{randomHost}";
        }

        private static dynamic CreateRandomExchangeProperties()
        {
            string baseAddress = GetRandomBaseAddressUri();
            string randomRelativeURL = GetRandomString();
            string randomContent = GetRandomString();

            return new
            {
                BaseAddress = baseAddress,
                RelativeUrl = randomRelativeURL,
                Url = new Uri(
                    new Uri(baseAddress),
                    randomRelativeURL),

                HttpMethod = HttpMethod.Get,
                ResponseStream = CreateStream(randomContent),
                ResponseHttpStatus = HttpStatusCode.OK
            };
        }

        private static HttpRequestMessage CreateExchangeRequestMessage(dynamic randomProperties)
        {
            return new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod
            };
        }

        private static HttpResponseMessage CreateExchangeResponseMessage(dynamic randomProperties)
        {
            HttpResponseMessage httpResponseMessage =
                new HttpResponseMessage(randomProperties.ResponseHttpStatus)
                {
                    StatusCode = randomProperties.ResponseHttpStatus,
                    Content = new StreamContent(randomProperties.ResponseStream)
                };

            return httpResponseMessage;
        }

        private static HttpExchange CreateHttpExchangeRequest(dynamic randomProperties)
        {
            return new HttpExchange
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = randomProperties.BaseAddress,
                    RelativeUrl = randomProperties.RelativeUrl
                }
            };
        }

        private static HttpExchange CreateHttpExchangeResponse(
           HttpExchange httpExchange,
           dynamic randomProperties)
        {
            HttpExchange mappedHttpExchange = httpExchange.DeepClone();
            Stream responseStream = randomProperties.ResponseStream;

            mappedHttpExchange.Response = new HttpExchangeResponse
            {
                StreamContent = responseStream.DeepClone()
            };

            return mappedHttpExchange;
        }

        private static byte[] ConvertStreamToByteArray(Stream stream)
        {
            using MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}