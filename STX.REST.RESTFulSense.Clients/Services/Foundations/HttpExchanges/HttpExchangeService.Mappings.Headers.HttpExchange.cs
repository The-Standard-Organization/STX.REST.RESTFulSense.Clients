﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Net.Http.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static NameValueHeader MapToNameValueHeader(
            NameValueHeaderValue nameValueHeaderValue)
        {
            return new NameValueHeader
            {
                Name = nameValueHeaderValue.Name,
                Value = nameValueHeaderValue.Value
            };
        }

        private static TransferCodingHeader MapToTransferCodingHeader(
            TransferCodingHeaderValue transferCodingHeaderValue)
        {
            return new TransferCodingHeader
            {
                Value = transferCodingHeaderValue.Value,
                Parameters =
                    MapArray(
                        transferCodingHeaderValue.Parameters,
                        MapToNameValueHeader)
            };
        }

        private static RetryConditionHeader MapToRetryConditionHeader(
            RetryConditionHeaderValue retryConditionHeaderValue)
        {
            return new RetryConditionHeader
            {
                Date = retryConditionHeaderValue.Date,
                Delta = retryConditionHeaderValue.Delta
            };
        }

        private static WarningHeader MapToWarningHeader(
            WarningHeaderValue warningHeaderValue)
        {
            return new WarningHeader
            {
                Code = warningHeaderValue.Code,
                Agent = warningHeaderValue.Agent,
                Text = warningHeaderValue.Text,
                Date = warningHeaderValue.Date
            };
        }

        private static ViaHeader MapToViaHeader(
            ViaHeaderValue viaHeaderValue)
        {
            return new ViaHeader
            {
                ProtocolName = viaHeaderValue.ProtocolName,
                ProtocolVersion = viaHeaderValue.ProtocolVersion,
                ReceivedBy = viaHeaderValue.ReceivedBy,
                Comment = viaHeaderValue.Comment
            };
        }

        private static AuthenticationHeader MapToAuthenticationHeader(
            AuthenticationHeaderValue authenticationHeaderValue)
        {
            return new AuthenticationHeader
            {
                Schema = authenticationHeaderValue.Scheme,
                Value = authenticationHeaderValue.Parameter
            };
        }

        private static ProductHeader MapToProductHeader(
            ProductHeaderValue productHeaderValue)
        {
            return new ProductHeader
            {
                Name = productHeaderValue.Name,
                Version = productHeaderValue.Version
            };
        }

        private static ProductInfoHeader MapToProductInfoHeader(
           ProductInfoHeaderValue productInfoHeaderValue)
        {
            ProductHeader productHeader = null;
            if (productInfoHeaderValue.Product is not null)
            {
                productHeader =
                    MapToProductHeader(
                        productInfoHeaderValue.Product);
            }

            return new ProductInfoHeader
            {
                Comment = productInfoHeaderValue.Comment,
                Product = productHeader
            };
        }

        private static CacheControlHeader MapToCacheControlHeader(
            CacheControlHeaderValue cacheControlHeaderValue)
        {
            return new CacheControlHeader
            {
                NoCache = cacheControlHeaderValue.NoCache,
                NoCacheHeaders = cacheControlHeaderValue.NoCacheHeaders.ToArray(),
                NoStore = cacheControlHeaderValue.NoStore,
                MaxAge = cacheControlHeaderValue.MaxAge,
                SharedMaxAge = cacheControlHeaderValue.SharedMaxAge,
                MaxStale = cacheControlHeaderValue.MaxStale,
                MaxStaleLimit = cacheControlHeaderValue.MaxStaleLimit,
                MinFresh = cacheControlHeaderValue.MinFresh,
                NoTransform = cacheControlHeaderValue.NoTransform,
                OnlyIfCached = cacheControlHeaderValue.OnlyIfCached,
                Public = cacheControlHeaderValue.Public,
                Private = cacheControlHeaderValue.Private,
                PrivateHeaders = cacheControlHeaderValue.PrivateHeaders.ToArray(),
                MustRevalidate = cacheControlHeaderValue.MustRevalidate,
                ProxyRevalidate = cacheControlHeaderValue.ProxyRevalidate,

                Extensions =
                    MapArray(cacheControlHeaderValue.Extensions,
                        MapToNameValueHeader)
            };
        }

        private static MediaTypeHeader MapToMediaTypeHeader(
            MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            return new MediaTypeHeader
            {
                CharSet = mediaTypeHeaderValue.CharSet,
                MediaType = mediaTypeHeaderValue.MediaType,
            };
        }

        private static ContentRangeHeader MapToContentRangeHeader(
            ContentRangeHeaderValue contentRangeHeaderValue)
        {
            return new ContentRangeHeader
            {
                Unit = contentRangeHeaderValue.Unit,
                From = contentRangeHeaderValue.From,
                To = contentRangeHeaderValue.To,
                Length = contentRangeHeaderValue.Length
            };
        }

        private static ContentDispositionHeader MapToContentDispositionHeader(
            ContentDispositionHeaderValue contentDispositionHeaderValue)
        {
            return new ContentDispositionHeader
            {
                DispositionType = contentDispositionHeaderValue.DispositionType,
                Name = contentDispositionHeaderValue.Name,
                FileName = contentDispositionHeaderValue.FileName,
                FileNameStar = contentDispositionHeaderValue.FileNameStar,
                CreationDate = contentDispositionHeaderValue.CreationDate,
                ModificationDate = contentDispositionHeaderValue.ModificationDate,
                ReadDate = contentDispositionHeaderValue.ReadDate,
                Size = contentDispositionHeaderValue.Size
            };
        }
    }
}
