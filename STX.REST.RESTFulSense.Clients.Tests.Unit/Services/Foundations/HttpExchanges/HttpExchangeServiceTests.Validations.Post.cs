// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Fact]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnPostIfHttpExchangeIsInvalidAsync()
        {
            // given
            HttpExchange nullHttpExchange = null;

            var invalidHttpExchangeException =
                new InvalidHttpExchangeException(
                    message: "Invalid HttpExchange error occurred, fix errors and try again.");

            invalidHttpExchangeException.UpsertDataList(
                key: nameof(HttpExchange),
                value: "Value is required");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
                    message: "HttpExchange validation errors occurred, fix errors and try again.",
                    innerException: invalidHttpExchangeException);

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(nullHttpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(
                    getAsyncTask.AsTask);

            //then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowHttpExchangeValidationExceptionOnPostIfHttpExchangeRequestIsNullAsync()
        {
            // given
            var httpExchange = new HttpExchange();

            var nullHttpExchangeRequestException =
                new NullHttpExchangeRequestException(
                    message: "Null HttpExchangeRequest error occurred, fix errors and try again.");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
                    message: "HttpExchange validation errors occurred, fix errors and try again.",
                    innerException: nullHttpExchangeRequestException);

            // when
            ValueTask<HttpExchange> postAsyncTask = this.httpExchangeService.PostAsync(httpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(postAsyncTask.AsTask);

            // then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}
