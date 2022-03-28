using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Common;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;

namespace Models
{
    public class CEMCreateAssignmentsPage: BasePage
    {
        
    private const string clientDrodownCreateAssignmentsID = "client-selector";
    // private const string userDropdownList = "div[aria-label=\"MANAGE ASSIGNMENTS\"]";
    private const string clientFromClientsListCSS = "li.p-dropdown-item";
    private const string userFromUsersListCSS = "td.e2e-id-users-source-checkbox.p-selection-column";
    private const string usersTableCSS = "div[class^=\"SourceObjectsList_container_\"]";
    private const string rolesTableCSS = "div.p-tree";
    private const string userDropdownOptionsUserGroups = "li[aria-label=\"User Groups\"]";
    private const string engagementDropdown = "*[div.p-checkbox-box]";
    private const string engagementDropdownOptions = "div.p-dropdown[2]";
    private const string engagementDropdownList = "";
    private const string expandRoles = "span.p-tree-toggler-icon";
    private const string rolesCheckboxes = "div.p-checkbox-box";//??
    private const string clearSearchbox = "";
    private const string selectScopeDopdownID = "assignment-scope-selector";
    private const string clientOptionScopeCSS = "[aria-label=\"Client\"]";
    private const string selectClientDropdownID = "client-selector";
    private const string filterButtonCSS = "button.p-column-filter-menu-button";

    public IWebElement SelectClientDropdown => this.GetElementByID(clientDrodownCreateAssignmentsID);
    public IWebElement UsersTableContainer => this.GetElementByCSS(usersTableCSS);
    public IWebElement RolesTableContainer => this.GetElementByCSS(rolesTableCSS);
    public ICollection<IWebElement> ClientsList => this.GetElementsByCSS(clientFromClientsListCSS);
    public ICollection<IWebElement> UsersList => this.GetElementsByCSS(userFromUsersListCSS);
    public IWebElement FilterButton => this.GetElementByCSS(filterButtonCSS);
    public IWebElement SelectScopeDropDown => this.GetElementByID(selectScopeDopdownID);
    public IWebElement ClientScopeOption => this.GetElementByCSS(clientOptionScopeCSS);

        public CEMCreateAssignmentsPage(ISeleniumTestObject testObject) : base(testObject)
    {
    }

        // Temporary commented this part due to re-design
        /* 
        public void ChooseRandomClientFromClientList()
        {
            var random = new Random();
            int index = random.Next(ClientsList.Count);
            IWebElement clientToClick = this.ClientsList.GetItemByIndex(index);
            clientToClick.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMCreateAssignmentsPage ChooseRandomClient()
        {
            this.WebDriver.Wait().ForClickableElement(By.Id(clientDrodownCreateAssignmentsID));
            this.SelectClientDropdown.Click();
            this.ChooseRandomClientFromClientList();
            return this;

        }
        */

        public void ChooseRandomClientFromClientList()
        {
            var random = new Random();
            int index = random.Next(ClientsList.Count);
            IWebElement clientToClick = this.ClientsList.GetItemByIndex(index);
            clientToClick.Click();
            this.WebDriver.Wait().ForPageLoad();

        }

        public CEMCreateAssignmentsPage ChooseRandomUserFromUserList()
        {
            var random = new Random();
            int index = random.Next(UsersList.Count);
            IWebElement clientToClick = this.UsersList.GetItemByIndex(index);
            clientToClick.Click();
            this.WebDriver.Wait().ForPageLoad();
            return this;
        }


        public CEMCreateAssignmentsPage ChooseRandomClient()
        {
            this.ChooseScopeClient();
            this.WebDriver.Wait().ForClickableElement(By.Id(selectClientDropdownID));
            this.SelectClientDropdown.Click();
            this.ChooseRandomClientFromClientList();
            return this;

        }

        public CEMCreateAssignmentsPage ChooseScopeClient()
        {
            this.WebDriver.Wait().ForClickableElement(By.Id(selectScopeDopdownID));
            this.SelectScopeDropDown.Click();
            this.ClientScopeOption.Click();
            this.WebDriver.Wait().ForPageLoad();
            return this;
        }

        public bool IfUsersAndRolesTablesWereLoaded()
    {
        return this.RolesTableContainer.Displayed && this.UsersTableContainer.Displayed;
        }


    }
}
