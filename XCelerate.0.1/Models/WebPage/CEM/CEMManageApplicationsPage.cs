using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using AngleSharp.Common;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    public class CEMManageApplicationsPage : BasePage
    {
        private const string addRoleButtonCSS = "button[e2e-id=\"addRole\"]";
        private const string roleNameInputFieldCCS = "input[type=\"text\"]";
        private const string rolesTableCSS = "table.p-datatable-table";
        private const string rolesCellsCSS = "td.p-editable-column";

        public IWebElement RolesTable => this.GetElementByCSS(rolesTableCSS);
        public IWebElement AddRoleButtonElement => this.GetElementByCSS(addRoleButtonCSS);
        public IWebElement RoleNameInput => this.GetElementByCSS(roleNameInputFieldCCS);
        public ICollection<IWebElement> RolesCellsList => this.GetElementsByCSS(rolesCellsCSS);



        /// <summary>
        /// Initializes a new instance of the <see cref="CEMManageApplicationsPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMManageApplicationsPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMManageApplicationsPage AddNewRole(string name)
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(addRoleButtonCSS));
            this.AddRoleButtonElement.Click();
            ICollection<IWebElement> rolesCellsListNew = WebDriver.FindElements(By.CssSelector(rolesCellsCSS));
            int rolesCount = rolesCellsListNew.Count;
            IWebElement emptyCellToClick = rolesCellsListNew.GetItemByIndex(rolesCount-1);
            emptyCellToClick.Click();
            this.WebDriver.Wait().ForClickableElementAndScrollIntoView(By.CssSelector(roleNameInputFieldCCS));
            this.RoleNameInput.SendKeys(name);
            this.AddRoleButtonElement.Click();
            this.WebDriver.Wait().ForPageLoad();

            return this;
        }

        public bool IsNewlyCreatedRoleIsShownInTheTable(string roleNameExpected)
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(addRoleButtonCSS));
            bool a = false;
            ICollection<IWebElement> rolesNamesListNew = WebDriver.FindElements(By.CssSelector(rolesCellsCSS));
            foreach (IWebElement roleName in rolesNamesListNew)
            {
                if (Equals(roleName.Text, roleNameExpected))
                {
                    a = true;
                    break;
                }
            }
            return a;
        }

        public override bool IsPageLoaded()
        {
            return this.AddRoleButtonElement.Displayed && this.RolesTable.Displayed;
        }

    }
}