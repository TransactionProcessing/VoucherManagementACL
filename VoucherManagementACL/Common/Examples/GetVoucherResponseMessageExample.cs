namespace VoucherManagementACL.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IExamplesProvider{VoucherManagementACL.DataTransferObjects.Responses.GetVoucherResponseMessage}" />
    public class GetVoucherResponseMessageExample : IExamplesProvider<GetVoucherResponseMessage>
    {
        #region Methods

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public GetVoucherResponseMessage GetExamples()
        {
            return new GetVoucherResponseMessage
                   {
                       Balance = ExampleData.Balance,
                       ContractId = ExampleData.ContractId,
                       EstateId = ExampleData.EstateId,
                       ExpiryDate = ExampleData.ExpiryDate,
                       ResponseCode = ExampleData.ResponseCode,
                       ResponseMessage = ExampleData.ResponseMessage,
                       Value = ExampleData.Value,
                       VoucherCode = ExampleData.VoucherCode,
                       VoucherId = ExampleData.VoucherId
                   };
        }

        #endregion
    }
}