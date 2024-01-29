// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using STX.REST.RESTFulSense.Clients.Models.Brokers.StatusDetails;

namespace STX.REST.RESTFulSense.Clients.Brokers.ErrorBrokers
{
    internal partial class ErrorBroker : IErrorBroker
    {
        public ErrorBroker() =>
            statusDetails = InitialiseStatusCodes();

        private static IQueryable<StatusDetail> InitialiseStatusCodes()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data\\StatusCodes.json");
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<StatusDetail>>(json).AsQueryable();
        }
    }
}
