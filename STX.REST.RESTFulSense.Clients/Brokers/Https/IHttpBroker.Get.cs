// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial interface IHttpBroker
    {
        ValueTask<T> GetContentAsync<T>(
            string relativeUrl,
            Func<string,
            ValueTask<T>> deserializationFunction = null);

        ValueTask<T> GetContentAsync<T>(
            string relativeUrl,
            CancellationToken cancellationToken,
            Func<string, ValueTask<T>> deserializationFunction = null);

        ValueTask<string> GetContentStringAsync(string relativeUrl);

        ValueTask<Stream> GetContentStreamAsync(string relativeUrl);
    }
}