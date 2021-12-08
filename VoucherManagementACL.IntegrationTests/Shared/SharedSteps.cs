﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.IntegrationTests.Shared
{
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using EstateManagement.DataTransferObjects;
    using EstateManagement.DataTransferObjects.Requests;
    using EstateManagement.DataTransferObjects.Responses;
    using Newtonsoft.Json;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using Shouldly;
    using TechTalk.SpecFlow;
    using VoucherManagementACL.DataTransferObjects.Responses;
    using ClientDetails = Common.ClientDetails;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public SharedSteps(ScenarioContext scenarioContext,
                         TestingContext testingContext)
        {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [Given(@"I create the following api scopes")]
        public async Task GivenICreateTheFollowingApiScopes(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                CreateApiScopeRequest createApiScopeRequest = new CreateApiScopeRequest
                                                              {
                                                                  Name = SpecflowTableHelper.GetStringRowValue(tableRow, "Name"),
                                                                  Description = SpecflowTableHelper.GetStringRowValue(tableRow, "Description"),
                                                                  DisplayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName")
                                                              };
                var createApiScopeResponse =
                    await this.CreateApiScope(createApiScopeRequest, CancellationToken.None).ConfigureAwait(false);

                createApiScopeResponse.ShouldNotBeNull();
                createApiScopeResponse.ApiScopeName.ShouldNotBeNullOrEmpty();
            }
        }

        private async Task<CreateApiScopeResponse> CreateApiScope(CreateApiScopeRequest createApiScopeRequest,
                                                                  CancellationToken cancellationToken)
        {
            CreateApiScopeResponse createApiScopeResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateApiScope(createApiScopeRequest, cancellationToken).ConfigureAwait(false);
            return createApiScopeResponse;
        }

        [Given(@"I have created the following estates")]
        [When(@"I create the following estates")]
        public async Task WhenICreateTheFollowingEstates(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

                CreateEstateRequest createEstateRequest = new CreateEstateRequest
                {
                    EstateId = Guid.NewGuid(),
                    EstateName = estateName
                };

                CreateEstateResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                          .CreateEstate(this.TestingContext.AccessToken, createEstateRequest, CancellationToken.None)
                                                          .ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                this.TestingContext.AddEstateDetails(response.EstateId, estateName);

                this.TestingContext.Logger.LogInformation($"Estate {estateName} created with Id {response.EstateId}");

                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                // Setup the subscriptions for the estate
                await Retry.For(async () =>
                {
                    await this.TestingContext.DockerHelper.PopulateSubscriptionServiceConfiguration(estateName).ConfigureAwait(false);
                }, retryFor: TimeSpan.FromMinutes(2), retryInterval: TimeSpan.FromSeconds(30));

                EstateResponse estate = null;
                await Retry.For(async () =>
                {
                    estate = await this.TestingContext.DockerHelper.EstateClient
                                       .GetEstate(this.TestingContext.AccessToken, estateDetails.EstateId, CancellationToken.None).ConfigureAwait(false);
                    estate.ShouldNotBeNull();
                }, TimeSpan.FromMinutes(2)).ConfigureAwait(false);

                estate.EstateName.ShouldBe(estateDetails.EstateName);
            }
        }

        [Given(@"I have assigned the following  operator to the merchants")]
        [When(@"I assign the following  operator to the merchants")]
        public async Task WhenIAssignTheFollowingOperatorToTheMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                // Lookup the merchant id
                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                // Lookup the operator id
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
                {
                    OperatorId = operatorId,
                    MerchantNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                    TerminalNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                };

                AssignOperatorResponse assignOperatorResponse = await this.TestingContext.DockerHelper.EstateClient.AssignOperatorToMerchant(token, estateDetails.EstateId, merchantId, assignOperatorRequest, CancellationToken.None).ConfigureAwait(false);

                assignOperatorResponse.EstateId.ShouldBe(estateDetails.EstateId);
                assignOperatorResponse.MerchantId.ShouldBe(merchantId);
                assignOperatorResponse.OperatorId.ShouldBe(operatorId);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} assigned to Estate {estateDetails.EstateName}");
            }
        }


        [Given(@"I have created the following operators")]
        [When(@"I create the following operators")]
        public async Task WhenICreateTheFollowingOperators(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
                Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

                CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest
                {
                    Name = operatorName,
                    RequireCustomMerchantNumber = requireCustomMerchantNumber,
                    RequireCustomTerminalNumber = requireCustomTerminalNumber
                };

                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                CreateOperatorResponse response = await this.TestingContext.DockerHelper.EstateClient.CreateOperator(this.TestingContext.AccessToken, estateDetails.EstateId, createOperatorRequest, CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldNotBe(Guid.Empty);
                response.OperatorId.ShouldNotBe(Guid.Empty);

                // Cache the estate id
                estateDetails.AddOperator(response.OperatorId, operatorName);

                this.TestingContext.Logger.LogInformation($"Operator {operatorName} created with Id {response.OperatorId} for Estate {estateDetails.EstateName}");
            }
        }

        [Given("I create the following merchants")]
        [When(@"I create the following merchants")]
        public async Task WhenICreateTheFollowingMerchants(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // lookup the estate id based on the name in the table
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);
                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest
                {
                    Name = merchantName,
                    Contact = new Contact
                    {
                        ContactName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContactName"),
                        EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress")
                    },
                    Address = new Address
                    {
                        AddressLine1 = SpecflowTableHelper.GetStringRowValue(tableRow, "AddressLine1"),
                        Town = SpecflowTableHelper.GetStringRowValue(tableRow, "Town"),
                        Region = SpecflowTableHelper.GetStringRowValue(tableRow, "Region"),
                        Country = SpecflowTableHelper.GetStringRowValue(tableRow, "Country")
                    }
                };

                CreateMerchantResponse response = await this.TestingContext.DockerHelper.EstateClient
                                                            .CreateMerchant(token, estateDetails.EstateId, createMerchantRequest, CancellationToken.None).ConfigureAwait(false);

                response.ShouldNotBeNull();
                response.EstateId.ShouldBe(estateDetails.EstateId);
                response.MerchantId.ShouldNotBe(Guid.Empty);

                // Cache the merchant id
                estateDetails.AddMerchant(response.MerchantId, merchantName);

                this.TestingContext.Logger.LogInformation($"Merchant {merchantName} created with Id {response.MerchantId} for Estate {estateDetails.EstateName}");
            }

            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");

                Guid merchantId = estateDetails.GetMerchantId(merchantName);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                await Retry.For(async () =>
                {
                    MerchantResponse merchant = await this.TestingContext.DockerHelper.EstateClient
                                                          .GetMerchant(token, estateDetails.EstateId, merchantId, CancellationToken.None)
                                                          .ConfigureAwait(false);

                    merchant.MerchantName.ShouldBe(merchantName);
                });
            }
        }
        
        [Given(@"I create a contract with the following values")]
        public async Task GivenICreateAContractWithTheFollowingValues(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid operatorId = estateDetails.GetOperatorId(operatorName);

                CreateContractRequest createContractRequest = new CreateContractRequest
                {
                    OperatorId = operatorId,
                    Description = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription")
                };

                CreateContractResponse contractResponse =
                    await this.TestingContext.DockerHelper.EstateClient.CreateContract(token, estateDetails.EstateId, createContractRequest, CancellationToken.None);

                estateDetails.AddContract(contractResponse.ContractId, createContractRequest.Description, operatorId);
            }
        }

        [When(@"I create the following Products")]
        public async Task WhenICreateTheFollowingProducts(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String token = this.TestingContext.AccessToken;
                if (String.IsNullOrEmpty(estateDetails.AccessToken) == false)
                {
                    token = estateDetails.AccessToken;
                }

                String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
                Contract contract = estateDetails.GetContract(contractName);
                String productValue = SpecflowTableHelper.GetStringRowValue(tableRow, "Value");

                AddProductToContractRequest addProductToContractRequest = new AddProductToContractRequest
                {
                    ProductName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName"),
                    DisplayText = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayText"),
                    Value = null
                };
                if (String.IsNullOrEmpty(productValue) == false)
                {
                    addProductToContractRequest.Value = Decimal.Parse(productValue);
                }

                AddProductToContractResponse addProductToContractResponse = await this.TestingContext.DockerHelper.EstateClient.AddProductToContract(token,
                                                                                                                                                     estateDetails.EstateId,
                                                                                                                                                     contract.ContractId,
                                                                                                                                                     addProductToContractRequest,
                                                                                                                                                     CancellationToken.None);

                contract.AddProduct(addProductToContractResponse.ProductId, addProductToContractRequest.ProductName, addProductToContractRequest.DisplayText,
                                    addProductToContractRequest.Value);
            }
        }
        
        [Given(@"the following api resources exist")]
        public async Task GivenTheFollowingApiResourcesExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String resourceName = SpecflowTableHelper.GetStringRowValue(tableRow, "ResourceName");
                String displayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String scopes = SpecflowTableHelper.GetStringRowValue(tableRow, "Scopes");
                String userClaims = SpecflowTableHelper.GetStringRowValue(tableRow, "UserClaims");

                List<String> splitScopes = scopes.Split(",").ToList();
                List<String> splitUserClaims = userClaims.Split(",").ToList();

                CreateApiResourceRequest createApiResourceRequest = new CreateApiResourceRequest
                {
                    Description = String.Empty,
                    DisplayName = displayName,
                    Name = resourceName,
                    Scopes = new List<String>(),
                    Secret = secret,
                    UserClaims = new List<String>()
                };
                splitScopes.ForEach(a =>
                {
                    createApiResourceRequest.Scopes.Add(a.Trim());
                });
                splitUserClaims.ForEach(a =>
                {
                    createApiResourceRequest.UserClaims.Add(a.Trim());
                });

                CreateApiResourceResponse createApiResourceResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateApiResource(createApiResourceRequest, CancellationToken.None).ConfigureAwait(false);

                createApiResourceResponse.ApiResourceName.ShouldBe(resourceName);
            }
        }

        [Given(@"the following clients exist")]
        public async Task GivenTheFollowingClientsExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");
                String clientName = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientName");
                String secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret");
                String allowedScopes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedScopes");
                String allowedGrantTypes = SpecflowTableHelper.GetStringRowValue(tableRow, "AllowedGrantTypes");

                List<String> splitAllowedScopes = allowedScopes.Split(",").ToList();
                List<String> splitAllowedGrantTypes = allowedGrantTypes.Split(",").ToList();

                CreateClientRequest createClientRequest = new CreateClientRequest
                {
                    Secret = secret,
                    AllowedGrantTypes = new List<String>(),
                    AllowedScopes = new List<String>(),
                    ClientDescription = String.Empty,
                    ClientId = clientId,
                    ClientName = clientName
                };

                splitAllowedScopes.ForEach(a =>
                {
                    createClientRequest.AllowedScopes.Add(a.Trim());
                });
                splitAllowedGrantTypes.ForEach(a =>
                {
                    createClientRequest.AllowedGrantTypes.Add(a.Trim());
                });

                CreateClientResponse createClientResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateClient(createClientRequest, CancellationToken.None).ConfigureAwait(false);

                createClientResponse.ClientId.ShouldBe(clientId);

                this.TestingContext.AddClientDetails(clientId, secret, allowedGrantTypes);
            }
        }

        [Given(@"I have a token to access the estate management and voucher management resources")]
        public async Task GivenIHaveATokenToAccessTheEstateManagementAndVoucherManagementResources(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String clientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId");

                ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

                if (clientDetails.GrantType == "client_credentials")
                {
                    TokenResponse tokenResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.GetToken(clientId, clientDetails.ClientSecret, CancellationToken.None).ConfigureAwait(false);

                    this.TestingContext.AccessToken = tokenResponse.AccessToken;
                }
            }
        }

        [When(@"I issue the following vouchers")]
        public async Task WhenIIssueTheFollowingVouchers(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                IssueVoucherRequest request = new IssueVoucherRequest
                                              {
                                                  Value = SpecflowTableHelper.GetDecimalValue(tableRow, "Value"),
                                                  RecipientEmail = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientEmail"),
                                                  RecipientMobile = SpecflowTableHelper.GetStringRowValue(tableRow, "RecipientMobile"),
                                                  EstateId = estateDetails.EstateId,
                                                  OperatorIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName"),
                                                  TransactionId = Guid.Parse(SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionId"))
                                              };

                IssueVoucherResponse response = await this.TestingContext.DockerHelper.VoucherManagementClient.IssueVoucher(this.TestingContext.AccessToken, request, CancellationToken.None)
                                                          .ConfigureAwait(false);

                response.VoucherId.ShouldNotBe(Guid.Empty);

                await Retry.For(async () =>
                                {
                                    var v = await this.TestingContext.DockerHelper.VoucherManagementClient
                                              .GetVoucher(this.TestingContext.AccessToken, estateDetails.EstateId, response.VoucherCode, CancellationToken.None)
                                              .ConfigureAwait(false);
                                    v.ShouldNotBeNull();
                                });

                estateDetails.AddVoucher(request.OperatorIdentifier,
                                         request.Value,
                                         request.TransactionId,
                                         response.VoucherCode,
                                         response.VoucherId);
            }
        }

        [When(@"I get the following vouchers the voucher is returned")]
        public async Task WhenIGetTheFollowingVouchersTheVoucherIsReturned(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String operatorIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid transactionId = Guid.Parse(SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionId"));

                (Guid transactionId, Decimal value, String voucherCode, Guid voucherId) voucher = estateDetails.GetVoucher(operatorIdentifier, transactionId);

                // Build URI 
                String uri = $"api/vouchers?applicationVersion=1.0.0&voucherCode={voucher.voucherCode}";

                String accessToken = estateDetails.GetVoucherRedemptionUserToken(operatorIdentifier);

                this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await this.TestingContext.DockerHelper.HttpClient.GetAsync(uri, CancellationToken.None).ConfigureAwait(false);

                String content = await response.Content.ReadAsStringAsync(CancellationToken.None).ConfigureAwait(false);
                //var gimp = JsonConvert.DeserializeObject<GetVoucherResponseMessage>(content);
                response.IsSuccessStatusCode.ShouldBeTrue();

                GetVoucherResponseMessage getVoucherResponse = JsonConvert.DeserializeObject<GetVoucherResponseMessage>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                getVoucherResponse.VoucherCode.ShouldBe(voucher.voucherCode);
                getVoucherResponse.VoucherId.ShouldBe(voucher.voucherId);
                getVoucherResponse.Value.ShouldBe(voucher.value);
            }
        }

        [When(@"I redeem the following vouchers the balance will be as expected")]
        public async Task WhenIRedeemTheFollowingVouchersTheBalanceWillBeAsExpected(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                String operatorIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
                Guid transactionId = Guid.Parse(SpecflowTableHelper.GetStringRowValue(tableRow, "TransactionId"));
                Decimal balance = SpecflowTableHelper.GetDecimalValue(tableRow, "Balance");

                (Guid transactionId, Decimal value, String voucherCode, Guid voucherId) voucher = estateDetails.GetVoucher(operatorIdentifier, transactionId);

                // Build URI 
                String uri = $"api/vouchers?applicationVersion=1.0.0&voucherCode={voucher.voucherCode}";

                String accessToken = estateDetails.GetVoucherRedemptionUserToken(operatorIdentifier);

                StringContent content = new StringContent(String.Empty);

                this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await this.TestingContext.DockerHelper.HttpClient.PutAsync(uri, content, CancellationToken.None).ConfigureAwait(false);

                response.IsSuccessStatusCode.ShouldBeTrue();

                RedeemVoucherResponseMessage redeemVoucherResponse = JsonConvert.DeserializeObject<RedeemVoucherResponseMessage>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                redeemVoucherResponse.Balance.ShouldBe(balance);
                redeemVoucherResponse.VoucherCode.ShouldBe(voucher.voucherCode);
            }
        }


        [Given(@"I have created the following security users")]
        public async Task GivenIHaveCreatedTheFollowingSecurityUsers(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String roleName = SpecflowTableHelper.GetStringRowValue(tableRow, "RoleName");
                String emailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress");
                String password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password");
                String givenName = SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName");
                String familyName = SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName");

                EstateDetails estateDetails = this.TestingContext.GetEstateDetails(tableRow);

                CreateUserRequest createUserRequest = new CreateUserRequest
                                                      {
                                                          EmailAddress = emailAddress,
                                                          FamilyName = familyName,
                                                          GivenName = givenName,
                                                          MiddleName = String.Empty,
                                                          Password = password,
                                                          PhoneNumber = "123456789",
                                                          Roles = new List<String>
                                                                  {
                                                                      roleName
                                                                  },
                                                          Claims = new Dictionary<String, String>
                                                                   {
                                                                       {"estateId", estateDetails.EstateId.ToString()},
                                                                       {"contractId", estateDetails.EstateId.ToString()}
                                                                   }
                };

                CreateUserResponse createUserResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateUser(createUserRequest, CancellationToken.None)
                                                                  .ConfigureAwait(false);

                createUserResponse.UserId.ShouldNotBe(Guid.Empty);

                // TODO: this needs to be dynamic :|
                estateDetails.AddVoucherRedemptionUser("Voucher", emailAddress, password);

                
            }
        }


        [Given(@"I am logged in as ""(.*)"" with password ""(.*)"" for Estate ""(.*)"" with client ""(.*)""")]
        public async Task GivenIAmLoggedInAsWithPasswordForEstateWithClient(String username, String password, String estateName, String clientId)
        {
            EstateDetails estateDetails = this.TestingContext.GetEstateDetails(estateName);

            ClientDetails clientDetails = this.TestingContext.GetClientDetails(clientId);

            TokenResponse tokenResponse = await this.TestingContext.DockerHelper.SecurityServiceClient
                                                    .GetToken(username, password, clientId, clientDetails.ClientSecret, CancellationToken.None).ConfigureAwait(false);

            estateDetails.AddVoucherRedemptionUserToken("Voucher", username, tokenResponse.AccessToken);
        }

        /// <summary>
        /// Givens the following security roles exist.
        /// </summary>
        /// <param name="table">The table.</param>
        [Given(@"the following security roles exist")]
        public async Task GivenTheFollowingSecurityRolesExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                String roleName = SpecflowTableHelper.GetStringRowValue(tableRow, "RoleName");

                CreateRoleRequest createRoleRequest = new CreateRoleRequest
                                                      {
                                                          RoleName = roleName
                                                      };

                CreateRoleResponse createRoleResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateRole(createRoleRequest, CancellationToken.None)
                                                                  .ConfigureAwait(false);

                createRoleResponse.RoleId.ShouldNotBe(Guid.Empty);
            }
        }

    }
}
