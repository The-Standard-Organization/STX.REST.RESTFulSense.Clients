// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.HttpClients;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpClients
{
    internal interface IHttpClientService
    {
        ValueTask<HttpClient> GetAsync(HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}