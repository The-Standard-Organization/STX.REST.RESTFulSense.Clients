﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    public class HttpExchangeContent
    {
        public HttpExchangeContentHeaders Headers { get; init; }
        public Stream StreamContent { get; set; }
    }
}
