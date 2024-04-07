// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
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

        private static Stream GetRandomStream() =>
            GetRandomStreamFiller().Create();

        private static Filler<Stream> GetRandomStreamFiller()
        {
            var filler = new Filler<Stream>();

            return filler;
        }
    }
}