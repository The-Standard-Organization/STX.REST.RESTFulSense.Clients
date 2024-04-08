// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.ErrorMappers
{
    public partial class ErrorMapperServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveStatusDetailByStatusCodeAsync()
        {
            // given
            HttpStatusCode randomStatusCode = GetRandomHttpStatusCode();
            int randomStatusCodeValue = (int)randomStatusCode;
            
            StatusDetail expectedStatusDetail =
                CreateRandomStatusDetail(randomStatusCodeValue);

            this.errorBrokerMock.Setup(broker =>
                 broker.SelectAllStatusDetails())
                    .Returns(new List<StatusDetail> { expectedStatusDetail }
                    .AsQueryable());

            // when
            StatusDetail actualStatusDetail =
                await errorMapperService.RetrieveStatusDetailByStatusCodeAsync(
                    randomStatusCodeValue);

            // then
            actualStatusDetail.Should().BeEquivalentTo(expectedStatusDetail);

            this.errorBrokerMock.Verify(broker =>
                broker.SelectAllStatusDetails(), Times.Once);

            this.errorBrokerMock.VerifyNoOtherCalls();
        }
    }
}