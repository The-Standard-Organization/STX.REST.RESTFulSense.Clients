// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class InvalidErrorMapperException : Xeption
    {
        public InvalidErrorMapperException()
            : base(message: "Status code is invalid.")
        { }
    }
}