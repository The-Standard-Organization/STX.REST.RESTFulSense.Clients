// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class NotFoundErrorMapperException : Xeption
    {
        public NotFoundErrorMapperException()
        : base(message: "Couldn't find any status detail")
        { }
        
        public NotFoundErrorMapperException(string message)
            : base(message)
        { }
    }
}