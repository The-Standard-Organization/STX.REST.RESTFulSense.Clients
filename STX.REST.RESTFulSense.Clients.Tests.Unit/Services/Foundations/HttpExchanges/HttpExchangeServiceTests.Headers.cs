// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Linq;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpExchanges
{
    public partial class HttpExchangeServiceTests
    {
        private static dynamic CreateRandomHttpRequestHeader()
        {
            return new
            {
                Accept = CreateRandomMediaTypeHeaderArray(),
                AcceptCharset = CreateRandomStringQualityHeaderArray(),
                AcceptEncoding = CreateRandomStringQualityHeaderArray(),
                AcceptLanguage = CreateRandomStringQualityHeaderArray(),
                Authorization = CreateRandomAuthorizationHeader(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = CreateRandomStringArray(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDateTime(),
                Expect = CreateRandomNameValueArray(),
                ExpectContinue = GetRandomBoolean(),
                ExpectCore = CreateRandomNameValueWithParametersArray(),
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
                TE = CreateTransferCodingHeaderArray(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateTransferCodingHeaderArray(),
                TransferEncodingChunked = GetRandomBoolean(),
                Upgrade = CreateRandomProductHeaderArray(),
                UserAgent = CreateRandomProductInfoHeaderArray(),
                Via = CreateRandomViaHeaderArray(),
                Warning = CreateRandomWarningHeaderArray()
            };
        }

        private static dynamic CreateRandomHttpResponseHeader()
        {
            return new
            {
                AcceptRanges = CreateRandomStringArray(),
                Age = GetRandomTimeSpan(),
                CacheControl = CreateRandomCacheControlHeader(),
                Connection = CreateRandomStringArray(),
                ConnectionClose = GetRandomBoolean(),
                Date = GetRandomDateTime(),
                ETag = CreateRandomQuotedString(),
                Location = new Uri(CreateRandomBaseAddress()),
                Pragma = CreateRandomNameValueArray(),
                ProxyAuthenticate = CreateRandomAuthorizationHeaderArray(),
                RetryAfter = CreateRandomRetryConditionHeader(),
                Server = CreateRandomProductInfoHeaderArray(),
                Trailer = CreateRandomStringArray(),
                TransferEncoding = CreateTransferCodingHeaderArray(),
                TransferEncodingChunked = GetRandomBoolean(),
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
                Charset = GetRandomString(),
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
            return new
            {
                Date = GetRandomDateTime(),
                EntityTag = CreateRandomQuotedString()
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

        private static dynamic[] CreateTransferCodingHeaderArray()
        {
            return Enumerable.Range(0, GetRandomNumber())
                .Select(item =>
                {
                    return new
                    {
                        Quality = GetRandomDoubleBetweenZeroAndOne(),
                        Value = GetRandomString(),
                        Parameters = CreateRandomNameValueArray()
                    };
                }).ToArray();
        }

        private static dynamic CreateRandomProductInfoHeader()
        {
            return new
            {
                Comment = $"({GetRandomString()})",
                Product = CreateRandomProductHeader()
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
                .Select(item => CreateRandomProductInfoHeader())
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
                Text = $"\"{GetRandomString()}\"",
                Date = GetRandomDateTime(),
            };
        }

        private static dynamic CreateRandomRetryConditionHeader()
        {
            return new
            {
                Date = GetRandomDateTime(),
                Delta = GetRandomTimeSpan()
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
    }
}
