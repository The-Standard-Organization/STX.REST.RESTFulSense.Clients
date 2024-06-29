// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    public class RangeConditionHeader
    {
        public DateTimeOffset? Date { get; init; }
        public string EntityTag { get; init; }
    }
}
