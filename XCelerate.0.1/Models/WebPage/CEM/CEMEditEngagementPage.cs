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
    public class CEMEditEngagementPage : CEMCreateEngagementPage
    {
 
        private const string activeStatusOption = "li[aria-label=\"Active\"]";
        private const string inActiveStatusOption = "li[aria-label=\"Inactive\"]";
        private const string AddNewButtonXPATH = "//button[text() = 'ADD NEW']";
        private const string ProjectNamesCSS = "td[role=\"cell\"] a";

        public IWebElement AddNewButton => this.GetElementByXPath(AddNewButtonXPATH);
        public ICollection<IWebElement> ProjectssList => this.GetElementsByCSS(ProjectNamesCSS);

        /// <summary>
        /// Initializes a new instance of the <see cref="CEMEditEngagementPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMEditEngagementPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMCreateProjectPage GoToCreateProjectPage()
        {
            this.AddNewButton.Click();
            this.WebDriver.Wait().ForPageLoad();
            return new CEMCreateProjectPage(this.TestObject);

        }

        public bool IfNewProjectShownInTable(string projectName)
        {
            bool a = false;
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(ProjectNamesCSS));
            foreach (IWebElement project in this.ProjectssList)
            {
                if (project.Text == projectName)
                {
                    a = true;
                    break;
                }

            }

            return a;
        }

        public CEMEditProjectPage GoToEditProjectPage(string projectName)
        {
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(ProjectNamesCSS));
            foreach (IWebElement project in this.ProjectssList)
            {
                if (project.Text == projectName)
                {
                    project.Click();
                    this.WebDriver.Wait().ForPageLoad();
                    break;
                }
            }

            return new CEMEditProjectPage(this.TestObject);

        }

    }
}