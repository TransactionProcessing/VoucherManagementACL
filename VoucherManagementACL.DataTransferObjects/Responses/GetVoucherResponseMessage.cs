using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagementACL.DataTransferObjects.Responses
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;

    [ExcludeFromCodeCoverage]
    public class GetVoucherResponseMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        [JsonProperty("response_code")]
        public String ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        /// <value>
        /// The response message.
        /// </value>
        [JsonProperty("response_message")]
        public String ResponseMessage { get; set; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        [JsonProperty("contract_id")]
        public Guid ContractId { get; set; }

        /// <summary>
        /// Gets or sets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty("estate_id")]
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        /// <value>
        /// The expiry date.
        /// </value>
        [JsonProperty("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public Decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        [JsonProperty("balance")]
        public Decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the voucher code.
        /// </summary>
        /// <value>
        /// The voucher code.
        /// </value>
        [JsonProperty("voucher_code")]
        public String VoucherCode { get; set; }

        /// <summary>
        /// Gets or sets the voucher identifier.
        /// </summary>
        /// <value>
        /// The voucher identifier.
        /// </value>
        [JsonProperty("voucher_id")]
        public Guid VoucherId { get; set; }

        #endregion
    }
}
