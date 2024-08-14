// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers.Exceptions;
using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers
{
    internal partial class ErrorMapperService
    {
        private delegate ValueTask<StatusDetail> ReturningStatusDetailFunction();

        private static async ValueTask<StatusDetail> TryCatch(
            ReturningStatusDetailFunction returningStatusDetailFunction)
        {
            try
            {
                return await returningStatusDetailFunction();
            }
            catch (InvalidErrorMapperException invalidErrorMapperException)
            {
                throw await CreateErrorMapperException(invalidErrorMapperException);
            }
            catch (NotFoundErrorMapperException notFoundErrorMapperException)
            {
                throw await CreateErrorMapperException(notFoundErrorMapperException);
            }
        }

        private static async ValueTask<ErrorMapperValidationException> CreateErrorMapperException(
            Xeption innerException) =>
                new ErrorMapperValidationException(
                    message: "Error mapper validation errors occurred, please try again.",
                    innerException: innerException);
    }
}