using System;
using System.Collections.Generic;
using AngleSharp.Common;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CEMManageClientsPage : BasePage
    {
        private const string EngagementsTabXPATH = "//span[text() = 'Engagements']";
        private const string ProjectsTab = "li.p-tabmenuitem[1]";
        private const string EntitiesTab = "li.p-tabmenuitem[2]"; 
        private const string UsersTab = "li.p-tabmenuitem[3]"; 
        private const string ApplicationsTab = "li.p-tabmenuitem[4]";
        private const string AddNewButtonXPATH = "//button[text() = 'ADD NEW']";
        private const string AssignButtonCCS = "button.p-button[1]";
        private const string Grid = "div.p-datatable";
        private const string ColumnHeaders = "*[th.p-sortable-column]";
        private const string SortingButton = "*[span.p-sortable-column-icon]";
        private const string EngagementNamesCSS = "td[role=\"cell\"] a";// a[href ^= "/client/"]
        private const string ClientCSS = "li.p-dropdown-item";
        private const string SelectClientDropdownCSS = "span.p-dropdown-label";
        //@TODO Change hardcoded client after test data creation step implimentation
        private const string ClientWithEntitiesCSS = "[aria-label=\"Accuracy Brokerage Co.\"]";

        public IWebElement ClientWithEntities => this.GetElementByCSS(ClientWithEntitiesCSS);
        public IWebElement SelectClientDropdown => this.GetElementByCSS(SelectClientDropdownCSS);
        public IWebElement AddNewButton => this.GetElementByXPath(AddNewButtonXPATH);
        public IWebElement AssignButton => this.GetElementByCSS(AssignButtonCCS);
        public IWebElement EngagementTab => this.GetElementByXPath(EngagementsTabXPATH);
        public ICollection<IWebElement> ClientsList => this.GetElementsByCSS(ClientCSS);
        public ICollection<IWebElement> EngagementsList => this.GetElementsByCSS(EngagementNamesCSS);


        /// <summary>
        /// Initializes a new instance of the <see cref="CEMManageClientsPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMManageClientsPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }
        public void ChooseClientWithEntitiesFromClientList()
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(ClientWithEntitiesCSS));
            ClientWithEntities.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMManageClientsPage ChooseClientWithEntities()
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(SelectClientDropdownCSS));
            this.SelectClientDropdown.Click();
            this.ChooseClientWithEntitiesFromClientList();
            return this;

        }

        public void ChooseRandomClientFromClientList()
        {
            var random = new Random();
            int index = random.Next(ClientsList.Count);
            IWebElement clientToClick = this.ClientsList.GetItemByIndex(index);
            clientToClick.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMManageClientsPage ChooseRandomClient()
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(SelectClientDropdownCSS));
            this.SelectClientDropdown.Click();
            this.ChooseRandomClientFromClientList();
            return this;

        }

        public CEMCreateEngagementPage GoToCreateEngagementPage()  
        {
            this.AddNewButton.Click();
            this.WebDriver.Wait().ForPageLoad();
            return new CEMCreateEngagementPage(this.TestObject);

        }

        public CEMEditEngagementPage GoToEditEngagementPage(string engagementName)
        {
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(EngagementNamesCSS));
            foreach (IWebElement engagement in this.EngagementsList)
            {
                if (engagement.Text == engagementName)
                {
                    engagement.Click();
                    this.WebDriver.Wait().ForPageLoad();
                    break;
                }
            }
            
            return new CEMEditEngagementPage(this.TestObject);

        }



        public bool IfEngagementsTabDisplayed()
        {
            return this.EngagementTab.Displayed;
        }



        public override bool IsPageLoaded()
        {
            return this.EngagementTab.Displayed && this.AddNewButton.Displayed;
        }

    }
}