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
    public class ETLProtocolsAdd : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string pageTitle = "h2[class*=\"styles_addNewProtocolTitle\"]";
        public const string selectedClientText = "h3.page-subtitle";
        public const string protocolNameInput = "name";
        public const string protocolTypeDropdown = "[e2e-id=\"protocolTypeId\"]";
        public const string protocolTypeDropdownList = "";
        public const string protocolUrlInput = "url";
        public const string mdmClientIdInput = "mdmClientId";
        public const string folderNameInput = "folderPath";
        public const string usernameInput = "username";
        public const string passwordInput = "password";
        public const string cancelButton = "[e2e-id=\"cancelButton\"]";
        public const string saveButton = "[e2e-id=\"submitButton\"]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLProtocolsAdd(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public ETLProtocolsAdd EnterProtocolName(string value)
        {
            ProtocolNameInputElement.SendKeys(value);
            return this;
        }

        public ETLProtocolsAdd EnterFolderName(string protocolFolderName)
        {
            FolderNameInputElement.SendKeys(protocolFolderName);
            return this;
        }

        public ETLProtocolsView SaveProtocol() 
        {
            this.ClickBtnSave();
            return new ETLProtocolsView(this.TestObject);
        }

        public LazyElement PageTitleElement => GetLazyElement(By.CssSelector(pageTitle), "{Page Title}");
        public LazyElement SelectedClientTextElement => GetLazyElement(By.CssSelector(selectedClientText), "{Selected Client Header}");
        public LazyElement ProtocolNameInputElement => GetLazyElement(By.Id(protocolNameInput), "{Protocol Name Input}");
        public LazyElement ProtocolTypeDropdownElement => GetLazyElement(By.CssSelector(protocolTypeDropdown), "{Protocol Type Dropdown}");

        public LazyElement ProtocolUrlInputElement => GetLazyElement(By.Id(protocolUrlInput), "{Protocol URL Input}");
        public LazyElement MdmClientIdInputElement => GetLazyElement(By.Id(mdmClientIdInput), "{MDM Client ID Input}");
        public LazyElement FolderNameInputElement => GetLazyElement(By.Id(folderNameInput), "{Folder Name Input}");
        public LazyElement UsernameInputElement => GetLazyElement(By.Id(usernameInput), "{Username Input}");
        public LazyElement PasswordInputElement => GetLazyElement(By.Id(passwordInput), "{Password Input}");
        public LazyElement CancelButtonElement => GetLazyElement(By.CssSelector(cancelButton), "{CancelButton}");
        public LazyElement SaveButtonElement => GetLazyElement(By.CssSelector(saveButton), "{SaveButton}");

        public override bool IsPageLoaded() => PageTitleElement.Displayed;

        public bool IsSelectedClientTextDisplayed() => SelectedClientTextElement.Displayed;
        public bool IsProtocolNameInputDisplayed() => ProtocolNameInputElement.Displayed;
        public bool IsProtocolTypeDropdownDisplayed() => ProtocolTypeDropdownElement.Displayed;
        public bool IsProtocolUrlInputDisplayed() => ProtocolUrlInputElement.Displayed;
        public bool IsMdmClientIdInputDisplayed() => MdmClientIdInputElement.Displayed;
        public bool IsFolderNameInputDisplayed() => FolderNameInputElement.Displayed;
        public bool IsUsernameInputDisplayed() => UsernameInputElement.Displayed;
        public bool IsPasswordInputDisplayed() => PasswordInputElement.Displayed;
        public bool IsCancelButtonDisplayed() => CancelButtonElement.Displayed;
        public bool IsSaveButtonDisplayed() => SaveButtonElement.Displayed;
    }
}