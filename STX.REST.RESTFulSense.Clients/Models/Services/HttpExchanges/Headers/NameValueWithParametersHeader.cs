// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    public class NameValueWithParametersHeader
    {
        public string Name { get; init; }
        public NameValueHeader[] Parameters { get; init; }
    }
}
