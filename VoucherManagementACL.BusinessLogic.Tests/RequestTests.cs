namespace VoucherManagementACL.BusinessLogic.Tests
{
    using Requests;
    using Shouldly;
    using Testing;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class RequestTests
    {
        #region Methods

        /// <summary>
        /// Processes the logon transaction request can be created is created.
        /// </summary>
        [Fact]
        public void GetVoucherRequest_CanBeCreated_IsCreated()
        {
            GetVoucherRequest request = GetVoucherRequest.Create(TestData.EstateId, TestData.ContractId, TestData.VoucherCode);

            request.EstateId.ShouldBe(TestData.EstateId);
            request.ContractId.ShouldBe(TestData.ContractId);
            request.VoucherCode.ShouldBe(TestData.VoucherCode);
        }
        
        [Fact]
        public void VersionCheckRequest_CanBeCreated_IsCreated()
        {
            VersionCheckRequest request = VersionCheckRequest.Create(TestData.ApplicationVersion);

            request.VersionNumber.ShouldBe(TestData.ApplicationVersion);
        }

        [Fact]
        public void RedeemVoucherRequest_CanBeCreated_IsCreated()
        {
            RedeemVoucherRequest request = RedeemVoucherRequest.Create(TestData.EstateId, TestData.ContractId, TestData.VoucherCode);

            request.EstateId.ShouldBe(TestData.EstateId);
            request.ContractId.ShouldBe(TestData.ContractId);
            request.VoucherCode.ShouldBe(TestData.VoucherCode);
        }

        #endregion
    }
}