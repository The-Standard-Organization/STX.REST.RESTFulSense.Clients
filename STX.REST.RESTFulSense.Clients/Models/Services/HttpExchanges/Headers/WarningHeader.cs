// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    internal class WarningHeader
    {
        public int Code { get; init; }
        public string Agent { get; init; }
        public string Text { get; init; }
        public DateTimeOffset? Date { get; init; }
    }
}
