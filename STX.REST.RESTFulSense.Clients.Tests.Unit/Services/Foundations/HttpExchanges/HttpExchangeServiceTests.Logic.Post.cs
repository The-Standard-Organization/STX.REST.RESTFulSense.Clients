// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, true, true)]

        private async Task ShouldSendHttpRequestWhenPostAsyncIsCalledAsync(
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
                CreateRandomHttpExchangeRequest(
                    randomProperties);

            HttpExchange expectedHttpExchange =
                CreateHttpExchangeResponse(
                     httpExchange: inputHttpExchange,
                     randomProperties: randomProperties);


            // when

            // then
        }
    }
}
