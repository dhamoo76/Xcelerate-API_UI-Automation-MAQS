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
    public class CEMCreateProjectPage : BasePage
    {
        private const string ProjectNameFieldCSS = "textarea[e2e-id=\"name\"]";
        private const string descriptionField = "textarea[e2e-id=\"description\"]";
        private const string lineOfBusinessDropdownCSS = "#lineOfBusiness";
        private const string lobOptionsCSS = "li.p-dropdown-item";
        private const string projectTypeDropdownCSS = "#type";
        private const string projectOptionsCSS = "li.p-dropdown-item";
        private const string engagementStatus = "div[e2e-id=\"status\"]";
        private const string sowField = "textarea[e2e-id=\"sow\"]";
        private const string sowDate = "input[name=\"sowDate\"]";
        private const string startDate = "input[name=\"scheduledStartDate\"]";
        private const string endDate = "input[name=\"scheduledEndDate\"]";

        private const string activeStatusOption = "li[aria-label=\"Active\"]";
        private const string inActiveStatusOption = "li[aria-label=\"Inactive\"]";

        private const string projectNameField = "textarea[e2e-id=\"assignButton\"]";
        private const string lineOfBusinessDropdown = "#lineOfBusiness";
        private const string lobOptions = "*[li.p-dropdown-item]";
        private const string projectTypeDropdown = "#type";
        private const string projectTypeOptions = "*[li.p-dropdown-item]";
        private const string projectStatus = "div[e2e-id=\"status\"]";
        private const string projectYear = "span#projectYear";



        public IWebElement ProjectNameField => this.GetElementByCSS(ProjectNameFieldCSS);
        public IWebElement LineOfBusinessDropdown => this.GetElementByCSS(lineOfBusinessDropdownCSS);
        public IWebElement ProjectTypeDropdown => this.GetElementByCSS(projectTypeDropdownCSS);
        public ICollection<IWebElement> LOBTypes => this.GetElementsByCSS(lobOptionsCSS);
        public ICollection<IWebElement> ProjectTypes => this.GetElementsByCSS(projectOptionsCSS);



        /// <summary>
        /// Initializes a new instance of the <see cref="CEMCreateProjectPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMCreateProjectPage(ISeleniumTestObject testObject) : base(testObject)
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

        public void ChooseRandomProjectTypeFromTypesList()
        {
            var random = new Random();
            int index = random.Next(LOBTypes.Count);
            IWebElement projectTypeToClick = this.LOBTypes.GetItemByIndex(index);
            projectTypeToClick.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMEditEngagementPage CreateValidProject(String randomName)
        {

            this.ProjectNameField.SendKeys(randomName);

            this.LineOfBusinessDropdown.Click();
            this.ChooseRandomLOBTypeFromTypesList();

            this.ProjectTypeDropdown.Click();
            this.ChooseRandomProjectTypeFromTypesList();

            this.ClickBtnSave();

            return new CEMEditEngagementPage(this.TestObject);
        }


    }
}