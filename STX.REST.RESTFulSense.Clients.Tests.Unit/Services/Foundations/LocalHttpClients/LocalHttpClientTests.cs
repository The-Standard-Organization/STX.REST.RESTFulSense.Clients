// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.HttpClients;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;
using STX.REST.RESTFulSense.Clients.Services.Foundations.LocalHttpClients;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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

        private static Stream CreateStreamContent(string content)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            var stream = new MemoryStream(contentBytes);

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
                ResponseStreamContent = CreateStreamContent(randomContent),
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
            return new HttpResponseMessage(randomProperties.ResponseHttpStatus)
            {
                Content = new StreamContent(randomProperties.ResponseStreamContent)
            };
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

            clonedLocalHttpClient.HttpResponse = new LocalHttpClientResponse
            {
                StreamContent = randomProperties.ResponseStreamContent
            };

            return clonedLocalHttpClient;
        }

    }
}