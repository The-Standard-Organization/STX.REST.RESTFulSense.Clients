// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using Xeptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private delegate ValueTask<HttpExchange> ReturningHttpExchangeFunction();

        private static async ValueTask<HttpExchange> TryCatch(
            ReturningHttpExchangeFunction returningHttpExchangeFunction)
        {
            try
            {
                return await returningHttpExchangeFunction();
            }
            catch (NullHttpExchangeException nullHttpExchangeException)
            {
                throw CreateHttpExchangeValidationException(
                    nullHttpExchangeException);
            }
        }

        private static HttpExchangeValidationException CreateHttpExchangeValidationException(
            Xeption innerException)
        {
            return new HttpExchangeValidationException(
                message: "Http exchange validation errors occurred, please try again.",
                innerException: innerException);
        }
    }
}