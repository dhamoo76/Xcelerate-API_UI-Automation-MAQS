using CognizantSoftvision.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using OpenQA.Selenium;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tests.UI.CEM
{
    [TestClass]
    public class CEMManageAssignmentTests : BaseSeleniumTest
    {
        // need to change class name for now until Max's code is checked in, to avoid merge conflict (from CEMManageAssignTests to CEMManageAssignmentTests). once his code is checked in, should append on that class. (CEMManageAssignTests)

        // code copied from another test case. this code is redundant and needs to be moved/refactored into a separate class so that all the test class can just call it.
        private CEMLandingPage landingPage;
        private CEMLeftMenuNavigation leftMenu;
        private const string userName = Config._userSurname;

        [TestInitialize]
        public void Login()
        {
            LoginPage page = new LoginPage(this.TestObject);
            leftMenu = new CEMLeftMenuNavigation(this.TestObject);
            string username = Config._loginUsername; //@TODO: move it to setting in the future app.config or as a part of request?
            landingPage = page.OpenLoginPage("cem").LoginWithValidCredentialsCEM(username);
        }

        [TestMethod]
        [Description("286185 | On Manage assignment page there is a dropdown")]
        //For a CEM user On Manage assignment page there is a dropdown with values
        public void VerifyDropdownExistsWithValues()
        {
            // declare constants
            // locators needs to be able to be called from the model, which currently is not possible
            // this should be changed as part of refactor
            const string selectScopeDopdownID = "assignment-scope-selector";
            const string clientOptionScopeCSS = "[aria-label=\"Client\"]";
            const string userOptionScopeCSS = "[aria-label=\"User\"]";

            // create instance of page
            CEMManageUsersPage manageUserPage = leftMenu.NavigateToManageUsersPage().GoToManageAssignmentsPage();

            // verify items are present to satisfy test case
            Assert.IsNotNull(selectScopeDopdownID);
            this.WebDriver.Wait().ForClickableElement(By.Id(selectScopeDopdownID));
            Assert.IsNotNull(clientOptionScopeCSS);
            Assert.IsNotNull(userOptionScopeCSS);
        }
    }
}
