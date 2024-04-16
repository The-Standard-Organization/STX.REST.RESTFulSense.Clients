// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers
{
    internal class ContentDispositionHeader
    {
        public string DispositionType { get; init; }
        public string Name { get; init; }
        public string FileName { get; init; }
        public string FileNameStar { get; init; }
        public DateTimeOffset? CreationDate { get; init; }
        public DateTimeOffset? ModificationDate { get; init; }
        public DateTimeOffset? ReadDate { get; init; }
        public long? Size { get; init; }
    }
}
