// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers.Exceptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal partial class ErrorMapperService
    {
        private static async ValueTask ValidateStatusCodeAsync(int statusCode)
        {
            if (statusCode == default(int))
            {
                throw new InvalidErrorMapperException(
                    message: "Status code is invalid.");
            }
        }

        private static async ValueTask ValidateStatusDetailAsync(
            StatusDetail maybeStatusDetail)
        {
            if (maybeStatusDetail is null)
            {
                throw new NotFoundErrorMapperException(
                    message: "Status detail not found");
            }
        }
    }
}