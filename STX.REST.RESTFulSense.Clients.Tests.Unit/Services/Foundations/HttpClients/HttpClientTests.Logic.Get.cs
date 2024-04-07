// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.HttpClients;
using Xunit;
using HttpClient = STX.REST.RESTFulSense.Clients.Models.HttpClients.HttpClient;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpClients
{
    public partial class HttpClientTests
    {
        [Fact]
        public async Task ShouldGetAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            Stream randomStream = GetRandomStream();

            var httpResponseMessage = new HttpResponseMessage();

            var inputHttpClient = new HttpClient
            {
                HttpRequest = new HttpRequest
                {
                    RelativeUrl = inputRelativeUrl
                },
            };

            var httpRequestMessage = new HttpRequestMessage();

            var expectedHttpClient = inputHttpClient.DeepClone();
            expectedHttpClient.HttpResponse = new HttpResponse
            {
                ContentStream = randomStream
            };

            this.httpClientBroker.Setup(broker =>
                    broker.SendRequestAsync(httpRequestMessage, default))
                .ReturnsAsync(httpResponseMessage);

            // when
            HttpClient actualHttpClient =
                await this.httpClientService.GetAsync(inputHttpClient);

            // then
            actualHttpClient.Should().BeEquivalentTo(expectedHttpClient);
            
            this.httpClientBroker.Verify(broker =>
                broker.SendRequestAsync(httpRequestMessage, default), Times.Once);
            
            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}