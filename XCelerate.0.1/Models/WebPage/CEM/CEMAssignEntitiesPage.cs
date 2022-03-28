using System;
using System.Collections.Generic;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CEMAssignEntitiesPage : BasePage
    {
        private const string closeAssignEntitiesForm = "button[e2e-id=\"closeButton\"]]";
        private const string clientField = "input[e2e-id=\"clientName\"]";
        private const string engagementField = "input[e2e-id=\"engagementName\"]";
        private const string projectField = "input[e2e-id=\"projectName\"]";
        private const string entityToPickCSS = "li.p-picklist-item div[class^=\"PickerTemplates_rowTemplate__\"]";
        private const string pickAllCheckbox = "div.p-checkbox-box";
        private const string pickCheckBoxCSS = "div.p-checkbox-box";
        private const string moveChosenToTheRight = "span.pi-angle-right";
        private const string moveAllToTheRightCSS = "span.pi-angle-double-right";
        private const string moveChosenToTheLeft = "span.pi-angle-left";
        private const string moveAllToTheLeft = "span.pi-angle-double-left";
        private const string assignButtonCSS = "button[e2e-id=\"assignButton\"]";
        

        public IWebElement MoveAllToTheRight => this.GetElementByCSS(moveAllToTheRightCSS);
       // public IWebElement AddNewButton => this.GetElementByXPath(AddNewButtonXPATH);
        public IWebElement AssignButton => this.GetElementByCSS(assignButtonCSS);
      //  public IWebElement EngagementTab => this.GetElementByXPath(EngagementsTabXPATH);
        public ICollection<IWebElement> CheckBoxesList => this.GetElementsByCSS(pickCheckBoxCSS);
        public ICollection<IWebElement> LegalEntitiesList => this.GetElementsByCSS(entityToPickCSS);


        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMAssignEntitiesPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMEditProjectPage AssignAllEntitiesToTheProject()
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(moveAllToTheRightCSS));
            this.MoveAllToTheRight.Click();
            this.AssignButton.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMEditProjectPage(this.TestObject);
        }

        public List<String> CollectAllENtitiesName()
        {
            List<String> entitiesNamesList = new List<String>();

            foreach (IWebElement entity in LegalEntitiesList)
            {
                entitiesNamesList.Add(entity.Text);
            }

            return entitiesNamesList;
        }

    }
}