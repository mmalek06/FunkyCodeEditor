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
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class RemovingTextFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RemovingText.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RemovingText", "\tIn order to remove text\r\n\tAs a user\r\n\tI want to press backspace or delete keys w" +
                    "ith or without text selected", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "RemovingText")))
            {
                CodeEditor.Tests.ScenarioTests.Features.RemovingTextFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
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
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Enter one character and press backspace - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void EnterOneCharacterAndPressBackspace_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter one character and press backspace - Green Path", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("Text to enter is \'a\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("Cursor should be at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Enter single empty line and press backspace - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void EnterSingleEmptyLineAndPressBackspace_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter single empty line and press backspace - Green Path", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.Then("Cursor should be at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
  testRunner.And("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Enter three empty lines and press delete - Red Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void EnterThreeEmptyLinesAndPressDelete_RedPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter three empty lines and press delete - Red Path", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 25
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.Then("Cursor should be at \'0\' \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
  testRunner.And("I should see \'3\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
  testRunner.And("Shown number of lines in the lines panel should be \'3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete from empty line - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void DeleteFromEmptyLine_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete from empty line - Green Path", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 35
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
  testRunner.And("I move caret to column number \'0\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
  testRunner.And("Cursor should be at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete last line when the cursor is at the second one - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void DeleteLastLineWhenTheCursorIsAtTheSecondOne_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete last line when the cursor is at the second one - Green Path", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 46
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
  testRunner.And("I move caret to column number \'0\' in line \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
 testRunner.Then("I should see \'2\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 52
  testRunner.And("Shown number of lines in the lines panel should be \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
  testRunner.And("The \'1\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
  testRunner.And("Cursor should be at \'0\' \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete last line that is not empty and when the cursor is at the second one - Gre" +
            "en Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void DeleteLastLineThatIsNotEmptyAndWhenTheCursorIsAtTheSecondOne_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete last line that is not empty and when the cursor is at the second one - Gre" +
                    "en Path", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 59
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
  testRunner.And("I move caret to column number \'0\' in line \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.Then("I should see \'2\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
  testRunner.And("Shown number of lines in the lines panel should be \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
  testRunner.And("The \'1\' line should be equal to \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Two non empty lines entered, delete pressed at the end of first one - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void TwoNonEmptyLinesEnteredDeletePressedAtTheEndOfFirstOne_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Two non empty lines entered, delete pressed at the end of first one - Green Path", ((string[])(null)));
#line 71
this.ScenarioSetup(scenarioInfo);
#line 72
 testRunner.Given("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 73
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
  testRunner.And("Text to enter is \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 76
  testRunner.And("I move caret to column number \'4\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
 testRunner.Then("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And("The \'0\' line should be equal to \'asdfqwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
  testRunner.And("Cursor should be at \'4\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Backspace first line when the cursor is at the second one - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void BackspaceFirstLineWhenTheCursorIsAtTheSecondOne_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Backspace first line when the cursor is at the second one - Green Path", ((string[])(null)));
#line 84
this.ScenarioSetup(scenarioInfo);
#line 85
 testRunner.Given("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 86
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
  testRunner.And("I move caret to column number \'0\' in line \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
 testRunner.Then("I should see \'2\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 91
  testRunner.And("Shown number of lines in the lines panel should be \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
  testRunner.And("The \'1\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
  testRunner.And("Cursor should be at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Two non empty lines entered, backspace pressed at the beginning of second one - G" +
            "reen Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecondOne_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Two non empty lines entered, backspace pressed at the beginning of second one - G" +
                    "reen Path", ((string[])(null)));
#line 97
this.ScenarioSetup(scenarioInfo);
#line 98
 testRunner.Given("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 99
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And("Text to enter is \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
  testRunner.And("I move caret to column number \'0\' in line \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
 testRunner.Then("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 105
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
  testRunner.And("The \'0\' line should be equal to \'asdfqwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
  testRunner.And("Cursor should be at \'4\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Four lines entered, delete pressed at the end of first - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void FourLinesEnteredDeletePressedAtTheEndOfFirst_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Four lines entered, delete pressed at the end of first - Green Path", ((string[])(null)));
#line 110
this.ScenarioSetup(scenarioInfo);
#line 111
 testRunner.Given("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 112
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("Text to enter is \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("Text to enter is \'xzcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 118
  testRunner.And("I move caret to column number \'4\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.Then("I should see \'3\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 121
  testRunner.And("Shown number of lines in the lines panel should be \'3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
  testRunner.And("The \'0\' line should be equal to \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("The \'1\' line should be equal to \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
  testRunner.And("The \'2\' line should be equal to \'xzcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
  testRunner.And("Cursor should be at \'4\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Four lines entered, delete pressed twice at the end of first - Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RemovingText")]
        public virtual void FourLinesEnteredDeletePressedTwiceAtTheEndOfFirst_GreenPath()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Four lines entered, delete pressed twice at the end of first - Green Path", ((string[])(null)));
#line 128
this.ScenarioSetup(scenarioInfo);
#line 129
 testRunner.Given("Text to enter is \'asdf\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 130
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
  testRunner.And("Text to enter is \'qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
  testRunner.And("Text to enter is \'xzcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 136
  testRunner.And("I move caret to column number \'4\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
 testRunner.Then("I should see \'2\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 140
  testRunner.And("Shown number of lines in the lines panel should be \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
  testRunner.And("The \'0\' line should be equal to \'asdfqwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
  testRunner.And("The \'1\' line should be equal to \'xzcv\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
