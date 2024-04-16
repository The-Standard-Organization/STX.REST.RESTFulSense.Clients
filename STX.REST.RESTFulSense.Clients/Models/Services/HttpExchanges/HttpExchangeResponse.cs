// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    internal class HttpExchangeResponse
    {
        public int StatusCode { get; init; }
        public int Version { get; init; }
        public HttpExchangeHeaders Headers { get; init; }
        public string ReasonPhrase { get; init; }
        public HttpExchangeContent Content { get; init; }
    }
}