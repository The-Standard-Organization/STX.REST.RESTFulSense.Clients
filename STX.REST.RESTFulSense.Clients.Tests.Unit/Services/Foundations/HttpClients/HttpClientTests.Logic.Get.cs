// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.HttpClients;
using Xunit;
using HttpClient = STX.REST.RESTFulSense.Clients.Models.HttpClients.HttpClient;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.HttpClients
{
    public partial class HttpClientTests
    {
        [Fact]
        public async Task ShouldGetAsync()
        {
            // given
            string randomString = GetRandomString();
            string randomContent = randomString;
            string inputRelativeUrl = randomString;
            StreamContent streamContent = CreateStreamContent(randomContent);
            var inputHttpRequestMessage = new HttpRequestMessage();
            
            var expectedHttpResponseMessage = CreateHttpResponseMessage(
                statusCode: HttpStatusCode.OK,
                content: randomContent);

            var inputHttpClient = new HttpClient
            {
                HttpRequest = new HttpRequest
                {
                    RelativeUrl = inputRelativeUrl
                },
            };

            
            var expectedHttpClient = inputHttpClient.DeepClone();
            
            expectedHttpClient.HttpResponse = new HttpResponse
            {
                StreamContent = streamContent
            };

            this.httpClientBroker.Setup(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            HttpClient actualHttpClient =
                await this.httpClientService.GetAsync(inputHttpClient);

            // then
            actualHttpClient.Should().BeEquivalentTo(expectedHttpClient);
            
            this.httpClientBroker.Verify(broker =>
                broker.SendRequestAsync(
                    inputHttpRequestMessage, default),
                    Times.Once);
            
            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}