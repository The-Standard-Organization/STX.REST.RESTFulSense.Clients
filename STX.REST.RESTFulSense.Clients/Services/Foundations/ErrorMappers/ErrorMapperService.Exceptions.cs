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
                throw new ErrorMapperValidationException(
                    innerException: invalidErrorMapperException);
            }
            catch (NotFoundErrorMapperException notFoundErrorMapperException)
            {
                throw new ErrorMapperValidationException(
                    innerException: notFoundErrorMapperException);
            }
        }
    }
}