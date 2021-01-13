namespace VoucherManagementACL.Testing
{
    using System;
    using System.Collections.Generic;
    using BusinessLogic.Requests;
    using Newtonsoft.Json;
    using SecurityService.DataTransferObjects.Responses;
    using VoucherManagement.DataTransferObjects;

    /// <summary>
    /// 
    /// </summary>
    public class TestData
    {
        #region Fields

        /// <summary>
        /// The estate identifier
        /// </summary>
        public static Guid EstateId = Guid.Parse("1C8354B7-B97A-46EA-9AD1-C43F33F7E3C3");
        
        #endregion

        public static String Token = "{\"access_token\": \"eyJhbGciOiJSUzI1NiIsImtpZCI6IjA4NGZlNTIwZmIzZmVhM2M0MmNmMjBiZWM2OGY1NDg2IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE1Nzc1NTIyMTQsImV4cCI6MTU3NzU1NTgxNCwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzM6NTAwMSIsImF1ZCI6WyJlc3RhdGVNYW5hZ2VtZW50IiwidHJhbnNhY3Rpb25Qcm9jZXNzb3IiLCJ0cmFuc2FjdGlvblByb2Nlc3NvckFDTCJdLCJjbGllbnRfaWQiOiJzZXJ2aWNlQ2xpZW50Iiwic2NvcGUiOlsiZXN0YXRlTWFuYWdlbWVudCIsInRyYW5zYWN0aW9uUHJvY2Vzc29yIiwidHJhbnNhY3Rpb25Qcm9jZXNzb3JBQ0wiXX0.JxK6kEvmvuMnL7ktgvv6N52fjqnFG-NSjPcQORIcFb4ravZAk5oNgsnEtjPcOHTXiktcr8i361GlYO1yiSGdfLKtCTaH3lihcbOb1wfQh3bYM_xmlqJUdirerR8ql9lxqBqm2_Q__PDFuFhMd9lAz-iFr3svuOXeQJB5HQ2rtA3xBDDked5SaEs1dMfbBJA6svRq831WCQSJgap2Db7XN7ais7AQhPYUcFOTGs9Qk33rF_k-2dnAzkEITjvgPwim-8YsEQfsbRYgZmIXfjOXcD81Y0G2_grugvc0SOj_hKXd4d62T-zU-mC4opuYauWKYFqV4UB4sf4V4rtLWeDWrA\",\"expires_in\": 3600,\"token_type\": \"Bearer\",\"scope\": \"estateManagement transactionProcessor transactionProcessorACL\"}";

        public static TokenResponse TokenResponse = TokenResponse.Create(TestData.Token);
        
        public static Guid ContractId = Guid.Parse("362CCDFD-C227-4D6E-884C-D6323E278175");

        public static IReadOnlyDictionary<String, String> DefaultAppSettings =>
            new Dictionary<String, String>
            {
                ["AppSettings:MinimumSupportedApplicationVersion"] = "1.0.5",
                ["AppSettings: SecurityService"] = "http://192.168.1.133:5001",
                ["AppSettings:TransactionProcessorApi"]  = "http://192.168.1.133:5002",
                ["AppSettings:ClientId"] = "ClientId",
                ["AppSettings:ClientSecret"] = "secret"
            };

        public static String OldApplicationVersion = "1.0.4";
        public static String NewerApplicationVersion = "1.0.5.1";
        public static String ApplicationVersion = "1.0.5";

        public static VersionCheckRequest VersionCheckRequest = VersionCheckRequest.Create(TestData.ApplicationVersion);

        public static String VoucherCode = "1231231234";

        public static GetVoucherResponse GetVoucherResponse =>
            new GetVoucherResponse
            {
                VoucherCode = TestData.VoucherCode,
                Balance = TestData.Balance,
                ExpiryDate = TestData.ExpiryDate,
                GeneratedDateTime = TestData.GeneratedDateTime,
                IsGenerated = TestData.IsGenerated,
                IsIssued = TestData.IsIssued,
                IsRedeemed = TestData.IsRedeemed,
                IssuedDateTime = TestData.IssuedDateTime,
                RedeemedDateTime = TestData.RedeemedDateTime,
                TransactionId = TestData.TransactionId,
                Value = TestData.Value,
                VoucherId = TestData.VoucherId
            };

        public static RedeemVoucherResponse RedeemVoucherResponse =>
            new RedeemVoucherResponse
            {
                VoucherCode = TestData.VoucherCode,
                RemainingBalance = TestData.Balance,
                ExpiryDate = TestData.ExpiryDate
            };

        public static Models.GetVoucherResponse GetVoucherResponseModel =>
            new Models.GetVoucherResponse
            {
                VoucherCode = TestData.VoucherCode,
                ExpiryDate = TestData.ExpiryDate,
                ContractId = TestData.ContractId,
                Value = TestData.Value,
                VoucherId = TestData.VoucherId,
                EstateId = TestData.EstateId,
                ResponseCode = TestData.ResponseCode,
                ResponseMessage = TestData.ResponseMessage
            };

        public static Models.RedeemVoucherResponse RedeemVoucherResponseModel =>
            new Models.RedeemVoucherResponse
            {
                VoucherCode = TestData.VoucherCode,
                ExpiryDate = TestData.ExpiryDate,
                ContractId = TestData.ContractId,
                Balance = TestData.Balance,
                EstateId = TestData.EstateId,
                ResponseCode = TestData.ResponseCode,
                ResponseMessage = TestData.ResponseMessage
            };

        public static String ResponseCode = "0000";
        public static String InvalidOperationErrorResponseCode = "0001";
        public static String HttpRequestErrorResponseCode = "0002";
        public static String GeneralErrorResponseCode = "0003";

        public static String ResponseMessage = "SUCCESS";
        public static String InvalidOperationErrorResponseMessage = "ERROR";
        public static String HttpRequestErrorResponseMessage = "Error Sending Request Message";
        public static String GeneralErrorResponseMessage = "General Error";

        public static Decimal Balance = 10.00m;

        public static DateTime ExpiryDate = new DateTime(2021,1,11);

        public static DateTime GeneratedDateTime = new DateTime(2020, 12, 11);

        public static Boolean IsGenerated = true;

        public static Boolean IsIssued = true;

        public static Boolean IsRedeemed = false;

        public static DateTime IssuedDateTime = new DateTime(2020, 12, 12);

        public static DateTime RedeemedDateTime = new DateTime();

        public static Guid TransactionId = Guid.Parse("793ACA88-B501-435E-BF08-1E5F639A7885");

        public static Decimal Value = 20.00m;

        public static Guid VoucherId = Guid.Parse("C12665AC-1301-47AA-B292-281EC4DE9721");
    }
}