// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.ErrorMappers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal interface IErrorMapperService
    {
        ValueTask<StatusDetail> RetrieveStatusDetailByStatusCodeAsync(int statusCode);
    }
}