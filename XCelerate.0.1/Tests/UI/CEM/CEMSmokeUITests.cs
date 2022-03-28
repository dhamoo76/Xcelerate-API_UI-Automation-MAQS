using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Collections.Generic;
using Tests.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tests
{
    /// <summary>
    /// Composite Selenium test class
    /// </summary>
    [TestClass]
    public class CEMSmokeUITests : BaseSeleniumTest
    {
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
        [Description ("288642 | Dashboards are displayed on Landing page")]
        //A CEM User see the Landing page after launching the CEM application
        public void CEM_01_LandingPageViewAfterLoginTest()
        {
            int _number = landingPage.DashboardsCount();
            Assert.AreEqual(6, _number, "There are not 6 dashboards on the Landing Page");
            //@TODO : Select different types of view of charts
        }

        [TestMethod]
        [Description("306370 | Verify Left Menu Items")]
        //There are 4 blocks on the page: Manage Org, Manage Users, Manage Groups and Manage Applications
        //@TODO: add TC numb
        public void CEM_02_LeftBarMenuItemsAreShownTest()
        {
            Assert.IsTrue(leftMenu.AreAllButtonsShown(),"Some Left Menu Button(s) is(are) not shown");
        }

        [TestMethod]
        [Description("243066 | Its possible to choose a Client to work on it's objects")]
        public void CEM_03_VerifyChooseClientFunctionalityTest()
        {
            CEMManageClientsPage manageClientsPage = leftMenu.NavigateToManageOrgPage()
                                                             .ChooseRandomClient();

            Assert.IsTrue(manageClientsPage.IfEngagementsTabDisplayed(), "Engagements tab was not loaded");
        }

        [TestMethod]
        [Description("243071 | It's possible to create an Engagement")]
        public void CEM_04_VerifyCreateEngagementTest()
        {
            string _randomName = DataHelper.GenerateRandomString(10, "engagement");
            
            CEMManageClientsPage manageClientsPage = leftMenu.NavigateToManageOrgPage()
                    .ChooseRandomClient()
                    .GoToCreateEngagementPage()
                    .CreateValidEngagement(_randomName);

            //@TODO COVER with test that newly created engagement is shown on engagement tab when it will be fixed

            Assert.IsTrue(manageClientsPage.IfEngagementsTabDisplayed(), "Engagements tab was not loaded");
        }

        [TestMethod]
        [Description("243084 | It's possible to create a project within an engagement")]
        public void CEM_05_VerifyCreateProjectTest()
        {
            string _randomName = DataHelper.GenerateRandomString(10, "project");

            
            CEMManageClientsPage manageClientsPage = new CEMManageClientsPage(this.TestObject);
            CEMEditEngagementPage editEngagementPage = new CEMEditEngagementPage(this.TestObject);

            leftMenu.NavigateToManageOrgPage()
                    .ChooseRandomClient()
                    .GoToCreateEngagementPage()
                    .CreateValidEngagement(_randomName);

            WebDriver.Navigate().Refresh();
            //Should be fixed
            //@TODO COVER with test that newly created engagement is shown on engagement tab when it will be fixed

            manageClientsPage.GoToEditEngagementPage(_randomName).GoToCreateProjectPage().CreateValidProject(_randomName);

            Assert.IsTrue(editEngagementPage.IfNewProjectShownInTable(_randomName), "Newly Created project was not added ");
        }

        [TestMethod]
        [Description("243344 | It's possible to assign entities to a project")]
        //@TODO Add clients only with ENTITIES
        //@TODO Add Assertion
        public void CEM_06_VerifyAssignEntityToTheProjectTest()
        {
            string _randomName = DataHelper.GenerateRandomString(10);
   
            CEMEditProjectPage editProjectPage = new CEMEditProjectPage(this.TestObject);
            CEMAssignEntitiesPage assignEntitiesPage = new CEMAssignEntitiesPage(this.TestObject);

            CEMManageClientsPage manageClientsPage = leftMenu.NavigateToManageOrgPage()
                                                             .ChooseClientWithEntities()
                                                             .GoToCreateEngagementPage().CreateValidEngagement(_randomName);

            WebDriver.Navigate().Refresh();
            //Should be fixed
            //@TODO COVER with test that newly created engagement is shown on engagement tab when it will be fixed

            List<String> entitiesNameList = manageClientsPage.GoToEditEngagementPage(_randomName)
                             .GoToCreateProjectPage()
                             .CreateValidProject(_randomName)
                             .GoToEditProjectPage(_randomName)
                             .GoToAssignEntitiesPage()
                             .CollectAllENtitiesName();

            assignEntitiesPage.AssignAllEntitiesToTheProject();

            Assert.IsTrue(editProjectPage.IfEntityAssignmentWasSuccessful(), " Entities were mot assigned to the Project");
        }

        [TestMethod]
        [Description("243358 | Create Assignment page content is displayed correctly")]
        public void CEM_07_VerifyCreateAssignmentPageTest()
        {
            CEMCreateAssignmentsPage createAssignmentsPage = leftMenu.NavigateToManageUsersPage()
                                                                     .GoToCreateAssignmentsPage()
                                                                     .ChooseRandomClient()
                                                                     .ChooseRandomUserFromUserList();

            Assert.IsTrue(createAssignmentsPage.IfUsersAndRolesTablesWereLoaded(), "Users And Roles tables were not loaded after Client was chosen");
        }

        [TestMethod]
        [Description("243368 | Manage Assignments page content is displayed correctly")]
        public void CEM_08_VerifyManageAssignmentsPageTest()
        {
            CEMManageUsersPage manageAssignmentsPage = leftMenu.NavigateToManageUsersPage()
                                                               .GoToManageAssignmentsPage()
                                                               .ChooseRandomClient();

            Assert.IsTrue(manageAssignmentsPage.IsGridLoaded(), "Grid was not loaded");
        }

        [TestMethod]
        [Description("261292 | It's possible to Navigate to Manage Applications tab")]
        public void CEM_09_VerifyManageApplicationsPageTest()
        {
            CEMManageApplicationsPage manageApplicationsPage = leftMenu.NavigateToManageAppsPage();
            
            Assert.IsTrue(manageApplicationsPage.IsPageLoaded(), "Manage Applications Page was not Loaded");
        }

        [TestMethod]
        [Description("261294 | It's possible to create a new role")]
        public void CEM_10_VerifyCreateNewRoleTest()
        {
            string randomRoleName = DataHelper.GenerateRandomString(10, "role");
            CEMManageApplicationsPage manageApplicationsPage = leftMenu.NavigateToManageAppsPage()
                                                                       .AddNewRole(randomRoleName);

            Assert.IsTrue(manageApplicationsPage.IsNewlyCreatedRoleIsShownInTheTable(randomRoleName), "New Role was not Added");
        }

        [TestMethod]
        [Description("288643 | It's possible to create User group with users")]
        public void CEM_11_VerifyCreateUserGroupsWithUsersTest()
        {
            string randomGroupName = DataHelper.GenerateRandomString(5, "group");
           
            CEMManageGroupPage manageGroupsPage = leftMenu.NavigateToManageUsersPage()
                                                          .GoToManageGroupPage()
                                                          .AddUserGroup(randomGroupName)
                                                          .AddUserToTheGroup(randomGroupName, userName);
            
            Assert.IsTrue(manageGroupsPage.IsUserAddedToTheUserGroup(userName), "New Role was not Added");
        }
    }
}