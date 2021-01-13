namespace VoucherManagementACL.Factories
{
    using DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="VoucherManagementACL.Factories.IModelFactory" />
    /// <seealso cref="IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public RedeemVoucherResponseMessage ConvertFrom(RedeemVoucherResponse model)
        {
            if (model == null)
            {
                return null;
            }

            RedeemVoucherResponseMessage responseMessage = new RedeemVoucherResponseMessage
                                                    {
                                                        VoucherCode = model.VoucherCode,
                                                        Balance = model.Balance,
                                                        ResponseMessage = model.ResponseMessage,
                                                        ContractId = model.ContractId,
                                                        EstateId = model.EstateId,
                                                        ExpiryDate = model.ExpiryDate,
                                                        ResponseCode = model.ResponseCode
                                                    };

            return responseMessage;
        }

        #endregion
    }
}