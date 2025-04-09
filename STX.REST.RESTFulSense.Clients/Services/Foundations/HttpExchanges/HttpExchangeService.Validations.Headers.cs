// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static bool IsInvalidMediaTypeHeader(
            MediaTypeHeader mediaTypeHeader)
        {
            return mediaTypeHeader is not null
                && (String.IsNullOrWhiteSpace(mediaTypeHeader.MediaType)
                    || String.IsNullOrWhiteSpace(mediaTypeHeader.CharSet));
        }

        private static bool IsInvalidMediaTypeHeaderArray(
            MediaTypeHeader[] mediaTypeHeaders)
        {
            return mediaTypeHeaders is not null
                && mediaTypeHeaders.Any(header => header is null
                    || IsInvalidMediaTypeHeader(header));
        }

        private static bool IsDoubleBetweenZeroAndOne(double value)
        {
            return value >= 0.0
                && value <= 1.0;
        }

        private static bool IsInvalidStringWithQualityHeader(
            StringWithQualityHeader stringWithQualityHeader)
        {
            return stringWithQualityHeader is not null
                && (String.IsNullOrWhiteSpace(stringWithQualityHeader.Value)
                    || (stringWithQualityHeader.Quality is not null
                        && !IsDoubleBetweenZeroAndOne(
                            stringWithQualityHeader.Quality.Value)));
        }

        private static bool IsInvalidStringWithQualityHeaderArray(
            StringWithQualityHeader[] stringWithQualityHeaders)
        {
            return stringWithQualityHeaders is not null
                && stringWithQualityHeaders.Any(header => header is null
                    || IsInvalidStringWithQualityHeader(header));
        }

        private static bool IsInvalidAuthenticationHeader(AuthenticationHeader authenticationHeader)
        {
            return authenticationHeader is not null
                && (String.IsNullOrWhiteSpace(authenticationHeader.Value)
                    || String.IsNullOrWhiteSpace(authenticationHeader.Schema));
        }

        private static bool IsInvalidNameValueHeader(NameValueHeader nameValueHeader)
        {
            return String.IsNullOrWhiteSpace(nameValueHeader.Value)
                || String.IsNullOrWhiteSpace(nameValueHeader.Name);
        }

        private static bool IsInvalidNameValueHeaderArray(NameValueHeader[] nameValueHeaders)
        {
            return nameValueHeaders is not null
                && nameValueHeaders.Any(header => header is null
                    || IsInvalidNameValueHeader(header));
        }

        private static bool IsInvalidNameValueHeader(NameValueWithParametersHeader nameValueWithParametersHeader)
        {
            return String.IsNullOrWhiteSpace(nameValueWithParametersHeader.Name)
                || IsInvalidNameValueHeaderArray(nameValueWithParametersHeader.Parameters);
        }

        private static bool IsInvalidNameValueWithParametersHeaderArray(
            NameValueWithParametersHeader[] nameValueWithParametersHeaders)
        {
            return nameValueWithParametersHeaders is not null
                && nameValueWithParametersHeaders.Any(header => header is null
                    || IsInvalidNameValueHeader(header));
        }

        private static bool IsInvalidStringArray(string[] stringArrayHeaders)
        {
            return stringArrayHeaders is not null
                && stringArrayHeaders.Any(header => String.IsNullOrWhiteSpace(header));
        }

        private static bool IsInvalidRangeItemHeader(RangeItemHeader rangeItemHeader)
        {
            return rangeItemHeader.From is null
                || rangeItemHeader.To is null
                || rangeItemHeader.From > rangeItemHeader.To;
        }

        private static bool IsInvalidRangeItemHeaderArray(RangeItemHeader[] rangeItemHeaders)
        {
            return rangeItemHeaders is not null
                && rangeItemHeaders.Any(header => header is null
                    || IsInvalidRangeItemHeader(header));
        }

        private static bool IsInvalidTransferCodingHeader(TransferCodingHeader transferCodingHeader)
        {
            return String.IsNullOrWhiteSpace(transferCodingHeader.Value)
                || IsInvalidNameValueHeaderArray(transferCodingHeader.Parameters)
                || (transferCodingHeader.Quality is not null
                        && !IsDoubleBetweenZeroAndOne(
                            transferCodingHeader.Quality.Value));
        }

        private static bool IsInvalidTransferCodingHeaderArray(TransferCodingHeader[] transferCodingHeaders)
        {
            return transferCodingHeaders is not null
                && transferCodingHeaders.Any(header => header is null
                    || IsInvalidTransferCodingHeader(header));
        }

        private static bool IsInvalidProductHeader(ProductHeader productHeader)
        {
            return productHeader is not null
                && (String.IsNullOrWhiteSpace(productHeader.Name)
                    || String.IsNullOrWhiteSpace(productHeader.Version));
        }

        private static bool IsInvalidProductInfoHeader(ProductInfoHeader productInfoHeader)
        {
            return productInfoHeader is not null
                && String.IsNullOrWhiteSpace(productInfoHeader.Comment)
                && (productInfoHeader.Product is null
                    || IsInvalidProductHeader(productInfoHeader.Product));
        }

        private static bool IsInvalidViaHeader(ViaHeader viaHeader)
        {
            return viaHeader is not null
                && (String.IsNullOrWhiteSpace(viaHeader.Comment)
                    || String.IsNullOrWhiteSpace(viaHeader.ProtocolName)
                    || String.IsNullOrWhiteSpace(viaHeader.ProtocolVersion)
                    || String.IsNullOrWhiteSpace(viaHeader.ReceivedBy));
        }

        private static bool IsInvalidWarningHeader(WarningHeader warningHeader)
        {
            return warningHeader is not null
                && (String.IsNullOrWhiteSpace(warningHeader.Agent)
                    || String.IsNullOrWhiteSpace(warningHeader.Text));
        }

        private static async ValueTask<dynamic> IsInvalidAcceptHeaderAsync(
            MediaTypeHeader[] mediaTypeHeaders)
        {
            return new
            {
                Condition = IsInvalidMediaTypeHeaderArray(mediaTypeHeaders),
                Message = "Accept header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidAcceptCharsetHeaderAsync(
            StringWithQualityHeader[] stringWithQualityHeaders)
        {
            return new
            {
                Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
                Message = "AcceptCharset header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidAcceptEncodingHeaderAsync(
            StringWithQualityHeader[] stringWithQualityHeaders)
        {
            return new
            {
                Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
                Message = "AcceptEncoding header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidAcceptLanguageHeaderAsync(
            StringWithQualityHeader[] stringWithQualityHeaders)
        {
            return new
            {
                Condition = IsInvalidStringWithQualityHeaderArray(stringWithQualityHeaders),
                Message = "AcceptLanguage header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidAuthorizationHeaderAsync(
            AuthenticationHeader authenticationHeader)
        {
            return new
            {
                Condition = IsInvalidAuthenticationHeader(authenticationHeader),
                Message = "Authorization header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidCacheControlHeaderAsync(
            CacheControlHeader cacheControlHeader)
        {
            return new
            {
                Condition = cacheControlHeader is not null
            && (IsInvalidNameValueHeaderArray(cacheControlHeader.Extensions)
                || IsInvalidStringArray(cacheControlHeader.NoCacheHeaders)
                || IsInvalidStringArray(cacheControlHeader.PrivateHeaders)),

                Message = "CacheControl header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidConnectionHeaderAsync(
            string[] connectionHeader)
        {
            return new
            {
                Condition = IsInvalidStringArray(connectionHeader),
                Message = "Connection header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidExpectHeaderAsync(
            NameValueWithParametersHeader[] expectHeader)
        {
            return new
            {
                Condition = IsInvalidNameValueWithParametersHeaderArray(expectHeader),
                Message = "Expect header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidFromHeaderAsync(
            string fromHeader)
        {
            return new
            {
                Condition = fromHeader is not null
                    && String.IsNullOrWhiteSpace(fromHeader),

                Message = "From header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidHostHeaderAsync(
            string hostHeader)
        {
            return new
            {
                Condition = hostHeader is not null
                && String.IsNullOrWhiteSpace(hostHeader),

                Message = "Host header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidIfMatchHeaderAsync(
            string[] ifMatchHeader)
        {
            return new
            {
                Condition = IsInvalidStringArray(ifMatchHeader),
                Message = "IfMatch header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidIfNoneMatchHeaderAsync(
            string[] ifMatchHeader)
        {
            return new
            {
                Condition = IsInvalidStringArray(ifMatchHeader),
                Message = "IfNoneMatch header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidIfRangeHeaderAsync(RangeConditionHeader rangeConditionHeader)
        {
            return new
            {
                Condition =
                rangeConditionHeader is not null
                    && rangeConditionHeader.Date is not null
                    && (rangeConditionHeader.EntityTag is not null
                        || (rangeConditionHeader.Date is null
                            && String.IsNullOrWhiteSpace(rangeConditionHeader.EntityTag))),

                Message = "IfRange header has a invalid configuration." +
                        "Only one of either date or entityTag can be set at a time. Please fix the errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidPragmaHeaderAsync(NameValueHeader[] nameValueHeaders)
        {
            return new
            {
                Condition = IsInvalidNameValueHeaderArray(nameValueHeaders),
                Message = "Pragma header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidProtocolHeaderAsync(
            string protocolHeader)
        {
            return new
            {
                Condition = protocolHeader is not null && String.IsNullOrWhiteSpace(protocolHeader),
                Message = "Protocol header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidProxyAuthorizationHeaderAsync(
            AuthenticationHeader authenticationHeader)
        {
            return new
            {
                Condition = IsInvalidAuthenticationHeader(authenticationHeader),
                Message = "ProxyAuthorization header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidRangeHeaderAsync(
            RangeHeader rangeHeader)
        {
            return new
            {
                Condition = rangeHeader is not null
                && (String.IsNullOrWhiteSpace(rangeHeader.Unit)
                    || IsInvalidRangeItemHeaderArray(rangeHeader.Ranges)),

                Message = "Range header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidTEHeaderAsync(
            TransferCodingHeader[] transferCodingHeaders)
        {
            return new
            {
                Condition = IsInvalidTransferCodingHeaderArray(transferCodingHeaders),
                Message = "TE header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidTrailerHeaderAsync(
            string[] trailerHeader)
        {
            return new
            {
                Condition = IsInvalidStringArray(trailerHeader),
                Message = "Trailer header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidTransferEncodingHeaderArrayAsync(
            TransferCodingHeader[] transferCodingHeaders)
        {
            return new
            {
                Condition = IsInvalidTransferCodingHeaderArray(transferCodingHeaders),
                Message = "TransferEncoding header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidUpgradeHeaderArrayAsync(ProductHeader[] productHeaders) => new
        {
            Condition = productHeaders is not null
                && productHeaders.Any(header => header is null
                    || IsInvalidProductHeader(header)),

            Message = "Upgrade header has invalid configuration, fix errors and try again."
        };

        private static async ValueTask<dynamic> IsInvalidUserAgentHeaderArrayAsync(
            ProductInfoHeader[] productInfoHeaders)
        {
            return new
            {
                Condition = productInfoHeaders is not null
                && productInfoHeaders.Any(header => header is null
                    || IsInvalidProductInfoHeader(header)),

                Message = "UserAgent header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidViaHeaderArrayAsync(ViaHeader[] viaHeaders)
        {
            return new
            {
                Condition = viaHeaders is not null
                && viaHeaders.Any(header => header is null
                    || IsInvalidViaHeader(header)),

                Message = "Via header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask<dynamic> IsInvalidWarningHeaderArrayAsync(
            WarningHeader[] warningHeaders)
        {
            return new
            {
                Condition = warningHeaders is not null
                && warningHeaders.Any(header => header is null
                    || IsInvalidWarningHeader(header)),

                Message = "Warning header has invalid configuration, fix errors and try again."
            };
        }

        private static async ValueTask ValidateHttpExchangeRequestHeadersAsync(
            HttpExchangeRequestHeaders httpExchangeRequestHeaders)
        {
            if (httpExchangeRequestHeaders is null)
                return;

            await ValidateHttpRequestHeadersAsync(
                (Rule: await IsInvalidAcceptHeaderAsync(httpExchangeRequestHeaders.Accept),
                Parameter: nameof(HttpExchangeRequestHeaders.Accept)),

                (Rule: await IsInvalidAcceptCharsetHeaderAsync(httpExchangeRequestHeaders.AcceptCharset),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptCharset)),

                (Rule: await IsInvalidAcceptEncodingHeaderAsync(httpExchangeRequestHeaders.AcceptEncoding),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptEncoding)),

                (Rule: await IsInvalidAcceptLanguageHeaderAsync(httpExchangeRequestHeaders.AcceptLanguage),
                Parameter: nameof(HttpExchangeRequestHeaders.AcceptLanguage)),

                (Rule: await IsInvalidAuthorizationHeaderAsync(httpExchangeRequestHeaders.Authorization),
                Parameter: nameof(HttpExchangeRequestHeaders.Authorization)),

                (Rule: await IsInvalidCacheControlHeaderAsync(httpExchangeRequestHeaders.CacheControl),
                Parameter: nameof(HttpExchangeRequestHeaders.CacheControl)),

                (Rule: await IsInvalidConnectionHeaderAsync(httpExchangeRequestHeaders.Connection),
                Parameter: nameof(HttpExchangeRequestHeaders.Connection)),

                (Rule: await IsInvalidExpectHeaderAsync(httpExchangeRequestHeaders.Expect),
                Parameter: nameof(HttpExchangeRequestHeaders.Expect)),

                (Rule: await IsInvalidFromHeaderAsync(httpExchangeRequestHeaders.From),
                Parameter: nameof(HttpExchangeRequestHeaders.From)),

                (Rule: await IsInvalidHostHeaderAsync(httpExchangeRequestHeaders.Host),
                Parameter: nameof(HttpExchangeRequestHeaders.Host)),

                (Rule: await IsInvalidIfMatchHeaderAsync(httpExchangeRequestHeaders.IfMatch),
                Parameter: nameof(HttpExchangeRequestHeaders.IfMatch)),

                (Rule: await IsInvalidIfNoneMatchHeaderAsync(httpExchangeRequestHeaders.IfNoneMatch),
                Parameter: nameof(HttpExchangeRequestHeaders.IfNoneMatch)),

                (Rule: await IsInvalidIfRangeHeaderAsync(httpExchangeRequestHeaders.IfRange),
                Parameter: nameof(HttpExchangeRequestHeaders.IfRange)),

                (Rule: await IsInvalidPragmaHeaderAsync(httpExchangeRequestHeaders.Pragma),
                Parameter: nameof(HttpExchangeRequestHeaders.Pragma)),

                (Rule: await IsInvalidProtocolHeaderAsync(httpExchangeRequestHeaders.Protocol),
                Parameter: nameof(HttpExchangeRequestHeaders.Protocol)),

                (Rule: await IsInvalidProxyAuthorizationHeaderAsync(httpExchangeRequestHeaders.ProxyAuthorization),
                Parameter: nameof(HttpExchangeRequestHeaders.ProxyAuthorization)),

                (Rule: await IsInvalidRangeHeaderAsync(httpExchangeRequestHeaders.Range),
                Parameter: nameof(HttpExchangeRequestHeaders.Range)),

                (Rule: await IsInvalidTEHeaderAsync(httpExchangeRequestHeaders.TE),
                Parameter: nameof(HttpExchangeRequestHeaders.TE)),

                (Rule: await IsInvalidTrailerHeaderAsync(httpExchangeRequestHeaders.Trailer),
                Parameter: nameof(HttpExchangeRequestHeaders.Trailer)),

                (Rule: await IsInvalidTransferEncodingHeaderArrayAsync(httpExchangeRequestHeaders.TransferEncoding),
                Parameter: nameof(HttpExchangeRequestHeaders.TransferEncoding)),

                (Rule: await IsInvalidUpgradeHeaderArrayAsync(httpExchangeRequestHeaders.Upgrade),
                Parameter: nameof(HttpExchangeRequestHeaders.Upgrade)),

                (Rule: await IsInvalidUserAgentHeaderArrayAsync(httpExchangeRequestHeaders.UserAgent),
                Parameter: nameof(HttpExchangeRequestHeaders.UserAgent)),

                (Rule: await IsInvalidViaHeaderArrayAsync(httpExchangeRequestHeaders.Via),
                Parameter: nameof(HttpExchangeRequestHeaders.Via)),

                (Rule: await IsInvalidWarningHeaderArrayAsync(httpExchangeRequestHeaders.Warning),
                Parameter: nameof(HttpExchangeRequestHeaders.Warning)));
        }

        private static async ValueTask ValidateHttpRequestHeadersAsync(
            params (dynamic Rule, string Parameter)[] validations)
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
