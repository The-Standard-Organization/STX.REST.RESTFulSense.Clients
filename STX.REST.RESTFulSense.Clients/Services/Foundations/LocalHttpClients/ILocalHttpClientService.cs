// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.LocalHttpClients
{
    internal interface ILocalHttpClientService
    {
        ValueTask<LocalHttpClient> GetAsync(LocalHttpClient localHttpClient, CancellationToken cancellationToken = default);
    }
}