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
    public class ETLPayloadsAdd : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        private readonly string timestamp = DateTime.Now.Ticks.ToString();

        //elements
        public const string pageTitle = "h2[class*=\"styles_addNewPayloadTitle\"]";
        public const string selectedClientText = "h3.page-subtitle";
        public const string payloadNameInput = "name";
        public const string protocolNameDropdown = "[e2e-id=\"protocolId\"]";
        public const string protocolNameDropdownSearchbox = "";
        public const string protocolNameDropdownList = "";
        public const string engagementDropdown = "[e2e-id=\"engagementId\"]";
        public const string engagementDropdownSearchbox = "";
        public const string engagementDropdownList = "";
        public const string projectDropdown = "[e2e-id=\"projectId\"]";
        public const string projectDropdownSearchbox = "";
        public const string projectDropdownList = "";
        public const string legalEntityDropdown = "[e2e-id=\"legalEntityId\"]";
        public const string legalEntityDropdownSearchbox = "";
        public const string legalEntityDropdownList = "";
        public const string notificationRecipientsInput = "[e2e-id=\"notificationRecipients\"]";
        public const string procedureDropdown = "[e2e-id=\"procedures\"]";
        public const string procedureDropdownSearchbox = "";
        public const string procedureDropdownList = "";
        public const string fileTypeInput = "[e2e-id=\"files[0].fileName\"]";
        public const string fileNameInput = "[e2e-id=\"files[0].filePattern\"]";
        public const string _inpFileNamesCss = "input[e2e-id*=\"filePattern\"]";
        public const string cancelButton = "[e2e-id=\"cancelButton\"]";
        public const string saveButton = "[e2e-id=\"submitButton\"]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLPayloadsAdd(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public LazyElement PageTitleElement => GetLazyElement(By.CssSelector(pageTitle), "{Page Title}");
        public LazyElement SelectedClientTextElement => GetLazyElement(By.CssSelector(selectedClientText), "{Selected Client Header}");
        public LazyElement PayloadNameInputElement => GetLazyElement(By.Id(payloadNameInput), "{Payload Name Input}");
        public LazyElement ProtocolNameDropdownElement => GetLazyElement(By.CssSelector(protocolNameDropdown), "{Protocol Name Dropdown}");
        public LazyElement EngagementDropdownElement => GetLazyElement(By.CssSelector(engagementDropdown), "{Engagement Dropdown}");
        public LazyElement ProjectDropdownElement => GetLazyElement(By.CssSelector(projectDropdown), "{Project Dropdown}");
        public LazyElement LegalEntityDropdownElement => GetLazyElement(By.CssSelector(legalEntityDropdown), "{Legal Entity Dropdown}");
        public LazyElement NotificationRecipientsInputElement => GetLazyElement(By.CssSelector(notificationRecipientsInput), "{Notification Recipients Input}");
        public LazyElement ProcedureDropdownElement => GetLazyElement(By.CssSelector(procedureDropdown), "{Procedure Dropdown}");
        public LazyElement FileTypeInputElement => GetLazyElement(By.CssSelector(fileTypeInput), "{File Type Input}");
        public LazyElement FileNameInputElement => GetLazyElement(By.CssSelector(fileNameInput), "{File Name Input}");
        public ICollection<IWebElement> InpFileNames => GetElementsByCSS(_inpFileNamesCss);
        public LazyElement CancelButtonElement => GetLazyElement(By.CssSelector(cancelButton), "{Cancel Button}");

        public LazyElement SaveButtonElement => GetLazyElement(By.CssSelector(saveButton), "{Save Button}");

        public override bool IsPageLoaded() => PageTitleElement.Displayed;

        public bool IsSelectedClientTextDisplayed() => SelectedClientTextElement.Displayed;
        public bool IsPayloadNameInputDisplayed() => PayloadNameInputElement.Displayed;
        public bool IsProtocolNameDropdownDisplayed() => ProtocolNameDropdownElement.Displayed;
        public bool IsEngagementDropdownDisplayed() => EngagementDropdownElement.Displayed;
        public bool IsProjectDropdownDisplayed() => ProjectDropdownElement.Displayed;
        public bool IsLegalEntityDropdownDisplayed() => LegalEntityDropdownElement.Displayed;

        public bool IsNotificationRecipientsInputDisplayed() => NotificationRecipientsInputElement.Displayed;
        public bool IsProcedureDropdownDisplayed() => ProcedureDropdownElement.Displayed;
        public bool IsCancelButtonDisplayed() => CancelButtonElement.Displayed;
        public bool IsSaveButtonDisplayed() => SaveButtonElement.Displayed;

        public ETLPayloadsAdd ChooseAnyProtocolNameInDropdown()
        {
            SelectItemInDropDown(ProtocolNameDropdownElement, "");
            return this;
        }

        public ETLPayloadsAdd EnterPayloadName(string payloadName)
        {
            PayloadNameInputElement.SendKeys(payloadName);
            return this;
        }

        public ETLPayloadsAdd ChooseAnyEngagementDropdown(string searchValue = "")
        {
            if(searchValue != "")
                SelectItemInDropDown(EngagementDropdownElement, searchValue);
            else
                SelectItemInDropDown(EngagementDropdownElement, "");
            return this;
        }
        public ETLPayloadsAdd ChooseAnyProjectDropdown(string searchValue = "")
        {
            if (searchValue != "")
                SelectItemInDropDown(ProjectDropdownElement, searchValue);
            else
                SelectItemInDropDown(ProjectDropdownElement, "");
            return this;
        }

        public ETLPayloadsAdd ChooseLEAnyDropdown(string searchValue = "")
        {
            if (searchValue != "")
                SelectItemInDropDown(LegalEntityDropdownElement, searchValue);
            else
                SelectItemInDropDown(LegalEntityDropdownElement, "");
            return this;
        }

        public ETLPayloadsAdd EnterNotificationRecipients(string notificationRecipient)
        {
            NotificationRecipientsInputElement.SendKeys(notificationRecipient);
            return this;
        }

        public ETLPayloadsAdd ChooseProjectDropdown(string projectValue)
        {
            SelectItemInDropDown(ProjectDropdownElement, projectValue);
            return this;
        }

        public ETLPayloadsAdd ChooseProcedureInDropdown(string procedure)
        {
            SelectItemInDropDown(ProcedureDropdownElement, procedure);
            return this;
        }

        public ETLPayloadsAdd ChooseEngagementDropdown(string engagmentValue)
        {
            SelectItemInDropDown(EngagementDropdownElement, engagmentValue);
            return this;
        }

        public ETLPayloadsAdd ChooseProtocolNameInDropdown(string protocolName)
        {
            SelectItemInDropDown(ProtocolNameDropdownElement, protocolName);
            return this;
        }

        public ETLPayloadsAdd EnterFileName(string fileName)
        {
            FileNameInputElement.SendKeys(fileName);
            return this;
        }

        public ETLPayloadsAdd EnterAllFileNames()
        {
            string input = "AutoFileName" + timestamp;
            int _num = 0;
            foreach(var item in InpFileNames)
            {
                item.SendKeys(input + _num.ToString() + ".XLSX");
                _num++;
            }
            return this;
        }
    }
}

