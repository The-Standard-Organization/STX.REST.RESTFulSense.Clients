// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    public class RangeHeader
    {
        public string Unit { get; init; }
        public RangeItemHeader[] Ranges { get; init; }
    }
}
