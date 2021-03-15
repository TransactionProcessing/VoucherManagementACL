@base @shared
Feature: GetVoucher

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

	When I issue the following vouchers
	| EstateName    | OperatorName    | Value | TransactionId                        | RecipientEmail                 | RecipientMobile |
	| Test Estate 1 | Voucher | 10.00 | 19f2776a-4230-40d4-8cd2-3649e18732e0 | testrecipient1@recipient.co.uk |                 |
	| Test Estate 1 | Voucher | 20.00 | 6351e047-8f31-4472-a294-787caa5fb738 |                                | 123456788       |
	| Test Estate 2 | Voucher | 10.00 | 29f2776a-4230-40d4-8cd2-3649e18732e0 | testrecipient1@recipient.co.uk |                 |
	| Test Estate 2 | Voucher | 20.00 | 7351e047-8f31-4472-a294-787caa5fb738 |                                | 123456788       |

@PRTest
Scenario: Get Vouchers
	Given I am logged in as "redemptionuser@testredemption1.co.uk" with password "123456" for Estate "Test Estate 1" with client "redemptionClient"

	When I get the following vouchers the voucher is returned
	| EstateName    | OperatorName    | Value | TransactionId                        |
	| Test Estate 1 | Voucher | 10.00 | 19f2776a-4230-40d4-8cd2-3649e18732e0 |
	| Test Estate 1 | Voucher | 20.00 | 6351e047-8f31-4472-a294-787caa5fb738 |

	Given I am logged in as "redemptionuser@testredemption2.co.uk" with password "123456" for Estate "Test Estate 2" with client "redemptionClient"

	When I get the following vouchers the voucher is returned
	| EstateName    | OperatorName | Value | TransactionId                        |
	| Test Estate 2 | Voucher      | 10.00 | 29f2776a-4230-40d4-8cd2-3649e18732e0 |
	| Test Estate 2 | Voucher      | 20.00 | 7351e047-8f31-4472-a294-787caa5fb738 |
