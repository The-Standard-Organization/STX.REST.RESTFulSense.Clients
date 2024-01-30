// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using STX.REST.RESTFulSense.Clients.Models.Brokers.Errors;

namespace STX.REST.RESTFulSense.Clients.Brokers.Errors
{
    internal interface IErrorBroker
    {
        IQueryable<StatusDetail> SelectAllStatusDetails();
    }
}
