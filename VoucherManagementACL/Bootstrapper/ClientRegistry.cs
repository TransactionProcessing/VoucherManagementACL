namespace VoucherManagementACL.Bootstrapper
{
    using System;
    using System.Net.Http;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;
    using SecurityService.Client;
    using Shared.General;
    using VoucherManagement.Client;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    public class ClientRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRegistry"/> class.
        /// </summary>
        public ClientRegistry()
        {
            this.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
            this.AddSingleton<IVoucherManagementClient, VoucherManagementClient>();
            this.AddSingleton<Func<String, String>>(container => serviceName => { return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString; });
            HttpClientHandler httpClientHandler = new HttpClientHandler
                                                  {
                                                      ServerCertificateCustomValidationCallback = (message,
                                                                                                   certificate2,
                                                                                                   arg3,
                                                                                                   arg4) =>
                                                                                                  {
                                                                                                      return true;
                                                                                                  }
                                                  };
            HttpClient httpClient = new HttpClient(httpClientHandler);
            this.AddSingleton(httpClient);
        }

        #endregion
    }
}