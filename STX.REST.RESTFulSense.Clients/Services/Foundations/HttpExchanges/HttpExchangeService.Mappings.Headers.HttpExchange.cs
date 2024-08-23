// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Threading.Tasks;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static NameValueHeader MapToNameValueHeader(
            NameValueHeaderValue nameValueHeaderValue)
        {
            if (nameValueHeaderValue is null)
                return null;

            return new NameValueHeader
            {
                Name = nameValueHeaderValue.Name,
                Value = nameValueHeaderValue.Value
            };
        }

        private static async ValueTask<TransferCodingHeader> MapToTransferCodingHeader(
            TransferCodingHeaderValue transferCodingHeaderValue)
        {
            if (transferCodingHeaderValue is null)
                return null;

            return new TransferCodingHeader
            {
                Value = transferCodingHeaderValue.Value,
                Parameters =
                    await MapArrayAsync(
                        transferCodingHeaderValue.Parameters,
                        MapToNameValueHeader)
            };
        }

        private static RetryConditionHeader MapToRetryConditionHeader(
            RetryConditionHeaderValue retryConditionHeaderValue)
        {
            if (retryConditionHeaderValue is null)
                return null;

            return new RetryConditionHeader
            {
                Date = retryConditionHeaderValue.Date,
                Delta = retryConditionHeaderValue.Delta
            };
        }

        private static WarningHeader MapToWarningHeader(
            WarningHeaderValue warningHeaderValue)
        {
            if (warningHeaderValue is null)
                return null;

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
            if (viaHeaderValue is null)
                return null;

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
            if (authenticationHeaderValue is null)
                return null;

            return new AuthenticationHeader
            {
                Schema = authenticationHeaderValue.Scheme,
                Value = authenticationHeaderValue.Parameter
            };
        }

        private static ProductHeader MapToProductHeader(
            ProductHeaderValue productHeaderValue)
        {
            if (productHeaderValue is null)
                return null;

            return new ProductHeader
            {
                Name = productHeaderValue.Name,
                Version = productHeaderValue.Version
            };
        }

        private static ProductInfoHeader MapToProductInfoHeader(
           ProductInfoHeaderValue productInfoHeaderValue)
        {
            if (productInfoHeaderValue is null)
                return null;

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
            if (cacheControlHeaderValue is null)
                return null;

            return new CacheControlHeader
            {
                NoCache = cacheControlHeaderValue.NoCache,

                NoCacheHeaders =
                    MapArray(
                        cacheControlHeaderValue.NoCacheHeaders,
                        @string => @string),

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

                PrivateHeaders =
                    MapArray(
                        cacheControlHeaderValue.PrivateHeaders,
                        @string => @string),

                MustRevalidate = cacheControlHeaderValue.MustRevalidate,
                ProxyRevalidate = cacheControlHeaderValue.ProxyRevalidate,

                Extensions =
                    MapArray(
                        cacheControlHeaderValue.Extensions,
                        MapToNameValueHeader)
            };
        }

        private static MediaTypeHeader MapToMediaTypeHeader(
            MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            if (mediaTypeHeaderValue is null)
                return null;

            return new MediaTypeHeader
            {
                CharSet = mediaTypeHeaderValue.CharSet,
                MediaType = mediaTypeHeaderValue.MediaType,
            };
        }

        private static ContentRangeHeader MapToContentRangeHeader(
            ContentRangeHeaderValue contentRangeHeaderValue)
        {
            if (contentRangeHeaderValue is null)
                return null;

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
            if (contentDispositionHeaderValue is null)
                return null;

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
