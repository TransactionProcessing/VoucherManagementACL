namespace VoucherManagementACL.Common.Examples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal static class ExampleData
    {
        #region Fields

        /// <summary>
        /// The balance
        /// </summary>
        internal static Decimal Balance = 0;

        /// <summary>
        /// The contract identifier
        /// </summary>
        internal static Guid ContractId = Guid.Parse("CDAD50FD-480B-4287-BC12-0D6E5302A9EC");

        /// <summary>
        /// The estate identifier
        /// </summary>
        internal static Guid EstateId = Guid.Parse("52A465EC-5F24-49A6-AAB8-D407AB2F7820");

        /// <summary>
        /// The expiry date
        /// </summary>
        internal static DateTime ExpiryDate = new DateTime(2021, 4, 8);

        /// <summary>
        /// The response code
        /// </summary>
        internal static String ResponseCode = "0000";

        /// <summary>
        /// The response message
        /// </summary>
        internal static String ResponseMessage = "SUCCESS";

        /// <summary>
        /// The value
        /// </summary>
        internal static Decimal Value = 10.00m;

        /// <summary>
        /// The voucher code
        /// </summary>
        internal static String VoucherCode = "1234567890";

        /// <summary>
        /// The voucher identifier
        /// </summary>
        internal static Guid VoucherId = Guid.Parse("41C536BE-CC53-4888-B7BE-3A13530EF262");

        #endregion
    }
}