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
    public class ETLProcedureStatus : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string clientIdDropdown = "div.styles_clientSelect__PHeFB";
        public const string clientIdDropdownSearchbox = "input.p-dropdown-filter";
        public const string clientIdDropdownList = "div.p-dropdown-items-wrapper";
        public const string payloadNameHeader = "";
        public const string dateReceivedHeader = "";
        public const string statusHeader = "";
        public const string actionsHeader = "";
        public const string searchByPayloadNameInput = "";
        public const string searchByProcedureStatusInput = "";
        public const string procedureResultsTable = "table.e2e-procedure-table";
        public const string refreshButton = "[e2e-id=\"RefreshButton\"]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLProcedureStatus(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public LazyElement ClientIdDropdownElement => GetLazyElement(By.CssSelector(clientIdDropdown), "{Client ID Dropdown}");
        public LazyElement ClientIdDropdownSearchboxElement => GetLazyElement(By.CssSelector(clientIdDropdownSearchbox), "{Client ID Dropdown Searchbox}");
        public LazyElement ClientIdDropdownListElement => GetLazyElement(By.CssSelector(clientIdDropdownList), "{Client ID Dropdown List}");
        public LazyElement RefreshButtonElement => GetLazyElement(By.CssSelector(refreshButton), "{Refresh Button}");
        public LazyElement ProcedureResultsTableElement => GetLazyElement(By.CssSelector(procedureResultsTable), "{Procedure Results Table}");

        public override bool IsPageLoaded() => throw new System.NotImplementedException();

        public bool IsClientIdDropdownDisplayed() => ClientIdDropdownElement.Displayed;
        public bool IsProcedureResultsTableDisplayed() => ProcedureResultsTableElement.Displayed;
        public bool IsRefreshButtonDisplayed() => RefreshButtonElement.Displayed;

        public ETLProcedureStatus SelectClient(string clientName)
        {
            this.SelectClientInDropdown(clientName);
            return this;
        }
    }
}