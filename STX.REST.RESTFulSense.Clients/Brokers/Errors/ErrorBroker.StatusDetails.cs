﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using STX.REST.RESTFulSense.Clients.Models.Services.ErrorMappers;

namespace STX.REST.RESTFulSense.Clients.Brokers.Errors
{
    internal partial class ErrorBroker : IErrorBroker
    {
        private static IEnumerable<StatusDetail> statusDetails = new List<StatusDetail>
        {
            new StatusDetail
            {
              Code = 100,
              Reason = "Continue",
              Description = "The 100 (Continue) status code indicates that the initial part of a request has been " +
                "received and has not yet been rejected by the server.  The server intends to send a final response " +
                "after the request has been fully received and acted upon.\n\nWhen the request contains an Expect " +
                "header field that includes a 100-continue expectation, the 100 response indicates that the server " +
                "wishes to receive the request payload body, as described in Section 5.1.1.  The client ought to " +
                "continue sending the request and discard the 100 response.\n\nIf the request did not contain an " +
                "Expect header field containing the 100-continue expectation, the client can simply discard this " +
                "interim response.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.2.1"
            },
            new StatusDetail
            {
              Code = 101,
              Reason = "Switching Protocols",
              Description = "The 101 (Switching Protocols) status code indicates that the server understands and " +
                "is willing to comply with the client's request, via the Upgrade header field " +
                "(Section 6.7 of [RFC7230]), for a change in the application protocol being used on this " +
                "connection.  The server MUST generate an Upgrade header field in the response that indicates " +
                "which protocol(s) will be switched to immediately after the empty line that terminates the 101 " +
                "response. \n\nIt is assumed that the server will only agree to switch protocols when it is " +
                "advantageous to do so.  For example, switching to a newer version of HTTP might be advantageous " +
                "over older versions, and switching to a real-time, synchronous protocol might be advantageous " +
                "when delivering resources that use such features.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.2.2"
            },
            new StatusDetail
            {
              Code = 102,
              Reason = "Processing",
              Description = "The server is processing the request, but no response is available yet.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.2.2"
            },
            new StatusDetail
            {
              Code = 200,
              Reason = "OK",
              Description = "The request has succeeded.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1"
            },
            new StatusDetail
            {
              Code = 201,
              Reason = "Created",
              Description = "The 201 (Created) status code indicates that the request has been fulfilled and has " +
                "resulted in one or more new resources being created.  The primary resource created by the request " +
                "is identified by either a Location header field in the response or, if no Location field is " +
                "received, by the effective request URI.\n\nThe 201 response payload typically describes and links " +
                "to the resource(s) created.  See Section 7.2 for a discussion of the meaning and purpose of " +
                "validator header fields, such as ETag and Last-Modified, in a 201 response.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.2"
            },
            new StatusDetail
            {
              Code = 202,
              Reason = "Accepted",
              Description = "The 202 (Accepted) status code indicates that the request has been accepted for " +
                "processing, but the processing has not been completed. The request might or might not " +
                "eventually be acted upon, as it might be disallowed when processing actually takes place.  " +
                "There is no facility in HTTP for re-sending a status code from an asynchronous operation.\n\n" +
                "The 202 response is intentionally noncommittal.  Its purpose is to allow a server to accept a " +
                "request for some other process (perhaps a batch-oriented process that is only run once per day) " +
                "without requiring that the user agent's connection to the server persist until the process is " +
                "completed.  The representation sent with this response ought to describe the request's current " +
                "status and point to (or embed) a status monitor that can provide the user with an estimate of " +
                "when the request will be fulfilled.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.3"
            },
            new StatusDetail
            {
              Code = 203,
              Reason = "Non-Authoritative Information",
              Description = "The 203 (Non-Authoritative Information) status code indicates that the request " +
                "was successful but the enclosed payload has been modified from that of the origin server's " +
                "200 (OK) response by a transforming proxy (Section 5.7.2 of [RFC7230]).  This status code " +
                "allows the proxy to notify recipients when a transformation has been applied, since that " +
                "knowledge might impact later decisions regarding the content.  For example, future cache " +
                "validation requests for the content might only be applicable along the same request path " +
                "(through the same proxies).\n\nThe 203 response is similar to the Warning code of 214 " +
                "Transformation Applied (Section 5.5 of [RFC7234]), which has the advantage of being applicable " +
                "to responses with any status code.\n\nA 203 response is cacheable by default; i.e., unless " +
                "otherwise indicated by the method definition or explicit cache controls " +
                "(see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.4"
            },
            new StatusDetail
            {
              Code = 204,
              Reason = "No Content",
              Description = "The 204 (No Content) status code indicates that the server has successfully " +
                "fulfilled the request and that there is no additional content to send in the response " +
                "payload body.  Metadata in the response header fields refer to the target resource and its " +
                "selected representation after the requested action was applied.\n\nFor example, if a " +
                "204 status code is received in response to a PUT request and the response contains an " +
                "ETag header field, then the PUT was successful and the ETag field-value contains the " +
                "entity-tag for the new representation of that target resource.\n\nThe 204 response allows " +
                "a server to indicate that the action has been successfully applied to the target resource, " +
                "while implying that the user agent does not need to traverse away from its current " +
                "\"document view\" (if any).  The server assumes that the user agent will provide some " +
                "indication of the success to its user, in accord with its own interface, and apply any " +
                "new or updated metadata in the response to its active representation.\n\nFor example, a " +
                "204 status code is commonly used with document editing interfaces corresponding to a \"save\" " +
                "action, such that the document being saved remains available to the user for editing.  It is " +
                "also frequently used with interfaces that expect automated data transfers to be prevalent, " +
                "such as within distributed version control systems.\n\nA 204 response is terminated by the " +
                "first empty line after the header fields because it cannot contain a message body. \n\nA " +
                "204 response is cacheable by default; i.e., unless otherwise indicated by the method " +
                "definition or explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.5"
            },
            new StatusDetail
            {
              Code = 205,
              Reason = "Reset Content",
              Description = "The 205 (Reset Content) status code indicates that the server has fulfilled the " +
                "request and desires that the user agent reset the \"document view\", which caused the request to " +
                "be sent, to its original state as received from the origin server.\n\nThis response is intended " +
                "to support a common data entry use case where the user receives content that supports data entry " +
                "(a form, notepad, canvas, etc.), enters or manipulates data in that space, causes the entered " +
                "data to be submitted in a request, and then the data entry mechanism is reset for the next entry " +
                "so that the user can easily initiate another input action.\n\nSince the 205 status code implies " +
                "that no additional content will be provided, a server MUST NOT generate a payload in a 205 " +
                "response.  In other words, a server MUST do one of the following for a 205 response: a) indicate " +
                "a zero-length body for the response by including a Content-Length header field with a value " +
                "of 0; b) indicate a zero-length payload for the response by including a Transfer-Encoding " +
                "header field with a value of chunked and a message body consisting of a single chunk of " +
                "zero-length; or, c) close the connection immediately after sending the blank line " +
                "terminating the header section.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.3.6"
            },
            new StatusDetail
            {
              Code = 206,
              Reason = "Partial Content",
              Description = "The 206 (Partial Content) status code indicates that the server is successfully " +
                "fulfilling a range request for the target resource by transferring one or more parts of the " +
                "selected representation that correspond to the satisfiable ranges found in the request's Range " +
                "header field (Section 3.1).\n\nIf a single part is being transferred, the server generating " +
                "the 206 response MUST generate a Content-Range header field, describing what range of the " +
                "selected representation is enclosed, and a payload consisting of the range.\n\nIf multiple " +
                "parts are being transferred, the server generating the 206 response MUST generate a " +
                "\"multipart/byteranges\" payload, as defined in Appendix A, and a Content-Type header field " +
                "containing the multipart/byteranges media type and its required boundary parameter. To avoid " +
                "confusion with single-part responses, a server MUST NOT generate a Content-Range header field " +
                "in the HTTP header section of a multiple part response (this field will be sent in each part " +
                "instead).\n\nWithin the header area of each body part in the multipart payload, the server MUST " +
                "generate a Content-Range header field corresponding to the range being enclosed in that body " +
                "part.  If the selected representation would have had a Content-Type header field in a 200 (OK) " +
                "response, the server SHOULD generate that same Content-Type field in the header area of each " +
                "body part.\n\nWhen multiple ranges are requested, a server MAY coalesce any of the ranges that " +
                "overlap, or that are separated by a gap that is smaller than the overhead of sending multiple " +
                "parts, regardless of the order in which the corresponding byte-range-spec appeared in the " +
                "received Range header field.  Since the typical overhead between parts of a multipart/byteranges " +
                "payload is around 80 bytes, depending on the selected representation's media type and the chosen " +
                "boundary parameter length, it can be less efficient to transfer many small disjoint parts than it " +
                "is to transfer the entire selected representation.\n\nA server MUST NOT generate a multipart " +
                "response to a request for a single range, since a client that does not request multiple parts " +
                "might not support multipart responses.  However, a server MAY generate a multipart/byteranges " +
                "payload with only a single body part if multiple ranges were requested and only one range was " +
                "found to be satisfiable or only one range remained after coalescing.  A client that cannot " +
                "process a multipart/byteranges response MUST NOT generate a request that asks for multiple " +
                "ranges.\n\nWhen a multipart response payload is generated, the server SHOULD send the parts " +
                "in the same order that the corresponding byte-range-spec appeared in the received Range header " +
                "field, excluding those ranges that were deemed unsatisfiable or that were coalesced into other " +
                "ranges.  A client that receives a multipart response MUST inspect the Content-Range header field " +
                "present in each body part in order to determine which range is contained in that body part; a " +
                "client cannot rely on receiving the same ranges that it requested, nor the same order that it " +
                "requested.\n\nWhen a 206 response is generated, the server MUST generate the following header " +
                "fields, in addition to those required above, if the field would have been sent in a 200 (OK) " +
                "response to the same request: Date, Cache-Control, ETag, Expires, Content-Location, and Vary." +
                "\n\nIf a 206 is generated in response to a request with an If-Range header field, the sender " +
                "SHOULD NOT generate other representation header fields beyond those required above, because the " +
                "client is understood to already have a prior response containing those header fields.  Otherwise, " +
                "the sender MUST generate all of the representation header fields that would have been sent in a " +
                "200 (OK) response to the same request.\n\nA 206 response is cacheable by default; i.e., unless " +
                "otherwise indicated by explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7233#section-4.1"
            },
            new StatusDetail
            {
              Code = 207,
              Reason = "Multi-Status",
              Description = "The message body that follows is an XML message and can contain a number of separate " +
                "response codes, depending on how many sub-requests were made.",
              Type = "https://tools.ietf.org/html/rfc4918#section-11.1"
            },
            new StatusDetail
            {
              Code = 208,
              Reason = "Already Reported",
              Description = "The 208 (Already Reported) status code can be used inside a DAV: propstat response " +
                "element to avoid enumerating the internal members of multiple bindings to the same collection " +
                "repeatedly.  For each binding to a collection inside the request's scope, only one will be " +
                "reported with a 200 status, while subsequent DAV:response elements for all other bindings will " +
                "use the 208 status, and no DAV:response elements for their descendants are included.\n\nNote " +
                "that the 208 status will only occur for \"Depth: infinity\" requests, and that it is of " +
                "particular importance when the multiple collection bindings cause a bind loop as discussed " +
                "in Section 2.2.\n\nA client can request the DAV:resource-id property in a PROPFIND request " +
                "to guarantee that they can accurately reconstruct the binding structure of a collection with " +
                "multiple bindings to a single resource. \n\nFor backward compatibility with clients not aware " +
                "of the 208 status code appearing in multistatus response bodies, it SHOULD NOT be used unless " +
                "the client has signaled support for this specification using the \"DAV\" request header " +
                "(see Section 8.2).  Instead, a 508 status should be returned when a binding loop is discovered.  " +
                "This allows the server to return the 508 as the top-level return status, if it discovers it " +
                "before it started the response, or in the middle of a multistatus, if it discovers it in the " +
                "middle of streaming out a multistatus response.",
              Type = "https://tools.ietf.org/html/rfc5842#section-7.1"
            },
            new StatusDetail
            {
              Code = 226,
              Reason = "IM Used",
              Description = "The server has fulfilled a GET request for the resource, and the response is a " +
                "representation of the result of one or more instance-manipulations applied to the current " +
                "instance.  The actual current instance might not be available except by combining this " +
                "response with other previous or future responses, as appropriate for the specific " +
                "instance-manipulation(s).  If so, the headers of the resulting instance are the result of " +
                "combining the headers from the status-226 response and the other instances, following the " +
                "rules in section 13.5.3 of the HTTP/1.1 specification [10].\n\nThe request MUST have included " +
                "an A-IM header field listing at least one instance-manipulation.  The response MUST include an " +
                "Etag header field giving the entity tag of the current instance. \n\nA response received with a " +
                "status code of 226 MAY be stored by a cache and used in reply to a subsequent request, subject " +
                "to the HTTP expiration mechanism and any Cache-Control headers, and to the requirements in " +
                "section 10.6.\n\nA response received with a status code of 226 MAY be used by a cache, in " +
                "conjunction with a cache entry for the base instance, to create a cache entry for the " +
                "current instance.",
              Type = "https://tools.ietf.org/html/rfc3229#section-10.4.1"
            },
            new StatusDetail
            {
              Code = 300,
              Reason = "Multiple Choices",
              Description = "The 300 (Multiple Choices) status code indicates that the target resource has " +
                "more than one representation, each with its own more specific identifier, and information " +
                "about the alternatives is being provided so that the user (or user agent) can select a " +
                "preferred representation by redirecting its request to one or more of those identifiers.  " +
                "In other words, the server desires that the user agent engage in reactive negotiation to " +
                "select the most appropriate representation(s) for its needs (Section 3.4).\n\nIf the server " +
                "has a preferred choice, the server SHOULD generate a Location header field containing a " +
                "preferred choice's URI reference. The user agent MAY use the Location field value for " +
                "automatic redirection.\n\nFor request methods other than HEAD, the server SHOULD generate " +
                "a payload in the 300 response containing a list of representation metadata and URI reference(s) " +
                "from which the user or user agent can choose the one most preferred.  The user agent MAY make a " +
                "selection from that list automatically if it understands the provided media type.  A specific " +
                "format for automatic selection is not defined by this specification because HTTP tries to remain " +
                "orthogonal to the definition of its payloads.  In practice, the representation is provided in some " +
                "easily parsed format believed to be acceptable to the user agent, as determined by shared design or " +
                "content negotiation, or in some commonly accepted hypertext format. \n\nA 300 response is cacheable " +
                "by default; i.e., unless otherwise indicated by the method definition or explicit cache controls " +
                "(see Section 4.2.2 of [RFC7234]).\n\nNote: The original proposal for the 300 status code defined " +
                "the URI header field as providing a list of alternative representations, such that it would be " +
                "usable for 200, 300, and 406 responses and be transferred in responses to the HEAD method. " +
                "However, lack of deployment and disagreement over syntax led to both URI and Alternates " +
                "(a subsequent proposal) being dropped from this specification.  It is possible to communicate " +
                "the list using a set of Link header fields [RFC5988], each with a relationship of \"alternate\", " +
                "though deployment is a chicken-and-egg problem.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.1"
            },
            new StatusDetail
            {
              Code = 301,
              Reason = "Moved Permanently",
              Description = "The 301 (Moved Permanently) status code indicates that the target resource has " +
                "been assigned a new permanent URI and any future references to this resource ought to use one " +
                "of the enclosed URIs. Clients with link-editing capabilities ought to automatically re-link " +
                "references to the effective request URI to one or more of the new references sent by the server, " +
                "where possible.\n\nThe server SHOULD generate a Location header field in the response containing " +
                "a preferred URI reference for the new permanent URI.  The user agent MAY use the Location field " +
                "value for automatic redirection.  The server's response payload usually contains a short " +
                "hypertext note with a hyperlink to the new URI(s).\n\nNote: For historical reasons, a user " +
                "agent MAY change the request method from POST to GET for the subsequent request.  If this " +
                "behavior is undesired, the 307 (Temporary Redirect) status code can be used instead.\n\nA " +
                "301 response is cacheable by default; i.e., unless otherwise indicated by the method definition " +
                "or explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.2"
            },
            new StatusDetail
            {
              Code = 302,
              Reason = "Found",
              Description = "The 302 (Found) status code indicates that the target resource resides temporarily " +
                "under a different URI.  Since the redirection might be altered on occasion, the client ought " +
                "to continue to use the effective request URI for future requests.\n\nThe server SHOULD generate " +
                "a Location header field in the response containing a URI reference for the different URI.  " +
                "The user agent MAY use the Location field value for automatic redirection.  The server's " +
                "response payload usually contains a short hypertext note with a hyperlink to the different " +
                "URI(s).\n\nNote: For historical reasons, a user agent MAY change the request method from " +
                "POST to GET for the subsequent request.  If this behavior is undesired, the 307 " +
                "(Temporary Redirect) status code can be used instead.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.3"
            },
            new StatusDetail
            {
              Code = 303,
              Reason = "See Other",
              Description = "The 303 (See Other) status code indicates that the server is redirecting the " +
                "user agent to a different resource, as indicated by a URI in the Location header field, " +
                "which is intended to provide an indirect response to the original request.  A user agent " +
                "can perform a retrieval request targeting that URI (a GET or HEAD request if using HTTP), " +
                "which might also be redirected, and present the eventual result as an answer to the original " +
                "request.  Note that the new URI in the Location header field is not considered equivalent " +
                "to the effective request URI.\n\nThis status code is applicable to any HTTP method.  It is " +
                "primarily used to allow the output of a POST action to redirect the user agent to a selected " +
                "resource, since doing so provides the information corresponding to the POST response in a form " +
                "that can be separately identified, bookmarked, and cached, independent of the original request." +
                "\n\nA 303 response to a GET request indicates that the origin server does not have a " +
                "representation of the target resource that can be transferred by the server over HTTP.  " +
                "However, the Location field value refers to a resource that is descriptive of the target " +
                "resource, such that making a retrieval request on that other resource might result in a " +
                "representation that is useful to recipients without implying that it represents the original " +
                "target resource.  Note that answers to the questions of what can be represented, what " +
                "representations are adequate, and what might be a useful description are outside the " +
                "scope of HTTP.\n\nExcept for responses to a HEAD request, the representation of a 303 " +
                "response ought to contain a short hypertext note with a hyperlink to the same URI reference " +
                "provided in the Location header field.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.4"
            },
            new StatusDetail
            {
              Code = 304,
              Reason = "Not Modified",
              Description = "The 304 (Not Modified) status code indicates that a conditional GET or HEAD " +
                "request has been received and would have resulted in a 200 (OK) response if it were not " +
                "for the fact that the condition evaluated to false.  In other words, there is no need for " +
                "the server to transfer a representation of the target resource because the request indicates " +
                "that the client, which made the request conditional, already has a valid representation; the " +
                "server is therefore redirecting the client to make use of that stored representation as if " +
                "it were the payload of a 200 (OK) response.\n\nThe server generating a 304 response MUST " +
                "generate any of the following header fields that would have been sent in a 200 (OK) response " +
                "to the same request: Cache-Control, Content-Location, Date, ETag, Expires, and Vary.\n\nSince " +
                "the goal of a 304 response is to minimize information transfer when the recipient already has " +
                "one or more cached representations, a sender SHOULD NOT generate representation metadata other " +
                "than the above listed fields unless said metadata exists for the purpose of guiding cache updates " +
                "(e.g., Last-Modified might be useful if the response does not have an ETag field).\n\nRequirements " +
                "on a cache that receives a 304 response are defined in Section 4.3.4 of [RFC7234].  If the " +
                "conditional request originated with an outbound client, such as a user agent with its own " +
                "cache sending a conditional GET to a shared proxy, then the proxy SHOULD forward the 304 " +
                "response to that client.\n\nA 304 response cannot contain a message-body; it is always " +
                "terminated by the first empty line after the header fields.",
              Type = "https://tools.ietf.org/html/rfc7232#section-4.1"
            },
            new StatusDetail
            {
              Code = 305,
              Reason = "Use Proxy",
              Description = "Deprecated and should no longer be used, but previously meant that the " +
                "requested resource is only available through a proxy, and the address is provided in the response.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.5"
            },
            new StatusDetail
            {
              Code = 306,
              Reason = "Switch Proxy",
              Description = "The 306 status code was defined in a previous version of this " +
                "specification, is no longer used, and the code is reserved.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.6"
            },
            new StatusDetail
            {
              Code = 307,
              Reason = "Temporary Redirect",
              Description = "The 307 (Temporary Redirect) status code indicates that the target resource " +
                "resides temporarily under a different URI and the user agent MUST NOT change the request " +
                "method if it performs an automatic redirection to that URI.  Since the redirection can " +
                "change over time, the client ought to continue using the original effective request URI " +
                "for future requests.\n\nThe server SHOULD generate a Location header field in the " +
                "response containing a URI reference for the different URI.  The user agent MAY use the " +
                "Location field value for automatic redirection.  The server's response payload usually " +
                "contains a short hypertext note with a hyperlink to the different URI(s).\n\nNote: This " +
                "status code is similar to 302 (Found), except that it does not allow changing the request " +
                "method from POST to GET.  This specification defines no equivalent counterpart for 301 " +
                "(Moved Permanently) ([RFC7238], however, defines the status code 308 " +
                "(Permanent Redirect) for this purpose).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.4.7"
            },
            new StatusDetail
            {
              Code = 308,
              Reason = "Permanent Redirect",
              Description = "The 308 (Permanent Redirect) status code indicates that the target resource " +
                "has been assigned a new permanent URI and any future references to this resource ought " +
                "to use one of the enclosed URIs. Clients with link editing capabilities ought to " +
                "automatically re-link references to the effective request URI (Section 5.5 of [RFC7230]) " +
                "to one or more of the new references sent by the server, where possible.\n\nThe server " +
                "SHOULD generate a Location header field ([RFC7231], Section 7.1.2) in the response " +
                "containing a preferred URI reference for the new permanent URI.  The user agent MAY " +
                "use the Location field value for automatic redirection.  The server's response payload " +
                "usually contains a short hypertext note with a hyperlink to the new URI(s).\n\nA 308 " +
                "response is cacheable by default; i.e., unless otherwise indicated by the method " +
                "definition or explicit cache controls (see [RFC7234], Section 4.2.2).\n\nNote: This " +
                "status code is similar to 301 (Moved Permanently) ([RFC7231], Section 6.4.2), except " +
                "that it does not allow changing the request method from POST to GET.",
              Type = "https://tools.ietf.org/html/rfc7538#section-3"
            },
            new StatusDetail
            {
              Code = 400,
              Reason = "Bad Request",
              Description = "The 400 (Bad Request) status code indicates that the server cannot or will " +
                "not process the request due to something that is perceived to be a client error (e.g., " +
                "malformed request syntax, invalid request message framing, or deceptive request routing).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            new StatusDetail
            {
              Code = 401,
              Reason = "Unauthorized",
              Description = "The 401 (Unauthorized) status code indicates that the request has not been " +
                "applied because it lacks valid authentication credentials for the target resource.  The " +
                "server generating a 401 response MUST send a WWW-Authenticate header field (Section 4.1) " +
                "containing at least one challenge applicable to the target resource.\n\nIf the request " +
                "included authentication credentials, then the 401 response indicates that authorization " +
                "has been refused for those credentials.  The user agent MAY repeat the request with a new " +
                "or replaced Authorization header field (Section 4.2).  If the 401 response contains the " +
                "same challenge as the prior response, and the user agent has already attempted " +
                "authentication at least once, then the user agent SHOULD present the enclosed " +
                "representation to the user, since it usually contains relevant diagnostic information.",
              Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            },
            new StatusDetail
            {
              Code = 402,
              Reason = "Payment Required",
              Description = "The 402 (Payment Required) status code is reserved for future use.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.2"
            },
            new StatusDetail
            {
              Code = 403,
              Reason = "Forbidden",
              Description = "The 403 (Forbidden) status code indicates that the server understood the " +
                "request but refuses to authorize it.  A server that wishes to make public why the " +
                "request has been forbidden can describe that reason in the response payload (if any).\n\n" +
                "If authentication credentials were provided in the request, the server considers them " +
                "insufficient to grant access.  The client SHOULD NOT automatically repeat the request " +
                "with the same credentials.  The client MAY repeat the request with new or different " +
                "credentials.  However, a request might be forbidden for reasons unrelated to the " +
                "credentials.\n\nAn origin server that wishes to \"hide\" the current existence of a " +
                "forbidden target resource MAY instead respond with a status code of 404 (Not Found).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            },
            new StatusDetail
            {
              Code = 404,
              Reason = "Not Found",
              Description = "The 404 (Not Found) status code indicates that the origin server did not " +
                "find a current representation for the target resource or is not willing to disclose " +
                "that one exists.  A 404 status code does not indicate whether this lack of representation " +
                "is temporary or permanent; the 410 (Gone) status code is preferred over 404 if the origin " +
                "server knows, presumably through some configurable means, that the condition is likely to " +
                "be permanent. \n\nA 404 response is cacheable by default; i.e., unless otherwise indicated " +
                "by the method definition or explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            },
            new StatusDetail
            {
              Code = 405,
              Reason = "Method Not Allowed",
              Description = "The 405 (Method Not Allowed) status code indicates that the method received in " +
                "the request-line is known by the origin server but not supported by the target resource.  " +
                "The origin server MUST generate an Allow header field in a 405 response containing a list " +
                "of the target resource's currently supported methods.\n\nA 405 response is cacheable by " +
                "default; i.e., unless otherwise indicated by the method definition or explicit cache " +
                "controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.5"
            },
            new StatusDetail
            {
              Code = 406,
              Reason = "Not Acceptable",
              Description = "The 406 (Not Acceptable) status code indicates that the target resource does " +
                "not have a current representation that would be acceptable to the user agent, according " +
                "to the proactive negotiation header fields received in the request (Section 5.3), and " +
                "the server is unwilling to supply a default representation.\n\nThe server SHOULD " +
                "generate a payload containing a list of available representation characteristics and " +
                "corresponding resource identifiers from which the user or user agent can choose the " +
                "one most appropriate.  A user agent MAY automatically select the most appropriate " +
                "choice from that list.  However, this specification does not define any standard " +
                "for such automatic selection, as described in Section 6.4.1.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.6"
            },
            new StatusDetail
            {
              Code = 407,
              Reason = "Proxy Authentication Required",
              Description = "The 407 (Proxy Authentication Required) status code is similar " +
                "to 401 (Unauthorized), but it indicates that the client needs to authenticate " +
                "itself in order to use a proxy.  The proxy MUST send a Proxy-Authenticate header " +
                "field (Section 4.3) containing a challenge applicable to that proxy for the target " +
                "resource.  The client MAY repeat the request with a new or replaced Proxy-Authorization " +
                "header field (Section 4.4).",
              Type = "https://tools.ietf.org/html/rfc7235#section-3.2"
            },
            new StatusDetail
            {
              Code = 408,
              Reason = "Request Timeout",
              Description = "The 408 (Request Timeout) status code indicates that the server did not " +
                "receive a complete request message within the time that it was prepared to wait.  " +
                "A server SHOULD send the \"close\" connection option (Section 6.1 of [RFC7230]) in " +
                "the response, since 408 implies that the server has decided to close the connection " +
                "rather than continue waiting.  If the client has an outstanding request in transit, " +
                "the client MAY repeat that request on a new connection.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.7"
            },
            new StatusDetail
            {
              Code = 409,
              Reason = "Conflict",
              Description = "The 409 (Conflict) status code indicates that the request could not be completed " +
                "due to a conflict with the current state of the target resource.  This code is used in " +
                "situations where the user might be able to resolve the conflict and resubmit the request.  " +
                "The server SHOULD generate a payload that includes enough information for a user to " +
                "recognize the source of the conflict.\n\nConflicts are most likely to occur in response " +
                "to a PUT request.  For example, if versioning were being used and the representation " +
                "being PUT included changes to a resource that conflict with those made by an earlier " +
                "(third-party) request, the origin server might use a 409 response to indicate that it " +
                "can't complete the request.  In this case, the response representation would likely " +
                "contain information useful for merging the differences based on the revision history.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            },
            new StatusDetail
            {
              Code = 410,
              Reason = "Gone",
              Description = "The 410 (Gone) status code indicates that access to the target resource is " +
                "no longer available at the origin server and that this condition is likely to be permanent.  " +
                "If the origin server does not know, or has no facility to determine, whether or not the " +
                "condition is permanent, the status code 404 (Not Found) ought to be used instead.\n\nThe 410 " +
                "response is primarily intended to assist the task of web maintenance by notifying the recipient " +
                "that the resource is intentionally unavailable and that the server owners desire that remote " +
                "links to that resource be removed.  Such an event is common for limited-time, promotional " +
                "services and for resources belonging to individuals no longer associated with the origin " +
                "server's site.  It is not necessary to mark all permanently unavailable resources as \"gone\" " +
                "or to keep the mark for any length of time -- that is left to the discretion of the server " +
                "owner. \n\nA 410 response is cacheable by default; i.e., unless otherwise indicated by the " +
                "method definition or explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.9"
            },
            new StatusDetail
            {
              Code = 411,
              Reason = "Length Required",
              Description = "The 411 (Length Required) status code indicates that the server refuses to accept " +
                "the request without a defined Content-Length (Section 3.3.2 of [RFC7230]).  The client MAY " +
                "repeat the request if it adds a valid Content-Length header field containing the length of " +
                "the message body in the request message.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.10"
            },
            new StatusDetail
            {
              Code = 412,
              Reason = "Precondition Failed",
              Description = "The 412 (Precondition Failed) status code indicates that one or more conditions " +
                "given in the request header fields evaluated to false when tested on the server.  This " +
                "response code allows the client to place preconditions on the current resource state (its " +
                "current representations and metadata) and, thus, prevent the request method from being " +
                "applied if the target resource is in an unexpected state.",
              Type = "https://tools.ietf.org/html/rfc7232#section-4.2"
            },
            new StatusDetail
            {
              Code = 413,
              Reason = "Payload Too Large",
              Description = "The 413 (Payload Too Large) status code indicates that the server is refusing " +
                "to process a request because the request payload is larger than the server is willing or " +
                "able to process.  The server MAY close the connection to prevent the client from " +
                "continuing the request.\n\nIf the condition is temporary, the server SHOULD generate " +
                "a Retry-After header field to indicate that it is temporary and after what time the " +
                "client MAY try again.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.11"
            },
            new StatusDetail
            {
              Code = 414,
              Reason = "URI Too Long",
              Description = "The 414 (URI Too Long) status code indicates that the server is refusing " +
                "to service the request because the request-target (Section 5.3 of [RFC7230]) is " +
                "longer than the server is willing to interpret. This rare condition is only likely " +
                "to occur when a client has improperly converted a POST request to a GET request with " +
                "long query information, when the client has descended into a \"black hole\" of " +
                "redirection (e.g., a redirected URI prefix that points to a suffix of itself) or " +
                "when the server is under attack by a client attempting to exploit potential " +
                "security holes. A 414 response is cacheable by default; i.e., unless otherwise " +
                "indicated by the method definition or explicit cache controls (see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.12"
            },
            new StatusDetail
            {
              Code = 415,
              Reason = "Unsupported Media Type",
              Description = "The 415 (Unsupported Media Type) status code indicates that the origin " +
                "server is refusing to service the request because the payload is in a format not " +
                "supported by this method on the target resource. The format problem might be due " +
                "to the request's indicated Content-Type or Content-Encoding, or as a result of " +
                "inspecting the data directly.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.13"
            },
            new StatusDetail
            {
              Code = 416,
              Reason = "Range Not Satisfiable",
              Description = "The 416 (Range Not Satisfiable) status code indicates that none of " +
                "the ranges in the request's Range header field (Section 3.1) overlap the current " +
                "extent of the selected resource or that the set of ranges requested has been " +
                "rejected due to invalid ranges or an excessiverequest of small or overlapping " +
                "ranges.\n\nFor byte ranges, failing to overlap the current extent means that the " +
                "first-byte-pos of all of the byte-range-spec values were greater than the current " +
                "length of the selected representation.  When this status code is generated in response " +
                "to a byte-range request, the sender SHOULD generate a Content-Range header field " +
                "specifying the current length of the selected representation (Section 4.2).",
              Type = "https://tools.ietf.org/html/rfc7233#section-4.4"
            },
            new StatusDetail
            {
              Code = 417,
              Reason = "Expectation Failed",
              Description = "The 417 (Expectation Failed) status code indicates that the expectation given " +
                "in the request's Expect header field (Section 5.1.1) could not be met by at least one of " +
                "the inbound servers.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.14"
            },
            new StatusDetail
            {
              Code = 418,
              Reason = "I'm a teapot",
              Description = "Any attempt to brew coffee with a teapot should result in the error code " +
                "\"418 I'm a teapot\". The resulting entity body MAY be short and stout.",
              Type = "https://tools.ietf.org/html/rfc2324#section-2.3.2"
            },
            new StatusDetail
            {
              Code = 421,
              Reason = "Misdirected Request",
              Description = "The 421 (Misdirected Request) status code indicates that the request was " +
                "directed at a server that is not able to produce a response. This can be sent by a server " +
                "that is not configured to produce responses for the combination of scheme and authority " +
                "that are included in the request URI.\n\nClients receiving a 421 (Misdirected Request) " +
                "response from a server MAY retry the request -- whether the request method is idempotent " +
                "or not -- over a different connection.  This is possible if a connection is reused " +
                "(Section 9.1.1) or if an alternative service is selected [ALT-SVC].\n\nThis status code " +
                "MUST NOT be generated by proxies.\n\nA 421 response is cacheable by default, i.e., unless " +
                "otherwiseindicated by the method definition or explicit cache controls " +
                "(see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7540#section-9.1.2"
            },
            new StatusDetail
            {
              Code = 422,
              Reason = "Unprocessable Entity",
              Description = "The 422 (Unprocessable Entity) status code means the server understands " +
                "the content type of the request entity (hence a 415(Unsupported Media Type) status " +
                "code is inappropriate), and the syntax of the request entity is correct (thus a 400 " +
                "(Bad Request) status code is inappropriate) but was unable to process the contained " +
                "instructions.  For example, this error condition may occur if an XML request body " +
                "contains well-formed (i.e., syntactically correct), but semantically erroneous, XML " +
                "instructions.",
              Type = "https://tools.ietf.org/html/rfc4918#section-11.2"
            },
            new StatusDetail
            {
              Code = 423,
              Reason = "Locked",
              Description = "The 423 (Locked) status code means the source or destination resource " +
                "of a method is locked.  This response SHOULD contain an appropriate precondition or " +
                "postcondition code, such as 'lock-token-submitted' or 'no-conflicting-lock'.",
              Type = "https://tools.ietf.org/html/rfc4918#section-11.3"
            },
            new StatusDetail
            {
              Code = 424,
              Reason = "Failed Dependency",
              Description = "The 424 (Failed Dependency) status code means that the method could not " +
                "be performed on the resource because the requested action depended on another action " +
                "and that action failed.  For example, if a command in a PROPPATCH method fails, then, " +
                "at minimum, the rest of the commands will also fail with 424 (Failed Dependency).",
              Type = "https://tools.ietf.org/html/rfc4918#section-11.4"
            },
            new StatusDetail
            {
              Code = 426,
              Reason = "Upgrade Required",
              Description = "The 426 (Upgrade Required) status code indicates that the server refuses " +
                "to perform the request using the current protocol but might be willing to do so after " +
                "the client upgrades to a different protocol.  The server MUST send an Upgrade header " +
                "field in a 426 response to indicate the required protocol(s) (Section 6.7 of [RFC7230]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.15"
            },
            new StatusDetail
            {
              Code = 428,
              Reason = "Precondition Required",
              Description = "The 428 status code indicates that the origin server requires the request " +
                "to be conditional.\n\nIts typical use is to avoid the \"lost update\" problem, where a " +
                "client GETs a resource's state, modifies it, and PUTs it back to the server, when " +
                "meanwhile a third party has modified the state on the server, leading to a conflict.  " +
                "By requiring requests to be conditional, the server can assure that clients are working " +
                "with the correct copies.\n\nResponses using this status code SHOULD explain how to " +
                "resubmit the request successfully.",
              Type = "https://tools.ietf.org/html/rfc6585#section-3"
            },
            new StatusDetail
            {
              Code = 429,
              Reason = "Too Many Requests",
              Description = "The 429 status code indicates that the user has sent too many requests " +
                "in a given amount of time (\"rate limiting\").\n\nThe response representations SHOULD " +
                "include details explaining the condition, and MAY include a Retry-After header " +
                "indicating how long to wait before making a new request.\n\nNote that this specification " +
                "does not define how the origin server identifies the user, nor how it counts requests.  " +
                "For example, an origin server that is limiting request rates can do so based upon counts " +
                "of requests on a per-resource basis, across the entire server, or even among a set of " +
                "servers.  Likewise, it might identify the user by its authentication credentials, or a " +
                "stateful cookie.\n\nResponses with the 429 status code MUST NOT be stored by a cache.",
              Type = "https://tools.ietf.org/html/rfc6585#section-4"
            },
            new StatusDetail
            {
              Code = 431,
              Reason = "Request Header Fields Too Large",
              Description = "The 431 status code indicates that the server is unwilling to process the " +
                "request because its header fields are too large.  The request MAY be resubmitted after " +
                "reducing the size of the request header fields.\n\nIt can be used both when the set of " +
                "request header fields in total is too large, and when a single header field is at fault.  " +
                "In the latter case, the response representation SHOULD specify which header field was too " +
                "large.\n\nResponses with the 431 status code MUST NOT be stored by a cache.",
              Type = "https://tools.ietf.org/html/rfc6585#section-5"
            },
            new StatusDetail
            {
              Code = 451,
              Reason = "Unavailable For Legal Reasons",
              Description = "This status code indicates that the server is denying access to the resource " +
                "as a consequence of a legal demand.\n\nThe server in question might not be an origin server.  " +
                "This type of legal demand typically most directly affects the operations of ISPs and search " +
                "engines.\n\nResponses using this status code SHOULD include an explanation, in the response " +
                "body, of the details of the legal demand: the party making it, the applicable legislation " +
                "or regulation, and what classes  of person and resource it applies to.\n\nThe use of the " +
                "451 status code implies neither the existence nor nonexistence of the resource named in " +
                "the request.  That is to say, it is possible that if the legal demands were removed, " +
                "a request for the resource still might not succeed.\n\nNote that in many cases clients can " +
                "still access the denied resource by using technical countermeasures such as a VPN or the " +
                "Tor network.\n\nA 451 response is cacheable by default, i.e., unless otherwise indicated " +
                "by the method definition or explicit cache controls; see [RFC7234].",
              Type = "https://tools.ietf.org/html/rfc7725#section-3"
            },
            new StatusDetail
            {
              Code = 499,
              Reason = "Client Closed Request",
              Description = "The client closed the request before the server could send a response.",
              Type = "N/A"
            },
            new StatusDetail
            {
              Code = 500,
              Reason = "Internal Server Error",
              Description = "The 500 (Internal Server Error) status code indicates that the server " +
                "encountered an unexpected condition that prevented it from fulfilling the request.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            },
            new StatusDetail
            {
              Code = 501,
              Reason = "Not Implemented",
              Description = "The 501 (Not Implemented) status code indicates that the server does not " +
                "support the functionality required to fulfill the request.  This is the appropriate " +
                "response when the server does not recognize the request method and is not capable of " +
                "supporting it for any resource.\n\nA 501 response is cacheable by default; i.e., unless " +
                "otherwise indicated by the method definition or explicit cache controls " +
                "(see Section 4.2.2 of [RFC7234]).",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2"
            },
            new StatusDetail
            {
              Code = 502,
              Reason = "Bad Gateway",
              Description = "The 502 (Bad Gateway) status code indicates that the server, while acting as " +
                "a gateway or proxy, received an invalid response from an inbound server it accessed while " +
                "attempting to fulfill the request.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.3"
            },
            new StatusDetail
            {
              Code = 503,
              Reason = "Service Unavailable",
              Description = "The 503 (Service Unavailable) status code indicates that the server is currently " +
                "unable to handle the request due to a temporary overload or scheduled maintenance, which " +
                "will likely be alleviated after some delay.  The server MAY send a Retry-After header field " +
                "(Section 7.1.3) to suggest an appropriate amount of time for the client to wait before " +
                "retrying the request.\n\nNote: The existence of the 503 status code does not imply that a " +
                "server has to use it when becoming overloaded.  Some servers might simply refuse the connection.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4"
            },
            new StatusDetail
            {
              Code = 504,
              Reason = "Gateway Timeout",
              Description = "The 504 (Gateway Timeout) status code indicates that the server, while acting " +
                "as a gateway or proxy, did not receive a timely response from an upstream server it needed " +
                "to access in order to complete the request.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.5"
            },
            new StatusDetail
            {
              Code = 505,
              Reason = "HTTP Version Not Supported",
              Description = "The 505 (HTTP Version Not Supported) status code indicates that the server " +
                "does not support, or refuses to support, the major version of HTTP that was used in the " +
                "request message.  The server is indicating that it is unable or unwilling to complete the " +
                "request using the same major version as the client, as described in Section 2.6 of [RFC7230], " +
                "other than with this error message.  The server SHOULD generate a representation for the 505 " +
                "response that describes why that version is not supported and what other protocols are " +
                "supported by that server.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.6.6"
            },
            new StatusDetail
            {
              Code = 506,
              Reason = "Variant Also Negotiates",
              Description = "The 506 status code indicates that the server has an internal configuration error: " +
                "the chosen variant resource is configured to engage in transparent content negotiation itself, " +
                "and is therefore not a proper end point in the negotiation process.",
              Type = "https://tools.ietf.org/html/rfc2295#section-8.1"
            },
            new StatusDetail
            {
              Code = 507,
              Reason = "Insufficient Storage",
              Description = "The 507 (Insufficient Storage) status code means the method could not be performed " +
                "on the resource because the server is unable to store the representation needed to successfully " +
                "complete the request.  This condition is considered to be temporary.  If the request that " +
                "received this status code was the result of a user action, the request MUST NOT be repeated " +
                "until it is requested by a separate user action.",
              Type = "https://tools.ietf.org/html/rfc4918#section-11.5"
            },
            new StatusDetail
            {
              Code = 508,
              Reason = "Loop Detected",
              Description = "The 508 (Loop Detected) status code indicates that the server terminated an " +
                "operation because it encountered an infinite loop while processing a request with " +
                "\"Depth: infinity\".  This status indicates that the entire operation failed.",
              Type = "https://tools.ietf.org/html/rfc5842#section-7.2"
            },
            new StatusDetail
            {
              Code = 510,
              Reason = "Not Extended",
              Description = "The policy for accessing the resource has not been met in the request.  The " +
                "server should send back all the information necessary for the client to issue an extended " +
                "request. It is outside the scope of this specification to specify how the extensions inform " +
                "the client. \n\nIf the 510 response contains information about extensions that were not " +
                "present in the initial request then the client MAY repeat the request if it has reason to " +
                "believe it can fulfill the extension policy by modifying the request according to the " +
                "information provided  in the 510 response. Otherwise the client MAY present any entity " +
                "included in the 510 response to the user, since that entity may include relevant " +
                "diagnostic information.",
              Type = "https://tools.ietf.org/html/rfc2774#section-7"
            },
            new StatusDetail
            {
              Code = 511,
              Reason = "Network Authentication Required",
              Description = "The 511 status code indicates that the client needs to authenticate to " +
                "gain network access.\n\nThe response representation SHOULD contain a link to a resource " +
                "that allows the user to submit credentials (e.g., with an HTML form).\n\nNote that the " +
                "511 response SHOULD NOT contain a challenge or the login interface itself, because " +
                "browsers would show the login interface as being associated with the originally requested " +
                "URL, which may cause confusion.\n\nThe 511 status SHOULD NOT be generated by origin servers; " +
                "it is intended for use by intercepting proxies that are interposed as a means of controlling " +
                "access to the network.",
              Type = "https://tools.ietf.org/html/rfc6585#section-6"
            }
        };
    }
}
