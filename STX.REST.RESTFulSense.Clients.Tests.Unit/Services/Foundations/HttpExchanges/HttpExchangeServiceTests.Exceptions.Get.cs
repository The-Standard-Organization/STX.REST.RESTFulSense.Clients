// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        [Theory]
        [MemberData(nameof(SendRequestDependencyExceptions))]
        private async Task ShouldThrowDependencyExceptionOnGetIfExternalExceptionOccurs(
            Exception exception, string exceptionMessage)
        {
            // given
            var httpExchange = new HttpExchange
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = CreateRandomUri().GetLeftPart(UriPartial.Authority),
                    RelativeUrl = CreateRandomUri().PathAndQuery,
                    HttpMethod = HttpMethod.Get.Method,
                    Version = GetRandomHttpVersion().ToString(),
                }
            };

            var failedHttpExchangeException =
                new FailedHttpExchangeException(
                    message: exceptionMessage,
                    innerException: exception);

            var expectedHttpExchangeDependencyException =
                new HttpExchangeDependencyException(
                    message: "HttpExchange dependency error occurred, contact support.",
                    innerException: failedHttpExchangeException);

            this.httpBroker.Setup(
                broker => broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .Throws(exception);

            // when
            ValueTask<HttpExchange> getTaskAsync = this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeDependencyException>(
                    getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyException);

            this.httpBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                    Times.Once);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}