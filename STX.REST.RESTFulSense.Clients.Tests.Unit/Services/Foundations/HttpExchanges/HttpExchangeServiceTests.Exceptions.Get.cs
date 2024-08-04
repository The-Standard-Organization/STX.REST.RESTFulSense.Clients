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
            dynamic dependencyException)
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

            var expectedHttpExchangeDependencyException =
                new HttpExchangeDependencyException(
                    message: "HttpExchange dependency error occurred, contact support.",
                    innerException: dependencyException.LocalizedException);

            this.httpBroker.Setup(
                broker => broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .Throws(dependencyException.ExternalException);

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

        [Fact]
        private async Task ShouldThrowServiceExceptionIfServiceErrorOccurs()
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

            var serviceException = new Exception();

            var failedHttpExchangeServiceException =
                new FailedHttpExchangeServiceException(
                    message: "Failed HttpExchange service error occurred, contact support.",
                    innerException: serviceException);

            var expectedHttpExchangeServiceException =
                new HttpExchangeServiceException(
                    message: "HttpExchange service error occurred, contact support.",
                    innerException: failedHttpExchangeServiceException);

            this.httpBroker.Setup(
                broker => broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .Throws(serviceException);

            // when
            ValueTask<HttpExchange> getTaskAsync = this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeServiceException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeServiceException>(
                    getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeServiceException);

            this.httpBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                    Times.Once);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}