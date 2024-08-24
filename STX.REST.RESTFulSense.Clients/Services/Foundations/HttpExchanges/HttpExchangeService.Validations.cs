// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static async ValueTask ValidateOnGetAsync(
            HttpExchange httpExchange,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion,
            HttpVersionPolicy defaultHttpVersionPolicy)
        {
            await ValidateAsync(
                (Rule: await IsInvalidAsync(httpExchange), Parameter: nameof(HttpExchange)),
                (Rule: await IsInvalidAsync(defaultHttpMethod), Parameter: nameof(HttpMethod)),
                (Rule: await IsInvalidAsync(defaultHttpVersion), Parameter: nameof(Version)),
                (Rule: await IsInvalidAsync(defaultHttpVersionPolicy), Parameter: nameof(HttpVersionPolicy)));

            await ValidateHttpExchangeAsync(
                httpExchange,
                defaultHttpMethod,
                defaultHttpVersion,
                defaultHttpVersionPolicy);
        }

        private static async ValueTask ValidateHttpExchangeAsync(
            HttpExchange httpExchange,
            HttpMethod httpMethod,
            Version httpVersion,
            HttpVersionPolicy httpVersionPolicy)
        {
            await ValidateHttpExchangeRequestAsync(
                httpExchangeRequest: httpExchange.Request,
                httpMethod,
                httpVersion,
                httpVersionPolicy);
        }

        private static async ValueTask ValidateHttpExchangeRequestAsync(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod httpMethod,
            Version httpVersion,
            HttpVersionPolicy httpVersionPolicy)
        {
            await ValidateHttpExchangeRequestNotNullAsync(httpExchangeRequest);

            await ValidateHttpExchangeRequestObjectAsync(
                (Rule: await IsInvalidAsync(httpExchangeRequest.BaseAddress),
                    Parameter: nameof(HttpExchangeRequest.BaseAddress)),

                (Rule: await IsInvalidAsync(httpExchangeRequest.RelativeUrl),
                    Parameter: nameof(HttpExchangeRequest.RelativeUrl)),

                (Rule: await IsInvalidHttpMethodAsync(httpExchangeRequest.HttpMethod, httpMethod),
                    Parameter: nameof(HttpExchangeRequest.HttpMethod)),

                (Rule: await IsInvalidHttpVersionAsync(httpExchangeRequest.Version, httpVersion),
                    Parameter: nameof(HttpExchangeRequest.Version)),

                (Rule: await IsInvalidHttpVersionPolicyAsync(httpExchangeRequest.VersionPolicy, httpVersionPolicy),
                    Parameter: nameof(HttpExchangeRequest.VersionPolicy)));

            await ValidateHttpExchangeRequestHeadersAsync(httpExchangeRequestHeaders: httpExchangeRequest.Headers);
        }

        private static async ValueTask ValidateHttpExchangeRequestNotNullAsync(
            HttpExchangeRequest httpExchangeRequest)
        {
            if (httpExchangeRequest is null)
            {
                throw new NullHttpExchangeRequestException(
                    message: "Null HttpExchangeRequest error occurred, fix errors and try again.");
            }
        }

        private static async ValueTask<dynamic> IsInvalidAsync(object @object) =>
            new
            {
                Condition = @object is null,
                Message = "Value is required"
            };

        private static async ValueTask<dynamic> IsInvalidAsync(string text) =>
            new
            {
                Condition = String.IsNullOrWhiteSpace(text),
                Message = "Value is required"
            };

        private static async ValueTask<dynamic> IsInvalidHttpMethodAsync(
            string customHttpMethod, HttpMethod defaultHttpMethod) =>
            new
            {
                Condition =
                    (defaultHttpMethod is null
                        && String.IsNullOrWhiteSpace(customHttpMethod))
                    || (defaultHttpMethod is not null
                        && !String.IsNullOrWhiteSpace(customHttpMethod)
                        && customHttpMethod != defaultHttpMethod.Method),

                Message = "HttpMethod is invalid"
            };

        private static async ValueTask<dynamic> IsInvalidHttpVersionAsync(
            string customHttpVersion, Version defaultHttpVersion) =>
            new
            {
                Condition =
                    (customHttpVersion is not null
                        && customHttpVersion != HttpVersion.Version10.ToString()
                        && customHttpVersion != HttpVersion.Version11.ToString()
                        && customHttpVersion != HttpVersion.Version20.ToString()
                        && customHttpVersion != HttpVersion.Version30.ToString())
                    || (customHttpVersion is null
                        && defaultHttpVersion != HttpVersion.Version10
                        && defaultHttpVersion != HttpVersion.Version11
                        && defaultHttpVersion != HttpVersion.Version20
                        && defaultHttpVersion != HttpVersion.Version30),

                Message = "HttpVersion is invalid"
            };

        private static async ValueTask<dynamic> IsInvalidHttpVersionPolicyAsync(
            int? customHttpVersionPolicy,
            HttpVersionPolicy defaultHttpVersionPolicy) =>
            new
            {
                Condition =
                    (customHttpVersionPolicy is not null
                        && customHttpVersionPolicy != (int)HttpVersionPolicy.RequestVersionOrLower
                        && customHttpVersionPolicy != (int)HttpVersionPolicy.RequestVersionOrHigher
                        && customHttpVersionPolicy != (int)HttpVersionPolicy.RequestVersionExact)
                    || (customHttpVersionPolicy is null
                        && defaultHttpVersionPolicy != HttpVersionPolicy.RequestVersionOrLower
                        && defaultHttpVersionPolicy != HttpVersionPolicy.RequestVersionOrHigher
                        && defaultHttpVersionPolicy != HttpVersionPolicy.RequestVersionExact),

                Message = "HttpVersionPolicy is invalid"
            };

        private static async ValueTask ValidateAsync(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHttpExchangeException =
                new InvalidHttpExchangeException(
                    message: "Invalid HttpExchange error occurred, fix errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHttpExchangeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHttpExchangeException.ThrowIfContainsErrors();
        }

        private static async ValueTask ValidateHttpExchangeRequestObjectAsync(
            params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHttpExchangeRequestException =
                new InvalidHttpExchangeRequestException(
                    message: "Invalid request, fix errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidHttpExchangeRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidHttpExchangeRequestException.ThrowIfContainsErrors();
        }
    }
}