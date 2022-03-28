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
    public class ETLProcedureLibrary : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string procedureSelectionForm = "div[e2e-id=\"procedure-select\"]";
        public const string pageTitle = "";
        public const string procedureDropdown = "procedureSelect";
        public const string procedureDropdownSearchbox = "";
        public const string procedureDropdownList = "";
        public const string addNewProcedureButton = "button[e2e-id=\"addNewProcedure\"]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLProcedureLibrary(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public LazyElement ProcedureSelectionFormElement => GetLazyElement(By.CssSelector(procedureSelectionForm), "{Procedure Selection Form}");
        public LazyElement ProcedureDropdownElement => GetLazyElement(By.Id(procedureDropdown), "{ProcedureDropdown}");
        public LazyElement AddNewProcedureButtonElement => GetLazyElement(By.CssSelector(addNewProcedureButton), "{AddNewProcedureButton}");

        public override bool IsPageLoaded() => ProcedureSelectionFormElement.Displayed;

        public bool IsProcedureDropdownDisplayed() => ProcedureDropdownElement.Displayed;
        public bool IsAddNewProcedureButtonDisplayed() => AddNewProcedureButtonElement.Displayed;

        public ETLProcedureView ChooseItemInProcedureDropdown(string procedure)
        {
            ProcedureDropdownElement.Click();
            SelectDropdownValue(procedure);
            return new ETLProcedureView(this.TestObject);
        }

        public ETLProcedureView ChooseAnyProcedureDropdown()
        {
            SelectItemInDropDown(ProcedureDropdownElement, "");
            return new ETLProcedureView(this.TestObject);
        }

        public ETLProcedureAdd ClickAddNewProcedureButton()
        {
            AddNewProcedureButtonElement.Click();
            return new ETLProcedureAdd(this.TestObject);
        }
    }
}