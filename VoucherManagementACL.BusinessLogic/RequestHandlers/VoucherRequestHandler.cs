namespace VoucherManagementACL.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler{VoucherManagementACL.BusinessLogic.Requests.GetVoucherRequest, VoucherManagementACL.Models.GetVoucherResponse}" />
    /// <seealso cref="MediatR.IRequestHandler{VoucherManagementACL.BusinessLogic.Requests.RedeemVoucherRequest, VoucherManagementACL.Models.RedeemVoucherResponse}" />
    public class VoucherRequestHandler : IRequestHandler<GetVoucherRequest, GetVoucherResponse>, IRequestHandler<RedeemVoucherRequest, RedeemVoucherResponse>
    {
        #region Fields

        /// <summary>
        /// The application service
        /// </summary>
        private readonly IVoucherManagementACLApplicationService ApplicationService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherRequestHandler" /> class.
        /// </summary>
        /// <param name="applicationService">The application service.</param>
        public VoucherRequestHandler(IVoucherManagementACLApplicationService applicationService)
        {
            this.ApplicationService = applicationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<GetVoucherResponse> Handle(GetVoucherRequest request,
                                                     CancellationToken cancellationToken)
        {
            return await this.ApplicationService.GetVoucher(request.EstateId, request.ContractId, request.VoucherCode, cancellationToken);
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<RedeemVoucherResponse> Handle(RedeemVoucherRequest request,
                                                        CancellationToken cancellationToken)
        {
            return await this.ApplicationService.RedeemVoucher(request.EstateId, request.ContractId, request.VoucherCode, cancellationToken);
        }

        #endregion
    }
}