// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges
{
    public class HttpExchangeContent
    {
        public HttpExchangeContentHeaders Headers { get; init; }
        public ValueTask<Stream> StreamContent { get; init; }
    }
}
