// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
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
        [Fact]
        private async Task ShouldSendHttpRequestWhenGetAsyncIsCalledAsync()
        {
            // given
            dynamic randomProperties = CreateRandomExchangeProperties();

            HttpExchange inputHttpExchange =
                CreateHttpExchangeRequest(randomProperties);

            HttpExchange expectedHttpExchage =
                CreateHttpExchangeResponse(
                    inputHttpExchange,
                    randomProperties);

            HttpRequestMessage inputHttpRequestMessage =
                CreateExchangeRequestMessage(randomProperties);

            HttpResponseMessage expectedHttpResponseMessage =
                CreateExchangeResponseMessage(randomProperties);

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
                expectedHttpExchage,
                options => options.Excluding(
                    x => x.Response.StreamContent));

            using MemoryStream actualContentStream =
                new MemoryStream();

            await actualHttpExchange.Response.StreamContent
                .CopyToAsync(actualContentStream);

            using MemoryStream expectedContentStream =
                new MemoryStream();

            await expectedHttpExchage.Response.StreamContent
                .CopyToAsync(expectedContentStream);

            actualContentStream.ToArray().Should().BeEquivalentTo(
                expectedContentStream.ToArray());

            this.httpBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                        Times.Once);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}