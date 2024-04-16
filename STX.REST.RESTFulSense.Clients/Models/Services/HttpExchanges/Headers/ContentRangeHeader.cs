// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    internal class ContentRangeHeader
    {
        public string Unit { get; init; }
        public long? From { get; init; }
        public long? To { get; init; }
        public long? Length { get; init; }
    }
}
