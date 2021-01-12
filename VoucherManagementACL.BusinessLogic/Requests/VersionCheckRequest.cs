namespace VoucherManagementACL.BusinessLogic.Requests
{
    using System;
    using MediatR;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest" />
    public class VersionCheckRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCheckRequest"/> class.
        /// </summary>
        /// <param name="versionNumber">The version number.</param>
        private VersionCheckRequest(String versionNumber)
        {
            this.VersionNumber = versionNumber;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the version number.
        /// </summary>
        /// <value>
        /// The version number.
        /// </value>
        public String VersionNumber { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified version number.
        /// </summary>
        /// <param name="versionNumber">The version number.</param>
        /// <returns></returns>
        public static VersionCheckRequest Create(String versionNumber)
        {
            return new VersionCheckRequest(versionNumber);
        }

        #endregion
    }
}