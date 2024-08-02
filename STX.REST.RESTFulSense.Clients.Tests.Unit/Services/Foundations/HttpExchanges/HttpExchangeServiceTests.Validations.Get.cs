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

            var nullHttpExchangeRequestException =
                new NullHttpExchangeRequestException(
                    message: "Null HttpExchange request error occurred, fix errors and try again.");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
                    message: "HttpExchange validation errors occurred, fix errors and try again.",
                    innerException: nullHttpExchangeRequestException);

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
        [InlineData(null, null, "POST", "0.1", InvalidVersionPolicy)]
        [InlineData("", "", "POST", "0.1", InvalidVersionPolicy)]
        [InlineData(" ", " ", "POST", "0.1", InvalidVersionPolicy)]
        private async Task ShouldThrowHttpExchangeValidationExceptionOnGetIfHttpExchangeRequestIsInvalidAsync(
            string invalidBaseAddress,
            string invalidRelativeUrl,
            string invalidHttpMethod,
            string invalidHttpVersion,
            int invalidHttpVersionPolicy)
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
                    VersionPolicy = invalidHttpVersionPolicy
                }
            };

            var invalidHttpExchangeRequestException = 
                new InvalidHttpExchangeRequestException(
                    message: "Invalid request, fix errors and try again.");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.BaseAddress),
                value: "Value is required");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.RelativeUrl),
                value: "Value is required");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.HttpMethod),
                value: "HttpMethod is invalid");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.Version),
                value: "HttpVersion is invalid");

            invalidHttpExchangeRequestException.UpsertDataList(
                key: nameof(HttpExchangeRequest.VersionPolicy),
                value: "HttpVersionPolicy is invalid");

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
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

            var expectedHttpExchangeValidationException =
                new HttpExchangeValidationException(
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
    }
}