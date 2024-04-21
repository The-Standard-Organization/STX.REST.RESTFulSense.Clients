// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using System.Threading;
using System.Threading.Tasks;

namespace STX.REST.RESTFulSense.Clients.Clients
{
    public interface IRESTFulApiClient
    {
        /// <summary>
        /// Used for simple scenarios.
        /// </summary>
        /// <typeparam name="TResponseContent"></typeparam>
        /// <param name="relativeUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<(TResponseContent, HttpExchange)> GetContentAsync<TResponseContent>(
            string relativeUrl,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Used for custom and advance scenarios where headers and other things must be specified,
        /// </summary>
        /// <typeparam name="TResponseContent"></typeparam>
        /// <param name="httpExchange">Only create HttpExchangeRequest, if HttpExchangeContent (under the request) or HttpExchangeResponse exists then an exception will thrown</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<(TResponseContent, HttpExchange)> GetContentAsync<TResponseContent>(
            HttpExchange httpExchange,
            CancellationToken cancellationToken = default);
    }
}
