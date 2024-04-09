// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.LocalHttpClients
{
    public partial class LocalHttpClientTest
    {
        [Fact]
        public async Task ShouldGetAsync()
        {
            // given
            dynamic randomProperties = CreateRandomProperties();
            LocalHttpClient inputLocalHttpClient =
                CreateHttpClient(randomProperties);
            
            LocalHttpClient expectedLocalHttpClient =
                CreateHttpClientResponse(inputLocalHttpClient, randomProperties);

            HttpRequestMessage inputHttpRequestMessage =
                CreateHttpRequestMessage(randomProperties);
            
            HttpResponseMessage expectedHttpResponseMessage =
                CreateHttpResponseMessage(randomProperties);

            this.httpClientBroker.Setup(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            LocalHttpClient actualLocalHttpClient =
                await this.localHttpClientService.GetAsync(inputLocalHttpClient);

            // then
            actualLocalHttpClient.Should().BeEquivalentTo(expectedLocalHttpClient);

            this.httpClientBroker.Verify(broker =>
                    broker.SendRequestAsync(
                        It.IsAny<HttpRequestMessage>(), default),
                        Times.Once);

            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}