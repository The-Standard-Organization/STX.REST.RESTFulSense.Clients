// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Net.Http.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;

namespace STX.REST.RESTFulSense.Clients.Services.Foundations.HttpExchanges
{
    internal partial class HttpExchangeService
    {
        private static HttpExchangeContentHeaders CreateHttpExchangeContentHeaders(
            HttpContentHeaders httpContentHeaders)
        {
            return new HttpExchangeContentHeaders
            {
                Allow = httpContentHeaders.Allow.ToArray(),
                
                ContentDisposition =
                    MapToContentDispositionHeader(
                        httpContentHeaders.ContentDisposition),

                ContentEncoding =
                    httpContentHeaders.ContentEncoding.ToArray(),

                ContentLanguage = httpContentHeaders.ContentLanguage.ToArray(),
                ContentLength = httpContentHeaders.ContentLength,
                ContentLocation = httpContentHeaders.ContentLocation,
                ContentMD5 = httpContentHeaders.ContentMD5,
                
                ContentRange = MapToContentRangeHeader(
                    httpContentHeaders.ContentRange),
                
                ContentType = MapToContentType(httpContentHeaders.ContentType),
                Expires = httpContentHeaders.Expires,
                LastModified = httpContentHeaders.LastModified
            };
        }

        private static HttpExchangeResponseHeaders CreateHttpExchangeResponseHeaders(
            HttpResponseHeaders httpResponseHeaders)
        {
            return new HttpExchangeResponseHeaders
            {
                AcceptRanges = httpResponseHeaders.AcceptRanges.ToArray(),
                Age = httpResponseHeaders.Age,
                
                CacheControl = MapToCacheControlHeader(
                    httpResponseHeaders.CacheControl),
                
                Connection = httpResponseHeaders.Connection.ToArray(),
                ConnectionClose = httpResponseHeaders.ConnectionClose,
                Date = httpResponseHeaders.Date,
                ETag = httpResponseHeaders.ETag.ToString(),
                Location = httpResponseHeaders.Location,
                
                Pragma = httpResponseHeaders.Pragma.Select(header =>
                    MapToNameValueHeader(header)).ToArray(),
                
                ProxyAuthenticate = httpResponseHeaders.ProxyAuthenticate
                    .Select(header =>
                        MapToAuthenticationHeader(header)).ToArray(),
                
                RetryAfter = MapToRetryConditionHeader(httpResponseHeaders),
                
                Server = (httpResponseHeaders.Server).Select(header =>
                    string.IsNullOrEmpty(header.Comment) ?
                        MapToProductInfoHeader(header.Product) :
                        MapToProductInfoHeader(header.Comment)).ToArray(),
                
                Trailer = httpResponseHeaders.Trailer.ToArray(),
                
                TransferEncoding = httpResponseHeaders.TransferEncoding
                    .Select(header => MapToTransferCodingHeader(header)).ToArray(),
                
                TransferEncodingChunked = httpResponseHeaders.TransferEncodingChunked,
                
                Upgrade = httpResponseHeaders.Upgrade.Select(header =>
                    MapToProductHeader(header)).ToArray(),
                
                Vary = httpResponseHeaders.Vary.ToArray(),
                
                Via = (httpResponseHeaders.Via).Select(header =>
                    MapToViaHeader(header)).ToArray(),
                
                Warning = (httpResponseHeaders.Warning).Select(header =>
                    MapToWarningHeader(header)).ToArray(),
                
                WwwAuthenticate = (httpResponseHeaders.WwwAuthenticate)
                    .Select(header =>
                        MapToAuthenticationHeader(header)).ToArray()
            };
        }

        private static TransferCodingHeader MapToTransferCodingHeader(
            TransferCodingHeaderValue transferCodingHeaderValue)
        {
            return new TransferCodingHeader
            {
                Value = transferCodingHeaderValue.Value,
                Parameters = transferCodingHeaderValue.Parameters.Select(header =>
                    MapToNameValueHeader(header)).ToArray()
            };
        }

        private static RetryConditionHeader MapToRetryConditionHeader(
            HttpResponseHeaders httpResponseHeaders)
        {
            return new RetryConditionHeader
            {
                Date = httpResponseHeaders.RetryAfter.Date,
                Delta = httpResponseHeaders.RetryAfter.Delta
            };
        }

        private static WarningHeader MapToWarningHeader(WarningHeaderValue warningHeaderValue)
        {
            return new WarningHeader
            {
                Code = warningHeaderValue.Code,
                Agent = warningHeaderValue.Agent,
                Text = warningHeaderValue.Text,
                Date = warningHeaderValue.Date
            };
        }

        private static ViaHeader MapToViaHeader(ViaHeaderValue viaHeaderValue)
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

        private static ProductInfoHeader MapToProductInfoHeader(ProductHeaderValue productHeaderValue)
        {
            return new ProductInfoHeader
            {
                Product = MapToProductHeader(productHeaderValue)
            };
        }
        
        private static ProductInfoHeader MapToProductInfoHeader(string comment)
        {
            return new ProductInfoHeader
            {
                Comment = comment
            };
        }

        private static ProductHeader MapToProductHeader(ProductHeaderValue productHeaderValue)
        {
            return new ProductHeader
            {
                Name = productHeaderValue.Name,
                Version = productHeaderValue.Version
            };
        }

        private static NameValueHeader MapToNameValueHeader(NameValueHeaderValue nameValueHeaderValue)
        {
            return new NameValueHeader
            {
                Name = nameValueHeaderValue.Name,
                Value = nameValueHeaderValue.Value
            };
        }

        private static CacheControlHeader MapToCacheControlHeader(CacheControlHeaderValue cacheControlHeaderValue)
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
                Extensions = cacheControlHeaderValue.Extensions
                    .Select(header => MapToNameValueHeader(header)).ToArray()
            };
        }

        private static MediaTypeHeader MapToContentType(MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            return new MediaTypeHeader
            {
                CharSet = mediaTypeHeaderValue.CharSet,
                MediaType = mediaTypeHeaderValue.MediaType
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
