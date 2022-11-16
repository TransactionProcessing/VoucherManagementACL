@base @shared
Feature: RedeemVoucher

Background: 

		Given the following security roles exist
	| RoleName |
	| VoucherRedemption   |

	Given I create the following api scopes
	| Name                 | DisplayName                       | Description                            |
	| estateManagement     | Estate Managememt REST Scope      | A scope for Estate Managememt REST     |
	| voucherManagement | Voucher Management REST  Scope | A scope for Voucher Management REST |
	| voucherManagementACL | Voucher Management ACL REST  Scope | A scope for Voucher Management ACL REST |

	Given the following api resources exist
	| ResourceName     | DisplayName            | Secret  | Scopes           | UserClaims                 |
	| estateManagement | Estate Managememt REST | Secret1 | estateManagement | merchantId, estateId, role |
	| voucherManagement | Voucher Management REST | Secret1 | voucherManagement |  |
	| voucherManagementACL | Voucher Management ACL REST | Secret1 | voucherManagementACL | estateId, role, contractId |

	Given the following clients exist
	| ClientId         | ClientName        | Secret  | AllowedScopes                      | AllowedGrantTypes  |
	| serviceClient    | Service Client    | Secret1 | estateManagement,voucherManagement | client_credentials |
	| redemptionClient | Redemption Client | Secret1 | voucherManagementACL               | password           |

	Given I have a token to access the estate management and voucher management resources
	| ClientId      | 
	| serviceClient | 

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |
	| Test Estate 2 |

	Given I have created the following security users
	| EmailAddress                         | Password | GivenName      | FamilyName | EstateName    | RoleName          |
	| redemptionuser@testredemption1.co.uk | 123456   | TestRedemption | User1      | Test Estate 1 | VoucherRedemption |
	| redemptionuser@testredemption2.co.uk | 123456   | TestRedemption | User2      | Test Estate 2 | VoucherRedemption |

	Given I have created the following operators
	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Voucher      | True                        | True                        |
	| Test Estate 2 | Voucher      | True                        | True                        |

	Given I create a contract with the following values
	| EstateName    | OperatorName     | ContractDescription       |
	| Test Estate 1 | Voucher          | Hospital 1 Contract       |
	| Test Estate 2 | Voucher          | Hospital 2 Contract       |

	When I create the following Products
	| EstateName    | OperatorName     | ContractDescription       | ProductName       | DisplayText     | Value |
	| Test Estate 1 | Voucher          | Hospital 1 Contract       | 10 KES            | 10 KES          |       |
	| Test Estate 2 | Voucher          | Hospital 2 Contract       | 10 KES            | 10 KES          |       |

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
	| Test Merchant 2 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 2 |

	Given I have assigned the following  operator to the merchants
	| OperatorName     | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Voucher          | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
	| Voucher          | Test Merchant 2 | 00000002       | 10000002       | Test Estate 2 |

	Given I have assigned the following devices to the merchants
	| DeviceIdentifier | MerchantName    | EstateName    |
	| 123456780        | Test Merchant 1 | Test Estate 1 |
	| 123456781        | Test Merchant 2 | Test Estate 2 |

	Given I make the following manual merchant deposits 
	| Reference | Amount  | DateTime | MerchantName    | EstateName    |
	| Deposit1  | 20.00 | Today    | Test Merchant 1 | Test Estate 1 |
	| Deposit1  | 20.00 | Today    | Test Merchant 2 | Test Estate 2 |

	When I perform the following transactions
	| DateTime | TransactionNumber | TransactionType | TransactionSource | MerchantName    | DeviceIdentifier | EstateName    | OperatorName     | TransactionAmount | CustomerAccountNumber | CustomerEmailAddress        | ContractDescription       | ProductName       | RecipientEmail       | RecipientMobile | MessageType   | AccountNumber | CustomerName     |
	| Today    | 1                 | Sale            | 1                 | Test Merchant 1 | 123456780        | Test Estate 1 | Voucher          | 10.00             |                       |                             | Hospital 1 Contract       | 10 KES            | test@recipient.co.uk |                 |               |               |                  |
	| Today    | 2                 | Sale            | 1                 | Test Merchant 2 | 123456781        | Test Estate 2 | Voucher          | 10.00             |                       |                             | Hospital 2 Contract       | 10 KES            | test@recipient.co.uk |                 |               |               |                  |

@PRTest
Scenario: Redeem Vouchers
	Given I am logged in as "redemptionuser@testredemption1.co.uk" with password "123456" for Estate "Test Estate 1" with client "redemptionClient"

	When I redeem the voucher for Estate 'Test Estate 1' and Merchant 'Test Merchant 1' transaction number 1 the voucher balance will be 0

	Given I am logged in as "redemptionuser@testredemption2.co.uk" with password "123456" for Estate "Test Estate 2" with client "redemptionClient"

	When I redeem the voucher for Estate 'Test Estate 2' and Merchant 'Test Merchant 2' transaction number 2 the voucher balance will be 0
