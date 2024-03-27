// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.ErrorMappers.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Errors;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal partial class ErrorMapperService
    {
        private delegate ValueTask<StatusDetail> ReturningErrorMappersFunction();

        private static async ValueTask<StatusDetail> TryCatch(ReturningErrorMappersFunction returningErrorMappersFunction)
        {
            try
            {
                return await returningErrorMappersFunction();
            }
            catch (InvalidErrorMapperException invalidErrorMapperException)
            {
                var errorMapperValidationException =
                    new ErrorMapperValidationException(invalidErrorMapperException);

                throw errorMapperValidationException;
            }
        }
    }
}