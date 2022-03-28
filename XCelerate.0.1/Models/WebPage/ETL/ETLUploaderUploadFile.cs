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
    public class ETLUploaderUploadFile : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string uploaderForm = "form[e2e-id=\"uploadForm\"]";
        public const string uploadFileTab = "div[aria-label=\"UPLOAD FILE\"]";
        public const string viewResultsTab = "div[aria-label=\"VIEW RESULTS\"]";
        public const string pageTitle = "h2.page-title";
        public const string solutionDropdown = "solution";
        public const string solutionDropdownList = "";
        public const string procedureDropdown = "procedure";
        public const string procedureDropdownList = "";
        public const string mandatoryFilesList = "div[class*=\"style_uploadRow\"]";
        public const string cancelButton = "[e2e-id=\"cancelButton\"]";
        public const string submitButton = "[e2e-id=\"submitButton\"]";
        public const string uploadFileInput = "/html/body/div[1]/div/div[2]/main/div/form/div[3]/div/div/span/input";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLUploaderUploadFile(ISeleniumTestObject testObject) : base(testObject)
        { }

        public LazyElement UploaderForm => GetLazyElement(By.CssSelector(uploaderForm));
        public LazyElement UploadFileTab => GetLazyElement(By.CssSelector(uploadFileTab));
        public LazyElement ViewResultsTab => GetLazyElement(By.CssSelector(viewResultsTab));
        public LazyElement SolutionDropdown => GetLazyElement(By.Id(solutionDropdown));
        public LazyElement ProcedureDropdown => GetLazyElement(By.Id(procedureDropdown));
        public LazyElement MandatoryFilesList => GetLazyElement(By.CssSelector(mandatoryFilesList));
        public LazyElement CancelButton => GetLazyElement(By.CssSelector(cancelButton));
        public LazyElement SubmitButton => GetLazyElement(By.CssSelector(submitButton));
        public LazyElement UploadFileInput => GetLazyElement(By.XPath(uploadFileInput));

        public override bool IsPageLoaded() => UploaderForm.Displayed;

        public bool IsUploadFileTabDisplayed() => UploadFileTab.Displayed;
        public bool IsViewResultsTabDisplayed() => ViewResultsTab.Displayed;
        public bool IsSolutionDropdownDisplayed() => SolutionDropdown.Displayed;
        public bool IsProcedureDropdownDisplayed() => ProcedureDropdown.Displayed;
        public bool IsMandatoryFilesListDisplayed() => MandatoryFilesList.Displayed;
        public bool IsCancelButtonDisplayed() => CancelButton.Displayed;
        public bool IsSubmitButtonDisplayed() => SubmitButton.Displayed;

        public ETLUploaderViewResults ClickViewResultsTab()
        {
            ViewResultsTab.Click();
            return new ETLUploaderViewResults(this.TestObject);
        }

        public ETLUploaderUploadFile ChooseSolutionInDropdown(string value)
        {
            SolutionDropdown.Click();
            SelectDropdownValue(value);
            return this;
        }

        public ETLUploaderUploadFile ChooseProcedureDropdown(string procedureName)
        {
            ProcedureDropdown.Click();
            SelectDropdownValue(procedureName);
            return this;
        }

        public ETLUploaderUploadFile ChooseAnySolutionInDropdown()
        {
            SelectItemInDropDown(SolutionDropdown, "");
            return this;
        }

        public ETLUploaderUploadFile ChooseAnyProcedureInDropdown()
        {
            SelectItemInDropDown(ProcedureDropdown, "");
            return this;
        }
    }
}