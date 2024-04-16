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
        public async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeIsNullAsync()
        {
            // given
            HttpExchange nullHttpExchange = null;

            var nullHttpExchangeException =
                new NullHttpExchangeException(
                    message: "Null HttpExchange error occurred, please fix to continue.");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
                    message: "HttpExchange validation errors occurred, please try again.",
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
    }
}