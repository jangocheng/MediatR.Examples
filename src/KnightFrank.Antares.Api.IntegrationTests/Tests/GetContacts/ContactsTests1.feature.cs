﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace KnightFrank.Antares.Api.IntegrationTests.Tests.GetContacts
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class ContactsFeature : Xunit.IClassFixture<ContactsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ContactsTests.feature"
#line hidden
        
        public ContactsFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Contacts", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(ContactsFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Contacts")]
        [Xunit.TraitAttribute("Description", "Retrieve all contacts details")]
        public virtual void RetrieveAllContactsDetails()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve all contacts details", ((string[])(null)));
#line 3
 this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "FirstName",
                        "Surname",
                        "Title"});
            table1.AddRow(new string[] {
                        "Tomasz",
                        "Bien",
                        "Mister"});
            table1.AddRow(new string[] {
                        "David",
                        "Dummy",
                        "Mister"});
#line 4
  testRunner.Given("User has defined multiple contact details", ((string)(null)), table1, "Given ");
#line 8
  testRunner.When("User retrieves all contacts details", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
  testRunner.Then("User should get OK http status code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "FirstName",
                        "Surname",
                        "Title"});
            table2.AddRow(new string[] {
                        "Tomasz",
                        "Bien",
                        "Mister"});
            table2.AddRow(new string[] {
                        "David",
                        "Dummy",
                        "Mister"});
#line 10
   testRunner.And("contacts should have following details", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Contacts")]
        [Xunit.TraitAttribute("Description", "Retrieve single contact details")]
        public virtual void RetrieveSingleContactDetails()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve single contact details", ((string[])(null)));
#line 16
 this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "FirstName",
                        "Surname",
                        "Title"});
            table3.AddRow(new string[] {
                        "Tomasz",
                        "Bien",
                        "Mister"});
#line 17
  testRunner.Given("User has defined a contact details", ((string)(null)), table3, "Given ");
#line 20
  testRunner.When("User retrieves contacts details for proper id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
  testRunner.Then("User should get OK http status code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
   testRunner.And("contact should have same details as inserted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "Contacts")]
        [Xunit.TraitAttribute("Description", "Retrieve error messages for improper contact id")]
        [Xunit.InlineDataAttribute("-2", "NotFound", new string[0])]
        [Xunit.InlineDataAttribute("\"A\"", "BadRequest", new string[0])]
        public virtual void RetrieveErrorMessagesForImproperContactId(string id, string statusCode, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve error messages for improper contact id", exampleTags);
#line 25
 this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "FirstName",
                        "Surname",
                        "Title"});
            table4.AddRow(new string[] {
                        "Tomasz",
                        "Bien",
                        "Mister"});
#line 26
  testRunner.Given("User has defined a contact details", ((string)(null)), table4, "Given ");
#line 29
  testRunner.When(string.Format("User retrieves contacts details for {0} id", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
  testRunner.Then(string.Format("User should get {0} http status code", statusCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ContactsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ContactsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
