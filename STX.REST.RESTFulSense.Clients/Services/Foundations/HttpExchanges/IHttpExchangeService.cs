// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal interface IHttpExchangeService
    {
        ValueTask<HttpExchange> GetAsync(
            HttpExchange httpExchange,
            CancellationToken cancellationToken = default);

        ValueTask<HttpExchange> PostAsync(
            HttpExchange httpExchange,
            CancellationToken cancellationToken = default);
    }
}