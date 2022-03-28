using System;
using System.Collections.Generic;
using AngleSharp.Common;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class BasePage : BaseSeleniumPageModel
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        public const string _btnBellHeaderE2e = "appBarMenuButton_0";
        public const string _btnUserHeaderE2e = "appBarMenuButton_1";

        // PAGE BASE SELECTOR
        public const string page = "#root";

        //topbar
        public const string RSMLogo = ".svg.MuiSvgIcon-root.MuiSvgIcon-fontSizeLarge";
        public const string usernameText = ".p-ml-auto";
        public const string usernameIcon = ".MuiButtonBase-root.MuiIconButton-colorInherit";


        // NOTIFICATION SELECTOR
        public const string notification = "div.MuiAlert-message";

        // SELECTOR
        public const string _dropdownValueLocator = "li.p-dropdown-item";

        /// CDS
        public const string _icnRsmCdsLogoE2e = "app-bar-icon-button-link";
        private const string _lblCentralDataServicesE2e = "app-bar-app-name";
        public const string _mnuManageListE2e = "nav-item-manage-lists";

        public const string _btnSaveE2e = "submitButton";
        public const string _btnCancelE2e = "cancelButton";
        public const string _btnDeleteE2e = "deleteButton";

        public const string _btnPreviousE2e = "previous-page-button";
        public const string _btnNextE2e = "next-page-button";
        public const string _selNumberPerPageCss = ".p-dropdown.p-component.p-inputwrapper.p-inputwrapper-filled";

        //most page pages with table has sortIcon
        private const string sortIcon = ".p-sortable-column-icon"; //e2e should be added

        //left sidebar
        public const string _btnEtlConfigurationId = "nav-item-сonfiguration";
        public const string _btnProcedureStatusId = "nav-item-procedure-status";
        public const string _btnEtlUploaderId = "nav-item-uploader";
        public const string _mnuEtlUploaderE2e = "nav-panel-content";
        public const string _lnkUploadFileXpath = "//div[@e2e-id='nav-panel-container']//span[1]";
        public const string _lnkViewResultsXpath = "//div[@e2e-id='nav-panel-container']//span[2]";
        public const string _btnProcedureLibraryId = "nav-item-procedure-library";

        //CEM
        private const string ToolBar = "div.MuiToolbar-root";
        private const string RSMbutton = "button.MuiButtonBase-root"; //should be changed 2 elements
        private const string LeftBar = "ul.MuiList-root.AppDrawers_appList__1hzVa";
        private const string SelectClientDropBox = "#clientSelect";
        private const string SearchClientBox = "div.p-dropdown-filter-container";
        // private const string ClientsList = "ul.MuiList-root.AppDrawers_appList__1hzVa";
        private const string ClearSearchBoxButton = "i.p-dropdown-clear-icon";
        private const string calendarField = "";
        private const string calendarDatePicker = "";
        private const string calendarDate = "";
        private const string checkBoxes = "div.p-checkbox";
        public const string clientIdDropdownSearchbox = "input.p-dropdown-filter";
        public const string clientIdDropdownList = "div.p-dropdown-items-wrapper";
        public const string clientIdDropdown = "div.styles_clientSelect__PHeFB";
        public const string firstDropdownValueCSS = ".p-dropdown-item";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public BasePage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        //get elements
        public LazyElement ClientIdDropdownElement => GetLazyElement(By.CssSelector(clientIdDropdown), "{Client ID Dropdown}");
        public LazyElement ClientIdDropdownSearchboxElement => GetLazyElement(By.CssSelector(clientIdDropdownSearchbox), "{Client ID Dropdown Searchbox}");
        public LazyElement ClientIdDropdownListElement => GetLazyElement(By.CssSelector(clientIdDropdownList), "{Client ID Dropdown List}");

        public LazyElement PageElement => GetLazyElement(By.CssSelector(page));
        public LazyElement NotificationElement => GetLazyElement(By.CssSelector(notification), "Notification");
        public LazyElement DropdownValueElement(string value) => GetLazyElement(By.CssSelector("li[aria-label*=\"" + value + "\"]"), "{Dropdown Element:" + value + "}");
        public ICollection<IWebElement> DropdownValuesList => this.GetElementsByCSS(_dropdownValueLocator);

        public LazyElement FirstDropdownValue() => GetLazyElement(By.CssSelector(firstDropdownValueCSS), "{First Dropdown Element");

        public LazyElement SubmitButton => GetLazyElementByCSSe2e(_btnSaveE2e, "Save Button");

        //CDS
        public LazyElement IcnRsmCdsLogo => GetLazyElementByCSSe2e(_icnRsmCdsLogoE2e, "RSMLogo");
        public LazyElement BtnBellHeader => GetLazyElementByCSSe2e(_btnBellHeaderE2e, "Bar menu icon");

        //ETL
        public LazyElement RSMLogoElement => GetLazyElement(By.CssSelector(RSMLogo), "RSMLogo");

        public LazyElement BtnUserHeader => GetLazyElementByCSSe2e(_btnUserHeaderE2e, "Username Icon");

        public LazyElement UsernameText => GetLazyElement(By.CssSelector(usernameText), "Username Text");

        public LazyElement UsernameIcon => GetLazyElement(By.CssSelector(usernameIcon), "Username Icon");

        public LazyElement MnuManageList => GetLazyElementByCSSe2e(_mnuManageListE2e, "Username Icon");

        public LazyElement BtnSave => GetLazyElementByCSSe2e(_btnSaveE2e, "Save Button");

        public LazyElement BtnCancel => GetLazyElementByCSSe2e(_btnCancelE2e, "Cancel Button");

        public LazyElement LblCentralDataServices => GetLazyElementByCSSe2e(_lblCentralDataServicesE2e, "Central Data Services Label");

        public LazyElement SortIcon => GetLazyElement(By.CssSelector(sortIcon), "Sort icon");

        public LazyElement BtnEtlConfiguration => GetLazyElement(By.Id(_btnEtlConfigurationId), "ETL Configuration Icon");

        public LazyElement BtnProcedureStatus => GetLazyElement(By.Id(_btnProcedureStatusId), "Procedure Status Icon");

        public LazyElement BtnEtlUploader => GetLazyElement(By.Id(_btnEtlUploaderId), "ETL Uploader Icon");

        public LazyElement MnuEtlUploader => GetLazyElementByCSSe2e(_mnuEtlUploaderE2e, "ETL Uploader Submenu");

        public LazyElement LnkUploadFile => GetLazyElement(By.XPath(_lnkUploadFileXpath));

        public LazyElement LnkViewResults => GetLazyElement(By.XPath(_lnkViewResultsXpath));

        public LazyElement BtnProcedureLibrary => GetLazyElement(By.Id(_btnProcedureLibraryId), "Procedure Library Icon");

        public LazyElement PreviousButton => GetLazyElementByCSSe2e(_btnPreviousE2e, "Previous button");

        public LazyElement BtnNext => GetLazyElementByCSSe2e(_btnNextE2e, "Previous button");

        public LazyElement SelNumberPerPage => GetLazyElement(By.CssSelector(_selNumberPerPageCss), "Number Per Page sel");


        // property

        public bool IsBtnSaveDisplayed() => BtnSave.Displayed;

        public bool IsBtnCancelDisplayed() => BtnCancel.Displayed;

        public bool IsBtnPreviousDisplayed() => IsElementDisplayed(PreviousButton);

        public bool IsBtnNextDisplayed() => IsElementDisplayed(BtnNext);

        public bool IsSelNumberPerPageDisplayed() => IsElementDisplayed(SelNumberPerPage);

        public override bool IsPageLoaded()
        {
            throw new System.NotImplementedException();
        }

        // SUBMIT
        public void ClickBtnSave()
        {
            var _saveBtn = "[e2e-id=\"" + _btnSaveE2e + "\"]";
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(_saveBtn));
            BtnSave.Click();
            this.WebDriver.Wait().ForPageLoad();
        }

        public bool IsNotificationMessageVisible() => NotificationElement.Displayed;

        public string GetNotificationMessage() => NotificationElement.Text;

        public void SelectDropdownValue(string value) => DropdownValueElement(value).Click();

        //FIND ELEMENT/S
        public LazyElement GetLazyElementByE2eContains(string e2eLocator, string elementName)
        {
            try
            {
                WebDriver.Wait().ForPageLoad();
                return GetLazyElement(By.XPath("//*[contains(@e2e-id,'" + e2eLocator + "')]"), elementName);
            }
            catch
            {
                return null;
            }
        }

        public IWebElement GetElementByCSS(string cssSelector)
        {
            return WebDriver.FindElement(By.CssSelector(cssSelector));
        }

        public ICollection<IWebElement> GetElementsByCSS(string cssSelector)
        {
            return WebDriver.FindElements(By.CssSelector(cssSelector));
        }

        public IWebElement GetElementByID(string id)
        {
            return WebDriver.FindElement(By.Id(id));
        }

        public IWebElement GetElementByXPath(string xPathSelector)
        {
            return WebDriver.FindElement(By.XPath(xPathSelector));
        }

        public void SelectFirstDropdownValue() => FirstDropdownValue().Click();

        public void ChooseRandomOptionFromDropdownList()
        {
            var random = new Random();
            this.WebDriver.Wait().ForVisibleElement(By.CssSelector(_dropdownValueLocator));
            int index = random.Next(DropdownValuesList.Count);
            IWebElement optionToClick = this.DropdownValuesList.GetItemByIndex(index);
            optionToClick.Click();
        }

        public void SelectItemInDropDown(IWebElement element, string value)
        {
            element.Click();
            if (String.IsNullOrEmpty(value))
            {
                ChooseRandomOptionFromDropdownList();
            }
            else
            {
                SelectDropdownValue(value);
            }
        }

        public LazyElement GetLazyElementByCSSe2e(string e2eElement, string elementName)
        {
            try
            {
                return GetLazyElement(By.CssSelector($"[e2e-id=\"{ e2eElement }\"]"), elementName);
            }
            catch
            {
                return null;
            }
        }

        public LazyElement GetLazyElementByCSSe2e(LazyElement parent, string e2eElement, string elementName) => GetLazyElement(parent, By.CssSelector("[e2e-id=\"" + e2eElement + "\"]"), elementName);
        public LazyElement GetLazyElementByCSS(LazyElement parent, string cssLocalor, string elementName) => GetLazyElement(parent, By.CssSelector(cssLocalor), elementName);

        public bool IsElementDisplayed(LazyElement emelent)
        {
            try
            {
                return emelent.Displayed;
            }
            catch
            {
                return false;
            }
        }

        //if element.Clear() doesn't work, it should help
        public void Clear(LazyElement element)
        {
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Backspace);

        }

        public void SelectClientInDropdown(string clientName)
        {
            ClientIdDropdownElement.Click();
            DropdownValueElement(clientName).Click();
        }

        public void SelectRandomClient()
        {
            ClientIdDropdownElement.Click();
            this.ChooseRandomOptionFromDropdownList();
        }

        public void ClickBtnEtlConfiguration()
        {
            BtnEtlConfiguration.Click();
        }
    }
}

  