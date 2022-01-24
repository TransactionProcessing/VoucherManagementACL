namespace VoucherManagementACL.Bootstrapper
{
    using BusinessLogic.Services;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    public class ApplicationServiceRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationServiceRegistry"/> class.
        /// </summary>
        public ApplicationServiceRegistry()
        {
            this.AddSingleton<IVoucherManagementACLApplicationService, VoucherManagementACLApplicationService>();
        }

        #endregion
    }
}