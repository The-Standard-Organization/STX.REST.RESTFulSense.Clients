// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net.Http;
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
                    exception: nullHttpExchangeException);
            }
            catch (InvalidHttpExchangeRequestException invalidHttpExchangeRequestException)
            {
                throw CreateHttpExchangeValidationException(
                    invalidHttpExchangeRequestException);
            }
            catch (InvalidHttpExchangeHeaderException invalidHttpExchangeHeaderException)
            {
                throw CreateHttpExchangeValidationException(
                    invalidHttpExchangeHeaderException);
            }
            catch(HttpRequestException httpRequestException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Failed http request error occurred, contact support.",
                        innerException: httpRequestException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidHttpExchangeHeaderArgumentException =
                    new InvalidHttpExchangeHeaderArgumentException(
                        message: "Invalid argument error occurred, contact support.",
                        innerException: argumentException);

                throw CreateHttpExchangeDependencyValidationException(
                    invalidHttpExchangeHeaderArgumentException);
            }
            catch (FormatException formatException)
            {
                var invalidHttpExchangeHeaderFormatException =
                    new InvalidHttpExchangeHeaderFormatException(
                        message: "Invalid format error occurred, contact support.",
                        innerException: formatException);

                throw CreateHttpExchangeDependencyValidationException(
                    invalidHttpExchangeHeaderFormatException);
            }
        }

        private static HttpExchangeValidationException CreateHttpExchangeValidationException(
            Xeption exception)
        {
            return new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: exception);
        }

        private static HttpExchangeDependencyValidationException CreateHttpExchangeDependencyValidationException(
            Xeption exception)
        {
            return new HttpExchangeDependencyValidationException(
                message: "HttpExchange dependency validation errors occurred, fix errors and try again.",
                innerException: exception);
        }

        private static HttpExchangeDependencyException CreateHttpExchangeDependencyException(
            Xeption exception)
        {
            return new HttpExchangeDependencyException(
                message: "HttpExchange dependency error occurred, contact support.",
                innerException: exception);
        }
    }
}