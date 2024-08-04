// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.Errors;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal partial class ErrorMapperService : IErrorMapperService
    {
        private readonly IErrorBroker errorBroker;

        public ErrorMapperService(IErrorBroker errorBroker) =>
            this.errorBroker = errorBroker;

        public ValueTask<StatusDetail> RetrieveStatusDetailByStatusCodeAsync(int statusCode) =>
        TryCatch(async () =>
        {
            ValidateStatusCode(statusCode);

            IQueryable<StatusDetail> statusDetails =
                await errorBroker.SelectAllStatusDetailsAsync();

            StatusDetail statusDetail = statusDetails
                .FirstOrDefault(statusDetail =>
                    statusDetail != null && statusDetail.Code == statusCode);

            ValidateStatusDetail(statusDetail);

            return statusDetail;
        });
    }
}