// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(false, false, true, false)]
        [InlineData(false, false, true, true)]
        [InlineData(false, true, false, false)]
        [InlineData(false, true, true, false)]
        [InlineData(false, true, true, true)]
        [InlineData(true, false, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, false, true, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, true, true, false)]
        [InlineData(true, true, true, true)]
        private async Task ShouldSendHttpRequestWhenPostAsyncIsCalledAsync(
            bool sendUrlParameters,
            bool sendRequestHeaders,
            bool sendRequestContent,
            bool sendRequestContentHeaders)
        {
            // given
            dynamic randomProperties =
                CreateRandomHttpProperties(
                    sendUrlParameters,
                    sendRequestHeaders,
                    sendRequestContent,
                    sendRequestContentHeaders,
                    httpMethod: HttpMethod.Post);

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
