// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(true, true, false)]
        [InlineData(true, true, true)]
        private async Task ShouldSendHttpRequestWhenGetAsyncIsCalledAsync(
            bool sendUrlParameters,
            bool sendRequestHeaders,
            bool sendRequestContent)
        {
            // given
            dynamic randomProperties =
                CreateRandomHttpProperties(
                    sendUrlParameters,
                    sendRequestHeaders,
                    sendRequestContent);

            HttpExchange inputHttpExchange =
                CreateHttpExchangeRequest(
                    randomProperties);

            HttpExchange expectedHttpExchange =
                CreateHttpExchangeResponse(
                     httpExchange: inputHttpExchange,
                     randomProperties: randomProperties);

            HttpRequestMessage inputHttpRequestMessage =
                CreateHttpRequestMessage(randomProperties);

            HttpRequestMessage expectedHttpRequestMessage =
                inputHttpRequestMessage;

            HttpResponseMessage expectedHttpResponseMessage =
                CreateHttpResponseMessage(randomProperties);

            this.httpBroker
                .Setup(broker =>
                    broker.SendRequestAsync(
                        It.Is(
                            SameHttpRequestMessageAs(expectedHttpRequestMessage)),
                        default))
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

            this.httpBroker.Verify(
                broker =>
                    broker.SendRequestAsync(
                        It.Is(
                            SameHttpRequestMessageAs(expectedHttpRequestMessage)),
                        default),
                Times.Once);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}