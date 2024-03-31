// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Net;
using Moq;
using STX.REST.RESTFulSense.Clients.Brokers.Errors;
using STX.REST.RESTFulSense.Clients.Models.Errors;
using STX.REST.RESTFulSense.Clients.Services.Foundations.ErrorMappers;
using Tynamix.ObjectFiller;

namespace STX.REST.RESTFulSense.Clients.Tests.Unit.Services.Foundations.ErrorMappers
{
    public partial class ErrorMapperServiceTests
    {
        private readonly Mock<IErrorBroker> errorBrokerMock;
        private readonly ErrorMapperService errorMapperService;

        public ErrorMapperServiceTests()
        {
            this.errorBrokerMock = new Mock<IErrorBroker>();
            
            this.errorMapperService =
                new ErrorMapperService(errorBrokerMock.Object);
        }

        private static HttpStatusCode GetRandomHttpStatusCode()
        {
            int min = (int)HttpStatusCode.Continue;
            int max = (int)HttpStatusCode.HttpVersionNotSupported;
            Random random = new Random();
            
            HttpStatusCode randomHttpStatusCode =
                (HttpStatusCode)random.Next(min, max + 1);

            return randomHttpStatusCode;
        }

        private static StatusDetail CreateRandomStatusDetail(int statusCode) =>
            CreateStatusDetailFiller(statusCode).Create();

        private static Filler<StatusDetail> CreateStatusDetailFiller(int statusCode)
        {
            var filler = new Filler<StatusDetail>();

            filler.Setup()
                .OnProperty(statusDetail => statusDetail.Code)
                .Use(statusCode);

            return filler;
        }
    }
}