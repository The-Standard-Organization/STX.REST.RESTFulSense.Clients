// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    public class HttpExchangeRequestHeaders
    {
        public MediaTypeHeader[] Accept { get; init; }
        public StringWithQualityHeader[] AcceptCharset { get; init; }
        public StringWithQualityHeader[] AcceptEncoding { get; init; }
        public StringWithQualityHeader[] AcceptLanguage { get; init; }
        public AuthenticationHeader Authorization { get; init; }
        public CacheControlHeader CacheControl { get; init; }
        public string Connection { get; init; }
        public bool? ConnectionClose { get; init; }
        public DateTimeOffset? Date { get; init; }
        public NameValueHeader[] Expect { get; init; }
        public bool? ExpectContinue { get; init; }
        public NameValueWithParameters[] ExpectCore { get; init; }
        public string From { get; init; }
        public string Host { get; init; }
        public string IfMatch { get; init; }
        public DateTimeOffset? IfModifiedSince { get; init; }
        public string IfNoneMatch { get; init; }
        public RangeConditionHeader IfRange { get; init; }
        public DateTimeOffset? IfUnmodifiedSince { get; init; }
        public int? MaxForwards { get; init; }
        public NameValueHeader[] Pragma { get; init; }
        public string Protocol { get; init; }
        public AuthenticationHeader ProxyAuthorization { get; init; }
        public RangeHeader Range { get; init; }
        public Uri Referrer { get; init; }
        public TransferCodingHeader[] TE { get; init; }
        public string[] Trailer { get; init; }
        public TransferCodingHeader[] TransferEncoding { get; init; }
        public bool? TransferEncodingChunked { get; init; }
        public ProductHeader[] Upgrade { get; init; }
        public ProductInfoHeader[] UserAgent { get; init; }
        public ViaHeader Via { get; init; }
        public WarningHeader Warning { get; init; }

    }
}
