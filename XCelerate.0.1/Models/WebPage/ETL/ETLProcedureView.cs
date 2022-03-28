using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class ETLProcedureView : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string pageTitle = "h2.page-title";
        public const string procedureNameInput = "name";
        public const string workflowDropdown = "[e2e-id=\"alteryXWorkflowId\"]";
        public const string questionsList = "div[e2e-id*=\"question\"]";
        public const string schemaTypeDropdown = "[e2e-id=\"alteryXQuestionsToFiles[0].schemaTypeId\"]";
        public const string addSchemaButton = "button[class*=\"addSchemaButton\"]";
        public const string fileTypeDropdown = "[e2e-id=\"alteryXQuestionsToFiles[0].fileTypeId\"]";
        public const string visibilityPrivateRadiobutton = "";
        public const string visibilityPublicRadiobutton = "";
        public const string audienceClientRadiobutton = "";
        public const string audienceSolutionRadiobutton = "";
        public const string clientDropdown = "[e2e-id=\"mdmClientId\"]";
        public const string solutionDropdown = "[e2e-id=\"applicationId\"]";
        public const string timeSavingsInput = "[name=\"timeSavings\"]";
        public const string cancelButton = "[e2e-id=\"cancelButton\"]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLProcedureView(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public LazyElement PageTitleElement => GetLazyElement(By.CssSelector(pageTitle), "{Page Title}");
        public LazyElement ProcedureNameInputElement => GetLazyElement(By.Id(procedureNameInput), "{Procedure Name Input}");
        public LazyElement WorkflowDropdownElement => GetLazyElement(By.CssSelector(workflowDropdown), "{Workflow Dropdown}");
        public LazyElement QuestionsListElement => GetLazyElement(By.CssSelector(questionsList), "{Questions List}");
        public LazyElement SchemaTypeDropdownElement => GetLazyElement(By.CssSelector(schemaTypeDropdown), "{Schema Type Dropdown}");
        public LazyElement AddSchemaButtonElement => GetLazyElement(By.CssSelector(addSchemaButton), "{Add Schema Button}");
        public LazyElement FileTypeDropdownElement => GetLazyElement(By.CssSelector(fileTypeDropdown), "{File Type Dropdown}");
        public LazyElement ClientDropdownElement => GetLazyElement(By.CssSelector(clientDropdown), "{Client Dropdown}");
        public LazyElement SolutionDropdownElement => GetLazyElement(By.CssSelector(solutionDropdown), "{Solution Dropdown}");
        public LazyElement TimeSavingsInputElement => GetLazyElement(By.CssSelector(timeSavingsInput), "{Time Savings Input}");
        public LazyElement CancelButtonElement => GetLazyElement(By.CssSelector(cancelButton), "{Cancel Button}");

        public override bool IsPageLoaded() => ProcedureNameInputElement.Displayed;

        public bool IsProcedureNameInputDisplayed() => ProcedureNameInputElement.Displayed;
        public bool IsWorkflowDropdownDisplayed() => WorkflowDropdownElement.Displayed;
        public bool IsQuestionsListDisplayed() => QuestionsListElement.Displayed;
        public bool IsSchemaTypeDropdownDisplayed() => SchemaTypeDropdownElement.Displayed;
        public bool IsAddSchemaButtonDisplayed() => AddSchemaButtonElement.Displayed;
        public bool IsFileTypeDropdownDisplayed() => FileTypeDropdownElement.Displayed;
        public bool IsClientDropdownDisplayed() => ClientDropdownElement.Displayed;
        public bool IsSolutionDropdownDisplayed() => SolutionDropdownElement.Displayed;
        public bool IsTimeSavingsInputDisplayed() => TimeSavingsInputElement.Displayed;
        public bool IsCancelButtonDisplayed() => CancelButtonElement.Displayed;
    }
}