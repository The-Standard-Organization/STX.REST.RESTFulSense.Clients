// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions
{
    public class NotFoundErrorMapperException : Xeption
    {
        public NotFoundErrorMapperException(int statusCode)
            : base(message: $"Status detail with { statusCode } not found, please correct and try again.")
        { }

        public NotFoundErrorMapperException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}