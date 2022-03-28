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
    public class CEMLeftMenuNavigation : BasePage
    {
        private const string ManageOrgButtonE2E = "nav-item-manage-clients";
        private const string ManageGroupsButtonCSS = "a[href=\"/usergroups\"]";
        private const string ManageAppButtonE2E = "nav-item-manage-applications";
        private const string ManageUsersButtonE2E = "nav-item-manage-users";

        private const string LeftBarItemsCSS = "div.MuiListItemIcon-root";
        private const string LeftBarItemsNamesCSS = "span.text-small";


        protected LazyElement ManageOrgButton => GetLazyElementByCSSe2e(ManageOrgButtonE2E, "Manage organisations button");
        public IWebElement ManageGroupsButton => this.GetElementByCSS(ManageGroupsButtonCSS);
        protected LazyElement ManageAppsButton => GetLazyElementByCSSe2e(ManageAppButtonE2E, "Manage applications button");
        protected LazyElement ManageUsersButton => GetLazyElementByCSSe2e(ManageUsersButtonE2E, "Manage users button");
        public ICollection<IWebElement> ButtonList => this.GetElementsByCSS(LeftBarItemsCSS);
        public ICollection<IWebElement> ButtonListNames => this.GetElementsByCSS(LeftBarItemsNamesCSS);

        public IList<string> ButtonNames = new List<string>()
        {
            "Manage Org",
            "Manage Users",
            "Manage Groups",
            "Manage Applications"
        };


        public CEMLeftMenuNavigation(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMManageClientsPage NavigateToManageOrgPage()
        {
            this.ManageOrgButton.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMManageClientsPage(this.TestObject);
        }

        public CEMManageApplicationsPage NavigateToManageAppsPage()
        {
            this.ManageAppsButton.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMManageApplicationsPage(this.TestObject);
        }

        public CEMManageUsersPage NavigateToManageUsersPage()
        {
            this.ManageUsersButton.Click();
            this.WebDriver.Wait().ForPageLoad();

            return new CEMManageUsersPage(this.TestObject);
        }

        public int CountAllLeftMenuButtons()
        {
            int buttonsCount = ButtonList.Count;
            return buttonsCount;
        }

        public bool AreAllButtonsShown()
        {
            bool a = true;
            foreach (IWebElement button in ButtonListNames)
            {
                Console.WriteLine(button.Text);
                if (!ButtonNames.Contains(button.Text))
                {
                    a = false;
                    break;
                }
            }
            return a;
        }







    }


}
