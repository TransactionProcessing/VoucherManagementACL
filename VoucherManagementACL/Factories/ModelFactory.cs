namespace VoucherManagementACL.Factories
{
    using DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        

        #endregion

        public GetVoucherResponseMessage ConvertFrom(GetVoucherResponse model)
        {
            if (model == null)
            {
                return null;
            }

            GetVoucherResponseMessage responseMessage = new GetVoucherResponseMessage
                                                        {
                                                            ContractId = model.ContractId,
                                                            EstateId = model.EstateId,
                                                            VoucherCode = model.VoucherCode,
                                                            VoucherId = model.VoucherId,
                                                            ExpiryDate = model.ExpiryDate,
                                                            Value = model.Value,
                                                            ResponseMessage = model.ResponseMessage,
                                                            ResponseCode = model.ResponseCode
                                                        };

            return responseMessage;
        }
    }
}