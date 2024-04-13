// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using STX.REST.RESTFulSense.Clients.Models.LocalHttpClients;
using Xunit;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.LocalHttpClients
{
    public partial class LocalHttpClientTest
    {
        [Fact]
        public async Task ShouldGetAsync()
        {
            // given
            dynamic randomProperties = CreateRandomProperties();

            LocalHttpClient inputLocalHttpClient =
                CreateHttpClient(randomProperties);

            LocalHttpClient expectedLocalHttpClient =
                CreateHttpClientResponse(inputLocalHttpClient, randomProperties);

            HttpRequestMessage inputHttpRequestMessage =
                CreateHttpRequestMessage(randomProperties);

            HttpResponseMessage expectedHttpResponseMessage =
                CreateHttpResponseMessage(randomProperties);

            this.httpClientBroker.Setup(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default))
                        .ReturnsAsync(expectedHttpResponseMessage);

            // when
            LocalHttpClient actualLocalHttpClient =
                await this.localHttpClientService.GetAsync(inputLocalHttpClient);
            
            // then
            actualLocalHttpClient.Should().BeEquivalentTo(
                expectedLocalHttpClient,
                options => options.Excluding(x => x.HttpResponse.StreamContent));

            using MemoryStream actualContentStream = new MemoryStream();
            await actualLocalHttpClient.HttpResponse.StreamContent.CopyToAsync(actualContentStream);
            using MemoryStream expectedContentStream = new MemoryStream();
            await expectedLocalHttpClient.HttpResponse.StreamContent.CopyToAsync(expectedContentStream);
            actualContentStream.ToArray().Should().BeEquivalentTo(expectedContentStream.ToArray());

            this.httpClientBroker.Verify(broker =>
                broker.SendRequestAsync(
                    It.IsAny<HttpRequestMessage>(), default),
                        Times.Once);

            this.httpClientBroker.VerifyNoOtherCalls();
        }
    }
}