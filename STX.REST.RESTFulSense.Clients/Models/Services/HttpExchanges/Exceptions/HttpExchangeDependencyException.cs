// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions
{
    internal class HttpExchangeDependencyException : Xeption
    {
        public HttpExchangeDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
