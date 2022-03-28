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
    public class ETLProtocolsView : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string viewProtocolsPage = "main.AppDrawers_content__hP5l1";
        public const string clientIdDropdown = "div.styles_clientSelect__PHeFB";
        public const string clientIdDropdownSearchbox = "input.p-dropdown-filter";
        public const string clientIdDropdownList = "div.p-dropdown-items-wrapper";
        public const string engagementDropdown = "[e2e-id=\"engagementId\"]";
        public const string projectDropdown = "[e2e-id=\"projectId\"]";
        public const string legalEntityDropdown = "[e2e-id=\"legalEntityId\"]";
        public const string resetButton = "[e2e-id=\"resetButton\"]";
        public const string payloadsTab = "a[id*=\"header_0\"]";
        public const string protocolsTab = "a[id*=\"header_1\"]";
        public const string addNewProtocolButton = "[e2e-id=\"add-new-button\"]";
        public const string protocolNameHeader = "";
        public const string protocolTypeHeader = "";
        public const string numberOfPayloadsHeader = "";
        public const string searchByProtocolNameInput = "";
        public const string searchByProtocolTypeInput = "";
        public const string searchByNumberOfPayloadsInput = "";
        public const string protocolsResultTable = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLProtocolsView(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public ETLProtocolsAdd ClickAddNewProtocolButton()
        {
            AddNewProtocolButtonElement.Click();
            return new ETLProtocolsAdd(this.TestObject);
        }


        public LazyElement ViewProtocolsPageElement => GetLazyElement(By.CssSelector(viewProtocolsPage), "{View Protocols Page}");
        public LazyElement ClientIdDropdownElement => GetLazyElement(By.CssSelector(clientIdDropdown), "{Client ID Dropdown}");
        public LazyElement ClientIdDropdownSearchboxElement => GetLazyElement(By.CssSelector(clientIdDropdownSearchbox), "{Client ID Dropdown Searchbox}");
        public LazyElement ClientIdDropdownListElement => GetLazyElement(By.CssSelector(clientIdDropdownList), "{Client ID Dropdown List}");
        public LazyElement EngagementDropdownElement => GetLazyElement(By.CssSelector(engagementDropdown), "{Engagement Dropdown}");
        public LazyElement ProjectDropdownElement => GetLazyElement(By.CssSelector(projectDropdown), "{Project Dropdown}");
        public LazyElement LegalEntityDropdownElement => GetLazyElement(By.CssSelector(legalEntityDropdown), "{Legal Entity Dropdown}");
        public LazyElement ResetButtonElement => GetLazyElement(By.CssSelector(resetButton), "{Reset Button}");
        public LazyElement PayloadsTabElement => GetLazyElement(By.CssSelector(payloadsTab), "{Payloads Tab}");
        public LazyElement ProtocolsTabElement => GetLazyElement(By.CssSelector(protocolsTab), "{Protocols Tab}");
        public LazyElement AddNewProtocolButtonElement => GetLazyElement(By.CssSelector(addNewProtocolButton), "{Add New Protocol Button}");
        public LazyElement ProtocolNameHeaderElement => GetLazyElement(By.CssSelector(protocolNameHeader), "{Protocol Name Header}");
        public LazyElement ProtocolTypeHeaderElement => GetLazyElement(By.CssSelector(protocolTypeHeader), "{Protocol Type Header}");
        public LazyElement NumberOfPayloadsHeaderElement => GetLazyElement(By.CssSelector(numberOfPayloadsHeader), "{Number of Payloads Header}");
        public LazyElement SearchByProtocolNameInputElement => GetLazyElement(By.CssSelector(searchByProtocolNameInput), "{Search By Protocol Name Input}");
        public LazyElement SearchByProtocolTypeInputElement => GetLazyElement(By.CssSelector(searchByProtocolNameInput), "{Search By Protocol Type Input}");
        public LazyElement SearchByNumberOfPayloadsInputElement => GetLazyElement(By.CssSelector(searchByNumberOfPayloadsInput), "{Search By Number of Payloads Input}");
        public LazyElement ProtocolsResultTableElement => GetLazyElement(By.CssSelector(protocolsResultTable), "{Protocols Result Table Input}");

        public override bool IsPageLoaded() => ViewProtocolsPageElement.Displayed;

        // GET DROPDOWNS OPTIONS BY LABEL
        public LazyElement GetClientByName(string clientName) => GetLazyElement(By.CssSelector("li[aria-label=\"" + clientName + "\"]"), "{Client Dropdown Element:" + clientName + "}");

        public bool IsClientIdDropdownDisplayed() => ClientIdDropdownElement.Displayed;
        public bool IsEngagementDropdownDisplayed() => EngagementDropdownElement.Displayed;
        public bool IsProjectDropdownDisplayed() => ProjectDropdownElement.Displayed;
        public bool IsLegalEntityDropdownDisplayed() => LegalEntityDropdownElement.Displayed;
        public bool IsResetButtonDisplayed() => ResetButtonElement.Displayed;
        public bool IsPayloadsTabDisplayed() => PayloadsTabElement.Displayed;
        public bool IsProtocolsTabDisplayed() => ProtocolsTabElement.Displayed;
        public bool IsAddNewProtocolButtonDisplayed() => AddNewProtocolButtonElement.Displayed;
    }
}

