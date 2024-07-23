// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static dynamic IsInvalidAcceptHeader(MediaTypeHeader[] mediaTypeHeaders) => new
        {
            Condition = mediaTypeHeaders is not null
                && mediaTypeHeaders.Any(header =>
                {
                    return string.IsNullOrWhiteSpace(header.MediaType) && string.IsNullOrWhiteSpace(header.CharSet);
                }),
            Message = "Accept header has invalid configuration. fix errors and try again."
        };

        private static dynamic IsInvalidRangeConditionHeader(RangeConditionHeader rangeConditionHeader) => new
        {
            Condition =
                    rangeConditionHeader is not null
                        && rangeConditionHeader.Date is not null
                        && rangeConditionHeader.EntityTag is not null,

            Message = "Range Condition header has invalid configuration. Exactly one of date and entityTag can be set at a time, fix errors and try again."
        };

        private static void ValidateHttpExchangeRequestHeaders(
            HttpExchangeRequestHeaders httpExchangeRequestHeaders)
        {
            if (httpExchangeRequestHeaders is null)
                return;

            ValidateHttpRequestHeaders(
                (Rule: IsInvalidRangeConditionHeader(httpExchangeRequestHeaders.IfRange),
                    Parameter: nameof(HttpExchangeRequestHeaders.IfRange)),
                 (Rule: IsInvalidAcceptHeader(httpExchangeRequestHeaders.Accept),
                    Parameter: nameof(HttpExchangeRequestHeaders.Accept))
                );
        }


        private static void ValidateHttpRequestHeaders(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHttpExchangeHeaderException =
                new InvalidHttpExchangeRequestHeaderException(
                    message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHttpExchangeHeaderException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHttpExchangeHeaderException.ThrowIfContainsErrors();
        }
    }
}
