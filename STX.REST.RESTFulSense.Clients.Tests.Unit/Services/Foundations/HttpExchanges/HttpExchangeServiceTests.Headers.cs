// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private static dynamic CreateRandomHttpRequestHeader()
        {
            bool isTransferEncodingChunked = GetRandomBoolean();

            return new
            {
                Accept = CreateRandomMediaTypeWithQualityHeaderArray(),
                AcceptCharset = CreateRandomStringQualityHeaderArray(),
                AcceptEncoding = CreateRandomStringQualityHeaderArray(),
                AcceptLanguage = CreateRandomStringQualityHeaderArray(),
                Authorization = CreateRandomAuthorizationHeader(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = CreateRandomStringArray(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDateTime(),
                Expect = CreateRandomNameValueWithParametersArray(),
                ExpectContinue = GetRandomBoolean(),
                From = GetRandomString(),
                Host = GetRandomString(),
                IfMatch = CreateRandomQuotedStringArray(),
                IfModifiedSince = GetRandomDateTime(),
                IfNoneMatch = CreateRandomQuotedStringArray(),
                IfRange = CreateRandomRangeConditionHeader(),
                IfUnmodifiedSince = GetRandomDateTime(),
                MaxForwards = GetRandomNumber(),
                Pragma = CreateRandomNameValueArray(),
                Protocol = GetRandomString(),
                ProxyAuthorization = CreateRandomAuthorizationHeader(),
                Range = CreateRandomRangeHeader(),
                Referrer = CreateRandomUri(),

                TE =
                    CreateRandomTransferEncodingWithQualityHeaderArray(
                        isTransferEncodingChunked),

                Trailer = CreateRandomStringArray(),

                TransferEncoding =
                    CreateRandomTransferEncodingHeaderArray(
                        isTransferEncodingChunked),

                TransferEncodingChunked = isTransferEncodingChunked,
                Upgrade = CreateRandomProductHeaderArray(),
                UserAgent = CreateRandomProductInfoHeaderArray(),
                Via = CreateRandomViaHeaderArray(),
                Warning = CreateRandomWarningHeaderArray()
            };
        }

        private static dynamic CreateRandomHttpResponseHeader()
        {
            bool isConnectionClose = GetRandomBoolean();
            string[] connectionArray = CreateRandomStringArray();
            if (isConnectionClose)
            {
                connectionArray =
                    connectionArray
                        .Append("close")
                        .ToArray();
            }

            bool isTransferEncodingChunked = GetRandomBoolean();

            return new
            {
                AcceptRanges = CreateRandomStringArray(),
                Age = GetRandomTimeSpan(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = connectionArray,
                ConnectionClose = isConnectionClose,
                Date = GetRandomDateTime(),
                ETag = CreateRandomQuotedString(),
                Location = new Uri(CreateRandomBaseAddress()),
                Pragma = CreateRandomNameValueArray(),
                ProxyAuthenticate = CreateRandomAuthorizationHeaderArray(),
                RetryAfter = CreateRandomRetryConditionHeader(),
                Server = CreateRandomProductInfoHeaderArray(),
                Trailer = CreateRandomStringArray(),

                TransferEncoding =
                    CreateRandomTransferEncodingHeaderArray(
                        isTransferEncodingChunked),

                TransferEncodingChunked = isTransferEncodingChunked,
                Upgrade = CreateRandomProductHeaderArray(),
                Vary = CreateRandomStringArray(),
                Via = CreateRandomViaHeaderArray(),
                Warning = CreateRandomWarningHeaderArray(),
                WwwAuthenticate = CreateRandomAuthorizationHeaderArray()
            };
        }

        private static dynamic CreateRandomHttpContentHeader()
        {
            return new
            {
                Allow = CreateRandomStringArray(),

                ContentDisposition =
                    CreateRandomContentDispositionHeader(),

                ContentEncoding = CreateRandomStringArray(),
                ContentLanguage = CreateRandomStringArray(),
                ContentLength = GetRandomNumber(),
                ContentLocation = CreateRandomUri(),
                ContentMD5 = CreateRandomByteArray(),
                ContentRange = CreateRandomRangeContentHeader(),
                ContentType = CreateRandomMediaTypeHeader(),
                Expires = GetRandomDateTime(),
                LastModified = GetRandomDateTime()
            };
        }

        private static dynamic[] CreateRandomMediaTypeHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                    CreateRandomMediaTypeHeader())
                .ToArray();
        }

        private static dynamic CreateRandomMediaTypeHeader()
        {
            string type = GetRandomString();
            string subtype = GetRandomString();

            return new
            {
                CharSet = GetRandomString(),
                MediaType = $"{type}/{subtype}",
                Quality = default(double?)
            };
        }

        private static dynamic[] CreateRandomMediaTypeWithQualityHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                    CreateRandomMediaTypeWithQualityHeader())
                .ToArray();
        }

        private static dynamic CreateRandomMediaTypeWithQualityHeader()
        {
            string type = GetRandomString();
            string subtype = GetRandomString();

            return new
            {
                CharSet = GetRandomString(),
                MediaType = $"{type}/{subtype}",
                Quality = GetRandomDoubleBetweenZeroAndOne(),
            };
        }

        private static dynamic[] CreateRandomStringQualityHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        Value = GetRandomString(),
                        Quality = GetRandomDoubleBetweenZeroAndOne()
                    };
                }).ToArray();
        }

        private static dynamic[] CreateRandomAuthorizationHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomAuthorizationHeader())
                .ToArray();
        }

        private static dynamic CreateRandomAuthorizationHeader()
        {
            return new
            {
                Schema = GetRandomString(),
                Value = GetRandomString()
            };
        }

        private static dynamic CreateRandomCacheControlHeader()
        {
            return new
            {
                NoCache = GetRandomBoolean(),
                NoCacheHeaders = CreateRandomStringArray(),
                NoStore = GetRandomBoolean(),
                MaxAge = GetRandomTimeSpan(),
                SharedMaxAge = GetRandomTimeSpan(),
                MaxStale = GetRandomBoolean(),
                MaxStaleLimit = GetRandomTimeSpan(),
                MinFresh = GetRandomTimeSpan(),
                NoTransform = GetRandomBoolean(),
                OnlyIfCached = GetRandomBoolean(),
                Public = GetRandomBoolean(),
                Private = GetRandomBoolean(),
                PrivateHeaders = CreateRandomStringArray(),
                MustRevalidate = GetRandomBoolean(),
                ProxyRevalidate = GetRandomBoolean(),
                Extensions = CreateRandomNameValueArray()
            };
        }

        private static dynamic CreateRandomRangeConditionHeader()
        {
            bool isEvenNumber = GetRandomNumber() % 2 == 0;

            return new
            {
                Date = isEvenNumber ? GetRandomDateTime() : (DateTimeOffset?)null,
                EntityTag = isEvenNumber ? null : CreateRandomQuotedString()
            };
        }

        private static dynamic CreateRandomRangeHeader()
        {
            return new
            {
                Unit = GetRandomString(),
                Ranges = CreateRandomRangeItemHeaderArray()
            };
        }

        private static dynamic CreateRandomRangeItemHeader()
        {
            long from = GetRandomLong();
            long to = CreateRandomLongToValue(from);

            return new
            {
                From = from,
                To = to
            };
        }

        private static dynamic CreateRandomRangeItemHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomRangeItemHeader())
                .ToArray();
        }

        private static dynamic[] CreateRandomTransferEncodingHeaderArray(bool isChunked)
        {
            IEnumerable<dynamic> transferEncondingHeaderEnumeration =
                Enumerable.Range(0, GetRandomNumber())
                    .Select(item =>
                    {
                        return new
                        {
                            Quality = default(double?),
                            Value = GetRandomString(),
                            Parameters = CreateRandomNameValueArray()
                        };
                    });

            if (isChunked)
            {
                transferEncondingHeaderEnumeration =
                    transferEncondingHeaderEnumeration.Append(
                        new
                        {
                            Quality = default(double?),
                            Value = "chunked",
                            Parameters = new dynamic[] { },
                        });
            }

            return transferEncondingHeaderEnumeration.ToArray();

        }

        private static dynamic[] CreateRandomTransferEncodingWithQualityHeaderArray(bool isChunked)
        {
            IEnumerable<dynamic> transferEncondingHeaderEnumeration =
                Enumerable.Range(0, GetRandomNumber())
                    .Select(item =>
                    {
                        return new
                        {
                            Quality = GetRandomDoubleBetweenZeroAndOne(),
                            Value = GetRandomString(),
                            Parameters = CreateRandomNameValueArray()
                        };
                    });

            if (isChunked)
            {
                transferEncondingHeaderEnumeration =
                    transferEncondingHeaderEnumeration.Append(
                        new
                        {
                            Quality = GetRandomDoubleBetweenZeroAndOne(),
                            Value = "chunked",
                            Parameters = new dynamic[] { },
                        });
            }

            return transferEncondingHeaderEnumeration.ToArray();
        }

        private static dynamic CreateRandomProductInfoWithProductHeader()
        {
            return new
            {
                Comment = default(string),
                Product = CreateRandomProductHeader()
            };
        }

        private static dynamic CreateRandomProductInfoWithCommentHeader()
        {
            return new
            {
                Product = default(dynamic),
                Comment = $"({GetRandomString()})",
            };
        }

        private static dynamic CreateRandomProductHeader()
        {
            return new
            {
                Name = GetRandomString(),
                Version = GetRandomString()
            };
        }

        private static dynamic[] CreateRandomProductInfoHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomProductInfoWithCommentHeader())
                .Concat(
                    Enumerable.Range(0, GetRandomNumber())
                        .Select(item => CreateRandomProductInfoWithProductHeader()))
                .ToArray();
        }

        private static dynamic[] CreateRandomViaHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomViaHeader())
                .ToArray();
        }

        private static dynamic CreateRandomViaHeader()
        {
            return new
            {
                ProtocolName = GetRandomString(),
                ProtocolVersion = GetRandomString(),
                ReceivedBy = GetRandomString(),
                Comment = $"({GetRandomString()})",
            };
        }

        private static dynamic[] CreateRandomWarningHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomWarningHeader())
                .ToArray();
        }

        private static dynamic CreateRandomWarningHeader()
        {
            return new
            {
                Code = GetRandomNumber(),
                Agent = GetRandomString(),
                Text = CreateRandomQuotedString(),
                Date = GetRandomDateTime(),
            };
        }

        private static dynamic CreateRandomRetryConditionHeader()
        {
            bool isEven = GetRandomNumber() % 2 == 0;

            return new
            {
                Date = isEven ? GetRandomDateTime() : default(DateTimeOffset?),
                Delta = isEven ? default(TimeSpan?) : GetRandomTimeSpan()
            };
        }

        private static dynamic[] CreateRandomProductHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item => CreateRandomProductHeader())
                .ToArray();
        }

        private static dynamic CreateRandomContentDispositionHeader()
        {
            return new
            {
                DispositionType = GetRandomString(),
                Name = GetRandomString(),
                FileName = GetRandomString(),
                FileNameStar = GetRandomString(),
                CreationDate = GetRandomDateTimeWithOutFractions(),
                ModificationDate = GetRandomDateTimeWithOutFractions(),
                ReadDate = GetRandomDateTimeWithOutFractions(),
                Size = GetRandomLong(),
            };
        }

        private static dynamic CreateRandomRangeContentHeader()
        {
            long from = GetRandomLong();
            long to = CreateRandomLongToValue(from);
            long length = to;

            return new
            {
                Unit = GetRandomString(),
                From = from,
                To = to,
                Length = length
            };
        }

        private static dynamic CreateAcceptHeaderException(MediaTypeHeader[] invalidAcceptHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Accept),
                value: "Accept header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Accept = invalidAcceptHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateAcceptCharsetHeaderException(
            StringWithQualityHeader[] invalidAcceptCharsetHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.AcceptCharset),
                value: "AcceptCharset header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                AcceptCharset = invalidAcceptCharsetHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateAcceptEncodingHeaderException(
            StringWithQualityHeader[] invalidAcceptEncodingHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.AcceptEncoding),
                value: "AcceptEncoding header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                AcceptEncoding = invalidAcceptEncodingHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateAcceptLanguageHeaderException(
            StringWithQualityHeader[] invalidAcceptLanguageHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.AcceptLanguage),
                value: "AcceptLanguage header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                AcceptLanguage = invalidAcceptLanguageHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateAuthorizationHeaderException(AuthenticationHeader invalidAuthenticationHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Authorization),
                value: "Authorization header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Authorization = invalidAuthenticationHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateCacheControlHeaderException(CacheControlHeader invalidCacheControlHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.CacheControl),
                value: "CacheControl header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                CacheControl = invalidCacheControlHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateConnectionHeaderException(string[] invalidConnectionHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Connection),
                value: "Connection header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Connection = invalidConnectionHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateExpectHeaderException(NameValueWithParametersHeader[] invalidExpectHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Expect),
                value: "Expect header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Expect = invalidExpectHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateFromHeaderException(string invalidFromHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.From),
                value: "From header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                From = invalidFromHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateHostHeaderException(string invalidHostHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Host),
                value: "Host header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Host = invalidHostHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateIfMatchHeaderException(string[] invalidIfMatchHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.IfMatch),
                value: "IfMatch header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                IfMatch = invalidIfMatchHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateIfNoneMatchHeaderException(string[] invalidIfNoneMatchHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.IfNoneMatch),
                value: "IfNoneMatch header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                IfNoneMatch = invalidIfNoneMatchHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateIfRangeHeaderException()
        {
            string randomString = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTime();

            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
               message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.IfRange),
                value: "IfRange header has a invalid configuration." +
                        "Only one date and one entityTag can be set at a time, fix errors and try again.");

            var httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                IfRange = new RangeConditionHeader
                {
                    Date = randomDateTimeOffset,
                    EntityTag = randomString
                }
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreatePragmaHeaderException(NameValueHeader[] invalidPragmaHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Pragma),
                value: "Pragma header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Pragma = invalidPragmaHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateProtocolHeaderException(string invalidProtocolHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Protocol),
                value: "Protocol header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Protocol = invalidProtocolHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateProxyAuthorizationHeaderException(AuthenticationHeader invalidAuthenticationHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.ProxyAuthorization),
                value: "ProxyAuthorization header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                ProxyAuthorization = invalidAuthenticationHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateRangeHeaderException(RangeHeader invalidRangeHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Range),
                value: "Range header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Range = invalidRangeHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateTEHeaderException(TransferCodingHeader[] invalidTEHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.TE),
                value: "TE header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                TE = invalidTEHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateTrailerHeaderException(string[] invalidTrailerHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Trailer),
                value: "Trailer header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Trailer = invalidTrailerHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateTransferEncodingException(TransferCodingHeader[] invalidTransferEncodingHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.TransferEncoding),
                value: "TransferEncoding header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                TransferEncoding = invalidTransferEncodingHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateUpgradeHeaderException(ProductHeader[] invalidUpgradeHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Upgrade),
                value: "Upgrade header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Upgrade = invalidUpgradeHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateUserAgentHeaderException(ProductInfoHeader[] invalidUserAgentHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.UserAgent),
                value: "UserAgent header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                UserAgent = invalidUserAgentHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateViaHeaderException(ViaHeader[] invalidViaHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Via),
                value: "Via header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Via = invalidViaHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateWarningHeaderException(WarningHeader[] invalidWarningHeader)
        {
            var invalidHttpExchangeRequestHeaderException = new InvalidHttpExchangeRequestHeaderException(
              message: "Invalid HttpExchange request header error occurred, fix errors and try again.");

            invalidHttpExchangeRequestHeaderException.UpsertDataList(
                key: nameof(HttpExchangeRequestHeaders.Warning),
                value: "Warning header has invalid configuration, fix errors and try again.");

            HttpExchangeRequestHeaders httpExchangeRequestHeaders = new HttpExchangeRequestHeaders
            {
                Warning = invalidWarningHeader
            };

            return new
            {
                HttpExchangeRequestHeaders = httpExchangeRequestHeaders,
                InvalidHttpExchangeRequestHeaderException = invalidHttpExchangeRequestHeaderException
            };
        }

        private static dynamic CreateAllowHeaderException(string[] invalidAllowHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.Allow),
                value: "Allow header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                Allow = invalidAllowHeader
            };

            return new
            {
                HttpExchangeContentHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static dynamic CreateContentDispositionHeaderException(
            ContentDispositionHeader invalidContentDispositionHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.ContentDisposition),
                value: "ContentDisposition header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                ContentDisposition = invalidContentDispositionHeader
            };

            return new
            {
                HttpExchangeHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static dynamic CreateContentEncodingHeaderException(string[] invalidContentEncodingHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.ContentEncoding),
                value: "ContentEncoding header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                ContentEncoding = invalidContentEncodingHeader
            };

            return new
            {
                HttpExchangeHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static dynamic CreateContentLanguageHeaderException(string[] invalidContentLanguageHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.ContentLanguage),
                value: "ContentLanguage header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                ContentEncoding = invalidContentLanguageHeader
            };

            return new
            {
                HttpExchangeHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static dynamic CreateContentRangeHeaderException(
            ContentRangeHeader invalidContentRangeHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.ContentRange),
                value: "ContentRange header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                ContentRange = invalidContentRangeHeader
            };

            return new
            {
                HttpExchangeHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static dynamic CreateContentTypeHeaderException(
            MediaTypeHeader invalidContentTypeHeader)
        {
            var invalidHttpExchangeContentHeaderException = new InvalidHttpExchangeContentHeaderException(
              message: "Invalid HttpExchange content header error occurred, fix errors and try again.");

            invalidHttpExchangeContentHeaderException.UpsertDataList(
                key: nameof(HttpExchangeContentHeaders.ContentType),
                value: "ContentType header has invalid configuration, fix errors and try again.");

            HttpExchangeContentHeaders httpExchangeContentHeaders = new HttpExchangeContentHeaders
            {
                ContentType = invalidContentTypeHeader
            };

            return new
            {
                HttpExchangeHeaders = httpExchangeContentHeaders,
                InvalidHttpExchangeContentHeaderException = invalidHttpExchangeContentHeaderException
            };
        }

        private static MediaTypeHeader[] CreateInvalidMediaTypes()
        {
            string[] invalidCharSetHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidStringMediaTypeHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<MediaTypeHeader> invalidMediaTypeHeaders =
               from invalidCharSetHeader in invalidCharSetHeaders
               from invalidStringMediaTypeHeader in invalidStringMediaTypeHeaders
               select new MediaTypeHeader
               {
                   CharSet = invalidCharSetHeader,
                   MediaType = invalidStringMediaTypeHeader
               };

            return invalidMediaTypeHeaders
                .Prepend(null)
                .Prepend(
                    new MediaTypeHeader
                    {
                        CharSet = null,
                        MediaType = null
                    })
                .ToArray();
        }

        private static StringWithQualityHeader[] CreateInvalidStringWithQualityHeaders()
        {
            string[] invalidValueHeaders = CreateInvalidStringArrayHeaders();

            return invalidValueHeaders
                .Select(invalidValueHeader =>
                    new StringWithQualityHeader
                    {
                        Value = invalidValueHeader
                    })
                .Prepend(null)
                .ToArray();
        }

        private static AuthenticationHeader[] CreateInvalidAuthenticationHeaders()
        {
            string[] invalidSchemaHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidValueHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<AuthenticationHeader> invalidAutenticationHeaders =
               from invalidSchemaHeader in invalidSchemaHeaders
               from invalidValueHeader in invalidValueHeaders
               select new AuthenticationHeader
               {
                   Schema = invalidSchemaHeader,
                   Value = invalidValueHeader
               };

            return invalidAutenticationHeaders
                .Prepend(
                    new AuthenticationHeader
                    {
                        Schema = null,
                        Value = null
                    })
                .ToArray();
        }

        private static CacheControlHeader[] CreateInvalidCacheControlHeaders()
        {
            NameValueHeader[] invalidExtensionHeaders =
                CreateInvalidNameValueHeaders();

            string[] invalidPrivateHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidNoCacheHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<CacheControlHeader> invalidCacheControlHeaders =
               from invalidExtensionHeader in invalidExtensionHeaders
               from invalidPrivateHeader in invalidPrivateHeaders
               from invalidNoCacheHeader in invalidNoCacheHeaders
               select new CacheControlHeader
               {
                   Extensions = new NameValueHeader[] { invalidExtensionHeader },
                   PrivateHeaders = new string[] { invalidPrivateHeader },
                   NoCacheHeaders = new string[] { invalidNoCacheHeader }
               };

            return invalidCacheControlHeaders
                .ToArray();
        }

        private static NameValueHeader[] CreateInvalidNameValueHeaders()
        {
            string[] invalidNameHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidValueHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<NameValueHeader> invalidNameValueHeaders =
               from invalidNameHeader in invalidNameHeaders
               from invalidValueHeader in invalidValueHeaders
               select new NameValueHeader
               {
                   Name = invalidNameHeader,
                   Value = invalidValueHeader
               };

            return invalidNameValueHeaders
                .Prepend(null)
                .ToArray();
        }

        private static string[] CreateInvalidStringArrayHeaders()
        {
            return new string[]
            {
                null,
                "",
                " "
            };
        }

        private static string[] CreateInvalidStringHeaders()
        {
            return new string[]
            {
                "",
                " "
            };
        }

        private static RangeItemHeader[] CreateInvalidRangeItemHeaders()
        {
            return new RangeItemHeader[]
            {
                new RangeItemHeader
                {
                    From = null,
                    To = null
                },
                new RangeItemHeader
                {
                    From = 1,
                    To = null
                },
                new RangeItemHeader
                {
                    From = null,
                    To = 1
                },
                new RangeItemHeader
                {
                    From = 1,
                    To = 0
                }
            };
        }

        private static NameValueWithParametersHeader[] CreateInvalidNameValueWithParametersHeaders()
        {
            NameValueHeader[] invalidParametersHeaders =
                CreateInvalidNameValueHeaders();

            string[] invalidNameHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<NameValueWithParametersHeader> invalidNameValueWithParametersHeaders =
                from invalidNameHeader in invalidNameHeaders
                select new NameValueWithParametersHeader
                {
                    Name = invalidNameHeader,
                    Parameters = null,
                };

            invalidNameValueWithParametersHeaders = invalidNameValueWithParametersHeaders.Concat(
                from invalidNameHeader in invalidNameHeaders
                from invalidParametersHeader in invalidParametersHeaders
                select new NameValueWithParametersHeader
                {
                    Name = invalidNameHeader,
                    Parameters = new NameValueHeader[] { invalidParametersHeader }
                });

            return invalidNameValueWithParametersHeaders.ToArray();
        }

        private static RangeHeader[] CreateInvalidRangeHeaders()
        {
            RangeItemHeader[] invalidRangeItemHeaders =
                CreateInvalidRangeItemHeaders();

            string[] invalidUnitHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<RangeHeader> invalidRangeHeaders =
                from invalidUnitHeader in invalidUnitHeaders
                select new RangeHeader
                {
                    Unit = invalidUnitHeader,
                    Ranges = null
                };

            invalidRangeHeaders = invalidRangeHeaders.Concat(
                from invalidUnitHeader in invalidUnitHeaders
                from invalidRangeItemHeader in invalidRangeItemHeaders
                select new RangeHeader
                {
                    Unit = invalidUnitHeader,
                    Ranges = new RangeItemHeader[] { invalidRangeItemHeader }
                });

            return invalidRangeHeaders
                .ToArray();
        }

        private static TransferCodingHeader[] CreateInvalidTransferCodingHeaders()
        {
            NameValueHeader[] invalidNameValueHeaders =
                CreateInvalidNameValueHeaders();

            string[] invalidValueHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<TransferCodingHeader> invalidTransferCodingHeaders =
                from invalidValueHeader in invalidValueHeaders
                select new TransferCodingHeader
                {
                    Parameters = null,
                    Value = invalidValueHeader
                };

            invalidTransferCodingHeaders = invalidTransferCodingHeaders.Concat(
                from invalidNameValueHeader in invalidNameValueHeaders
                from invalidValueHeader in invalidValueHeaders
                select new TransferCodingHeader
                {
                    Parameters = new NameValueHeader[] { invalidNameValueHeader },
                    Value = invalidValueHeader
                });

            return invalidTransferCodingHeaders
                .Prepend(null)
                .ToArray();
        }

        private static ProductHeader[] CreateInvalidProductHeaders()
        {
            string[] invalidNameHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidVersionHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<ProductHeader> invalidProductHeaders =
                from invalidNameHeader in invalidNameHeaders
                from invalidVersionHeader in invalidVersionHeaders
                select new ProductHeader
                {
                    Name = invalidNameHeader,
                    Version = invalidVersionHeader
                };

            return invalidProductHeaders
                .Prepend(null)
                .ToArray();
        }

        private static ProductInfoHeader[] CreateInvalidProductInfoHeaders()
        {
            ProductHeader[] invalidProductHeaders =
                CreateInvalidProductHeaders();

            string[] invalidCommentHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<ProductInfoHeader> invalidProductInfoHeaders =
                from invalidCommentHeader in invalidCommentHeaders
                from invalidProductHeader in invalidProductHeaders
                select new ProductInfoHeader
                {
                    Comment = invalidCommentHeader,
                    Product = invalidProductHeader
                };

            return invalidProductInfoHeaders
                .Prepend(null)
                .ToArray();
        }

        private static ViaHeader[] CreateInvalidViaHeaders()
        {
            string[] invalidCommentHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidProtocolNameHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidProtocolVersionHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidReceivedByHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<ViaHeader> invalidViaHeaders =
                from invalidCommentHeader in invalidCommentHeaders
                from invalidProtocolNameHeader in invalidProtocolNameHeaders
                from invalidProtocolVersionHeader in invalidProtocolVersionHeaders
                from invalidReceivedByHeader in invalidReceivedByHeaders
                select new ViaHeader
                {
                    Comment = invalidCommentHeader,
                    ProtocolName = invalidProtocolNameHeader,
                    ProtocolVersion = invalidProtocolVersionHeader,
                    ReceivedBy = invalidReceivedByHeader
                };

            return invalidViaHeaders
                .Prepend(null)
                .ToArray();
        }

        private static WarningHeader[] CreateInvalidWarningHeaders()
        {
            string[] invalidAgentHeaders = CreateInvalidStringArrayHeaders();
            string[] invalidTextHeaders = CreateInvalidStringArrayHeaders();

            IEnumerable<WarningHeader> invalidViaHeaders =
                from invalidAgentHeader in invalidAgentHeaders
                from invalidTextHeader in invalidTextHeaders
                select new WarningHeader
                {
                    Agent = invalidAgentHeader,
                    Text = invalidTextHeader
                };

            return invalidViaHeaders
                .Prepend(null)
                .ToArray();
        }

        private static ContentDispositionHeader[] CreateInvalidContentDispositionHeaders()
        {
            string[] invalidDispositionTypes = CreateInvalidStringArrayHeaders();
            string[] invalidNames = CreateInvalidStringArrayHeaders();
            string[] invalidFileNames = CreateInvalidStringArrayHeaders();
            string[] invalidFileNameStars = CreateInvalidStringArrayHeaders();

            IEnumerable<ContentDispositionHeader> invalidContentDispositionHeaders =
                from invalidDispositionType in invalidDispositionTypes
                from invalidName in invalidNames
                from invalidFileName in invalidFileNames
                from invalidFileNameStar in invalidFileNameStars
                select new ContentDispositionHeader
                {
                    DispositionType = invalidDispositionType,
                    Name = invalidName,
                    FileName = invalidFileName,
                    FileNameStar = invalidFileNameStar,
                };

            return invalidContentDispositionHeaders
                .Prepend(null)
                .ToArray();
        }

        private static ContentRangeHeader[] CreateInvalidContentRangeHeaders()
        {
            string[] invalidUnits = CreateInvalidStringArrayHeaders();
            long?[] invalidFroms = [null, 0];
            long?[] invalidTos = [null, 0];
            long?[] invalidLengths = [null, 0];

            IEnumerable<ContentRangeHeader> invalidContentRangeHeaders =
                from invalidUnit in invalidUnits
                from invalidFrom in invalidFroms
                from invalidTo in invalidTos
                from invalidLength in invalidLengths
                select new ContentRangeHeader
                {
                    Unit = invalidUnit,
                    From = invalidFrom,
                    To = invalidTo,
                    Length = invalidLength
                };

            return invalidContentRangeHeaders
                .Prepend(null)
                .ToArray();
        }

        private static TheoryData<dynamic> CreateRequestHeadersValidationExceptions(TheoryData<dynamic> theoryData)
        {
            MediaTypeHeader[] invalidAcceptHeaders = CreateInvalidMediaTypes();
            invalidAcceptHeaders
                .Select(invalidAcceptHeader =>
                    {
                        theoryData.Add(
                            CreateAcceptHeaderException(
                                new MediaTypeHeader[] { invalidAcceptHeader }));

                        return theoryData;
                    })
                .ToArray();

            StringWithQualityHeader[] invalidAcceptCharsetHeaders = CreateInvalidStringWithQualityHeaders();
            invalidAcceptCharsetHeaders
                .Select(invalidAcceptCharsetHeader =>
                {
                    theoryData.Add(
                        CreateAcceptCharsetHeaderException(
                            new StringWithQualityHeader[] { invalidAcceptCharsetHeader }));

                    return theoryData;
                })
                .ToArray();

            StringWithQualityHeader[] invalidAcceptEncodingHeaders = CreateInvalidStringWithQualityHeaders();
            invalidAcceptEncodingHeaders
                .Select(invalidAcceptEncodingHeader =>
                {
                    theoryData.Add(
                        CreateAcceptEncodingHeaderException(
                            new StringWithQualityHeader[] { invalidAcceptEncodingHeader }));

                    return theoryData;
                })
                .ToArray();

            StringWithQualityHeader[] invalidAcceptLanguageHeaders = CreateInvalidStringWithQualityHeaders();
            invalidAcceptLanguageHeaders
                .Select(invalidAcceptLanguageHeader =>
                {
                    theoryData.Add(
                        CreateAcceptLanguageHeaderException(
                            new StringWithQualityHeader[] { invalidAcceptLanguageHeader }));

                    return theoryData;
                })
                .ToArray();

            AuthenticationHeader[] invalidAuthorizationHeaders = CreateInvalidAuthenticationHeaders();
            invalidAuthorizationHeaders
                .Select(invalidAuthorizationHeader =>
                {
                    theoryData.Add(
                        CreateAuthorizationHeaderException(invalidAuthorizationHeader));

                    return theoryData;
                })
                .ToArray();

            CacheControlHeader[] invalidCacheControlHeaders = CreateInvalidCacheControlHeaders();
            invalidCacheControlHeaders
                .Select(invalidCacheControlHeader =>
                {
                    theoryData.Add(
                        CreateCacheControlHeaderException(invalidCacheControlHeader));

                    return theoryData;
                })
                .ToArray();

            string[] invalidConnectionHeaders = CreateInvalidStringArrayHeaders();
            invalidConnectionHeaders
                .Select(invalidConnectionHeader =>
                {
                    theoryData.Add(
                        CreateConnectionHeaderException(
                            new string[] { invalidConnectionHeader }));

                    return theoryData;
                })
                .ToArray();

            NameValueWithParametersHeader[] invalidExpectHeaders = CreateInvalidNameValueWithParametersHeaders();
            invalidExpectHeaders
                .Select(invalidExpectHeader =>
                {
                    theoryData.Add(
                        CreateExpectHeaderException(
                            new NameValueWithParametersHeader[] { invalidExpectHeader }));

                    return theoryData;
                })
                .ToArray();

            string[] invalidFromHeaders = CreateInvalidStringHeaders();
            invalidFromHeaders
                .Select(invalidFromHeader =>
                {
                    theoryData.Add(
                        CreateFromHeaderException(invalidFromHeader));

                    return theoryData;
                })
                .ToArray();

            string[] invalidHostHeaders = CreateInvalidStringHeaders();
            invalidHostHeaders
                .Select(invalidHostHeader =>
                {
                    theoryData.Add(
                        CreateHostHeaderException(invalidHostHeader));

                    return theoryData;
                })
                .ToArray();

            string[] invalidIfMatchHeaders = CreateInvalidStringArrayHeaders();
            invalidIfMatchHeaders
                .Select(invalidIfMatchHeader =>
                {
                    theoryData.Add(
                        CreateIfMatchHeaderException(
                            new string[] { invalidIfMatchHeader }));

                    return theoryData;
                })
                .ToArray();

            string[] invalidIfNoneMatchHeaders = CreateInvalidStringArrayHeaders();
            invalidIfNoneMatchHeaders
                .Select(invalidIfNoneMatchHeader =>
                {
                    theoryData.Add(
                        CreateIfNoneMatchHeaderException(
                            new string[] { invalidIfNoneMatchHeader }));

                    return theoryData;
                })
                .ToArray();

            theoryData.Add(
                CreateIfRangeHeaderException());

            NameValueHeader[] invalidPragmaHeaders = CreateInvalidNameValueHeaders();
            invalidPragmaHeaders
                .Select(invalidPragmaHeader =>
                {
                    theoryData.Add(
                        CreatePragmaHeaderException(
                            new NameValueHeader[] { invalidPragmaHeader }));

                    return theoryData;
                })
                .ToArray();

            string[] invalidProtocolHeaders = CreateInvalidStringHeaders();
            invalidProtocolHeaders
                .Select(invalidProtocolHeader =>
                {
                    theoryData.Add(
                        CreateProtocolHeaderException(invalidProtocolHeader));

                    return theoryData;
                })
                .ToArray();

            AuthenticationHeader[] invalidProxyAuthorizationHeaders = CreateInvalidAuthenticationHeaders();
            invalidProxyAuthorizationHeaders
                .Select(invalidProxyAuthorizationHeader =>
                {
                    theoryData.Add(
                        CreateProxyAuthorizationHeaderException(invalidProxyAuthorizationHeader));

                    return theoryData;
                })
                .ToArray();

            RangeHeader[] invalidRangeHeaders = CreateInvalidRangeHeaders();
            invalidRangeHeaders
                .Select(invalidRangeHeader =>
                {
                    theoryData.Add(
                        CreateRangeHeaderException(invalidRangeHeader));

                    return theoryData;
                })
                .ToArray();

            TransferCodingHeader[] invalidTEHeaders = CreateInvalidTransferCodingHeaders();
            invalidTEHeaders
                .Select(invalidTEHeader =>
                {
                    theoryData.Add(
                        CreateTEHeaderException(
                            new TransferCodingHeader[] { invalidTEHeader }));

                    return theoryData;
                })
                .ToArray();

            string[] invalidTrailerHeaders = CreateInvalidStringArrayHeaders();
            invalidTrailerHeaders
                .Select(invalidTrailerHeader =>
                {
                    theoryData.Add(
                        CreateTrailerHeaderException(
                            new string[] { invalidTrailerHeader }));

                    return theoryData;
                })
                .ToArray();

            TransferCodingHeader[] invalidTransferEncondingHeaders = CreateInvalidTransferCodingHeaders();
            invalidTransferEncondingHeaders
                .Select(invalidTransferEncondingHeader =>
                {
                    theoryData.Add(
                        CreateTransferEncodingException(
                            new TransferCodingHeader[] { invalidTransferEncondingHeader }));

                    return theoryData;
                })
                .ToArray();

            ProductHeader[] invalidUpgradeHeaders = CreateInvalidProductHeaders();
            invalidUpgradeHeaders
                .Select(invalidUpgradeHeader =>
                {
                    theoryData.Add(
                        CreateUpgradeHeaderException(
                            new ProductHeader[] { invalidUpgradeHeader }));

                    return theoryData;
                })
                .ToArray();

            ProductInfoHeader[] invalidUserAgentHeaders = CreateInvalidProductInfoHeaders();
            invalidUserAgentHeaders
                .Select(invalidUserAgentHeader =>
                {
                    theoryData.Add(
                        CreateUserAgentHeaderException(
                            new ProductInfoHeader[] { invalidUserAgentHeader }));

                    return theoryData;
                })
                .ToArray();

            ViaHeader[] invalidViaHeaders = CreateInvalidViaHeaders();
            invalidViaHeaders
                .Select(invalidViaHeader =>
                {
                    theoryData.Add(
                        CreateViaHeaderException(
                            new ViaHeader[] { invalidViaHeader }));

                    return theoryData;
                })
                .ToArray();

            WarningHeader[] invalidWarningHeaders = CreateInvalidWarningHeaders();
            invalidWarningHeaders
                .Select(invalidWarningHeader =>
                {
                    theoryData.Add(
                        CreateWarningHeaderException(
                            new WarningHeader[] { invalidWarningHeader }));

                    return theoryData;
                })
                .ToArray();

            return theoryData;
        }

        private static TheoryData<dynamic> CreateRequestContentHeadersValidationExceptions(TheoryData<dynamic> theoryData)
        {
            string[] invalidAllowHeaders = CreateInvalidStringArrayHeaders();
            invalidAllowHeaders
                .Select(invalidAllowHeader =>
                {
                    theoryData.Add(
                        CreateAllowHeaderException(
                            new string[] { invalidAllowHeader }));

                    return theoryData;
                })
                .ToArray();

            ContentDispositionHeader[] invalidContentDispositionHeaders =
                CreateInvalidContentDispositionHeaders();

            invalidContentDispositionHeaders
                .Select(invalidContentDispositionHeader =>
                    {
                        theoryData.Add(
                            CreateContentDispositionHeaderException(
                                invalidContentDispositionHeader));

                        return theoryData;
                    })
                    .ToArray();

            string[] invalidContentEncodingHeaders = CreateInvalidStringArrayHeaders();
            invalidContentEncodingHeaders
                .Select(invalidContentEncodingHeader =>
                {
                    theoryData.Add(
                        CreateContentEncodingHeaderException(
                            new string[] { invalidContentEncodingHeader }));

                    return theoryData;
                })
                .ToArray();

            string[] invalidContentLanguageHeaders = CreateInvalidStringArrayHeaders();
            invalidContentLanguageHeaders
                .Select(invalidContentLanguageHeader =>
                {
                    theoryData.Add(
                        CreateContentLanguageHeaderException(
                            new string[] { invalidContentLanguageHeader }));

                    return theoryData;
                })
                .ToArray();

            ContentRangeHeader[] invalidContentRangeHeaders =
                CreateInvalidContentRangeHeaders();

            invalidContentRangeHeaders
                .Select(invalidContentRangeHeader =>
                {
                    theoryData.Add(
                        CreateContentRangeHeaderException(
                            invalidContentRangeHeader));

                    return theoryData;
                })
                    .ToArray();

            MediaTypeHeader[] invalidContentTypeHeaders = CreateInvalidMediaTypes();
            invalidContentTypeHeaders
                .Select(invalidContentTypeHeader =>
                {
                    theoryData.Add(
                        CreateContentTypeHeaderException(
                           invalidContentTypeHeader));

                    return theoryData;
                })
                .ToArray();

            return theoryData;
        }

        private static dynamic CreateSendRequestDependencyExceptions(TheoryData<dynamic> theoryData)
        {
            var someMessage = GetRandomString();
            var httpRequestException = new HttpRequestException();

            var failedRequestHttpExchangeException = new FailedRequestHttpExchangeException(
                message: "Failed http request error occurred, contact support.",
                innerException: httpRequestException);

            theoryData.Add(new
            {
                ExternalException = httpRequestException,
                LocalizedException = failedRequestHttpExchangeException
            });

            var taskCanceledException = new TaskCanceledException();

            var taskCanceledHttpExchangeException = new TaskCanceledHttpExchangeException(
                message: "Request timeout error occurred, please contact support.",
                innerException: taskCanceledException);

            theoryData.Add(new
            {
                ExternalException = taskCanceledException,
                LocalizedException = taskCanceledHttpExchangeException
            });

            var objectDisposedException = new ObjectDisposedException(someMessage);

            var objectDisposedHttpExchangeException = new ObjectDisposedHttpExchangeException(
                message: "Object already disposed error occurred, please fix errors and try again.",
                innerException: objectDisposedException);

            theoryData.Add(new
            {
                ExternalException = objectDisposedException,
                LocalizedException = objectDisposedHttpExchangeException
            });

            var invalidOperationException = new InvalidOperationException();

            var invalidOperationHttpExchangeException = new InvalidOperationHttpExchangeException(
                message: "Invalid http request operation error occurred, please contact support.",
                innerException: invalidOperationException);

            theoryData.Add(new
            {
                ExternalException = invalidOperationException,
                LocalizedException = invalidOperationHttpExchangeException
            });

            var argumentNullException = new ArgumentNullException();

            var invalidArgumentHttpExchangeException = new InvalidArgumentHttpExchangeException(
                message: "Invalid argument error occurred, contact support.",
                innerException: argumentNullException);

            theoryData.Add(new
            {
                ExternalException = argumentNullException,
                LocalizedException = invalidArgumentHttpExchangeException
            });

            var formatException = new FormatException();

            var invalidFormatHttpExchangeException = new InvalidFormatHttpExchangeException(
                message: "Invalid format error occurred, contact support.",
                innerException: formatException);

            theoryData.Add(new
            {
                ExternalException = formatException,
                LocalizedException = invalidFormatHttpExchangeException
            });

            return theoryData;
        }
    }
}
