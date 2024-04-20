// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static void ValidateHttpExchange(HttpExchange httpExchange)
        {
            ValidateHttpExchangeNotNull(httpExchange);
            ValidateHttpExchangeRequestNotNull(httpExchange);
        }

        private static void ValidateHttpExchangeNotNull(HttpExchange httpExchange)
        {
            if (httpExchange is null)
            {
                throw new NullHttpExchangeException(
                    message: "Null HttpExchange error occurred, " +
                             "fix errors and try again.");
            }
        }

        private static void ValidateHttpExchangeRequestNotNull(HttpExchange httpExchange)
        {
            if (httpExchange.Request is null)
            {
                throw new NullHttpExchangeRequestException(
                    message: "Null HttpExchange request error occurred, fix errors and try again.");
            }
        }
    }
}