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
using STX.REST.RESTFulSense.Clients.Brokers.HttpClients;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;
using STX.REST.RESTFulSense.Clients.Services.Foundations.LocalHttpClients;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.LocalHttpClients
{
    public partial class LocalHttpClientTest
    {
        private readonly Mock<IHttpClientBroker> httpClientBroker;
        private readonly ILocalHttpClientService localHttpClientService;

        public LocalHttpClientTest()
        {
            this.httpClientBroker = new Mock<IHttpClientBroker>();

            this.localHttpClientService =
                new LocalHttpClientService(httpClientBroker.Object);
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

        private static dynamic CreateRandomProperties()
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

        private static HttpRequestMessage CreateHttpRequestMessage(dynamic randomProperties)
        {
            return new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod
            };
        }

        private static HttpResponseMessage CreateHttpResponseMessage(dynamic randomProperties)
        {
            HttpResponseMessage httpResponseMessage =
                new HttpResponseMessage(randomProperties.ResponseHttpStatus)
                {
                    StatusCode = randomProperties.ResponseHttpStatus,
                    Content = new StreamContent(randomProperties.ResponseStream)
                };

            return httpResponseMessage;
        }

        private static LocalHttpClient CreateHttpClient(dynamic randomProperties)
        {
            return new LocalHttpClient
            {
                HttpRequest = new LocalHttpClientRequest
                {
                    BaseAddress = randomProperties.BaseAddress,
                    RelativeUrl = randomProperties.RelativeUrl
                }
            };
        }

        private static LocalHttpClient CreateHttpClientResponse(
            LocalHttpClient localHttpClient,
            dynamic randomProperties)
        {
            LocalHttpClient clonedLocalHttpClient = localHttpClient.DeepClone();
            Stream responseStream = randomProperties.ResponseStream;

            clonedLocalHttpClient.HttpResponse = new LocalHttpClientResponse
            {
                StreamContent = responseStream.DeepClone()
            };

            return clonedLocalHttpClient;
        }
    }
}