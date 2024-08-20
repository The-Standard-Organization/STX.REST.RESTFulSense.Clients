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
            catch (InvalidHttpExchangeException invalidHttpExchangeException)
            {
                throw await CreateHttpExchangeValidationException(
                    exception: invalidHttpExchangeException);
            }
            catch (NullHttpExchangeRequestException nullHttpExchangeRequestException)
            {
                throw await CreateHttpExchangeValidationException(
                    exception: nullHttpExchangeRequestException);
            }
            catch (InvalidHttpExchangeRequestException invalidHttpExchangeRequestException)
            {
                throw await CreateHttpExchangeValidationException(
                    invalidHttpExchangeRequestException);
            }
            catch (InvalidHttpExchangeRequestHeaderException invalidHttpExchangeHeaderException)
            {
                throw await CreateHttpExchangeValidationException(
                    invalidHttpExchangeHeaderException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedHttpExchangeRequestException =
                    new FailedHttpExchangeRequestException(
                        message: "Failed http request error occurred, contact support.",
                        innerException: httpRequestException);

                throw await CreateHttpExchangeDependencyException(
                    failedHttpExchangeRequestException);
            }
            catch (TaskCanceledException taskCanceledException)
            {
                var taskCanceledHttpExchangeException =
                    new TaskCanceledHttpExchangeException(
                        message: "Request timeout error occurred, please contact support.",
                        innerException: taskCanceledException);

                throw await CreateHttpExchangeDependencyException(
                    taskCanceledHttpExchangeException);
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                var objectDisposedHttpExchangeException =
                    new ObjectDisposedHttpExchangeException(
                        message: "Object already disposed error occurred, please fix errors and try again.",
                        innerException: objectDisposedException);

                throw await CreateHttpExchangeDependencyException(
                    objectDisposedHttpExchangeException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                var invalidOperationHttpExchangeException =
                    new InvalidOperationHttpExchangeException(
                        message: "Invalid http request operation error occurred, please contact support.",
                        innerException: invalidOperationException);

                throw await CreateHttpExchangeDependencyException(
                    invalidOperationHttpExchangeException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidArgumentHttpExchangeException =
                    new InvalidArgumentHttpExchangeException(
                        message: "Invalid argument error occurred, contact support.",
                        innerException: argumentException);

                throw await CreateHttpExchangeDependencyException(
                    invalidArgumentHttpExchangeException);
            }
            catch (FormatException formatException)
            {
                var invalidFormatHttpExchangeException =
                    new InvalidFormatHttpExchangeException(
                        message: "Invalid format error occurred, contact support.",
                        innerException: formatException);

                throw await CreateHttpExchangeDependencyException(
                    invalidFormatHttpExchangeException);
            }
            catch (Exception serviceException)
            {
                var failedHttpExchangeServiceException =
                    new FailedHttpExchangeServiceException(
                        message: "Failed HttpExchange service error occurred, contact support.",
                        innerException: serviceException);

                throw await CreateHttpExchangeServiceException(
                    failedHttpExchangeServiceException);
            }
        }

        private static async ValueTask<HttpExchangeValidationException> CreateHttpExchangeValidationException(
            Xeption exception)
        {
            return new HttpExchangeValidationException(
                message: "HttpExchange validation errors occurred, fix errors and try again.",
                innerException: exception);
        }

        private static async ValueTask<HttpExchangeDependencyException> CreateHttpExchangeDependencyException(
            Xeption exception)
        {
            return new HttpExchangeDependencyException(
                message: "HttpExchange dependency error occurred, contact support.",
                innerException: exception);
        }

        private static async ValueTask<HttpExchangeServiceException> CreateHttpExchangeServiceException(
            Xeption exception)
        {
            return new HttpExchangeServiceException(
                message: "HttpExchange service error occurred, contact support.",
                innerException: exception);
        }
    }
}