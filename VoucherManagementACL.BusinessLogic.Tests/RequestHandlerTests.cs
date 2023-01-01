namespace VoucherManagementACL.BusinessLogic.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using RequestHandlers;
    using Requests;
    using Services;
    using Shared.General;
    using Shouldly;
    using Testing;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class RequestHandlerTests
    {
        #region Methods

        public RequestHandlerTests()
        {
            this.SetupMemoryConfiguration();
        }

        private void SetupMemoryConfiguration()
        {
            if (ConfigurationReader.IsInitialised == false)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
                ConfigurationReader.Initialise(configuration);
            }
        }

        [Fact]
        public async Task VoucherRequestHandler_GetVoucherRequest_Handle_RequestIsHandled()
        {
            Mock<IVoucherManagementACLApplicationService> voucherManagementACLApplicationService = new Mock<IVoucherManagementACLApplicationService>();
            VoucherRequestHandler requestHandler = new VoucherRequestHandler(voucherManagementACLApplicationService.Object);

            Should.NotThrow(async () =>
                            {
                                await requestHandler.Handle(TestData.GetVoucherRequest, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task VoucherRequestHandler_RedeemVoucherRequest_Handle_RequestIsHandled()
        {
            Mock<IVoucherManagementACLApplicationService> voucherManagementACLApplicationService = new Mock<IVoucherManagementACLApplicationService>();
            VoucherRequestHandler requestHandler = new VoucherRequestHandler(voucherManagementACLApplicationService.Object);
            
            Should.NotThrow(async () =>
                            {
                                await requestHandler.Handle(TestData.RedeemVoucherRequest, CancellationToken.None);
                            });
        }


        [Fact]
        public async Task VersionCheckRequestHandler_Handle_RequestIsHandled()
        {
            VersionCheckRequestHandler requestHandler = new VersionCheckRequestHandler();
            
            Should.NotThrow(async () =>
                            {
                                await requestHandler.Handle(TestData.VersionCheckRequest, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task VersionCheckRequestHandler_Handle_OldVersion_ErrorThrown()
        {
            VersionCheckRequestHandler requestHandler = new VersionCheckRequestHandler();
            
            VersionCheckRequest request = VersionCheckRequest.Create(TestData.OldApplicationVersion);
            Should.Throw<VersionIncompatibleException>(async () =>
                            {
                                await requestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task VersionCheckRequestHandler_Handle_NewerVersionBuildNumber_RequestIsHandled()
        {
            VersionCheckRequestHandler requestHandler = new VersionCheckRequestHandler();
            
            VersionCheckRequest request = VersionCheckRequest.Create(TestData.NewerApplicationVersion);
            Should.NotThrow(async () =>
                            {
                                await requestHandler.Handle(request, CancellationToken.None);
                            });
        }

        [Fact]
        public async Task VersionCheckRequestHandler_Handle_InvalidVersionBuildNumber_RequestIsHandled()
        {
            VersionCheckRequestHandler requestHandler = new VersionCheckRequestHandler();

            VersionCheckRequest request = VersionCheckRequest.Create("invalid");
            Should.Throw<VersionIncompatibleException>(async () =>
                                                       {
                                                           await requestHandler.Handle(request, CancellationToken.None);
                                                       }); ;
        }

        #endregion
    }
}