// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    internal class HttpExchangeRequest
    {
        public string BaseAddress { get; init; }
        public string RelativeUrl { get; init; }
        public string HttpMethod { get; init; }
        public string Version { get; init; }
        public HttpExchangeRequestHeaders Headers { get; init; }
        public HttpExchangeContent Content { get; init; }
    }
}