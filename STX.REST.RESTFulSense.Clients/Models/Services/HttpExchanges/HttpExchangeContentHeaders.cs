// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    public class HttpExchangeContentHeaders
    {
        public string[] Allow { get; init; }
        public ContentDispositionHeader ContentDisposition { get; init; }
        public string[] ContentEncoding { get; init; }
        public string[] ContentLanguage { get; init; }
        public long? ContentLength { get; init; }
        public Uri ContentLocation { get; init; }
        public byte[] ContentMD5 { get; init; }
        public ContentRangeHeader ContentRange { get; init; }
        public MediaTypeHeader ContentType { get; init; }
        public DateTimeOffset? Expires { get; init; }
        public DateTimeOffset? LastModified { get; init; }
    }
}
