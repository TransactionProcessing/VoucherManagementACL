namespace VoucherManagementACL.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest{VoucherManagementACL.Models.RedeemVoucherResponse}" />
    public class RedeemVoucherRequest : IRequest<RedeemVoucherResponse>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeemVoucherRequest"/> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        private RedeemVoucherRequest(Guid estateId, Guid contractId, String voucherCode)
        {
            this.EstateId = estateId;
            this.ContractId = contractId;
            this.VoucherCode = voucherCode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public Guid ContractId { get; }
        /// <summary>
        /// Gets the voucher code.
        /// </summary>
        /// <value>
        /// The voucher code.
        /// </value>
        public String VoucherCode { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified version number.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <returns></returns>
        public static RedeemVoucherRequest Create(Guid estateId, Guid contractId, String voucherCode)
        {
            return new RedeemVoucherRequest(estateId, contractId, voucherCode);
        }

        #endregion
    }
}