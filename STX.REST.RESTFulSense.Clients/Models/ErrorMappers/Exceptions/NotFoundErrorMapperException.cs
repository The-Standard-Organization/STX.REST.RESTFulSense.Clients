// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class NotFoundErrorMapperException : Xeption
    {
        public NotFoundErrorMapperException()
            : base(message: "Status detail not found")
        { }
        
        public NotFoundErrorMapperException(Xeption innerException)
            : base(message: "Status detail not found", innerException)
        { }
        
        public NotFoundErrorMapperException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}