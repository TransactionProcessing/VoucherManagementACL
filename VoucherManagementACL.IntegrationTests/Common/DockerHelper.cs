using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.IntegrationTests.Common
{
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateManagement.Client;
    using EstateReporting.Database;
    using EventStore.Client;
    using global::Shared.Logger;
    using Microsoft.Data.SqlClient;
    using SecurityService.Client;
    using VoucherManagement.Client;

    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        #region Fields

        /// <summary>
        /// The estate client
        /// </summary>
        public IEstateClient EstateClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;

        public IVoucherManagementClient VoucherManagementClient;

        /// <summary>
        /// The test identifier
        /// </summary>
        public Guid TestId;
        
        /// <summary>
        /// The containers
        /// </summary>
        protected List<IContainerService> Containers;

        /// <summary>
        /// The estate management API port
        /// </summary>
        protected Int32 EstateManagementApiPort;

        /// <summary>
        /// The event store HTTP port
        /// </summary>
        protected Int32 EventStoreHttpPort;

        /// <summary>
        /// The security service port
        /// </summary>
        protected Int32 SecurityServicePort;

        /// <summary>
        /// The test networks
        /// </summary>
        protected List<INetworkService> TestNetworks;

        protected String SecurityServiceContainerName;

        protected String EstateManagementContainerName;

        protected String EventStoreContainerName;

        protected String EstateReportingContainerName;

        protected String SubscriptionServiceContainerName;

        protected String VoucherManagementContainerName;

        protected String VoucherManagementACLContainerName;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly NlogLogger Logger;

        private readonly TestingContext TestingContext;

        private Int32 VoucherManagementPort;
        private Int32 VoucherManagementACLPort;

        /// <summary>
        /// The HTTP client
        /// </summary>
        public HttpClient HttpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="testingContext">The testing context.</param>
        public DockerHelper(NlogLogger logger, TestingContext testingContext)
        {
            this.Logger = logger;
            this.TestingContext = testingContext;
            this.Containers = new List<IContainerService>();
            this.TestNetworks = new List<INetworkService>();
        }

        #endregion

        //public const Int32 VoucherManagementDockerPort = 5007;
        //public const Int32 VoucherManagementACLDockerPort = 5008;

        //public static IContainerService SetupVoucherManagementACLContainer(String containerName, ILogger logger, String imageName,
        //                                                       List<INetworkService> networkServices,
        //                                                       String hostFolder,
        //                                                       (String URL, String UserName, String Password)? dockerCredentials,
        //                                                       String securityServiceContainerName,
        //                                                       String voucherManagementContainerName,
        //                                                       (string clientId, string clientSecret) clientDetails,
        //                                                       Boolean forceLatestImage = false,
        //                                                       Int32 securityServicePort = DockerHelper.SecurityServiceDockerPort,
        //                                                       List<String> additionalEnvironmentVariables = null)
        //{
        //    logger.LogInformation("About to Start Voucher Management ACL Container");

        //    List<String> environmentVariables = new List<String>();
        //    environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
        //    environmentVariables.Add($"AppSettings:VoucherManagementApi=http://{voucherManagementContainerName}:{DockerHelper.VoucherManagementDockerPort}");
        //    environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
        //    environmentVariables.Add($"urls=http://*:{DockerHelper.VoucherManagementACLDockerPort}");
        //    environmentVariables.Add($"AppSettings:ClientId={clientDetails.clientId}");
        //    environmentVariables.Add($"AppSettings:ClientSecret={clientDetails.clientSecret}");

        //    if (additionalEnvironmentVariables != null)
        //    {
        //        environmentVariables.AddRange(additionalEnvironmentVariables);
        //    }

        //    ContainerBuilder voucherManagementAclContainer = new Builder().UseContainer().WithName(containerName)
        //                                                               .WithEnvironment(environmentVariables.ToArray())
        //                                                      .UseImage(imageName, forceLatestImage).ExposePort(DockerHelper.VoucherManagementACLDockerPort)
        //                                                      .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

        //    if (String.IsNullOrEmpty(hostFolder) == false)
        //    {
        //        voucherManagementAclContainer = voucherManagementAclContainer.Mount(hostFolder, "/home/txnproc/trace", MountType.ReadWrite);
        //    }

        //    if (dockerCredentials.HasValue)
        //    {
        //        voucherManagementAclContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
        //    }

        //    // Now build and return the container                
        //    IContainerService builtContainer = voucherManagementAclContainer.Build().Start().WaitForPort($"{DockerHelper.VoucherManagementACLDockerPort}/tcp", 30000);

        //    logger.LogInformation("Voucher Management ACL Container Started");

        //    return builtContainer;
        //}

        //public static IContainerService SetupVoucherManagementContainer(String containerName, ILogger logger, String imageName,
        //                                                       List<INetworkService> networkServices,
        //                                                       String hostFolder,
        //                                                       (String URL, String UserName, String Password)? dockerCredentials,
        //                                                       String securityServiceContainerName,
        //                                                       String estateManagementContainerName,
        //                                                       String eventStoreContainerName,
        //                                                       (String sqlServerContainerName, String sqlServerUserName, String sqlServerPassword)
        //                                                           sqlServerDetails,
        //                                                       (string clientId, string clientSecret) clientDetails,
        //                                                       Boolean forceLatestImage = false,
        //                                                       Int32 securityServicePort = DockerHelper.SecurityServiceDockerPort,
        //                                                       List<String> additionalEnvironmentVariables = null)
        //{
        //    logger.LogInformation("About to Start Voucher Management Container");

        //    List<String> environmentVariables = new List<String>();
        //    environmentVariables.Add($"EventStoreSettings:ConnectionString=https://{eventStoreContainerName}:{DockerHelper.EventStoreHttpDockerPort}");
        //    environmentVariables.Add($"AppSettings:SecurityService=http://{securityServiceContainerName}:{securityServicePort}");
        //    environmentVariables.Add($"AppSettings:EstateManagementApi=http://{estateManagementContainerName}:{DockerHelper.EstateManagementDockerPort}");
        //    environmentVariables.Add($"SecurityConfiguration:Authority=http://{securityServiceContainerName}:{securityServicePort}");
        //    environmentVariables.Add($"urls=http://*:{DockerHelper.VoucherManagementDockerPort}");
        //    environmentVariables.Add($"AppSettings:ClientId={clientDetails.clientId}");
        //    environmentVariables.Add($"AppSettings:ClientSecret={clientDetails.clientSecret}");
        //    environmentVariables
        //        .Add($"ConnectionStrings:EstateReportingReadModel=\"server={sqlServerDetails.sqlServerContainerName};user id={sqlServerDetails.sqlServerUserName};password={sqlServerDetails.sqlServerPassword};database=EstateReportingReadModel\"");

        //    if (additionalEnvironmentVariables != null)
        //    {
        //        environmentVariables.AddRange(additionalEnvironmentVariables);
        //    }

        //    ContainerBuilder voucherManagementContainer = new Builder().UseContainer().WithName(containerName)
        //                                                               .WithEnvironment(environmentVariables.ToArray())
        //                                                      .UseImage(imageName, forceLatestImage).ExposePort(DockerHelper.VoucherManagementDockerPort)
        //                                                      .UseNetwork(networkServices.ToArray()).Mount(hostFolder, "/home", MountType.ReadWrite);

        //    if (String.IsNullOrEmpty(hostFolder) == false)
        //    {
        //        voucherManagementContainer = voucherManagementContainer.Mount(hostFolder, "/home/txnproc/trace", MountType.ReadWrite);
        //    }

        //    if (dockerCredentials.HasValue)
        //    {
        //        voucherManagementContainer.WithCredential(dockerCredentials.Value.URL, dockerCredentials.Value.UserName, dockerCredentials.Value.Password);
        //    }

        //    // Now build and return the container                
        //    IContainerService builtContainer = voucherManagementContainer.Build().Start().WaitForPort($"{DockerHelper.VoucherManagementDockerPort}/tcp", 30000);

        //    logger.LogInformation("Voucher Management  Container Started");

        //    return builtContainer;
        //}

        private async Task LoadEventStoreProjections()
        {
            //Start our Continous Projections - we might decide to do this at a different stage, but now lets try here
            String projectionsFolder = "../../../projections/continuous";
            IPAddress[] ipAddresses = Dns.GetHostAddresses("127.0.0.1");

            if (!String.IsNullOrWhiteSpace(projectionsFolder))
            {
                DirectoryInfo di = new DirectoryInfo(projectionsFolder);

                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles();

                    EventStoreProjectionManagementClient projectionClient = new EventStoreProjectionManagementClient(ConfigureEventStoreSettings(this.EventStoreHttpPort));

                    foreach (FileInfo file in files)
                    {
                        String projection = File.ReadAllText(file.FullName);
                        String projectionName = file.Name.Replace(".js", String.Empty);

                        try
                        {
                            Logger.LogInformation($"Creating projection [{projectionName}]");
                            await projectionClient.CreateContinuousAsync(projectionName, projection, trackEmittedStreams: true).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(new Exception($"Projection [{projectionName}] error", e));
                        }
                    }
                }
            }

            Logger.LogInformation("Loaded projections");
        }

        #region Methods

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            String traceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            this.SecurityServiceContainerName = $"securityservice{testGuid:N}";
            this.EstateManagementContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            this.EstateReportingContainerName = $"estatereporting{testGuid:N}";
            this.SubscriptionServiceContainerName = $"subscription{testGuid:N}";
            this.VoucherManagementContainerName = $"vouchermanagement{testGuid:N}";
            this.VoucherManagementACLContainerName = $"vouchermanagementacl{testGuid:N}";

            String eventStoreAddress = $"http://{this.EventStoreContainerName}";

            (String, String, String) dockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);
            IContainerService eventStoreContainer =
                DockerHelper.SetupEventStoreContainer(this.EventStoreContainerName, this.Logger, "eventstore/eventstore:20.10.0-buster-slim", testNetwork, traceFolder);
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint($"{DockerHelper.EventStoreHttpDockerPort}/tcp").Port;

            await Retry.For(async () => { await this.PopulateSubscriptionServiceConfiguration().ConfigureAwait(false); },
                            retryFor:TimeSpan.FromMinutes(2),
                            retryInterval:TimeSpan.FromSeconds(30));

            IContainerService estateManagementContainer = DockerHelper.SetupEstateManagementContainer(this.EstateManagementContainerName,
                                                                                                      this.Logger,
                                                                                                      "stuartferguson/estatemanagement",
                                                                                                      new List<INetworkService>
                                                                                                      {
                                                                                                          testNetwork,
                                                                                                          Setup.DatabaseServerNetwork
                                                                                                      },
                                                                                                      traceFolder,
                                                                                                      dockerCredentials,
                                                                                                      this.SecurityServiceContainerName,
                                                                                                      eventStoreAddress,
                                                                                                      (Setup.SqlServerContainerName, "sa", "thisisalongpassword123!"),
                                                                                                      ("serviceClient", "Secret1"),
                                                                                                      true);

            IContainerService securityServiceContainer = DockerHelper.SetupSecurityServiceContainer(this.SecurityServiceContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    traceFolder,
                                                                                                    dockerCredentials,
                                                                                                    true);

            IContainerService voucherManagementContainer = DockerHelper.SetupVoucherManagementContainer(this.VoucherManagementContainerName,
                                                                                                        this.Logger,
                                                                                                        "stuartferguson/vouchermanagement",
                                                                                                        new List<INetworkService>
                                                                                                        {
                                                                                                            testNetwork,
                                                                                                            Setup.DatabaseServerNetwork
                                                                                                        },
                                                                                                        traceFolder,
                                                                                                        dockerCredentials,
                                                                                                        this.SecurityServiceContainerName,
                                                                                                        this.EstateManagementContainerName,
                                                                                                        eventStoreAddress,
                                                                                                        (Setup.SqlServerContainerName, "sa", "thisisalongpassword123!"),
                                                                                                        ("serviceClient", "Secret1"),
                                                                                                        true);

            IContainerService voucherManagementAclContainer = SetupVoucherManagementACLContainer(this.VoucherManagementACLContainerName,
                                                                                                 this.Logger,
                                                                                                 "vouchermanagementacl",
                                                                                                 new List<INetworkService>
                                                                                                 {
                                                                                                     testNetwork
                                                                                                 },
                                                                                                 traceFolder,
                                                                                                 dockerCredentials,
                                                                                                 this.SecurityServiceContainerName,
                                                                                                 this.VoucherManagementContainerName,
                                                                                                 ("serviceClient", "Secret1"));

            IContainerService estateReportingContainer = DockerHelper.SetupEstateReportingContainer(this.EstateReportingContainerName,
                                                                                                    this.Logger,
                                                                                                    "stuartferguson/estatereporting",
                                                                                                    new List<INetworkService>
                                                                                                    {
                                                                                                        testNetwork,
                                                                                                        Setup.DatabaseServerNetwork
                                                                                                    },
                                                                                                    traceFolder,
                                                                                                    dockerCredentials,
                                                                                                    this.SecurityServiceContainerName,
                                                                                                    eventStoreAddress,
                                                                                                    (Setup.SqlServerContainerName, "sa", "thisisalongpassword123!"),
                                                                                                    ("serviceClient", "Secret1"),
                                                                                                    true);

            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         voucherManagementContainer,
                                         estateReportingContainer,
                                         voucherManagementAclContainer
                                     });

            // Cache the ports
            this.EstateManagementApiPort = estateManagementContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.VoucherManagementPort = voucherManagementContainer.ToHostExposedEndpoint("5007/tcp").Port;
            this.VoucherManagementACLPort = voucherManagementAclContainer.ToHostExposedEndpoint("5008/tcp").Port;

            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementApiPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"http://127.0.0.1:{this.SecurityServicePort}";
            String VoucherManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.VoucherManagementPort}";
            String VoucherManagementAclBaseAddressResolver(String api) => $"http://127.0.0.1:{this.VoucherManagementACLPort}";

            HttpClient httpClient = new HttpClient();
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            this.VoucherManagementClient = new VoucherManagementClient(VoucherManagementBaseAddressResolver, httpClient);

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(VoucherManagementAclBaseAddressResolver(string.Empty));

            await this.LoadEventStoreProjections().ConfigureAwait(false);
        }
        protected async Task PopulateSubscriptionServiceConfiguration()
        {
            EventStorePersistentSubscriptionsClient client = new EventStorePersistentSubscriptionsClient(ConfigureEventStoreSettings(this.EventStoreHttpPort));

            PersistentSubscriptionSettings settings = new PersistentSubscriptionSettings(resolveLinkTos: true);

            await client.CreateAsync("$ce-EstateAggregate", "Reporting", settings);
            await client.CreateAsync("$ce-MerchantAggregate", "Reporting", settings);
            await client.CreateAsync("$ce-ContractAggregate", "Reporting", settings);
            await client.CreateAsync("$ce-VoucherAggregate", "Reporting", settings);
        }

        private static EventStoreClientSettings ConfigureEventStoreSettings(Int32 eventStoreHttpPort)
        {
            String connectionString = $"http://127.0.0.1:{eventStoreHttpPort}";

            EventStoreClientSettings settings = new EventStoreClientSettings();
            settings.CreateHttpMessageHandler = () => new SocketsHttpHandler
                                                      {
                                                          SslOptions =
                                                          {
                                                              RemoteCertificateValidationCallback = (sender,
                                                                                                     certificate,
                                                                                                     chain,
                                                                                                     errors) => true,
                                                          }
                                                      };
            settings.ConnectionName = "Specflow";
            settings.ConnectivitySettings = new EventStoreClientConnectivitySettings
                                            {
                                                Address = new Uri(connectionString),
                                            };

            settings.DefaultCredentials = new UserCredentials("admin", "changeit");
            return settings;
        }

        private async Task RemoveEstateReadModel()
        {
            List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

            foreach (Guid estateId in estateIdList)
            {
                String databaseName = $"EstateReportingReadModel{estateId}";

                await Retry.For(async () =>
                {
                    // Build the connection string (to master)
                    String connectionString = Setup.GetLocalConnectionString(databaseName);
                    EstateReportingContext context = new EstateReportingContext(connectionString);
                    await context.Database.EnsureDeletedAsync(CancellationToken.None);
                });
            }
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun()
        {
            await RemoveEstateReadModel().ConfigureAwait(false);

            if (this.Containers.Any())
            {
                foreach (IContainerService containerService in this.Containers)
                {
                    containerService.StopOnDispose = true;
                    containerService.RemoveOnDispose = true;
                    containerService.Dispose();
                }
            }

            if (this.TestNetworks.Any())
            {
                foreach (INetworkService networkService in this.TestNetworks)
                {
                    networkService.Stop();
                    networkService.Remove(true);
                }
            }
        }

        #endregion
    }
}
