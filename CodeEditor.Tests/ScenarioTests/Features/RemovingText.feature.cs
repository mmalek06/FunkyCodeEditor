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
    [NUnit.Framework.DescriptionAttribute("RemovingText")]
    public partial class RemovingTextFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RemovingText.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RemovingText", "\tIn order to remove text\r\n\tAs a user\r\n\tI want to press backspace or delete keys w" +
                    "ith or without text selected", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Enter one character and press backspace")]
        public virtual void EnterOneCharacterAndPressBackspace()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter one character and press backspace", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter single empty line and press backspace")]
        public virtual void EnterSingleEmptyLineAndPressBackspace()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter single empty line and press backspace", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enter three empty lines and press delete")]
        public virtual void EnterThreeEmptyLinesAndPressDelete()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enter three empty lines and press delete", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete from empty line")]
        public virtual void DeleteFromEmptyLine()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete from empty line", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete last line when the cursor is at the second one")]
        public virtual void DeleteLastLineWhenTheCursorIsAtTheSecondOne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete last line when the cursor is at the second one", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete last line that is not empty and when the cursor is at the second one")]
        public virtual void DeleteLastLineThatIsNotEmptyAndWhenTheCursorIsAtTheSecondOne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete last line that is not empty and when the cursor is at the second one", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Two non empty lines entered, delete pressed at the end of first one")]
        public virtual void TwoNonEmptyLinesEnteredDeletePressedAtTheEndOfFirstOne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Two non empty lines entered, delete pressed at the end of first one", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Backspace first line when the cursor is at the second one")]
        public virtual void BackspaceFirstLineWhenTheCursorIsAtTheSecondOne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Backspace first line when the cursor is at the second one", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Two non empty lines entered, backspace pressed at the beginning of second one")]
        public virtual void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecondOne()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Two non empty lines entered, backspace pressed at the beginning of second one", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Four lines entered, delete pressed at the end of first")]
        public virtual void FourLinesEnteredDeletePressedAtTheEndOfFirst()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Four lines entered, delete pressed at the end of first", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Four lines entered, delete pressed twice at the end of first")]
        public virtual void FourLinesEnteredDeletePressedTwiceAtTheEndOfFirst()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Four lines entered, delete pressed twice at the end of first", ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove from collapsed line with brackets only")]
        public virtual void RemoveFromCollapsedLineWithBracketsOnly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove from collapsed line with brackets only", ((string[])(null)));
#line 145
this.ScenarioSetup(scenarioInfo);
#line 146
 testRunner.Given("Text to enter is \'{}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 147
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 148
  testRunner.And("I request folding for position starting at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
  testRunner.And("I move caret to column number \'5\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
 testRunner.Then("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 152
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
  testRunner.And("The \'0\' line should be equal to \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
  testRunner.And("Cursor should be at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
  testRunner.And("I should see no folding", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove from collapsed line with brackets and text")]
        public virtual void RemoveFromCollapsedLineWithBracketsAndText()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove from collapsed line with brackets and text", ((string[])(null)));
#line 158
this.ScenarioSetup(scenarioInfo);
#line 159
 testRunner.Given("Text to enter is \'asdf \'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 160
  testRunner.And("Text to enter is \'{}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 161
  testRunner.And("Text to enter is \' qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 162
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 163
  testRunner.And("I request folding for position starting at \'5\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
  testRunner.And("I move caret to column number \'10\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
  testRunner.And("I hit backspace key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 166
 testRunner.Then("I should see \'1\' lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 167
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
  testRunner.And("The \'0\' line should be equal to \'asdf  qwer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
  testRunner.And("Cursor should be at \'5\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
  testRunner.And("I should see no folding", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Press delete after collapse")]
        public virtual void PressDeleteAfterCollapse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Press delete after collapse", ((string[])(null)));
#line 173
this.ScenarioSetup(scenarioInfo);
#line 174
 testRunner.Given("Text to enter is \'{\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 175
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
  testRunner.And("Text to enter is newline", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
  testRunner.And("Text to enter is \'}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
 testRunner.When("I enter text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 179
  testRunner.And("I request folding for position starting at \'0\' \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
  testRunner.And("I move caret to column number \'5\' in line \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
  testRunner.And("I hit delete key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
 testRunner.Then("I should see folding on position starting at \'0\' \'0\' and ending at \'0\' \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 183
  testRunner.And("Shown number of lines in the lines panel should be \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
  testRunner.And("The \'0\' line should be equal to \'{...}\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
