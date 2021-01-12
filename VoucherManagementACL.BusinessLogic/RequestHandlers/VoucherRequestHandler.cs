namespace VoucherManagementACL.BusinessLogic.RequestHandlers
{
    using MediatR;
    using Models;
    using Requests;
    using Services;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoucherRequestHandler : IRequestHandler<GetVoucherRequest, GetVoucherResponse>
    {
        #region Fields

        /// <summary>
        /// The application service
        /// </summary>
        private readonly IVoucherManagementACLApplicationService ApplicationService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherRequestHandler"/> class.
        /// </summary>
        /// <param name="applicationService">The application service.</param>
        public VoucherRequestHandler(IVoucherManagementACLApplicationService applicationService)
        {
            this.ApplicationService = applicationService;
        }

        public async Task<GetVoucherResponse> Handle(GetVoucherRequest request, CancellationToken cancellationToken)
        {
            return await this.ApplicationService.GetVoucher(request.EstateId, request.ContractId, request.VoucherCode, cancellationToken);
        }

        #endregion

        #region Methods

        #endregion
    }
}