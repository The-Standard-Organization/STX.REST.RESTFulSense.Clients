// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    internal class ViaHeader
    {
        public string ProtocolName { get; init; }
        public string ProtocolVersion { get; init; }
        public string ReceivedBy { get; init; }
        public string Comment { get; init; }
    }
}
