namespace VoucherManagementACL.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IExamplesProvider{VoucherManagementACL.DataTransferObjects.Responses.RedeemVoucherResponseMessage}" />
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