// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    internal class RetryConditionHeader
    {
        public DateTimeOffset? Date { get; init; }
        public TimeSpan? Delta { get; init; }
    }
}
