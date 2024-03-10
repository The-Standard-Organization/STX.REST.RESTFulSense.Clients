// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace STX.REST.RESTFulSense.Clients.Brokers.Https
{
    internal partial class HttpBroker : HttpClient, IHttpBroker
    {
        private readonly HttpClient httpClient;

        public HttpBroker() { }

        public HttpBroker(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private static async ValueTask<T> DeserializeResponseContent<T>(
           HttpResponseMessage responseMessage,
           Func<string, ValueTask<T>> deserializationFunction = null)
        {
            string responseString =
                await responseMessage.Content.ReadAsStringAsync();

            return deserializationFunction == null
                ? JsonConvert.DeserializeObject<T>(responseString)
                : await deserializationFunction(responseString);
        }
    }
}