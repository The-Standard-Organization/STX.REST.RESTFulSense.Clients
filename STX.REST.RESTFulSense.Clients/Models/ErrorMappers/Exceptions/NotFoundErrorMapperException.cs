// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class NotFoundErrorMapperException : Xeption
    {
        public NotFoundErrorMapperException(string message)
            : base(message)
        { }

        public NotFoundErrorMapperException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}