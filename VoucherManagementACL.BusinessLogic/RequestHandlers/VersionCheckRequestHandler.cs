namespace VoucherManagementACL.BusinessLogic.RequestHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using MediatR;
    using Requests;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="VersionCheckRequest" />
    public class VersionCheckRequestHandler : IRequestHandler<VersionCheckRequest>
    {
        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Version number [{requestVersion}] is less than the Minimum Supported version [{minimumVersion}]</exception>
        public async Task<Unit> Handle(VersionCheckRequest request,
                                       CancellationToken cancellationToken)
        {
            // Get the minimum version from the config
            String versionFromConfig = ConfigurationReader.GetValue("AppSettings", "MinimumSupportedApplicationVersion");

            // Convert to an assembly version
            Version minimumVersion = Version.Parse(versionFromConfig);

            Version.TryParse(request.VersionNumber, out Version requestVersion);

            if (requestVersion == null || requestVersion.CompareTo(minimumVersion) < 0)
            {
                // This is not compatible
                throw new VersionIncompatibleException($"Version Mismatch - Version number [{requestVersion}] is less than the Minimum Supported version [{minimumVersion}]");
            }

            return default;
        }

        #endregion
    }
}