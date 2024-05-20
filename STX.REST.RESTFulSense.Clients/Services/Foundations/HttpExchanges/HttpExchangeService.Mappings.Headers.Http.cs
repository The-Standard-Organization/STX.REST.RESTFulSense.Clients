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
                mediaTypeHeader.MediaType,
                mediaTypeHeader.Quality ?? default);
        }
        
        private static StringWithQualityHeaderValue MapToStringWithQualityHeaderValue(
            StringWithQualityHeader stringWithQualityHeader)
        {
            if (stringWithQualityHeader is null)
                return null;
            
            return new StringWithQualityHeaderValue(
                stringWithQualityHeader.Value,
                stringWithQualityHeader.Quality ?? default);
        }
        
        private static RangeConditionHeaderValue MapToRangeConditionHeaderValue(
            RangeConditionHeader rangeConditionHeader)
        {
            if (rangeConditionHeader is null)
                return null;
            
            return
                rangeConditionHeader.Date is not null
                    ? new RangeConditionHeaderValue((DateTimeOffset)rangeConditionHeader.Date)
                    : new RangeConditionHeaderValue(rangeConditionHeader.EntityTag);
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

        private static RangeItemHeaderValue MapToRangeItemHeaderValue(
            RangeItemHeader rangeItemHeader)
        {
            if (rangeItemHeader is null)
                return null;
            
            return new RangeItemHeaderValue(
                rangeItemHeader.From, rangeItemHeader.To);
        }

        private static TransferCodingWithQualityHeaderValue MapToTransferCodingWithQualityHeaderValue(
            TransferCodingHeader transferCodingHeader)
        {
            if (transferCodingHeader is null)
                return null;

            return new TransferCodingWithQualityHeaderValue(
                transferCodingHeader.Value,
                transferCodingHeader.Quality ?? default);
        }
        
        private static ProductInfoHeaderValue MapToProductInfoHeaderValue(
            ProductInfoHeader productInfoHeader)
        {
            if (productInfoHeader is null)
                return null;
            
            return  string.IsNullOrEmpty(productInfoHeader.Comment)
                ? new ProductInfoHeaderValue(productInfoHeader.Product.Name,
                    productInfoHeader.Product.Version)
                : new ProductInfoHeaderValue(productInfoHeader.Comment);
        }
        
        private static NameValueWithParametersHeaderValue MapToNameValueWithParametersHeaderValue(
            NameValueHeader nameValueHeader)
        {
            if (nameValueHeader is null)
                return null;

            return new NameValueWithParametersHeaderValue(
                nameValueHeader.Name,
                nameValueHeader.Value);
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
            cacheControlHeader.NoCacheHeaders.Select(header =>
            {
                cacheControlHeaderValue.NoCacheHeaders.Add(header);

                return header;
            }).ToArray();
            
            cacheControlHeader.PrivateHeaders.Select(header =>
            {
                cacheControlHeaderValue.PrivateHeaders.Add(header);

                return header;
            }).ToArray();
            
            cacheControlHeader.Extensions.Select(header =>
            {
                var nameValueHeaderValue =MapToNameValueHeaderValue(header);
                cacheControlHeaderValue.Extensions.Add(nameValueHeaderValue);

                return header;
            }).ToArray();

            return cacheControlHeaderValue;
        }

        private static NameValueHeaderValue MapToNameValueHeaderValue(
            NameValueHeader nameValueHeader)
        {
            if (nameValueHeader is null)
                return null;

            return new NameValueHeaderValue(
                nameValueHeader.Name,
                nameValueHeader.Value);
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

        private static ProductHeaderValue MapToProductHeaderValue(ProductHeader productHeader)
        {
            if (productHeader is null)
                return null;

            return new ProductHeaderValue(
                productHeader.Name,
                productHeader.Version);
        }
        
        private static ViaHeaderValue MapToViaHeaderValue(ViaHeader viaHeader)
        {
            if (viaHeader is null)
                return null;

            return new ViaHeaderValue(
                viaHeader.ProtocolName,
                viaHeader.ProtocolVersion,
                viaHeader.ReceivedBy,
                viaHeader.Comment);
        }
        
        private static WarningHeaderValue MapToWarningHeaderValue(WarningHeader warningHeader)
        {
            if (warningHeader is null)
                return null;

            return new WarningHeaderValue(
                warningHeader.Code,
                warningHeader.Agent,
                warningHeader.Text,
                warningHeader.Date ?? default);
        }
    }
}