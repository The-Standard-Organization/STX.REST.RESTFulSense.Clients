// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

namespace STX.REST.RESTFulSense.Clients.Models.Brokers.Errors
{
    internal class StatusDetail
    {
        public int Code { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
