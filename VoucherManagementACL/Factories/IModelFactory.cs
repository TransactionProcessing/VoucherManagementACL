namespace VoucherManagementACL.Factories
{
    using DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        GetVoucherResponseMessage ConvertFrom(GetVoucherResponse model);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        RedeemVoucherResponseMessage ConvertFrom(RedeemVoucherResponse model);

        #endregion
    }
}