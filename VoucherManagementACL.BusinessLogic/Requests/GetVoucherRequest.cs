namespace VoucherManagementACL.BusinessLogic.Requests
{
    using System;
    using MediatR;
    using Models;

    public class GetVoucherRequest : IRequest<GetVoucherResponse>
    {
        #region Constructors

        private GetVoucherRequest(Guid estateId, Guid contractId, String voucherCode)
        {
            this.EstateId = estateId;
            this.ContractId = contractId;
            this.VoucherCode = voucherCode;
        }

        #endregion

        #region Properties

        public Guid EstateId { get; }
        public Guid ContractId { get; }
        public String VoucherCode { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified version number.
        /// </summary>
        /// <param name="versionNumber">The version number.</param>
        /// <returns></returns>
        public static GetVoucherRequest Create(Guid estateId, Guid contractId, String voucherCode)
        {
            return new GetVoucherRequest(estateId,contractId,voucherCode);
        }

        #endregion
    }
}