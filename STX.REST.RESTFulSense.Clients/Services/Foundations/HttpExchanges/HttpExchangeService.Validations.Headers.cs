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
            Condition = IsInvalidMediaTypeHeaderArray(mediaTypeHeaders),
            Message = "Accept header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidAcceptCharsetHeader(
            StringWithQualityHeader[] stringWithQualityHeaders) => new
        {
            Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
            Message = "Accept Charset header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidAcceptEncodingHeader(
            StringWithQualityHeader[] stringWithQualityHeaders) => new
        {
            Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
            Message = "Accept Encoding header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidAcceptLanguageHeader(
            StringWithQualityHeader[] stringWithQualityHeaders) => new
        {
            Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
            Message = "Accept Language header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidAuthorizationHeader(
            AuthenticationHeader authenticationHeader) => new
        {
            Condition = IsInvalidAuthenticationHeader(authenticationHeader),
            Message = "Authorization header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidCacheControlHeader(
            CacheControlHeader cacheControlHeader) => new
        {
            Condition = cacheControlHeader != null
                && (IsInvalidNameValueHeaderArray(cacheControlHeader.Extensions)
                    || IsInvalidStringArray(cacheControlHeader.NoCacheHeaders)
                    || IsInvalidStringArray(cacheControlHeader.PrivateHeaders)),

            Message = "Cache Control header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidConnectionHeader(
            string[] connectionHeader) => new
            {
                Condition = IsInvalidStringArray(connectionHeader),
                Message = "Connection header has invalid configuration, fix errors and try again."
            };

        private static dynamic IsInvalidExpectHeader(
            NameValueWithParametersHeader[] expectHeader) => new
            {
                Condition = IsInvalidNameValueWithParametersHeaderArray(expectHeader),
                Message = "Expect header has invalid configuration, fix errors and try again."
            };

        private static dynamic IsInvalidFromHeader(
            string fromHeader) => new
            {
                Condition = fromHeader is not null
                    && string.IsNullOrWhiteSpace(fromHeader),

                Message = "From header has invalid configuration, fix errors and try again."
            };

        private static dynamic IsInvalidHostHeader(
            string hostHeader) => new
            {
                Condition = hostHeader is not null
                    && string.IsNullOrWhiteSpace(hostHeader),

                Message = "Host header has invalid configuration, fix errors and try again."
            };

        private static dynamic IsInvalidIfMatchHeader(
            string[] ifMatchHeader) => new
            {
                Condition = IsInvalidStringArray(ifMatchHeader),
                Message = "IfMatch header has invalid configuration, fix errors and try again."
            };

        private static dynamic IsInvalidIfNoneMatchHeader(
            string[] ifMatchHeader) => new
        {
            Condition = IsInvalidStringArray(ifMatchHeader),
            Message = "IfNoneMatch header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidIfRangeHeader(RangeConditionHeader rangeConditionHeader) => new
        {
            Condition =
                rangeConditionHeader is not null
                    && rangeConditionHeader.Date is not null
                    && (rangeConditionHeader.EntityTag is not null
                        || (rangeConditionHeader.Date is null
                            && string.IsNullOrWhiteSpace(rangeConditionHeader.EntityTag))),

            Message = "IfRange header has invalid configuration. Exactly one of date and entityTag can be set at a time, fix errors and try again."
        };

        private static dynamic IsInvalidPragmaHeader(NameValueHeader[] nameValueHeaders) => new
        {
            Condition = IsInvalidNameValueHeaderArray(nameValueHeaders),
            Message = "Pragma header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidProtocolHeader(
            string protocolHeader) => new
        {
            Condition = protocolHeader is not null
                && string.IsNullOrWhiteSpace(protocolHeader),

            Message = "Protocol header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidProxyAuthorizationHeader(
            AuthenticationHeader authenticationHeader) => new
        {
            Condition = IsInvalidAuthenticationHeader(authenticationHeader),
            Message = "ProxyAuthentication header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidRangeHeader(
            RangeHeader rangeHeader) => new
        {
            Condition = rangeHeader is not null
                && (string.IsNullOrWhiteSpace(rangeHeader.Unit)
                    || IsInvalidRangeItemHeaderArray(rangeHeader.Ranges)),

            Message = "Range header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidTEHeader(TransferCodingHeader[] transferCodingHeaders) => new
        {
            Condition = IsInvalidTransferCodingHeaderArray(transferCodingHeaders),
            Message = "TE header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidTrailerHeader(
            string[] trailerHeader) => new
        {
            Condition = IsInvalidStringArray(trailerHeader),
            Message = "Trailer header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidTransferEncodingHeaderArray(TransferCodingHeader[] transferCodingHeaders) => new
        {
            Condition = IsInvalidTransferCodingHeaderArray(transferCodingHeaders),
            Message = "TransferEncoding header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidUpgradeHeaderArray(ProductHeader[] productHeaders) => new
        {
            Condition = productHeaders is not null
                && productHeaders.Any(header => header is null
                    || IsInvalidProductHeader(header)),

            Message = "Upgrade header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidUserAgentHeaderArray(ProductInfoHeader[] productInfoHeaders) => new
        {
            Condition = productInfoHeaders is not null
                && productInfoHeaders.Any(header => header is null
                    || IsInvalidProductInfoHeader(header)),

            Message = "UserAgent header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidViaHeaderArray(ViaHeader[] viaHeaders) => new
        {
            Condition = viaHeaders is not null
                && viaHeaders.Any(header => header is null
                    || IsInvalidViaHeader(header)),

            Message = "Via header has invalid configuration, fix errors and try again."
        };

        private static dynamic IsInvalidWarningHeaderArray(WarningHeader[] warningHeaders) => new
        {
            Condition = warningHeaders is not null
                && warningHeaders.Any(header => header is null
                    || IsInvalidWarningHeader(header)),

            Message = "Warning header has invalid configuration, fix errors and try again."
        };

        private static void ValidateHttpExchangeRequestHeaders(
            HttpExchangeRequestHeaders httpExchangeRequestHeaders)
        {
            if (httpExchangeRequestHeaders is null)
                return;

            ValidateHttpRequestHeaders(
                (Rule: IsInvalidAcceptHeader(httpExchangeRequestHeaders.Accept),
                Parameter: nameof(HttpExchangeRequestHeaders.Accept)),

                (Rule: IsInvalidAcceptCharsetHeader(httpExchangeRequestHeaders.AcceptCharset),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptCharset)),

                (Rule: IsInvalidAcceptEncodingHeader(httpExchangeRequestHeaders.AcceptEncoding),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptEncoding)),

                (Rule: IsInvalidAcceptLanguageHeader(httpExchangeRequestHeaders.AcceptLanguage),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptLanguage)),

                (Rule: IsInvalidAuthorizationHeader(httpExchangeRequestHeaders.Authorization),
                Parameter: nameof(HttpExchangeRequestHeaders.Authorization)),

                (Rule: IsInvalidCacheControlHeader(httpExchangeRequestHeaders.CacheControl),
                Parameter: nameof(HttpExchangeRequestHeaders.CacheControl)),

                (Rule: IsInvalidConnectionHeader(httpExchangeRequestHeaders.Connection),
                Parameter: nameof(HttpExchangeRequestHeaders.Connection)),

                (Rule: IsInvalidExpectHeader(httpExchangeRequestHeaders.Expect),
                Parameter: nameof(HttpExchangeRequestHeaders.Expect)),

                (Rule: IsInvalidFromHeader(httpExchangeRequestHeaders.From),
                Parameter: nameof(HttpExchangeRequestHeaders.From)),

                (Rule: IsInvalidHostHeader(httpExchangeRequestHeaders.Host),
                Parameter: nameof(HttpExchangeRequestHeaders.Host)),

                (Rule: IsInvalidIfMatchHeader(httpExchangeRequestHeaders.IfMatch),
                Parameter: nameof(HttpExchangeRequestHeaders.IfMatch)),

                (Rule: IsInvalidIfNoneMatchHeader(httpExchangeRequestHeaders.IfNoneMatch),
                Parameter: nameof(HttpExchangeRequestHeaders.IfNoneMatch)),

                (Rule: IsInvalidIfRangeHeader(httpExchangeRequestHeaders.IfRange),
                Parameter: nameof(HttpExchangeRequestHeaders.IfRange)),

                (Rule: IsInvalidPragmaHeader(httpExchangeRequestHeaders.Pragma),
                Parameter: nameof(HttpExchangeRequestHeaders.Pragma)),

                (Rule: IsInvalidProtocolHeader(httpExchangeRequestHeaders.Protocol),
                Parameter: nameof(HttpExchangeRequestHeaders.Protocol)),

                (Rule: IsInvalidProxyAuthorizationHeader(httpExchangeRequestHeaders.ProxyAuthorization),
                Parameter: nameof(HttpExchangeRequestHeaders.ProxyAuthorization)),

                (Rule: IsInvalidRangeHeader(httpExchangeRequestHeaders.Range),
                Parameter: nameof(HttpExchangeRequestHeaders.Range)),

                (Rule: IsInvalidTEHeader(httpExchangeRequestHeaders.TE),
                Parameter: nameof(HttpExchangeRequestHeaders.TE)),

                (Rule: IsInvalidTrailerHeader(httpExchangeRequestHeaders.Trailer),
                Parameter: nameof(HttpExchangeRequestHeaders.Trailer)),

                (Rule: IsInvalidTransferEncodingHeaderArray(httpExchangeRequestHeaders.TransferEncoding),
                Parameter: nameof(HttpExchangeRequestHeaders.TransferEncoding)),

                (Rule: IsInvalidUpgradeHeaderArray(httpExchangeRequestHeaders.Upgrade),
                Parameter: nameof(HttpExchangeRequestHeaders.Upgrade)),

                (Rule: IsInvalidUserAgentHeaderArray(httpExchangeRequestHeaders.UserAgent),
                Parameter: nameof(HttpExchangeRequestHeaders.UserAgent)),

                (Rule: IsInvalidViaHeaderArray(httpExchangeRequestHeaders.Via),
                Parameter: nameof(HttpExchangeRequestHeaders.Via)),

                (Rule: IsInvalidWarningHeaderArray(httpExchangeRequestHeaders.Warning),
                Parameter: nameof(HttpExchangeRequestHeaders.Warning))
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
