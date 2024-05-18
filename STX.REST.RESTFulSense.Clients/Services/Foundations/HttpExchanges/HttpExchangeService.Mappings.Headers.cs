// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Net.Http.Headers;
using System.Linq;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges.Headers;
using STX.REST.RESTFulSense.Clients.Models.Services.HttpExchanges;

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
                    MapToContentDispositionHeader(httpContentHeaders),

                ContentEncoding =
                    httpContentHeaders.ContentEncoding.ToArray(),

                ContentLanguage = httpContentHeaders.ContentLanguage.ToArray(),
                ContentLength = httpContentHeaders.ContentLength,
                ContentLocation = httpContentHeaders.ContentLocation,
                ContentMD5 = httpContentHeaders.ContentMD5,
                ContentRange = MapToContentRangeHeader(httpContentHeaders),
                ContentType = MapToContentType(httpContentHeaders),
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
                CacheControl = MapToCacheControlHeader(httpResponseHeaders),
                Connection = httpResponseHeaders.Connection.ToArray(),
                ConnectionClose = httpResponseHeaders.ConnectionClose,
                Date = httpResponseHeaders.Date,
                ETag = httpResponseHeaders.ETag.ToString(),
                Location = httpResponseHeaders.Location,
                
                Pragma = (httpResponseHeaders.Pragma).Select(header =>
                   (NameValueHeader)MapToNameValueHeader(header)).ToArray(),
                
                ProxyAuthenticate = (httpResponseHeaders.ProxyAuthenticate)
                    .Select(header =>
                        (AuthenticationHeader)MapToAuthenticationHeader(header)).ToArray(),
                
                RetryAfter = new RetryConditionHeader
                {
                    Date = httpResponseHeaders.RetryAfter.Date,
                    Delta = httpResponseHeaders.RetryAfter.Delta
                },
                
                Server = (httpResponseHeaders.Server).Select(header =>
                    string.IsNullOrEmpty(header.Comment) ?
                        (ProductInfoHeader)MapToProductInfoHeader(header.Product)
                        : (ProductInfoHeader)MapToProductInfoHeader(header.Comment)).ToArray(),
                
                Trailer = httpResponseHeaders.Trailer.ToArray(),
                
                TransferEncoding = (httpResponseHeaders.TransferEncoding)
                    .Select(header => new TransferCodingHeader
                    {
                        Value = header.Value,
                        // Quality = h
                        Parameters = (header.Parameters).Select(header =>
                            new NameValueHeader
                            {
                              Name  = header.Name,
                              Value = header.Value
                            }).ToArray()
                        
                    }).ToArray(),
                
                TransferEncodingChunked = httpResponseHeaders.TransferEncodingChunked,
                
                Upgrade = (httpResponseHeaders.Upgrade.Select(header =>
                    new ProductHeader
                    {
                        Name = header.Name,
                        Version = header.Version
                    })).ToArray(),
                
                Vary = httpResponseHeaders.Vary.ToArray(),
                
                Via = (httpResponseHeaders.Via).Select(header =>
                    (ViaHeader)MapToViaHeader(header)).ToArray(),
                
                Warning = (httpResponseHeaders.Warning).Select(header =>
                    (WarningHeader)MapToWarningHeader(header)).ToArray(),
                
                WwwAuthenticate = (httpResponseHeaders.WwwAuthenticate)
                    .Select(header =>
                        (AuthenticationHeader)MapToAuthenticationHeader(header)).ToArray()
            };
        }

        private static WarningHeader MapToWarningHeader(WarningHeaderValue header)
        {
            return new WarningHeader
            {
                Code = header.Code,
                Agent = header.Agent,
                Text = header.Text,
                Date = header.Date
            };
        }

        private static ViaHeader MapToViaHeader(ViaHeaderValue header)
        {
            return new ViaHeader
            {
                ProtocolName = header.ProtocolName,
                ProtocolVersion = header.ProtocolVersion,
                ReceivedBy = header.ReceivedBy,
                Comment = header.Comment
            };
        }

        private static AuthenticationHeader MapToAuthenticationHeader(AuthenticationHeaderValue header)
        {
            return new AuthenticationHeader
            {
                Schema = header.Scheme,
                Value = header.Parameter
            };
        }

        private static ProductInfoHeader MapToProductInfoHeader(ProductHeaderValue header)
        {
            return new ProductInfoHeader
            {
                Product = MapToProductHeader(header)
            };
        }
        
        private static ProductInfoHeader MapToProductInfoHeader(string comment)
        {
            return new ProductInfoHeader
            {
                Comment = comment
            };
        }

        private static ProductHeader MapToProductHeader(ProductHeaderValue header)
        {
            return new ProductHeader
            {
                Name = header.Name,
                Version = header.Version
            };
        }

        private static NameValueHeader MapToNameValueHeader(NameValueHeaderValue header)
        {
            return new NameValueHeader
            {
                Name = header.Name,
                Value = header.Value
            };
        }

        private static CacheControlHeader MapToCacheControlHeader(HttpResponseHeaders httpResponseHeaders)
        {
            return new CacheControlHeader
            {
                NoCache = httpResponseHeaders.CacheControl.NoCache,
                NoCacheHeaders = httpResponseHeaders.CacheControl.NoCacheHeaders.ToArray(),
                NoStore = httpResponseHeaders.CacheControl.NoStore,
                MaxAge = httpResponseHeaders.CacheControl.MaxAge,
                SharedMaxAge = httpResponseHeaders.CacheControl.SharedMaxAge,
                MaxStale = httpResponseHeaders.CacheControl.MaxStale,
                MaxStaleLimit = httpResponseHeaders.CacheControl.MaxStaleLimit,
                MinFresh = httpResponseHeaders.CacheControl.MinFresh,
                NoTransform = httpResponseHeaders.CacheControl.NoTransform,
                OnlyIfCached = httpResponseHeaders.CacheControl.OnlyIfCached,
                Public = httpResponseHeaders.CacheControl.Public,
                Private = httpResponseHeaders.CacheControl.Private,
                PrivateHeaders = httpResponseHeaders.CacheControl.PrivateHeaders.ToArray(),
                MustRevalidate = httpResponseHeaders.CacheControl.MustRevalidate,
                ProxyRevalidate = httpResponseHeaders.CacheControl.ProxyRevalidate,
                Extensions = (httpResponseHeaders.CacheControl.Extensions)
                    .Select(header => new NameValueHeader
                    {
                        Name = header.Name,
                        Value = header.Value
                    }).ToArray()
            };
        }

        private static MediaTypeHeader MapToContentType(HttpContentHeaders httpContentHeaders)
        {
            return new MediaTypeHeader
            {
                CharSet = httpContentHeaders.ContentType.CharSet,
                MediaType = httpContentHeaders.ContentType.MediaType
            };
        }

        private static ContentRangeHeader MapToContentRangeHeader(
            HttpContentHeaders httpContentHeaders)
        {
            return new ContentRangeHeader
            {
                Unit = httpContentHeaders.ContentRange.Unit,
                From = httpContentHeaders.ContentRange.From,
                To = httpContentHeaders.ContentRange.To,
                Length = httpContentHeaders.ContentRange.Length
            };
        }

        private static ContentDispositionHeader MapToContentDispositionHeader(HttpContentHeaders httpContentHeaders)
        {
            return new ContentDispositionHeader
            {
                DispositionType = httpContentHeaders.ContentDisposition.DispositionType,
                Name = httpContentHeaders.ContentDisposition.Name,
                FileName = httpContentHeaders.ContentDisposition.FileName,
                FileNameStar = httpContentHeaders.ContentDisposition.FileNameStar,
                CreationDate = httpContentHeaders.ContentDisposition.CreationDate,
                ModificationDate = httpContentHeaders.ContentDisposition.ModificationDate,
                ReadDate = httpContentHeaders.ContentDisposition.ReadDate,
                Size = httpContentHeaders.ContentDisposition.Size
            };
        }
    }
}
