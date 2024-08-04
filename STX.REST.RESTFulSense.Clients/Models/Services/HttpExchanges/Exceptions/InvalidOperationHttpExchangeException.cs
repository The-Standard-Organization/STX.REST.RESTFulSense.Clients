// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions
{
    public class InvalidOperationHttpExchangeException : Xeption
    {
        public InvalidOperationHttpExchangeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
