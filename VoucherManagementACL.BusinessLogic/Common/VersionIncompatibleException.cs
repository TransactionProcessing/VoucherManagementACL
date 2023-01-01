namespace VoucherManagementACL.BusinessLogic.Common;

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class VersionIncompatibleException : Exception
{
    public VersionIncompatibleException(String message) : base(message)
    {
            
    }

    public VersionIncompatibleException(String message, Exception innerException) : base(message, innerException)
    {

    }
}