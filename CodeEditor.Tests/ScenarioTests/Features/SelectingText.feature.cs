﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace CodeEditor.Tests.ScenarioTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("SelectingText")]
    public partial class SelectingTextFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SelectingText.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SelectingText", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter one line and select chars in the middle - left to right")]
        public virtual void EnterOneLineAndSelectCharsInTheMiddle_LeftToRight()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter one line and select chars in the middle - left to right", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("Text to enter is \'asdfqwerzxcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
  testRunner.And("I select text from \'4\' \'0\' to \'8\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.Then("Selected text should be \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter one line and select chars in the middle - right to left")]
        public virtual void EnterOneLineAndSelectCharsInTheMiddle_RightToLeft()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter one line and select chars in the middle - right to left", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
 testRunner.Given("Text to enter is \'asdfqwerzxcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
  testRunner.And("I select text from \'8\' \'0\' to \'4\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.Then("Selected text should be \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter two lines and select text")]
        public virtual void EnterTwoLinesAndSelectText()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter two lines and select text", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
 testRunner.Given("Text to enter is \'I saw Susie sitting in a shoe shine shop.\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("Text to enter is \'Where she sits she shines, and where she shines she sits.\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
  testRunner.And("I select text from \'1\' \'0\' to \'10\' \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.Then("Selected text should be \' saw Susie sitting in a shoe shine shop.Where she \'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter two lines where first one is shorter than the second one")]
        public virtual void EnterTwoLinesWhereFirstOneIsShorterThanTheSecondOne()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter two lines where first one is shorter than the second one", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
 testRunner.Given("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
  testRunner.And("Text to enter is \'asdf xzcv qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
  testRunner.And("I select text from \'4\' \'0\' to \'9\' \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.Then("Selected text should be \'asdf xzcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
