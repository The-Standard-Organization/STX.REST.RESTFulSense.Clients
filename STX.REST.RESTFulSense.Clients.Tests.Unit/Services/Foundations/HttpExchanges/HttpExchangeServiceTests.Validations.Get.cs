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
        [Fact]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeIsInvalidAsync()
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
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeRequestIsInvalidAsync(
            string invalidBaseAddress,
            string invalidRelativeUrl)
        {
            // given
            var httpExchange = new HttpExchange()
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = invalidBaseAddress,
                    RelativeUrl = invalidRelativeUrl
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

        [Theory]
        [MemberData(nameof(GetRequestConfigValidationExceptions))]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpConfigIsInvalidAsync(
            dynamic validationException)
        {
            // given
            var invalidArgumentHttpExchangeException = new InvalidArgumentHttpExchangeException(
                message: "Invalid argument, fix errors and try again.");

            invalidArgumentHttpExchangeException.UpsertDataList(
                key: validationException.ConfigKey,
                value: validationException.ConfigValue);

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidArgumentHttpExchangeException);

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(validationException.HttpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
               await Assert.ThrowsAsync<HttpExchangeValidationException>(getAsyncTask.AsTask);

            // then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpVersionIsInvalidAsync()
        {
            // given
            var httpExchange = new HttpExchange()
            {
                Request = new HttpExchangeRequest
                {
                    BaseAddress = CreateRandomUri().GetLeftPart(UriPartial.Authority),
                    RelativeUrl = CreateRandomUri().PathAndQuery,
                    HttpMethod = HttpMethod.Get.Method,
                    Version = "0.1"
                }
            };

            var invalidHttpExchangeRequestException = new InvalidHttpExchangeRequestException(
                message: "Invalid HttpExchange request error occurred, fix errors and try again.");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.Version),
                value: "HttpVersion is invalid");

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

        [Theory]
        [MemberData(nameof(GetRequestValidationExceptions))]
        private async Task ShouldThrowHttpExchangeValidationExceptionIfInvalidRequestHeaderWhenGetAsync(
            dynamic invalidHeaderException)
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
                    Headers = invalidHeaderException.HttpExchangeRequestHeaders
                }
            };

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidHeaderException.InvalidHttpExchangeRequestHeaderException);

            this.httpBroker
                .Setup(broker =>
                    broker.SendRequestAsync(
                        It.IsAny<HttpRequestMessage>(),
                        default));

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(httpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(getAsyncTask.AsTask);

            // then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker
                .Verify(broker =>
                    broker.SendRequestAsync(
                        It.IsAny<HttpRequestMessage>(),
                        default),
                    Times.Never);

            this.httpBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetContentValidationExceptions))]
        private async Task ShouldThrowHttpExchangeValidationExceptionIfInvalidContentHeaderWhenGetAsync(
            dynamic invalidHeaderException)
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
                    Headers = invalidHeaderException.HttpExchangeContentHeaders
                }
            };

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidHeaderException.InvalidHttpExchangeContentHeaderException);

            this.httpBroker
                .Setup(broker =>
                    broker.SendRequestAsync(
                        It.IsAny<HttpRequestMessage>(),
                        default));

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(httpExchange);

            HttpExchangeValidationException actualHttpExchangeValidationException =
                await Assert.ThrowsAsync<HttpExchangeValidationException>(getAsyncTask.AsTask);

            // then
            actualHttpExchangeValidationException.Should().BeEquivalentTo(
                expectedHttpExchangeValidationException);

            this.httpBroker
                .Verify(broker =>
                    broker.SendRequestAsync(
                        It.IsAny<HttpRequestMessage>(),
                        default),
                    Times.Never);

            this.httpBroker.VerifyNoOtherCalls();
        }
    }
}