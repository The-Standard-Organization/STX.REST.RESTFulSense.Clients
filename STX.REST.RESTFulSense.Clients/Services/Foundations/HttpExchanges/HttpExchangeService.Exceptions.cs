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
            catch(TaskCanceledException taskCanceledException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Request timeout error occurred, please contact support.",
                        innerException: taskCanceledException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
            catch(ObjectDisposedException objectDisposedException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Object already disposed error occurred, please fix errors and try again.",
                        innerException: objectDisposedException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
            catch(InvalidOperationException invalidOperationException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Invalid http request operation error occurred, please contact support.",
                        innerException: invalidOperationException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
            catch (ArgumentException argumentException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Invalid argument error occurred, contact support.",
                        innerException: argumentException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
            catch (FormatException formatException)
            {
                var failedHttpExchangeException =
                    new FailedHttpExchangeException(
                        message: "Invalid format error occurred, contact support.",
                        innerException: formatException);

                throw CreateHttpExchangeDependencyException(
                    failedHttpExchangeException);
            }
        }

        private static HttpExchangeValidationException CreateHttpExchangeValidationException(
            Xeption exception)
        {
            return new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
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