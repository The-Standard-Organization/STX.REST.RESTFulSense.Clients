// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Errors;
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
                new InvalidErrorMapperException();

            var expectedErrorMapperValidationException =
                new ErrorMapperValidationException(invalidErrorMapperException);

            // when
            ValueTask<StatusDetail> retrieveStatusDetailByStatusCodeTask =
                this.errorMapperService.RetrieveStatusDetailByStatusCodeAsync(invalidStatusCodeValue);
            
            // then
            await Assert.ThrowsAsync<ErrorMapperValidationException>(() =>
                retrieveStatusDetailByStatusCodeTask.AsTask());
            
            this.errorBrokerMock.VerifyNoOtherCalls();
        }
    }
}