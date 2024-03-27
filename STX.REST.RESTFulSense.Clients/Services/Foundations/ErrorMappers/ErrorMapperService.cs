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

        public ErrorMapperService(IErrorBroker errorBroker)
        {
            this.errorBroker = errorBroker;
        }

        public ValueTask<StatusDetail> RetrieveStatusDetailByStatusCodeAsync(int statusCode)
        {
            IQueryable<StatusDetail> statusDetails = errorBroker.SelectAllStatusDetails();

            return new ValueTask<StatusDetail>(
                statusDetails.FirstOrDefault(statusDetail => statusDetail.Code == statusCode));
        }
    }
}