using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using System.Threading;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class ETLPayloadsView : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string viewPayloadsPage = "main.AppDrawers_content__hP5l1";
        public const string engagementDropdown = "[e2e-id=\"engagementId\"]";
        public const string engagementDropdownSearchbox = "input.p-dropdown-filter";
        public const string engagementDropdownList = "div.p-dropdown-items-wrapper";
        public const string projectDropdown = "[e2e-id=\"projectId\"]";
        public const string projectDropdownSearchbox = "input.p-dropdown-filter";
        public const string projectDropdownList = "div.p-dropdown-items-wrapper";
        public const string legalEntityDropdown = "[e2e-id=\"legalEntityId\"]";
        public const string legalEntityDropdownSearchbox = "input.p-dropdown-filter";
        public const string legalEntityDropdownList = "div.p-dropdown-items-wrapper";
        public const string resetButton = "[e2e-id=\"resetButton\"]";
        public const string payloadsTab = "a[id*=\"header_0\"]";
        public const string protocolsTab = "a[id*=\"header_1\"]";
        public const string addNewPayloadButton = "[e2e-id=\"add-new-button\"]";
        public const string payloadNameHeader = "";

        public const string engagementHeader = "";
        public const string projectHeader = "//th[.=\"Project\"]";
        public const string legalEntityHeader = "";
        public const string protocolNameHeader = "";
        public const string searchByPayloadNameInput = "input[placeholder=\"Search by Payload Name\"]";

        public const string searchByEngagementInput = "input[placeholder=\"Search by Engagement\"]";
        public const string searchByProjectInput = "input[placeholder=\"Search by Project\"]";
        public const string searchByLegalEntityInput = "input[placeholder=\"Search by Legal Entity\"]";
        public const string searchByProtocolNameInput = "input[placeholder=\"Search by Protocol Name\"]";
        public const string payloadsResultTable = "table.p-datatable-scrollable-body-table";
        public const string notificationMessage = "div.MuiAlert-message";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLPayloadsView(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public ETLProtocolsView ClickProtocolsTab()
        {
            ProtocolsTabElement.Click();
            return new ETLProtocolsView(this.TestObject);
        }

        public ETLPayloadsView SelectClient(string clientName)
        {
            SelectClientInDropdown(clientName);
            return this;
        }

        public ETLPayloadsView SelectAnyClient()
        {
            //wait bug fixing: clients are not visible from the beggining
            Thread.Sleep(3000);

            SelectRandomClient();
            return this;
        }

        public LazyElement ViewPayloadsPageElement => GetLazyElement(By.CssSelector(viewPayloadsPage), "{View Payloads Page}");
        public LazyElement EngagementDropdownElement => GetLazyElement(By.CssSelector(engagementDropdown), "{Engagement Dropdown}");
        public LazyElement EngagementDropdownSearchboxElement => GetLazyElement(By.CssSelector(engagementDropdownSearchbox), "{Engagement Dropdown Searchbox}");
        public LazyElement EngagementDropdownListElement => GetLazyElement(By.CssSelector(engagementDropdownList), "{Engagement Dropdown List}");

        public ETLPayloadsAdd ClickAddNewPayloadButton()
        {
            AddNewPayloadButtonElement.Click();
            return new ETLPayloadsAdd(this.TestObject);
        }

        public LazyElement ProjectDropdownElement => GetLazyElement(By.CssSelector(projectDropdown), "{Project Dropdown}");
        public LazyElement ProjectDropdownSearchboxElement => GetLazyElement(By.CssSelector(projectDropdownSearchbox), "{Project Dropdown Searchbox}");
        public LazyElement ProjectDropdownListElement => GetLazyElement(By.CssSelector(projectDropdownList), "{Project Dropdown List}");
        public LazyElement LegalEntityDropdownElement => GetLazyElement(By.CssSelector(legalEntityDropdown), "{Legal Entity Dropdown}");
        public LazyElement LegalEntityDropdownSearchboxElement => GetLazyElement(By.CssSelector(legalEntityDropdownSearchbox), "{Legal Entity Dropdown Searchbox}");
        public LazyElement LegalEntityDropdownListElement => GetLazyElement(By.CssSelector(legalEntityDropdownList), "{Legal Entity Dropdown List}");
        public LazyElement ResetButtonElement => GetLazyElement(By.CssSelector(resetButton), "{Reset Button}");
        public LazyElement PayloadsTabElement => GetLazyElement(By.CssSelector(payloadsTab), "{Payloads Tab}");
        public LazyElement ProtocolsTabElement => GetLazyElement(By.CssSelector(protocolsTab), "{Protocols Tab}");
        public LazyElement AddNewPayloadButtonElement => GetLazyElement(By.CssSelector(addNewPayloadButton), "{Add New Payload Button}");

        public ETLProcedureStatus ClickBtnProcedureStatus()
        {
            BtnProcedureStatus.Click();
            return new ETLProcedureStatus(this.TestObject);
        }

        public LazyElement PayloadNameHeaderElement => GetLazyElement(By.CssSelector(payloadNameHeader), "{Payload Name Header}");
        public LazyElement EngagementHeaderElement => GetLazyElement(By.CssSelector(engagementHeader), "{Engagement Header}");
        public LazyElement ProjectHeaderElement => GetLazyElement(By.CssSelector(projectHeader), "{Project Header}");
        public LazyElement LegalEntityHeaderElement => GetLazyElement(By.CssSelector(legalEntityHeader), "{Legal Entity Header}");
        public LazyElement ProtocolNameHeaderElement => GetLazyElement(By.CssSelector(protocolNameHeader), "{Protocol Name Header}");

        public LazyElement SearchByPayloadNameInputElement => GetLazyElement(By.CssSelector(searchByPayloadNameInput), "{Search By Payload Name Input}");
        public LazyElement SearchByEngagementInputElement => GetLazyElement(By.CssSelector(searchByEngagementInput), "{Search By Engagement Input}");
        public LazyElement SearchByProjectInputElement => GetLazyElement(By.CssSelector(searchByProjectInput), "{Search By Project Input}");
        public LazyElement SearchByLegalEntityInputElement => GetLazyElement(By.CssSelector(searchByLegalEntityInput), "{Search By Legal Entity Input}");
        public LazyElement SearchByProtocolNameInputElement => GetLazyElement(By.CssSelector(searchByProtocolNameInput), "{Search By Protocol Name Input}");
        public LazyElement PayloadsResultTableElement => GetLazyElement(By.CssSelector(payloadsResultTable), "{Payloads Result Table Input}");

        public override bool IsPageLoaded() => ViewPayloadsPageElement.Displayed;

        public bool IsClientIdDropdownDisplayed() => ClientIdDropdownElement.Displayed;
        public bool IsEngagementDropdownDisplayed() => EngagementDropdownElement.Displayed;
        public bool IsProjectDropdownDisplayed() => ProjectDropdownElement.Displayed;
        public bool IsLegalEntityDropdownDisplayed() => LegalEntityDropdownElement.Displayed;
        public bool IsResetButtonDisplayed() => ResetButtonElement.Displayed;
        public bool IsPayloadsTabDisplayed() => PayloadsTabElement.Displayed;
        public bool IsProtocolsTabDisplayed() => ProtocolsTabElement.Displayed;
        public bool IsAddNewPayloadButtonDisplayed() => AddNewPayloadButtonElement.Displayed;

        public bool IsProtocolsTabClickable() => ViewPayloadsPageElement.Wait().UntilClickableElement(By.Id(protocolsTab));

        //@TODO: move it to menu class
        public ETLProcedureLibrary ClickBtnProcedureLibrary()
        {
            BtnProcedureLibrary.Click();
            return new ETLProcedureLibrary(this.TestObject);
        }

        public ETLUploaderUploadFile ClickLnkUploadFile()
        {
            BtnEtlUploader.HoverOver();
            LnkUploadFile.Click();
            return new ETLUploaderUploadFile(this.TestObject);
        }

        public ETLUploaderViewResults ClickLnkViewResults()
        {
            BtnEtlUploader.HoverOver();
            LnkViewResults.Click();
            return new ETLUploaderViewResults(this.TestObject);
        }
    }
}