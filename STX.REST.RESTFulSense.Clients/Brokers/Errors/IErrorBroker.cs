// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers;

namespace STX.REST.RESTFulSense.Clients.Brokers.Errors
{
    internal interface IErrorBroker
    {
        ValueTask<IQueryable<StatusDetail>> SelectAllStatusDetailsAsync();
    }
}
