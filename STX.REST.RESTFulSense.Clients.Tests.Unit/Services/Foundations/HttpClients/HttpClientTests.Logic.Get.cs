// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpClients
{
    public partial class LocalHttpClientsTests
    {
        [Fact]
        public async Task ShouldGetAsync()
        {
            // given
            dynamic randomProperties = CreateRandomProperties();
            LocalHttpClients inputLocalHttpClients = CreateHttpClient(randomProperties);
            LocalHttpClients expectedLocalHttpClients = CreateHttpClientResponse(inputLocalHttpClients, randomProperties);

            HttpRequestMessage inputHttpRequestMessage = CreateHttpRequestMessage(randomProperties);
            HttpResponseMessage expectedHttpResponseMessage = CreateHttpResponseMessage(randomProperties);

            this.httpClientBroker.Setup(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            LocalHttpClients actualLocalHttpClients =
                await this.httpClientService.GetAsync(inputLocalHttpClients);

            // then
            actualLocalHttpClients.Should().BeEquivalentTo(expectedLocalHttpClients);

            this.httpClientBroker.Verify(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default),
                    Times.Once);

            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}