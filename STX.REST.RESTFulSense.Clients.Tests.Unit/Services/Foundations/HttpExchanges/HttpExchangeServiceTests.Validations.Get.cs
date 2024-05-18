// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Fact]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeIsNullAsync()
        {
            // given
            HttpExchange nullHttpExchange = null;

            var nullHttpExchangeException =
                new NullHttpExchangeException(
                    message: "Null HttpExchange error occurred, fix errors and try again.");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
                    message: "HttpExchange validation errors occurred, fix errors and try again.",
                    innerException: nullHttpExchangeException);

            // when
            ValueTask<HttpExchange> getAsyncTask =
                httpExchangeService.GetAsync(nullHttpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(
                    getAsyncTask.AsTask);

            //then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeRequestIsNullAsync()
        {
            // given
            var httpExchange = new HttpExchange();

            var invalidHttpExchangeRequestException = new InvalidHttpExchangeRequestException(
                message: "Invalid HttpExchange request error occurred, fix errors and try again.");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchange.Request),
                value: "Value is required");

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidHttpExchangeRequestException);

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(httpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(
                    getAsyncTask.AsTask);

            //then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null, null, "POST", "0.1")]
        [InlineData("", "", "POST", "0,1")]
        [InlineData(" ", " ", "POST", "0.1")]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeRequestIsInvalidAsync(
            string invalidBaseAddress,
            string invalidRelativeUrl,
            string invalidHttpMethod,
            string invalidHttpVersion)
        {
            // given
            var httpExchange = new HttpExchange()
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = invalidBaseAddress,
                    RelativeUrl = invalidRelativeUrl,
                    HttpMethod = invalidHttpMethod,
                    Version = invalidHttpVersion,
                }
            };

            var invalidHttpExchangeRequestException = new InvalidHttpExchangeRequestException(
                message: "Invalid HttpExchange request error occurred, fix errors and try again.");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.BaseAddress),
                value: "Value is required");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.RelativeUrl),
                value: "Value is required");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.HttpMethod),
                value: "HttpMethod required is invalid");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.Version),
                value: "HttpVersion required is invalid");

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidHttpExchangeRequestException);

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(httpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
               await Assert.ThrowsAsync<HttpExchangeValidationException>(getAsyncTask.AsTask);

            // then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}