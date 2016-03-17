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
namespace KnightFrank.Antares.UITests.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class CreatePropertyUITestsFeature : Xunit.IClassFixture<CreatePropertyUITestsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CreateProperty.feature"
#line hidden
        
        public CreatePropertyUITestsFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create Property UI tests", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void SetFixture(CreatePropertyUITestsFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Create Property UI tests")]
        [Xunit.TraitAttribute("Description", "Create new property for UK Address")]
        public virtual void CreateNewPropertyForUKAddress()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create new property for UK Address", new string[] {
                        "ignore"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("User navigates to create property page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Country"});
            table1.AddRow(new string[] {
                        "United Kingdom"});
#line 6
 testRunner.When("User selects country on create property page", ((string)(null)), table1, "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "PropertyNumber",
                        "PropertyName",
                        "AddressLine2",
                        "AddressLine3",
                        "Postcode",
                        "City",
                        "County"});
            table2.AddRow(new string[] {
                        "55",
                        "Knight Frank",
                        "Baker Street",
                        "",
                        "W1U 8AN",
                        "London",
                        "London"});
#line 9
  testRunner.And("User fills in address details on create property page", ((string)(null)), table2, "And ");
#line 12
  testRunner.And("User clicks save button on create property page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.Then("New property should be created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                CreatePropertyUITestsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CreatePropertyUITestsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion