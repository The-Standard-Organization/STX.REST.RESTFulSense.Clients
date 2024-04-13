// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    internal class HttpExchangeResponse
    {
        public HttpExchangeContent Content { get; init; }
        public HttpExchangeResponseHeaders Headers { get; init; }
        public bool IsSuccessStatusCode { get; init; }
        public string ReasonPhrase { get; init; }
        public int StatusCode { get; init; }
        public int Version { get; init; }
    }
}