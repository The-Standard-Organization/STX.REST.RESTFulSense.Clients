// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static AuthenticationHeaderValue MapToAuthenticationHeaderValue(
            AuthenticationHeader authenticationHeader)
        {
            if (authenticationHeader is null)
                return null;

            return new AuthenticationHeaderValue(
                authenticationHeader.Schema,
                authenticationHeader.Value);
        }
    }
}