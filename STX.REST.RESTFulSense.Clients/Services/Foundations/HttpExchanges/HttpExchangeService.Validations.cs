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
        private static void ValidateHttpExchange(
            HttpExchange httpExchange,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion,
            HttpVersionPolicy defaultHttpVersionPolicy)
        {
            ValidateHttpExchangeNotNull(httpExchange);
            ValidateHttpExchangeRequestNotNull(httpExchange.Request);
            ValidateHttpExchangeRequestNotInvalid(
                httpExchange.Request,
                defaultHttpMethod,
                defaultHttpVersion);
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
            Validate(
                (Rule: IsInvalid(httpExchangeRequest), Parameter: nameof(HttpExchange.Request)));
        }

        private static void ValidateHttpExchangeRequestNotInvalid(
            HttpExchangeRequest httpExchangeRequest,
            HttpMethod defaultHttpMethod,
            Version defaultHttpVersion)
        {
            Validate(
                (Rule: IsInvalid(httpExchangeRequest.BaseAddress),
                    Parameter: nameof(HttpExchangeRequest.BaseAddress)),

                (Rule: IsInvalid(httpExchangeRequest.RelativeUrl),
                    Parameter: nameof(HttpExchangeRequest.RelativeUrl)),

                (Rule: IsInvalidHttpMethod(httpExchangeRequest.HttpMethod, defaultHttpMethod),
                    Parameter: nameof(HttpExchangeRequest.HttpMethod)),

                (Rule: IsInvalidHttpVersion(httpExchangeRequest.Version, defaultHttpVersion),
                    Parameter: nameof(HttpExchangeRequest.Version)));
        }

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Value is required"
        };

        private static dynamic IsInvalidHttpMethod(string customHttpMethod, HttpMethod defaultHttpMethod) => new
        {
            Condition =
                (defaultHttpMethod is null
                    && string.IsNullOrWhiteSpace(customHttpMethod))
                || (defaultHttpMethod is not null
                    && !string.IsNullOrWhiteSpace(customHttpMethod)
                    && customHttpMethod != defaultHttpMethod.Method),

            Message = "HttpMethod required is invalid"
        };

        private static dynamic IsInvalidHttpVersion(string customHttpVersion, Version defaultHttpVersion) => new
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

            Message = "HttpVersion required is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidHttpExchangeRequestException =
                new InvalidHttpExchangeRequestException(
                    message: "Invalid HttpExchange request error occurred, fix errors and try again.");

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