// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial class HttpBroker : IHttpBroker
    {
        private readonly HttpClient httpClient;

        public HttpBroker(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}