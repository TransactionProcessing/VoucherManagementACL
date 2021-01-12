namespace VoucherManagementACL.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IVoucherManagementACLApplicationService
    {
        /// <summary>
        /// Processes the logon transaction.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetVoucherResponse> GetVoucher(Guid estateId,
                                            Guid contractId,
                                            String voucherCode,
                                            CancellationToken cancellationToken);
    }


}
