// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static NameValueHeaderValue MapToNameValueHeaderValue(
            NameValueHeader nameValueHeader)
        {
            if (nameValueHeader is null)
                return null;

            return new NameValueHeaderValue(
                nameValueHeader.Name,
                nameValueHeader.Value);
        }

        private static AuthenticationHeaderValue MapToAuthenticationHeaderValue(
            AuthenticationHeader authenticationHeader)
        {
            if (authenticationHeader is null)
                return null;

            return new AuthenticationHeaderValue(
                authenticationHeader.Schema,
                authenticationHeader.Value);
        }

        private static MediaTypeWithQualityHeaderValue MapToMediaTypeWithQualityHeaderValue(
            MediaTypeHeader mediaTypeHeader)
        {
            if (mediaTypeHeader is null)
                return null;

            return new MediaTypeWithQualityHeaderValue(
                mediaTypeHeader.MediaType)
            {
                Quality = mediaTypeHeader.Quality,
                CharSet = mediaTypeHeader.CharSet
            };
        }

        private static MediaTypeHeaderValue MapToMediaTypeHeaderValue(
            MediaTypeHeader mediaTypeHeader)
        {
            if (mediaTypeHeader is null)
                return null;

            return new MediaTypeWithQualityHeaderValue(
                mediaTypeHeader.MediaType)
            {
                CharSet = mediaTypeHeader.CharSet
            };
        }

        private static StringWithQualityHeaderValue MapToStringWithQualityHeaderValue(
            StringWithQualityHeader stringWithQualityHeader)
        {
            if (stringWithQualityHeader is null)
                return null;

            return new StringWithQualityHeaderValue(
                stringWithQualityHeader.Value,
                stringWithQualityHeader.Quality.Value);
        }

        private static StringWithQualityHeaderValue MapToStringHeaderValue(
            StringWithQualityHeader stringWithQualityHeader)
        {
            if (stringWithQualityHeader is null)
                return null;

            return new StringWithQualityHeaderValue(
                stringWithQualityHeader.Value);
        }

        private static RangeConditionHeaderValue MapToRangeConditionHeaderValue(
            RangeConditionHeader rangeConditionHeader)
        {
            if (rangeConditionHeader is null)
                return null;

            return
                rangeConditionHeader.Date is not null
                    ? new RangeConditionHeaderValue(rangeConditionHeader.Date.Value)
                    : new RangeConditionHeaderValue(rangeConditionHeader.EntityTag);
        }

        private static RangeItemHeaderValue MapToRangeItemHeaderValue(
                    RangeItemHeader rangeItemHeader)
        {
            if (rangeItemHeader is null)
                return null;

            return new RangeItemHeaderValue(
                rangeItemHeader.From, rangeItemHeader.To);
        }

        private static RangeHeaderValue MapToRangeHeaderValue(
            RangeHeader rangeHeader)
        {
            if (rangeHeader is null)
                return null;

            var rangeHeaderValue = new RangeHeaderValue
            {
                Unit = rangeHeader.Unit,
            };

            rangeHeader.Ranges.Select(header =>
            {
                var rangeItemHeaderValue = MapToRangeItemHeaderValue(header);
                rangeHeaderValue.Ranges.Add(rangeItemHeaderValue);

                return header;
            }).ToArray();

            return rangeHeaderValue;
        }

        private static TransferCodingWithQualityHeaderValue MapToTransferCodingWithQualityHeaderValue(
            TransferCodingHeader transferCodingHeader)
        {
            if (transferCodingHeader is null)
                return null;

            TransferCodingWithQualityHeaderValue transferCodingWithQualityHeaderValue =
                new TransferCodingWithQualityHeaderValue(
                    transferCodingHeader.Value,
                    transferCodingHeader.Quality.Value);

            transferCodingHeader.Parameters.Select(header =>
            {
                var nameValueHeaderValue = MapToNameValueHeaderValue(header);
                transferCodingWithQualityHeaderValue.Parameters.Add(nameValueHeaderValue);

                return header;
            }).ToArray();

            return transferCodingWithQualityHeaderValue;
        }

        private static TransferCodingHeaderValue MapToTransferCodingHeaderValue(
            TransferCodingHeader transferCodingHeader)
        {
            if (transferCodingHeader is null)
                return null;

            var transferCodingHeaderValue =
                new TransferCodingHeaderValue(transferCodingHeader.Value);

            transferCodingHeader.Parameters.Select(header =>
            {
                var nameValueHeaderValue = MapToNameValueHeaderValue(header);
                transferCodingHeaderValue.Parameters.Add(nameValueHeaderValue);

                return header;
            }).ToArray();

            return transferCodingHeaderValue;
        }

        private static ProductHeaderValue MapToProductHeaderValue(
            ProductHeader productHeader)
        {
            if (productHeader is null)
                return null;

            return new ProductHeaderValue(
                name: productHeader.Name,
                version: productHeader.Version);
        }

        private static ProductInfoHeaderValue MapToProductInfoHeaderValue(
            ProductInfoHeader productInfoHeader)
        {
            if (productInfoHeader is null)
                return null;

            return String.IsNullOrEmpty(productInfoHeader.Comment)
                ? new ProductInfoHeaderValue(MapToProductHeaderValue(productInfoHeader.Product))
                : new ProductInfoHeaderValue(productInfoHeader.Comment);
        }

        private static NameValueWithParametersHeaderValue MapToNameValueWithParametersHeaderValue(
            NameValueWithParametersHeader nameValueWithParameters)
        {
            if (nameValueWithParameters is null)
                return null;

            NameValueWithParametersHeaderValue nameValueWithParametersHeaderValue =
                new NameValueWithParametersHeaderValue(
                    nameValueWithParameters.Name);

            nameValueWithParameters.Parameters.Select(header =>
            {
                var nameValueHeaderValue = MapToNameValueHeaderValue(header);
                nameValueWithParametersHeaderValue.Parameters.Add(nameValueHeaderValue);

                return header;
            }).ToArray();

            return nameValueWithParametersHeaderValue;
        }

        private static CacheControlHeaderValue MapToCacheControlHeaderValue(
            CacheControlHeader cacheControlHeader)
        {
            if (cacheControlHeader is null)
                return null;

            var cacheControlHeaderValue = new CacheControlHeaderValue
            {
                NoCache = cacheControlHeader.NoCache,
                NoStore = cacheControlHeader.NoStore,
                MaxAge = cacheControlHeader.MaxAge,
                SharedMaxAge = cacheControlHeader.SharedMaxAge,
                MaxStale = cacheControlHeader.MaxStale,
                MaxStaleLimit = cacheControlHeader.MaxStaleLimit,
                MinFresh = cacheControlHeader.MinFresh,
                NoTransform = cacheControlHeader.NoTransform,
                OnlyIfCached = cacheControlHeader.OnlyIfCached,
                Public = cacheControlHeader.Public,
                Private = cacheControlHeader.Private,
                MustRevalidate = cacheControlHeader.MustRevalidate,
                ProxyRevalidate = cacheControlHeader.ProxyRevalidate,
            };

            if (cacheControlHeader.NoCacheHeaders is not null)
            {
                cacheControlHeader.NoCacheHeaders.Select(header =>
                {
                    cacheControlHeaderValue.NoCacheHeaders.Add(header);

                    return header;
                }).ToArray();
            }

            if (cacheControlHeader.PrivateHeaders is not null)
            {
                cacheControlHeader.PrivateHeaders.Select(header =>
                {
                    cacheControlHeaderValue.PrivateHeaders.Add(header);

                    return header;
                }).ToArray();
            }

            if (cacheControlHeader.Extensions is not null)
            {
                cacheControlHeader.Extensions.Select(header =>
                {
                    var nameValueHeaderValue = MapToNameValueHeaderValue(header);
                    cacheControlHeaderValue.Extensions.Add(nameValueHeaderValue);

                    return header;
                }).ToArray();
            }

            return cacheControlHeaderValue;
        }

        private static ViaHeaderValue MapToViaHeaderValue(ViaHeader viaHeader)
        {
            if (viaHeader is null)
                return null;

            return new ViaHeaderValue(
                protocolVersion: viaHeader.ProtocolVersion,
                receivedBy: viaHeader.ReceivedBy,
                protocolName: viaHeader.ProtocolName,
                comment: viaHeader.Comment);
        }

        private static WarningHeaderValue MapToWarningHeaderValue(WarningHeader warningHeader)
        {
            if (warningHeader is null)
                return null;

            return new WarningHeaderValue(
                code: warningHeader.Code,
                agent: warningHeader.Agent,
                text: warningHeader.Text,
                date: warningHeader.Date.Value);
        }
    }
}