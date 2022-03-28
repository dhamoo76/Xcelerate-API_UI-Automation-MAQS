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
    public class CEMManageUsersPage : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        private const string loaderElememt = "i.p-datatable-loading-icon";
        // private const string searchBoxFromColumn = "button.p-button"; - deleted 
        // @TODO: Think about adding ids for create assignment and manage assignment
        private const string createAssignmentToggleXPATH = "//span[text()='Create Assignments']";
        private const string manageAssignmentToggleXPATH = "//span[text()='Manage Assignments']";
        private const string manageGroupToggleXPATH = "//span[text()='Manage User Groups']";
        private const string selectScopeDopdownID = "assignment-scope-selector";
        private const string clientOptionScopeCSS = "[aria-label=\"Client\"]";
        private const string selectClientDropdownID = "client-selector";
        // private const string userDropdownList = "div[aria-label=\"MANAGE ASSIGNMENTS\"]";
        private const string clientFromClientsListCSS = "li.p-dropdown-item";
        private const string usersTableCSS = "div[class^=\"SourceObjectsList_container_\"]";
        private const string rolesTableCSS = "div.p-tree";
        private const string filterButtonCSS = "button.p-column-filter-menu-button";
        private const string gridLoadedCSS = "div.p-datatable-wrapper";


        public IWebElement SelectClientDropdown => this.GetElementByID(selectClientDropdownID);
        public IWebElement FilterButton => this.GetElementByCSS(filterButtonCSS);
        public IWebElement gridLoaded => this.GetElementByCSS(gridLoadedCSS);
        public ICollection<IWebElement> ClientsList => this.GetElementsByCSS(clientFromClientsListCSS);
        public IWebElement CreateAssignmentsToggle => this.GetElementByXPath(createAssignmentToggleXPATH);
        public IWebElement ManageAssignmentsToggle => this.GetElementByXPath(manageAssignmentToggleXPATH);
        public IWebElement ManageGroupToggle => this.GetElementByXPath(manageGroupToggleXPATH);
        public IWebElement SelectScopeDropDown => this.GetElementByID(selectScopeDopdownID);
        public IWebElement ClientScopeOption => this.GetElementByCSS(clientOptionScopeCSS);


        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMManageUsersPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMCreateAssignmentsPage GoToCreateAssignmentsPage()
        {
            this.WebDriver.Wait().ForClickableElement(By.XPath(createAssignmentToggleXPATH));
            this.CreateAssignmentsToggle.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMCreateAssignmentsPage(this.TestObject);
        }

        public CEMManageUsersPage GoToManageAssignmentsPage()
        {
            this.WebDriver.Wait().ForClickableElement(By.XPath(manageAssignmentToggleXPATH));
            this.ManageAssignmentsToggle.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMManageUsersPage(this.TestObject);
        }

        public CEMManageGroupPage GoToManageGroupPage()
        {
            this.WebDriver.Wait().ForClickableElement(By.XPath(manageGroupToggleXPATH));
            this.ManageGroupToggle.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMManageGroupPage(this.TestObject);
        }

        public void ChooseRandomClientFromClientList()
        {
            var random = new Random();
            int index = random.Next(ClientsList.Count);
            IWebElement clientToClick = this.ClientsList.GetItemByIndex(index);
            clientToClick.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMManageUsersPage ChooseRandomClient()
        {
            this.ChooseScopeClient();
            this.WebDriver.Wait().ForClickableElement(By.Id(selectClientDropdownID));
            this.SelectClientDropdown.Click();
            this.ChooseRandomClientFromClientList();
            return this;

        }

        public CEMManageUsersPage ChooseScopeClient()
        {
            this.WebDriver.Wait().ForClickableElement(By.Id(selectScopeDopdownID));
            this.SelectScopeDropDown.Click();
            this.ClientScopeOption.Click();
            this.WebDriver.Wait().ForPageLoad();
            return this;
        }
        
        public bool IsGridLoaded()
        {
            return gridLoaded.Displayed;
            //return FilterButton.Displayed;
        }


    }
}