// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    public class HttpExchangeResponseHeaders
    {
        public string[] AcceptRanges { get; init; }
        public TimeSpan? Age { get; init; }
        public CacheControlHeader CacheControl { get; init; }
        public string[] Connection { get; init; }
        public bool? ConnectionClose { get; init; }
        public DateTimeOffset? Date { get; init; }
        public string ETag { get; init; }
        public Uri Location { get; init; }
        public NameValueHeader[] Pragma { get; init; }
        public AuthenticationHeader ProxyAuthenticate { get; init; }
        public RetryConditionHeader RetryAfter { get; init; }
        public ProductInfoHeader Server { get; init; }
        public string[] Trailer { get; init; }
        public TransferCodingHeader[] TransferEncoding { get; init; }
        public bool? TransferEncodingChunked { get; init; }
        public ProductHeader[] Upgrade { get; init; }
        public string[] Vary { get; init; }
        public ViaHeader Via { get; init; }
        public WarningHeader Warning { get; init; }
        public AuthenticationHeader WwwAuthenticate { get; init; }
    }
}
