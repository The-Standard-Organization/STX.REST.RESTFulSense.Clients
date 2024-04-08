// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions;
using STX.REST.RESTFulSense.Clients.Models;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal partial class ErrorMapperService
    {
        private static void ValidateStatusCode(int statusCode)
        {
            if (statusCode == default(int))
            {
                throw new InvalidErrorMapperException(message: "Status code is invalid.");
            }
        }

        private static void ValidateStatusDetail(StatusDetail maybeStatusDetail)
        {
            if (maybeStatusDetail is null)
            {
                throw new NotFoundErrorMapperException(message: "Status detail not found");
            }
        }
    }
}