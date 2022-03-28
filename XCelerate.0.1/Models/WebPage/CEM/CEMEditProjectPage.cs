using System;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CEMEditProjectPage : CEMCreateProjectPage
    {
        private const string activeStatusOption = "li[aria-label=\"Active\"]";
        private const string inActiveStatusOption = "li[aria-label=\"Inactive\"]";
        private const string assignButtonXPATH = "//button[text() = 'ASSIGN']";
        private const string popUpMessageCSS = "div.MuiAlert - message"; // Not sure that such a popup exists
        private const string EntityAssignmentCSS = "tr:nth-child(1)";

        public IWebElement AssignButton => this.GetElementByXPath(assignButtonXPATH);
        public IWebElement popUpMessage => this.GetElementByCSS(popUpMessageCSS);
        public IWebElement EntityAssignment => this.GetElementByCSS(EntityAssignmentCSS);


        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMEditProjectPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMAssignEntitiesPage GoToAssignEntitiesPage()
        {
            this.WebDriver.Wait().ForClickableElement(By.XPath(assignButtonXPATH));
            this.AssignButton.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMAssignEntitiesPage(this.TestObject);
        }

        public bool IfEntityAssignmentWasSuccessful()
        {
            this.WebDriver.Wait().ForPageLoad();
            return this.EntityAssignment.Displayed;
            /*this.WebDriver.Wait().ForVisibleElement(By.CssSelector(popUpMessageCSS));
            return bool.Equals(popUpMessage.Text, "Project Assignments Updated");*/
        }



    }
}