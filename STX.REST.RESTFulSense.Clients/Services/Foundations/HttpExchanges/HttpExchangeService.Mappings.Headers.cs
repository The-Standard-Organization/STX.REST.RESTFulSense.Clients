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
