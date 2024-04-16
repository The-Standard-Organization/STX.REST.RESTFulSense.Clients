// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Collections.Generic;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    internal class HttpExchangeHeaders
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; init; }
    }
}
