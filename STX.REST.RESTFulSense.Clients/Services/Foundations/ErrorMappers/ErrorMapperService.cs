// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Brokers.Errors;
using STX.REST.RESTFulSense.Clients.Models.Errors;

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

                StatusDetail statusDetail = errorBroker.SelectAllStatusDetails()
                    .FirstOrDefault(statusDetail => statusDetail.Code == statusCode);

                return await ValueTask.FromResult(statusDetail);
            });
    }
}