namespace VoucherManagementACL.BusinessLogic.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using SecurityService.Client;
    using Services;
    using Shared.General;
    using Shouldly;
    using Testing;
    using TransactionProcessor.Client;
    using TransactionProcessor.DataTransferObjects;
    using Xunit;
    using GetVoucherResponse = Models.GetVoucherResponse;
    using RedeemVoucherResponse = Models.RedeemVoucherResponse;

    public class VoucherManagementACLApplicationService
    {
        public VoucherManagementACLApplicationService()
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
        public async Task VoucherManagementACLApplicationService_GetVoucher_VoucherRetrieved()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.GetVoucherByCode(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.GetVoucherResponse);
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            GetVoucherResponse voucherResponse = await applicationService.GetVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.VoucherCode.ShouldBe(TestData.GetVoucherResponse.VoucherCode);
            voucherResponse.ContractId.ShouldBe(TestData.ContractId);
            voucherResponse.EstateId.ShouldBe(TestData.EstateId);
            voucherResponse.ExpiryDate.ShouldBe(TestData.GetVoucherResponse.ExpiryDate);
            voucherResponse.Value.ShouldBe(TestData.GetVoucherResponse.Value);
            voucherResponse.VoucherId.ShouldBe(TestData.GetVoucherResponse.VoucherId);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_GetVoucher_InvalidOperationExceptionErrorInGetVoucher_GetVoucherIsNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.GetVoucherByCode(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new InvalidOperationException(TestData.InvalidOperationErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            GetVoucherResponse voucherResponse = await applicationService.GetVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.InvalidOperationErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.InvalidOperationErrorResponseCode);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_GetVoucher_HttpRequestExceptionErrorInGetVoucher_GetVoucherNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.GetVoucherByCode(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new HttpRequestException(TestData.HttpRequestErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            GetVoucherResponse voucherResponse = await applicationService.GetVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.HttpRequestErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.HttpRequestErrorResponseCode);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_GetVoucher_OtherExceptionErrorInInGetVoucher_GetVoucherNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.GetVoucherByCode(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new Exception(TestData.GeneralErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            GetVoucherResponse voucherResponse = await applicationService.GetVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.GeneralErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.GeneralErrorResponseCode);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_RedeemVoucher_VoucherRedeemed()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.RedeemVoucher(It.IsAny<String>(), It.IsAny<RedeemVoucherRequest>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.RedeemVoucherResponse);
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            RedeemVoucherResponse voucherResponse = await applicationService.RedeemVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.VoucherCode.ShouldBe(TestData.RedeemVoucherResponse.VoucherCode);
            voucherResponse.ContractId.ShouldBe(TestData.ContractId);
            voucherResponse.EstateId.ShouldBe(TestData.EstateId);
            voucherResponse.ExpiryDate.ShouldBe(TestData.RedeemVoucherResponse.ExpiryDate);
            voucherResponse.Balance.ShouldBe(TestData.RedeemVoucherResponse.RemainingBalance);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_RedeemVoucher_InvalidOperationExceptionErrorInGetVoucher_GetVoucherIsNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.RedeemVoucher(It.IsAny<String>(), It.IsAny<RedeemVoucherRequest>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new InvalidOperationException(TestData.InvalidOperationErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            RedeemVoucherResponse voucherResponse = await applicationService.RedeemVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.InvalidOperationErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.InvalidOperationErrorResponseCode);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_RedeemVoucher_HttpRequestExceptionErrorInGetVoucher_GetVoucherNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.RedeemVoucher(It.IsAny<String>(), It.IsAny<RedeemVoucherRequest>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new HttpRequestException(TestData.HttpRequestErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            RedeemVoucherResponse voucherResponse = await applicationService.RedeemVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.HttpRequestErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.HttpRequestErrorResponseCode);
        }

        [Fact]
        public async Task VoucherManagementACLApplicationService_RedeemVoucher_OtherExceptionErrorInInGetVoucher_GetVoucherNotSuccessful()
        {
            Mock<ITransactionProcessorClient> voucherManagementClient = new Mock<ITransactionProcessorClient>();
            voucherManagementClient.Setup(v => v.RedeemVoucher(It.IsAny<String>(), It.IsAny<RedeemVoucherRequest>(), It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new Exception("Error", new Exception(TestData.GeneralErrorResponseMessage)));
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);

            IVoucherManagementACLApplicationService applicationService =
                new Services.VoucherManagementACLApplicationService(securityServiceClient.Object, voucherManagementClient.Object);

            RedeemVoucherResponse voucherResponse = await applicationService.RedeemVoucher(TestData.EstateId, TestData.ContractId, TestData.VoucherCode, CancellationToken.None);

            voucherResponse.ShouldNotBeNull();
            voucherResponse.ResponseMessage.ShouldBe(TestData.GeneralErrorResponseMessage);
            voucherResponse.ResponseCode.ShouldBe(TestData.GeneralErrorResponseCode);
        }


    }
}
