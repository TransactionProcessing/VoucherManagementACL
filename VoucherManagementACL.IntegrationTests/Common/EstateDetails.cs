using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.IntegrationTests.Common
{
    using System.Linq;

    public class EstateDetails
    {
        #region Fields

        /// <summary>
        /// The contracts
        /// </summary>
        private List<Contract> Contracts;

        /// <summary>
        /// The merchants
        /// </summary>
        private readonly Dictionary<String, Guid> Merchants;

        /// <summary>
        /// The merchant users
        /// </summary>
        private readonly Dictionary<String, Dictionary<String, String>> MerchantUsers;

        private readonly Dictionary<String, Dictionary<String, String>> VoucherRedemptionUsers;

        /// <summary>
        /// The merchant users tokens
        /// </summary>
        private Dictionary<String, Dictionary<String, String>> MerchantUsersTokens;

        private Dictionary<String, Dictionary<String, String>> VoucherRedemptionUsersTokens;

        /// <summary>
        /// The operators
        /// </summary>
        private readonly Dictionary<String, Guid> Operators;

        private readonly Dictionary<Guid, List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)>> OperatorVouchers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateDetails"/> class.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        private EstateDetails(Guid estateId,
                              String estateName)
        {
            this.EstateId = estateId;
            this.EstateName = estateName;
            this.Merchants = new Dictionary<String, Guid>();
            this.Operators = new Dictionary<String, Guid>();
            this.MerchantUsers = new Dictionary<String, Dictionary<String, String>>();
            this.VoucherRedemptionUsers = new Dictionary<String, Dictionary<String, String>>();
            this.VoucherRedemptionUsersTokens = new Dictionary<String, Dictionary<String, String>>();
            this.OperatorVouchers = new Dictionary<Guid, List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)>>();
            this.Contracts = new List<Contract>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public String AccessToken { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; }

        /// <summary>
        /// Gets the name of the estate.
        /// </summary>
        /// <value>
        /// The name of the estate.
        /// </value>
        public String EstateName { get; }

        /// <summary>
        /// Gets the estate password.
        /// </summary>
        /// <value>
        /// The estate password.
        /// </value>
        public String EstatePassword { get; private set; }

        /// <summary>
        /// Gets the estate user.
        /// </summary>
        /// <value>
        /// The estate user.
        /// </value>
        public String EstateUser { get; private set; }

        /// <summary>
        /// Gets or sets the transaction responses.
        /// </summary>
        /// <value>
        /// The transaction responses.
        /// </value>
        //private Dictionary<(Guid merchantId, String transactionNumber), SerialisedMessage> TransactionResponses { get; }

        /// <summary>
        /// Gets the reconciliation responses.
        /// </summary>
        /// <value>
        /// The reconciliation responses.
        /// </value>
        //private Dictionary<Guid, SerialisedMessage> ReconciliationResponses { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the contract.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="contractName">Name of the contract.</param>
        /// <param name="operatorId">The operator identifier.</param>
        public void AddContract(Guid contractId,
                                String contractName,
                                Guid operatorId)
        {
            this.Contracts.Add(new Contract
            {
                ContractId = contractId,
                Description = contractName,
                OperatorId = operatorId,
            });
        }

        public void AddVoucher(String operatorId,
                               Decimal voucherValue,
                               Guid transactionId,
                               String voucherCode,
                               Guid voucherId)
        {
            (Guid transactionId, Decimal value, String voucherCode, Guid voucherId) voucherData = (transactionId, voucherValue, voucherCode, voucherId);

            KeyValuePair<String, Guid> operatorRecord = this.Operators.Single(o => o.Key == operatorId);
            
            if (this.OperatorVouchers.ContainsKey(operatorRecord.Value))
            {
                List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)> vouchersList = this.OperatorVouchers[operatorRecord.Value];
                vouchersList.Add(voucherData);
                }
            else
            {
                List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)> vouchers =
                    new List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)>();
                vouchers.Add(voucherData);
                this.OperatorVouchers.Add(operatorRecord.Value, vouchers);
            }
        }

        public (Guid transactionId, Decimal value, String voucherCode, Guid voucherId) GetVoucher(String operatorId, Guid transactionId)
        {
            KeyValuePair<String, Guid> operatorRecord = this.Operators.Single(o => o.Key == operatorId);
            KeyValuePair<Guid, List<(Guid transactionId, Decimal value, String voucherCode, Guid voucherId)>> vouchers = this.OperatorVouchers.Single(ov => ov.Key == operatorRecord.Value);

            (Guid transactionId, Decimal value, String voucherCode, Guid voucherId) voucher = vouchers.Value.Single(v => v.transactionId == transactionId);

            return voucher;
        }

        /// <summary>
        /// Adds the merchant.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        public void AddMerchant(Guid merchantId,
                                String merchantName)
        {
            this.Merchants.Add(merchantName, merchantId);
        }

        /// <summary>
        /// Adds the merchant user.
        /// </summary>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void AddMerchantUser(String merchantName,
                                    String userName,
                                    String password)
        {
            if (this.MerchantUsers.ContainsKey(merchantName))
            {
                Dictionary<String, String> merchantUsersList = this.MerchantUsers[merchantName];
                if (merchantUsersList.ContainsKey(userName) == false)
                {
                    merchantUsersList.Add(userName, password);
                }
            }
            else
            {
                Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
                merchantUsersList.Add(userName, password);
                this.MerchantUsers.Add(merchantName, merchantUsersList);
            }
        }

        public void AddVoucherRedemptionUser(String operatorName,
                                    String userName,
                                    String password)
        {
            if (this.VoucherRedemptionUsers.ContainsKey(operatorName))
            {
                Dictionary<String, String> voucherRedemptionUsersList = this.VoucherRedemptionUsers[operatorName];
                if (voucherRedemptionUsersList.ContainsKey(userName) == false)
                {
                    voucherRedemptionUsersList.Add(userName, password);
                }
            }
            else
            {
                Dictionary<String, String> voucherRedemptionUsersList = new Dictionary<String, String>();
                voucherRedemptionUsersList.Add(userName, password);
                this.VoucherRedemptionUsers.Add(operatorName, voucherRedemptionUsersList);
            }
        }

        /// <summary>
        /// Adds the merchant user token.
        /// </summary>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="token">The token.</param>
        public void AddMerchantUserToken(String merchantName,
                                         String userName,
                                         String token)
        {
            if (this.MerchantUsersTokens.ContainsKey(merchantName))
            {
                Dictionary<String, String> merchantUsersList = this.MerchantUsersTokens[merchantName];
                if (merchantUsersList.ContainsKey(userName) == false)
                {
                    merchantUsersList.Add(userName, token);
                }
            }
            else
            {
                Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
                merchantUsersList.Add(userName, token);
                this.MerchantUsersTokens.Add(merchantName, merchantUsersList);
            }
        }

        public String GetVoucherRedemptionUserToken(String operatorName)
        {
            KeyValuePair<String, Dictionary<String, String>> x = this.VoucherRedemptionUsersTokens.SingleOrDefault(x => x.Key == operatorName);

            if (x.Value != null)
            {
                return x.Value.First().Value;
            }

            return string.Empty;
        }

        public void AddVoucherRedemptionUserToken(String operatorName,
                                         String userName,
                                         String token)
        {
            if (this.VoucherRedemptionUsersTokens.ContainsKey(operatorName))
            {
                Dictionary<String, String> merchantUsersList = this.VoucherRedemptionUsersTokens[operatorName];
                if (merchantUsersList.ContainsKey(userName) == false)
                {
                    merchantUsersList.Add(userName, token);
                }
            }
            else
            {
                Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
                merchantUsersList.Add(userName, token);
                this.VoucherRedemptionUsersTokens.Add(operatorName, merchantUsersList);
            }
        }

        /// <summary>
        /// Adds the operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        public void AddOperator(Guid operatorId,
                                String operatorName)
        {
            this.Operators.Add(operatorName, operatorId);
        }
        
        /// <summary>
        /// Creates the specified estate identifier.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="estateName">Name of the estate.</param>
        /// <returns></returns>
        public static EstateDetails Create(Guid estateId,
                                           String estateName)
        {
            return new EstateDetails(estateId, estateName);
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="contractName">Name of the contract.</param>
        /// <returns></returns>
        public Contract GetContract(String contractName)
        {
            if (this.Contracts.Any() == false)
            {
                return null;
            }
            return this.Contracts.Single(c => c.Description == contractName);
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <returns></returns>
        public Contract GetContract(Guid contractId)
        {
            return this.Contracts.Single(c => c.ContractId == contractId);
        }

        /// <summary>
        /// Gets the merchant identifier.
        /// </summary>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <returns></returns>
        public Guid GetMerchantId(String merchantName)
        {
            if (merchantName == "InvalidMerchant")
            {
                return Guid.Parse("D59320FA-4C3E-4900-A999-483F6A10C69A");
            }

            return this.Merchants.Single(m => m.Key == merchantName).Value;
        }

        /// <summary>
        /// Gets the operator identifier.
        /// </summary>
        /// <param name="operatorName">Name of the operator.</param>
        /// <returns></returns>
        public Guid GetOperatorId(String operatorName)
        {
            return this.Operators.Single(o => o.Key == operatorName).Value;
        }

        /// <summary>
        /// Sets the estate user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void SetEstateUser(String userName,
                                  String password)
        {
            this.EstateUser = userName;
            this.EstatePassword = password;
        }

        /// <summary>
        /// Sets the estate user token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public void SetEstateUserToken(String accessToken)
        {
            this.AccessToken = accessToken;
        }

        #endregion
    }
}
