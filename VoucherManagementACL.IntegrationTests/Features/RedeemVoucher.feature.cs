﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace VoucherManagementACL.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "shared")]
    public partial class RedeemVoucherFeature : object, Xunit.IClassFixture<RedeemVoucherFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "base",
                "shared"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "RedeemVoucher.feature"
#line hidden
        
        public RedeemVoucherFeature(RedeemVoucherFeature.FixtureData fixtureData, VoucherManagementACL_IntegrationTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "RedeemVoucher", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoleName"});
            table1.AddRow(new string[] {
                        "VoucherRedemption"});
#line 6
  testRunner.Given("the following security roles exist", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DisplayName",
                        "Description"});
            table2.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST Scope",
                        "A scope for Estate Managememt REST"});
            table2.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST  Scope",
                        "A scope for Voucher Management REST"});
            table2.AddRow(new string[] {
                        "voucherManagementACL",
                        "Voucher Management ACL REST  Scope",
                        "A scope for Voucher Management ACL REST"});
#line 10
 testRunner.Given("I create the following api scopes", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table3.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "merchantId, estateId, role"});
            table3.AddRow(new string[] {
                        "voucherManagement",
                        "Voucher Management REST",
                        "Secret1",
                        "voucherManagement",
                        ""});
            table3.AddRow(new string[] {
                        "voucherManagementACL",
                        "Voucher Management ACL REST",
                        "Secret1",
                        "voucherManagementACL",
                        "estateId, role, contractId"});
#line 16
 testRunner.Given("the following api resources exist", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table4.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement,voucherManagement",
                        "client_credentials"});
            table4.AddRow(new string[] {
                        "redemptionClient",
                        "Redemption Client",
                        "Secret1",
                        "voucherManagementACL",
                        "password"});
#line 22
 testRunner.Given("the following clients exist", ((string)(null)), table4, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table5.AddRow(new string[] {
                        "serviceClient"});
#line 27
 testRunner.Given("I have a token to access the estate management and voucher management resources", ((string)(null)), table5, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName"});
            table6.AddRow(new string[] {
                        "Test Estate 1"});
            table6.AddRow(new string[] {
                        "Test Estate 2"});
#line 31
 testRunner.Given("I have created the following estates", ((string)(null)), table6, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmailAddress",
                        "Password",
                        "GivenName",
                        "FamilyName",
                        "EstateName",
                        "RoleName"});
            table7.AddRow(new string[] {
                        "redemptionuser@testredemption1.co.uk",
                        "123456",
                        "TestRedemption",
                        "User1",
                        "Test Estate 1",
                        "VoucherRedemption"});
            table7.AddRow(new string[] {
                        "redemptionuser@testredemption2.co.uk",
                        "123456",
                        "TestRedemption",
                        "User2",
                        "Test Estate 2",
                        "VoucherRedemption"});
#line 36
 testRunner.Given("I have created the following security users", ((string)(null)), table7, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "RequireCustomMerchantNumber",
                        "RequireCustomTerminalNumber"});
            table8.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "True",
                        "True"});
            table8.AddRow(new string[] {
                        "Test Estate 2",
                        "Voucher",
                        "True",
                        "True"});
#line 41
 testRunner.Given("I have created the following operators", ((string)(null)), table8, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription"});
            table9.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract"});
            table9.AddRow(new string[] {
                        "Test Estate 2",
                        "Voucher",
                        "Hospital 2 Contract"});
#line 46
 testRunner.Given("I create a contract with the following values", ((string)(null)), table9, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "EstateName",
                        "OperatorName",
                        "ContractDescription",
                        "ProductName",
                        "DisplayText",
                        "Value"});
            table10.AddRow(new string[] {
                        "Test Estate 1",
                        "Voucher",
                        "Hospital 1 Contract",
                        "10 KES",
                        "10 KES",
                        ""});
            table10.AddRow(new string[] {
                        "Test Estate 2",
                        "Voucher",
                        "Hospital 2 Contract",
                        "10 KES",
                        "10 KES",
                        ""});
#line 51
 testRunner.When("I create the following Products", ((string)(null)), table10, "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "MerchantName",
                        "AddressLine1",
                        "Town",
                        "Region",
                        "Country",
                        "ContactName",
                        "EmailAddress",
                        "EstateName"});
            table11.AddRow(new string[] {
                        "Test Merchant 1",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 1"});
            table11.AddRow(new string[] {
                        "Test Merchant 2",
                        "Address Line 1",
                        "TestTown",
                        "Test Region",
                        "United Kingdom",
                        "Test Contact 1",
                        "testcontact1@merchant1.co.uk",
                        "Test Estate 2"});
#line 56
 testRunner.Given("I create the following merchants", ((string)(null)), table11, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "OperatorName",
                        "MerchantName",
                        "MerchantNumber",
                        "TerminalNumber",
                        "EstateName"});
            table12.AddRow(new string[] {
                        "Voucher",
                        "Test Merchant 1",
                        "00000001",
                        "10000001",
                        "Test Estate 1"});
            table12.AddRow(new string[] {
                        "Voucher",
                        "Test Merchant 2",
                        "00000002",
                        "10000002",
                        "Test Estate 2"});
#line 61
 testRunner.Given("I have assigned the following  operator to the merchants", ((string)(null)), table12, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "DeviceIdentifier",
                        "MerchantName",
                        "EstateName"});
            table13.AddRow(new string[] {
                        "123456780",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table13.AddRow(new string[] {
                        "123456781",
                        "Test Merchant 2",
                        "Test Estate 2"});
#line 66
 testRunner.Given("I have assigned the following devices to the merchants", ((string)(null)), table13, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Reference",
                        "Amount",
                        "DateTime",
                        "MerchantName",
                        "EstateName"});
            table14.AddRow(new string[] {
                        "Deposit1",
                        "20.00",
                        "Today",
                        "Test Merchant 1",
                        "Test Estate 1"});
            table14.AddRow(new string[] {
                        "Deposit1",
                        "20.00",
                        "Today",
                        "Test Merchant 2",
                        "Test Estate 2"});
#line 71
 testRunner.Given("I make the following manual merchant deposits", ((string)(null)), table14, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "DateTime",
                        "TransactionNumber",
                        "TransactionType",
                        "TransactionSource",
                        "MerchantName",
                        "DeviceIdentifier",
                        "EstateName",
                        "OperatorName",
                        "TransactionAmount",
                        "CustomerAccountNumber",
                        "CustomerEmailAddress",
                        "ContractDescription",
                        "ProductName",
                        "RecipientEmail",
                        "RecipientMobile",
                        "MessageType",
                        "AccountNumber",
                        "CustomerName"});
            table15.AddRow(new string[] {
                        "Today",
                        "1",
                        "Sale",
                        "1",
                        "Test Merchant 1",
                        "123456780",
                        "Test Estate 1",
                        "Voucher",
                        "10.00",
                        "",
                        "",
                        "Hospital 1 Contract",
                        "10 KES",
                        "test@recipient.co.uk",
                        "",
                        "",
                        "",
                        ""});
            table15.AddRow(new string[] {
                        "Today",
                        "2",
                        "Sale",
                        "1",
                        "Test Merchant 2",
                        "123456781",
                        "Test Estate 2",
                        "Voucher",
                        "10.00",
                        "",
                        "",
                        "Hospital 2 Contract",
                        "10 KES",
                        "test@recipient.co.uk",
                        "",
                        "",
                        "",
                        ""});
#line 76
 testRunner.When("I perform the following transactions", ((string)(null)), table15, "When ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Redeem Vouchers")]
        [Xunit.TraitAttribute("FeatureTitle", "RedeemVoucher")]
        [Xunit.TraitAttribute("Description", "Redeem Vouchers")]
        [Xunit.TraitAttribute("Category", "PRTest")]
        public void RedeemVouchers()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Redeem Vouchers", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 82
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
#line 83
 testRunner.Given("I am logged in as \"redemptionuser@testredemption1.co.uk\" with password \"123456\" f" +
                        "or Estate \"Test Estate 1\" with client \"redemptionClient\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 85
 testRunner.When("I redeem the voucher for Estate \'Test Estate 1\' and Merchant \'Test Merchant 1\' tr" +
                        "ansaction number 1 the voucher balance will be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 87
 testRunner.Given("I am logged in as \"redemptionuser@testredemption2.co.uk\" with password \"123456\" f" +
                        "or Estate \"Test Estate 2\" with client \"redemptionClient\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 89
 testRunner.When("I redeem the voucher for Estate \'Test Estate 2\' and Merchant \'Test Merchant 2\' tr" +
                        "ansaction number 2 the voucher balance will be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                RedeemVoucherFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RedeemVoucherFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
