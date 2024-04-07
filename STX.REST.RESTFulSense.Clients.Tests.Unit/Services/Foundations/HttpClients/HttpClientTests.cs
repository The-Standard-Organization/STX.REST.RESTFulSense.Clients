// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Models.HttpClients;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpClients;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpClients
{
    public partial class HttpClientTests
    {
        private readonly Mock<IHttpClientBroker> httpClientBroker;
        private readonly IHttpClientService httpClientService;

        public HttpClientTests()
        {
            this.httpClientBroker = new Mock<IHttpClientBroker>();
            this.httpClientService =
                new HttpClientService(httpClientBroker.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static StreamContent CreateStreamContent(string content)
        {
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            var stream = new MemoryStream(contentBytes);

            return new StreamContent(stream);
        }

        private static Uri GetRandomUri(string relativeUrl)
        {
            string randomHost = GetRandomString();

            return new Uri($"https://{randomHost}/{relativeUrl}");
        }

        private static dynamic CreateRandomProperties()
        {
            string randomContent = GetRandomString();
            string randomRelativeURL = GetRandomString();

            return new
            {
                Url = GetRandomUri(randomRelativeURL),
                RelativeUrl = randomRelativeURL,
                HttpMethod = HttpMethod.Get,
                ResponseStreamContent = CreateStreamContent(randomContent),
                ResponseHttpStatus = HttpStatusCode.OK
            };
        }

        private static HttpRequestMessage CreateHttpRequestMessage(dynamic randomProperties) =>
            new HttpRequestMessage()
            {
                RequestUri = randomProperties.Url,
                Method = randomProperties.HttpMethod
            };

        private static HttpResponseMessage CreateHttpResponseMessage(dynamic randomProperties)
        {
            return new HttpResponseMessage(randomProperties.ResponseHttpStatus)
            {
                Content = randomProperties.ResponseStreamContent
            };
        }

        private static Models.HttpClients.HttpClient CreateHttpClient(dynamic randomProperties)
        {
            return new Models.HttpClients.HttpClient
            {
                HttpRequest = new HttpRequest
                {
                    RelativeUrl = randomProperties.RelativeUrl
                }
            };
        }

        private static Models.HttpClients.HttpClient CreateHttpClientResponse(Models.HttpClients.HttpClient httpClient, dynamic randomProperties)
        {
            Models.HttpClients.HttpClient clonedHttpClient = httpClient.DeepClone();

            clonedHttpClient.HttpResponse = new HttpResponse
            {
                StreamContent = randomProperties.ResponseStreamContent
            };

            return clonedHttpClient;
        }

    }
}