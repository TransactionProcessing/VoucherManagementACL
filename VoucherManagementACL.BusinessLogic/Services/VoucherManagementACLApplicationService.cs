namespace VoucherManagementACL.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.General;
    using VoucherManagement.Client;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ITransactionProcessorACLApplicationService" />
    public class VoucherManagementACLApplicationService : IVoucherManagementACLApplicationService
    {
        #region Fields

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        private readonly IVoucherManagementClient VoucherManagementClient;

        #endregion

        #region Constructors

        public VoucherManagementACLApplicationService(ISecurityServiceClient securityServiceClient,
                                                      IVoucherManagementClient voucherManagementClient)
        {
            this.SecurityServiceClient = securityServiceClient;
            this.VoucherManagementClient = voucherManagementClient;
        }

        #endregion

        #region Methods
        
        /*public async Task<ProcessLogonTransactionResponse> ProcessLogonTransaction(Guid estateId,
                                                                                   Guid merchantId,
                                                                                   DateTime transactionDateTime,
                                                                                   String transactionNumber,
                                                                                   String deviceIdentifier,
                                                                                   CancellationToken cancellationToken)
        {
            // Get a client token to call the Transaction Processor
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");

            TokenResponse accessToken = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);

            LogonTransactionRequest logonTransactionRequest = new LogonTransactionRequest();
            logonTransactionRequest.TransactionNumber = transactionNumber;
            logonTransactionRequest.DeviceIdentifier = deviceIdentifier;
            logonTransactionRequest.TransactionDateTime = transactionDateTime;
            logonTransactionRequest.TransactionType = "LOGON";

            SerialisedMessage requestSerialisedMessage = new SerialisedMessage();
            requestSerialisedMessage.Metadata.Add("EstateId", estateId.ToString());
            requestSerialisedMessage.Metadata.Add("MerchantId", merchantId.ToString());
            requestSerialisedMessage.SerialisedData = JsonConvert.SerializeObject(logonTransactionRequest,
                                                                                  new JsonSerializerSettings
                                                                                  {
                                                                                      TypeNameHandling = TypeNameHandling.All
                                                                                  });

            ProcessLogonTransactionResponse response = null;

            try
            {
                SerialisedMessage responseSerialisedMessage =
                    await this.TransactionProcessorClient.PerformTransaction(accessToken.AccessToken, requestSerialisedMessage, cancellationToken);

                LogonTransactionResponse logonTransactionResponse = JsonConvert.DeserializeObject<LogonTransactionResponse>(responseSerialisedMessage.SerialisedData);

                response = new ProcessLogonTransactionResponse
                           {
                               ResponseCode = logonTransactionResponse.ResponseCode,
                               ResponseMessage = logonTransactionResponse.ResponseMessage,
                               EstateId = estateId,
                               MerchantId = merchantId
                           };
            }
            catch(Exception ex)
            {
                if (ex.InnerException is InvalidOperationException)
                {
                    // This means there is an error in the request
                    response = new ProcessLogonTransactionResponse
                               {
                                   ResponseCode = "0001", // Request Message error
                                   ResponseMessage = ex.InnerException.Message,
                                   EstateId = estateId,
                                   MerchantId = merchantId
                    };
                }
                else if (ex.InnerException is HttpRequestException)
                {
                    // Request Send Exception
                    response = new ProcessLogonTransactionResponse
                    {
                                   ResponseCode = "0002", // Request Message error
                                   ResponseMessage = "Error Sending Request Message",
                                   EstateId = estateId,
                                   MerchantId = merchantId
                    };
                }
                else
                {
                    response = new ProcessLogonTransactionResponse
                    {
                                   ResponseCode = "0003", // General error
                                   ResponseMessage = "General Error",
                                   EstateId = estateId,
                                   MerchantId = merchantId
                    };
                }
            }

            return response;
        }*/
        
        #endregion

        public async Task<GetVoucherResponse> GetVoucher(Guid estateId,
                                                         Guid contractId,
                                                         String voucherCode,
                                                         CancellationToken cancellationToken)
        {
            // Get a client token to call the Voucher Management
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");

            TokenResponse accessToken = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);

            GetVoucherResponse response = null;

            try
            {
                VoucherManagement.DataTransferObjects.GetVoucherResponse getVoucherResponse = await this.VoucherManagementClient.GetVoucher(accessToken.AccessToken, estateId, voucherCode, cancellationToken);

                response = new GetVoucherResponse
                         {
                             ResponseCode = "0000", // Success
                             ContractId = contractId,
                             EstateId = estateId,
                             ExpiryDate = getVoucherResponse.ExpiryDate,
                             Value = getVoucherResponse.Value,
                             VoucherCode = getVoucherResponse.VoucherCode,
                             VoucherId = getVoucherResponse.VoucherId
                         };
            }
            catch (Exception ex)
            {
                if (ex.InnerException is InvalidOperationException)
                {
                    // This means there is an error in the request
                    response = new GetVoucherResponse
                    {
                        ResponseCode = "0001", // Request Message error
                        ResponseMessage = ex.InnerException.Message,
                    };
                }
                else if (ex.InnerException is HttpRequestException)
                {
                    // Request Send Exception
                    response = new GetVoucherResponse
                    {
                        ResponseCode = "0002", // Request Message error
                        ResponseMessage = "Error Sending Request Message",
                    };
                }
                else
                {
                    response = new GetVoucherResponse
                    {
                        ResponseCode = "0003", // General error
                        ResponseMessage = "General Error",
                    };
                }
            }

            return response;



        }
    }
}