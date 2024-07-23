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
                (Rule: IsInvalid(defaultHttpVersion), Parameter: nameof(Version)));

            ValidateHttpExchange(httpExchange, defaultHttpMethod, defaultHttpVersion);
        }

        private static void ValidateHttpExchange(HttpExchange httpExchange, HttpMethod httpMethod, Version httpVersion)
        {
            ValidateHttpExchangeRequest(httpExchangeRequest: httpExchange.Request, httpMethod, httpVersion);
        }

        private static void ValidateHttpExchangeRequest(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod httpMethod,
            Version httpVersion)
        {
            ValidateHttpExchangeRequestNotNull(httpExchangeRequest);

            ValidateInput(
                (Rule: IsInvalid(httpExchangeRequest.BaseAddress),
                    Parameter: nameof(HttpExchangeRequest.BaseAddress)),
                (Rule: IsInvalid(httpExchangeRequest.RelativeUrl),
                    Parameter: nameof(HttpExchangeRequest.RelativeUrl)),
                (Rule: IsInvalidHttpMethod(httpExchangeRequest.HttpMethod, httpMethod),
                    Parameter: nameof(HttpExchangeRequest.HttpMethod)),
                (Rule: IsInvalidHttpVersion(httpExchangeRequest.Version, httpVersion),
                    Parameter: nameof(HttpExchangeRequest.Version)));

            ValidateHttpExchangeRequestHeaders(httpExchangeRequestHeaders: httpExchangeRequest.Headers);
        }

        private static void ValidateHttpMethod(HttpExchangeRequest httpExchangeRequest, HttpMethod httpMethod)
        {
            ValidateInput((
                Rule: IsInvalidHttpMethod(httpExchangeRequest.HttpMethod, httpMethod),
                Parameter: nameof(HttpExchangeRequest.HttpMethod)));
        }

        private static void ValidateVersion(HttpExchangeRequest httpExchangeRequest, Version httpVersion)
        {
            ValidateInput((
                Rule: IsInvalidHttpVersion(httpExchangeRequest.Version, httpVersion),
                    Parameter: nameof(HttpExchangeRequest.Version)));
        }

        private static void ValidateHttpExchangeNotNull(HttpExchange httpExchange)
        {
            if (httpExchange is null)
            {
                throw new NullHttpExchangeException(
                    message: "Null HttpExchange error occurred, fix errors and try again.");
            }
        }

        private static void ValidateHttpExchangeRequestNotNull(HttpExchangeRequest httpExchangeRequest)
        {
            ValidateInput(
                (Rule: IsInvalid(httpExchangeRequest), Parameter: nameof(HttpExchange.Request)));
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
                Condition = string.IsNullOrWhiteSpace(text),
                Message = "Value is required"
            };

        private static dynamic IsInvalidHttpMethod(string customHttpMethod, HttpMethod defaultHttpMethod) =>
            new
            {
                Condition =
                    (defaultHttpMethod is null
                        && string.IsNullOrWhiteSpace(customHttpMethod))
                    || (defaultHttpMethod is not null
                        && !string.IsNullOrWhiteSpace(customHttpMethod)
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

        private static void ValidateInput(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHttpExchangeRequestException =
                new InvalidArgumentHttpExchangeException(
                    message: "Invalid argument, fix errors and try again.");

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