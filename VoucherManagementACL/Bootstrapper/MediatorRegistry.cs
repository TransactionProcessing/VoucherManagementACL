namespace VoucherManagementACL.Bootstrapper
{
    using BusinessLogic.RequestHandlers;
    using BusinessLogic.Requests;
    using Lamar;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    public class MediatorRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MediatorRegistry"/> class.
        /// </summary>
        public MediatorRegistry()
        {
            this.AddTransient<ServiceFactory>(context => { return t => context.GetService(t); });
            this.AddTransient<IMediator, Mediator>();
            this.AddSingleton<IRequestHandler<VersionCheckRequest, Unit>, VersionCheckRequestHandler>();
            this.AddSingleton<IRequestHandler<GetVoucherRequest, GetVoucherResponse>, VoucherRequestHandler>();
            this.AddSingleton<IRequestHandler<RedeemVoucherRequest, RedeemVoucherResponse>, VoucherRequestHandler>();
        }

        #endregion
    }
}