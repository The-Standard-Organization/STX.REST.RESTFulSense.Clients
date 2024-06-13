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
        [MemberData(nameof(ArgumentExceptions))]
        private async Task ShouldThrowDependencyValidationExceptionOnGetIfArgumentExceptionOccurs(
            ArgumentException argumentException)
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
        
            var invalidHttpExchangeHeaderArgumentException = new InvalidHttpExchangeHeaderArgumentException(
                message: "Invalid argument error occurred, contact support.",
                innerException: argumentException);

            var expectedHttpExchangeDependencyValidationException =
                new HttpExchangeDependencyValidationException(
                    message: "HttpExchange dependency validation errors occurred, fix errors and try again.",
                    innerException: invalidHttpExchangeHeaderArgumentException);

            this.httpBroker.Setup(
                broker => broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .Throws(argumentException);

            // when
            ValueTask<HttpExchange> getTaskAsync = this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyValidationException actualHttpExchangeDependencyValidationException =
                await Assert.ThrowsAsync<HttpExchangeDependencyValidationException>(
                    getTaskAsync.AsTask);
        
            // then
            actualHttpExchangeDependencyValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyValidationException);
        
            this.httpBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                    Times.Once);
        
            this.httpBroker.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnGetIfFormatExceptionOccurs()
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

            var formatException = new FormatException();

            var invalidHttpExchangeHeaderFormatException =
                new InvalidHttpExchangeHeaderFormatException(
                    message: "Invalid format error occurred, contact support.",
                    innerException: formatException);

            var expectedHttpExchangeDependencyValidationException =
                new HttpExchangeDependencyValidationException(
                    message: "HttpExchange dependency validation errors occurred, fix errors and try again.",
                    innerException: invalidHttpExchangeHeaderFormatException);
            
            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(It.IsAny<HttpRequestMessage>(), default))
                    .ThrowsAsync(formatException);
            
            // when
            ValueTask<HttpExchange> getTaskAsync = this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyValidationException actualHttpExchangeDependencyValidationException =
                await Assert.ThrowsAsync<HttpExchangeDependencyValidationException>(
                    getTaskAsync.AsTask);
            
            // then
            actualHttpExchangeDependencyValidationException.Should().BeEquivalentTo(
                    expectedHttpExchangeDependencyValidationException);
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnGetIfHttpRequestExceptionOccursAsync()
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

            var httpRequestException = new HttpRequestException();

            var failedHttpExchangeException = new FailedHttpExchangeException(
                message: "Failed http request error occurred, contact support.",
                innerException: httpRequestException);

            var expectedHttpExchangeDependencyException = new HttpExchangeDependencyException(
                message: "HttpExchange dependency error occurred, contact support.",
                innerException: failedHttpExchangeException);

            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(It.IsAny<HttpRequestMessage>(), default))
                    .ThrowsAsync(httpRequestException);

            // when
            ValueTask<HttpExchange> getTaskAsync =
                this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeDependencyException>(getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyException);
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnGetIfTaskCanceledExceptionOccurs()
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

            var taskCanceledException = new TaskCanceledException();

            var failedHttpExchangeException = new FailedHttpExchangeException(
                message: "Request timeout error occurred, please contact support.",
                innerException: taskCanceledException);

            var expectedHttpExchangeDependencyException =
                new HttpExchangeDependencyException(
                    message: "HttpExchange dependency error occurred, contact support.",
                    innerException: failedHttpExchangeException);

            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(It.IsAny<HttpRequestMessage>(), default))
                    .ThrowsAsync(taskCanceledException);

            // when
            ValueTask<HttpExchange> getTaskAsync =
                this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeDependencyException>(
                    getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyException);
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnGetIfInvalidOperationExceptionOccurs()
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

            var invalidOperationException = new InvalidOperationException();

            var invalidHttpExchangeException = new InvalidHttpExchangeRequestException(
                message: "Invalid http request operation error occurred, please contact support.",
                innerException: invalidOperationException);

            var expectedHttpExchangeDependencyException =
                new HttpExchangeDependencyException(
                    message: "HttpExchange dependency error occurred, contact support.",
                    innerException: invalidHttpExchangeException);

            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(It.IsAny<HttpRequestMessage>(), default))
                    .ThrowsAsync(invalidOperationException);

            // when
            ValueTask<HttpExchange> getTaskAsync =
                this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeDependencyException>(
                    getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyException);
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnGetIfObjectDisposedExceptionOccurs()
        {
            // given
            string randomString = GetRandomString();
            string someMessage = randomString;

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

            var objectDisposedException = new ObjectDisposedException(
                objectName: someMessage,
                message: someMessage);

            var invalidHttpExchangeException = new InvalidHttpExchangeRequestException(
                message: "Object already disposed error occurred, please fix errors and try again.",
                innerException: objectDisposedException);

            var expectedHttpExchangeDependencyException =
                new HttpExchangeDependencyException(
                    message: "HttpExchange dependency error occurred, contact support.",
                    innerException: invalidHttpExchangeException);

            this.httpBroker.Setup(broker =>
                broker.SendRequestAsync(It.IsAny<HttpRequestMessage>(), default))
                    .ThrowsAsync(objectDisposedException);

            // when
            ValueTask<HttpExchange> getTaskAsync =
                this.httpExchangeService.GetAsync(httpExchange);

            HttpExchangeDependencyException actualHttpExchangeDependencyException =
                await Assert.ThrowsAsync<HttpExchangeDependencyException>(
                    getTaskAsync.AsTask);

            // then
            actualHttpExchangeDependencyException.Should().BeEquivalentTo(
                expectedHttpExchangeDependencyException);
        }
    }
}