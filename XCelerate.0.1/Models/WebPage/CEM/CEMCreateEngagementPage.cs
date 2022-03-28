using System;
using System.Collections.Generic;
using System.IO;
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
    public class CEMCreateEngagementPage : BasePage
    {
        private const string EngagementNameFieldCSS = "textarea[e2e-id=\"name\"]";
        private const string descriptionField = "textarea[e2e-id=\"description\"]";
        private const string lineOfBusinessDropdownCSS= "#lineOfBusiness";
        private const string lobOptionsCSS = "li.p-dropdown-item";
        private const string engagementTypeDropdownCSS = "#type";
        private const string engagementOptionsCSS = "li.p-dropdown-item";
        private const string engagementStatus = "div[e2e-id=\"status\"]";
        private const string sowField = "textarea[e2e-id=\"sow\"]";
        private const string sowDate = "input[name=\"sowDate\"]";
        private const string startDate = "input[name=\"scheduledStartDate\"]";
        private const string endDate = "input[name=\"scheduledEndDate\"]";

        private const string activeStatusOption = "li[aria-label=\"Active\"]";
        private const string inActiveStatusOption = "li[aria-label=\"Inactive\"]";

        public IWebElement EngagementNameField => this.GetElementByCSS(EngagementNameFieldCSS);
        public IWebElement LineOfBusinessDropdown => this.GetElementByCSS(lineOfBusinessDropdownCSS);
        public IWebElement EngagementTypeDropdown => this.GetElementByCSS(engagementTypeDropdownCSS);
        public IWebElement EngagementTab => this.GetElementByXPath(engagementOptionsCSS);
        public ICollection<IWebElement> LOBTypes => this.GetElementsByCSS(lobOptionsCSS);
        public ICollection<IWebElement> EngagementTypes => this.GetElementsByCSS(engagementOptionsCSS);



        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMCreateEngagementPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public void ChooseRandomLOBTypeFromTypesList()
        {
            var random = new Random();
            int index = random.Next(LOBTypes.Count);
            IWebElement lobTypeToClick = this.LOBTypes.GetItemByIndex(index);
            lobTypeToClick.Click();
            this.WebDriver.Wait().ForPageLoad();
        }

        public void ChooseRandomEngagementTypeFromTypesList()
        {
            var random = new Random();
            int index = random.Next(LOBTypes.Count);
            IWebElement engagemenTypeToClick = this.LOBTypes.GetItemByIndex(index);
            engagemenTypeToClick.Click();
            this.WebDriver.Wait().ForPageLoad();
        }

        public CEMManageClientsPage CreateValidEngagement(String randomName)
        {
           
            this.EngagementNameField.SendKeys(randomName);

            this.LineOfBusinessDropdown.Click();
            this.ChooseRandomLOBTypeFromTypesList();

            this.EngagementTypeDropdown.Click();
            this.ChooseRandomEngagementTypeFromTypesList();

            this.ClickBtnSave();

            return new CEMManageClientsPage(this.TestObject);
        }

    }
}