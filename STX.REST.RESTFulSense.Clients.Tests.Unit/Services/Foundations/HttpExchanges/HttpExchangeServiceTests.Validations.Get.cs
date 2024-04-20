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
            httpExchange.Request = null;

            var invalidHttpExchangeException = new NullHttpExchangeRequestException(
                message: "Null HttpExchange request error occurred, fix errors and try again.");
            
            invalidHttpExchangeException.AddData(
                key: nameof(HttpExchange.Request),
                values: "Value is required");

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidHttpExchangeException);

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
    }
}