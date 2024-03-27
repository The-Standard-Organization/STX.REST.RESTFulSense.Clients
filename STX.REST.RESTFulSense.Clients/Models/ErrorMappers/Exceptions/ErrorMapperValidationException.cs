// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class ErrorMapperValidationException : Xeption
    {
        public ErrorMapperValidationException(Xeption innerException)
            : base(message: "Error mapper validation errors occured, please try again.", innerException)
        { }
    }
}