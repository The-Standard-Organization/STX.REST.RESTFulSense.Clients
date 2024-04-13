// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.ErrorMappers;
using STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions;
using Xeptions;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.ErrorMappers
{
    public partial class ErrorMapperServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfStatusCodeIsInvalid()
        {
            // given
            int invalidStatusCode = default;
            int invalidStatusCodeValue = invalidStatusCode;

            var invalidErrorMapperException =
                new InvalidErrorMapperException(message: "Status code is invalid.");

            var expectedErrorMapperValidationException =
                new ErrorMapperValidationException(
                    message: "Error mapper validation errors occurred, please try again.",
                    innerException: invalidErrorMapperException);

            // when
            ValueTask<StatusDetail> retrieveStatusDetailByStatusCodeTask =
                this.errorMapperService.RetrieveStatusDetailByStatusCodeAsync(
                        invalidStatusCodeValue);

            ErrorMapperValidationException actualErrorMapperValidationException =
                await Assert.ThrowsAsync<ErrorMapperValidationException>(() =>
                    retrieveStatusDetailByStatusCodeTask.AsTask());
            // then
            actualErrorMapperValidationException.Should().BeEquivalentTo(
                expectedErrorMapperValidationException);

            this.errorBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfStatusDetailNotFound()
        {
            // given
            int randomStatusCode = (int)GetRandomHttpStatusCode();
            int randomStatusCodeValue = randomStatusCode;
            StatusDetail nullStatusDetail = null;
            var innerException = new Exception();

            var notFoundErrorMapperException =
                new NotFoundErrorMapperException(
                    message: "Status detail not found",
                    innerException: innerException.InnerException.As<Xeption>());

            var expectedErrorMapperValidationException =
                new ErrorMapperValidationException(
                    message: "Error mapper validation errors occurred, please try again.",
                    innerException: notFoundErrorMapperException);

            this.errorBrokerMock.Setup(broker =>
                broker.SelectAllStatusDetails())
                    .Returns(new List<StatusDetail> { nullStatusDetail }.AsQueryable());

            // when
            ValueTask<StatusDetail> retrieveStatusDetailByStatusCodeTask =
                this.errorMapperService.RetrieveStatusDetailByStatusCodeAsync(
                    randomStatusCodeValue);

            ErrorMapperValidationException actualErrorMapperValidationException =
                await Assert.ThrowsAsync<ErrorMapperValidationException>(() =>
                    retrieveStatusDetailByStatusCodeTask.AsTask());

            // then
            actualErrorMapperValidationException.Should().BeEquivalentTo(
                expectedErrorMapperValidationException);

            this.errorBrokerMock.Verify(broker =>
                broker.SelectAllStatusDetails(), Times.Once);

            errorBrokerMock.VerifyNoOtherCalls();
        }
    }
}