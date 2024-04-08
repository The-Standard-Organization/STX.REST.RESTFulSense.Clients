// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using STX.REST.RESTFulSense.Clients.Models;

namespace STX.REST.RESTFulSense.Clients.Brokers.Errors
{
    internal partial class ErrorBroker : IErrorBroker
    {
        public IQueryable<StatusDetail> SelectAllStatusDetails() =>
            statusDetails.AsQueryable();
    }
}
