// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
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
            dynamic randomProperties = CreateRandomProperties();
            HttpClient inputHttpClient = CreateHttpClient(randomProperties);
            HttpClient expectedHttpClient = CreateHttpClientResponse(inputHttpClient, randomProperties);

            HttpRequestMessage inputHttpRequestMessage = CreateHttpRequestMessage(randomProperties);
            HttpResponseMessage expectedHttpResponseMessage = CreateHttpResponseMessage(randomProperties);

            this.httpClientBroker.Setup(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            HttpClient actualHttpClient =
                await this.httpClientService.GetAsync(inputHttpClient);

            // then
            actualHttpClient.Should().BeEquivalentTo(expectedHttpClient);

            this.httpClientBroker.Verify(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default),
                    Times.Once);

            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}