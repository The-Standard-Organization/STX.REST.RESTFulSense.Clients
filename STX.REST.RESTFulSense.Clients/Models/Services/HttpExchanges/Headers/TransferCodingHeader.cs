// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    public class TransferCodingHeader
    {
        public double? Quality { get; init; }
        public string Value { get; init; }
        public NameValueHeader[] Parameters { get; init; }
    }
}
