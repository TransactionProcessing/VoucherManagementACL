namespace VoucherManagementACL.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class RedeemVoucherResponseMessageExample : IExamplesProvider<RedeemVoucherResponseMessage>
    {
        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public RedeemVoucherResponseMessage GetExamples()
        {
            return new RedeemVoucherResponseMessage
                   {
                       Balance = ExampleData.Balance,
                       ContractId = ExampleData.ContractId,
                       EstateId = ExampleData.EstateId,
                       ExpiryDate = ExampleData.ExpiryDate,
                       ResponseCode = ExampleData.ResponseCode,
                       ResponseMessage = ExampleData.ResponseMessage,
                       VoucherCode = ExampleData.VoucherCode
                   };
        }
    }
}