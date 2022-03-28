using System;
using System.Collections.Generic;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CEMManageGroupPage : BasePage
    {
        private const string addUserGroupButtonCSS = "button[e2e-id=\"addGroup\"]";
        private const string searchUserGroupBox = "input.p-listbox-filter";
        private const string userGroupCSS = "li.p-listbox-item";
        private const string editGroupNameButton = "*[i[e2e-id=\"editIcon\"]]";
        private const string addUsersButtonCSS = "button[e2e-id=\"buttonAssignUsers\"]";
        private const string unAssignButton = "button.p-button";
        private const string editGroupNameFormPopUp = "div[role=\"dialog\"]";
        private const string allColumnsCells = "td[role=\"cell\"]";//Need to be updated in the future

        // Add Users Pop up
        private const string searchUserFieldCSS = "input[placeholder=\"Search Users\"]";
        private const string userList = "ul.p-listbox-list";
        private const string user = "*[li.p-listbox-item]";
        private const string moveChosenToTheRight = "span.pi-angle-right";
        private const string moveAllToTheRightCSS = "span.pi-angle-double-right";
        private const string moveChosenToTheLeft = "span.pi-angle-left";
        private const string moveAllToTheLeft = "span.pi-angle-double-left";
        private const string saveUserAssignmentsButtonCSS = "button[e2e-id=\"userGroupAssignDialogSaveButton\"]";

        //Create New Group Pop Up
        private const string createNewGroupPopUp = "div.p-dialog";
        private const string groupNameFieldCSS = "input[e2e-id=\"userGroupInput\"]";
        private const string saveNewUserGroupButtonCSS = "button[e2e-id=\"saveUserGroupButton\"]";
        


        public IWebElement AddUserGroupButton => this.GetElementByCSS(addUserGroupButtonCSS);
        public IWebElement GroupNameField => this.GetElementByCSS(groupNameFieldCSS);
        public IWebElement SaveNewGroupButton => this.GetElementByCSS(saveNewUserGroupButtonCSS);
        public IWebElement AddUsersButton => this.GetElementByCSS(addUsersButtonCSS);
        public IWebElement SearchUsersField => this.GetElementByCSS(searchUserFieldCSS);
        public IWebElement MoveAllUsersToTheRight => this.GetElementByCSS(moveAllToTheRightCSS);
        public IWebElement SaveAssignmentsButton => this.GetElementByCSS(saveUserAssignmentsButtonCSS);


        public ICollection<IWebElement> UserGroupsList => this.GetElementsByCSS(userGroupCSS);
        public ICollection<IWebElement> UsersCells => this.GetElementsByCSS(allColumnsCells);



        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMManageGroupPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public CEMManageGroupPage AddUserGroup(string name)
        {
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(addUserGroupButtonCSS));
            this.AddUserGroupButton.Click();
            this.WebDriver.Wait().ForVisibleElement(By.CssSelector(createNewGroupPopUp));
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(groupNameFieldCSS));
            this.GroupNameField.Click();
            this.GroupNameField.SendKeys(name);
            this.WebDriver.Wait().ForClickableElement(By.CssSelector(saveNewUserGroupButtonCSS));
            this.SaveNewGroupButton.Click();
            this.WebDriver.Wait().ForPageLoad();
            return this;

        }

        public CEMManageGroupPage AddUserToTheGroup(string userGroupName, string userName)
        {
         ICollection<IWebElement> userGroupsListNew = this.GetElementsByCSS(userGroupCSS);
         foreach (IWebElement userGroup in userGroupsListNew)
         {
             if (Equals(userGroup.Text, userGroupName))
             {
                 userGroup.Click();
                 WebDriver.Wait().ForPageLoad();
                 break;
             }
         }

         WebDriver.Wait().ForClickableElement(By.CssSelector(addUsersButtonCSS));
         this.AddUsersButton.Click();

         WebDriver.Wait().ForClickableElement(By.CssSelector(searchUserFieldCSS));
         this.SearchUsersField.SendKeys(userName);

         WebDriver.Wait().ForPageLoad();

         WebDriver.Wait().ForClickableElement(By.CssSelector(moveAllToTheRightCSS));
         this.MoveAllUsersToTheRight.Click();

         WebDriver.Wait().ForClickableElement(By.CssSelector(saveUserAssignmentsButtonCSS));
         this.SaveAssignmentsButton.Click();

         WebDriver.Wait().ForPageLoad();

         return this;
        }

        public bool IsUserAddedToTheUserGroup(string userName)
        {
            bool a = false;
         ICollection<IWebElement> usersCellsNew =this.GetElementsByCSS(allColumnsCells);
         foreach (IWebElement userCell in usersCellsNew)
         {
             if (userCell.Text.Contains(userName))
             {
                 a = true;
             }
         }

         return a;
        }

}
}