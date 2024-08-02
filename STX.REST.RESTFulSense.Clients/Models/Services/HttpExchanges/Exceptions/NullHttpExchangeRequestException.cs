// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions
{
    public class NullHttpExchangeRequestException : Xeption
    {
        public NullHttpExchangeRequestException(string message) : base(message)
        { }
    }
}