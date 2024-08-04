// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Exceptions;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static void ValidateOnGet(
            HttpExchange httpExchange,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion,
            HttpVersionPolicy defaultHttpVersionPolicy)
        {
            Validate(
                (Rule: IsInvalid(httpExchange), Parameter: nameof(HttpExchange)),
                (Rule: IsInvalid(defaultHttpMethod), Parameter: nameof(HttpMethod)),
                (Rule: IsInvalid(defaultHttpVersion), Parameter: nameof(Version)),
                (Rule: IsInvalid(defaultHttpVersionPolicy), Parameter: nameof(HttpVersionPolicy)));

            ValidateHttpExchange(
                httpExchange,
                defaultHttpMethod,
                defaultHttpVersion,
                defaultHttpVersionPolicy);
        }

        private static void ValidateHttpExchange(
            HttpExchange httpExchange,
            HttpMethod httpMethod,
            Version httpVersion,
            HttpVersionPolicy httpVersionPolicy)
        {
            ValidateHttpExchangeRequest(
                httpExchangeRequest: httpExchange.Request,
                httpMethod,
                httpVersion,
                httpVersionPolicy);
        }

        private static void ValidateHttpExchangeRequest(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod httpMethod,
            Version httpVersion,
            HttpVersionPolicy httpVersionPolicy)
        {
            ValidateHttpExchangeRequestNotNull(httpExchangeRequest);

            ValidateHttpExchangeRequestObject(
                (Rule: IsInvalid(httpExchangeRequest.BaseAddress),
                    Parameter: nameof(HttpExchangeRequest.BaseAddress)),

                (Rule: IsInvalid(httpExchangeRequest.RelativeUrl),
                    Parameter: nameof(HttpExchangeRequest.RelativeUrl)),

                (Rule: IsInvalidHttpMethod(httpExchangeRequest.HttpMethod, httpMethod),
                    Parameter: nameof(HttpExchangeRequest.HttpMethod)),

                (Rule: IsInvalidHttpVersion(httpExchangeRequest.Version, httpVersion),
                    Parameter: nameof(HttpExchangeRequest.Version)),

                (Rule: IsInvalidHttpVersionPolicy(httpExchangeRequest.VersionPolicy, httpVersionPolicy),
                    Parameter: nameof(HttpExchangeRequest.VersionPolicy)));

            ValidateHttpExchangeRequestHeaders(httpExchangeRequestHeaders: httpExchangeRequest.Headers);
        }

        private static void ValidateHttpExchangeRequestNotNull(HttpExchangeRequest httpExchangeRequest)
        {
            if (httpExchangeRequest is null)
            {
                throw new NullHttpExchangeRequestException(
                    message: "Null HttpExchangeRequest error occurred, fix errors and try again.");
            }
        }

        private static dynamic IsInvalid(object @object) =>
            new
            {
                Condition = @object is null,
                Message = "Value is required"
            };

        private static dynamic IsInvalid(string text) =>
            new
            {
                Condition = String.IsNullOrWhiteSpace(text),
                Message = "Value is required"
            };

        private static dynamic IsInvalidHttpMethod(string customHttpMethod, HttpMethod defaultHttpMethod) =>
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

        private static dynamic IsInvalidHttpVersion(string customHttpVersion, Version defaultHttpVersion) =>
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

        private static dynamic IsInvalidHttpVersionPolicy(
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
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

        private static void ValidateHttpExchangeRequestObject(params (dynamic Rule, string Parameter)[] validations)
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