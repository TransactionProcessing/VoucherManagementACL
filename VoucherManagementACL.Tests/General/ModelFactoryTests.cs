namespace VoucherManagementACL.Tests.General
{
    using DataTransferObjects.Responses;
    using Models;
    using Shouldly;
    using Testing;
    using VoucherManagementACL.Factories;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_ConvertFrom_GetVoucherResponse_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();

            GetVoucherResponse model = TestData.GetVoucherResponseModel;
            GetVoucherResponseMessage dto = modelFactory.ConvertFrom(model);

            dto.ShouldNotBeNull();
            dto.ContractId.ShouldBe(model.ContractId);
            dto.VoucherCode.ShouldBe(model.VoucherCode);
            dto.EstateId.ShouldBe(model.EstateId);
            dto.Value.ShouldBe(model.Value);
            dto.ExpiryDate.ShouldBe(model.ExpiryDate);
            dto.VoucherId.ShouldBe(model.VoucherId);
            dto.ResponseMessage.ShouldBe(model.ResponseMessage);
            dto.ResponseCode.ShouldBe(model.ResponseCode);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_GetVoucherResponse_NullValue_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();

            GetVoucherResponse model = null;
            GetVoucherResponseMessage dto = modelFactory.ConvertFrom(model);

            dto.ShouldBeNull();
        }
    }
}
