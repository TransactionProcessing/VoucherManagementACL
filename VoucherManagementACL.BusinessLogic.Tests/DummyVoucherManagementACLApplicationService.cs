namespace VoucherManagementACL.BusinessLogic.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Services;

public class DummyVoucherManagementACLApplicationService : IVoucherManagementACLApplicationService
{
    public async Task<GetVoucherResponse> GetVoucher(Guid estateId,
                                                     Guid contractId,
                                                     String voucherCode,
                                                     CancellationToken cancellationToken) {
        return new GetVoucherResponse();
    }

    public async Task<RedeemVoucherResponse> RedeemVoucher(Guid estateId,
                                                           Guid contractId,
                                                           String voucherCode,
                                                           CancellationToken cancellationToken) {
        return new RedeemVoucherResponse();
    }
}