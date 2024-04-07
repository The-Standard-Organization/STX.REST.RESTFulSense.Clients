// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Https;
using STX.REST.RESTFulSense.Clients.Services.Foundations.HttpClients;
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
        
        private HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, string content)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = CreateStreamContent(content)
            };
        }
        
    }
}