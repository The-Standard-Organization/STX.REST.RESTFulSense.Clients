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
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeRequestIsNullAsync()
        {
            // given
            var httpExchange = new HttpExchange();

            var invalidArgumentHttpExchangeException = new InvalidArgumentHttpExchangeException(
                message: "Invalid argument, fix errors and try again.");

            invalidArgumentHttpExchangeException.UpsertDataList(
                key: nameof(HttpExchange.Request),
                value: "Value is required");

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidArgumentHttpExchangeException);

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

            var invalidArgumentHttpExchangeException = new InvalidArgumentHttpExchangeException(
                message: "Invalid argument, fix errors and try again.");

            invalidArgumentHttpExchangeException.UpsertDataList(
                key: nameof(HttpExchangeRequest.BaseAddress),
                value: "Value is required");

            invalidArgumentHttpExchangeException.UpsertDataList(
                key: nameof(HttpExchangeRequest.RelativeUrl),
                value: "Value is required");

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidArgumentHttpExchangeException);

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
        [MemberData(nameof(GetInvalidRequestConfigs))]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpConfigIsInvalidAsync(
            dynamic invalidRequestConfig)
        {
            // given
            var invalidArgumentHttpExchangeException = new InvalidArgumentHttpExchangeException(
                message: "Invalid argument, fix errors and try again.");

            invalidArgumentHttpExchangeException.UpsertDataList(
                key: invalidRequestConfig.ConfigKey,
                value: invalidRequestConfig.ConfigValue);

            var expectedHttpExchangeValidationException = new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: invalidArgumentHttpExchangeException);

            // when
            ValueTask<HttpExchange> getAsyncTask = httpExchangeService.GetAsync(invalidRequestConfig.HttpExchange);

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