// ---------------------------------------------------------------
// Copyright (c) Aspen Publishing. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions
{
    public class InvalidHttpExchangeHeaderFormatException : Xeption
    {
        public InvalidHttpExchangeHeaderFormatException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}