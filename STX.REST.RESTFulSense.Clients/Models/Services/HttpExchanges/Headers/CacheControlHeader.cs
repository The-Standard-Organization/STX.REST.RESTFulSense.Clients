// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    public class CacheControlHeader
    {
        public bool NoCache { get; init; }
        public string[] NoCacheHeaders { get; init; }
        public bool NoStore { get; init; }
        public TimeSpan? MaxAge { get; init; }
        public TimeSpan? SharedMaxAge { get; init; }
        public bool MaxStale { get; init; }
        public TimeSpan? MaxStaleLimit { get; init; }
        public TimeSpan? MinFresh { get; init; }
        public bool NoTransform { get; init; }
        public bool OnlyIfCached { get; init; }
        public bool Public { get; init; }
        public bool Private { get; init; }
        public string[] PrivateHeaders { get; init; }
        public bool MustRevalidate { get; init; }
        public bool ProxyRevalidate { get; init; }
        public NameValueHeader[] Extensions { get; init; }
    }
}
