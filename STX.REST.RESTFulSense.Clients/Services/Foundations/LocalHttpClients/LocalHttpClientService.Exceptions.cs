// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.LocalHttpClients
{
    internal partial class LocalHttpClientService
    {
        private delegate ValueTask<LocalHttpClient> ReturningLocalHttpClientFunction();

        private static async ValueTask<LocalHttpClient> TryCatch(
            ReturningLocalHttpClientFunction returningLocalHttpClientFunction)
        {
            try
            {
                return await returningLocalHttpClientFunction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}