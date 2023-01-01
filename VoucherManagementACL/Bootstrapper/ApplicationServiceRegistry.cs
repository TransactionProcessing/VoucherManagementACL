namespace VoucherManagementACL.Bootstrapper
{
    using System.Diagnostics.CodeAnalysis;
    using BusinessLogic.Services;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;

    [ExcludeFromCodeCoverage]
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