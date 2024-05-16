// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Fact]
        private async Task ShouldSendHttpRequestWhenGetAsyncIsCalledAsync()
        {
            // given
            dynamic randomProperties = CreateRandomHttpProperties();

            HttpExchange inputHttpExchange =
                CreateHttpExchangeRequest(randomProperties);

            HttpExchange expectedHttpExchange =
                CreateHttpExchangeResponse(
                     httpExchange: inputHttpExchange,
                     randomProperties: randomProperties);

            HttpRequestMessage inputHttpRequestMessage =
                CreateHttpRequestMessage(randomProperties);

            HttpResponseMessage expectedHttpResponseMessage =
                CreateHttpResponseMessage(randomProperties);

            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            HttpExchange actualHttpExchange =
                await this.httpExchangeService.GetAsync(
                    inputHttpExchange);

            // then
            actualHttpExchange.Should().BeEquivalentTo(
                expectedHttpExchange,
                options =>
                    options.Excluding(httpExchange =>
                        httpExchange.Response.Content.StreamContent));

            byte[] actualStreamContent =
                await ConvertStreamToByteArray(
                    actualHttpExchange.Response.Content.StreamContent);

            byte[] expectedStreamContent =
                await ConvertStreamToByteArray(
                    expectedHttpExchange.Response.Content.StreamContent);

            actualStreamContent.Should().BeEquivalentTo(
                expectedStreamContent);

            this.httpBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                        Times.Once);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}