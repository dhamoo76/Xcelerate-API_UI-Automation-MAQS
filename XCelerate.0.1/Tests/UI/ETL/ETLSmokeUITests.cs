using Magenic.Maqs.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using Tests.API.Utilities.Common;
using System.IO;
using System.Threading;
using Tests.Common;
using Tests.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Models.WebPage.ETL.Behaviors;

namespace Tests
{
    [TestClass]
    public class ETLSmokeUITests : CommonTokenBaseClass
    {
        private ETLPayloadsView payloadsViewPage;
        private ETLProtocolsView protocolsViewPage;

        //save values to clean after
        private string _protocolName;

        //@todo:
        //add pre-conditions:
        // CEM API: create assignments client, LE, Project, Eng., JObId
        // ETL API: create procedure and use it default

        [TestInitialize]
        public void TestSetUp()
        {
            LoginPage page = new LoginPage(TestObject);

            // @TODO: the page from the beggining should be etl configuration
            payloadsViewPage = page.OpenLoginPage("etl").LoginWithValidCredentialsETL(Config._loginUsername, Config._loginPassword);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("242877 | Login to ETL")]
        public void ETL_01_LandingPageViewTest()
        {
            payloadsViewPage.ClickBtnEtlConfiguration();
            VerifyLandingPage();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("242878 | Clients dropdown choosing")]
        public void ETL_02_SelectClientTest()
        {
            // select client
            payloadsViewPage.SelectAnyClient();
            VerifyPayloadPage();

            // verify protocols view elements
            protocolsViewPage = payloadsViewPage.ClickProtocolsTab();
            VerifyProtocolPage();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("242879 | Create a Protocol")]
        public void ETL_03_CreateProtocolTest()
        {
            ETLProtocolsAdd protocolsAdd = payloadsViewPage.SelectClient(Config._clientName)
                                                           .ClickProtocolsTab()
                                                           .ClickAddNewProtocolButton();
            VerifyProtocolAddPage(protocolsAdd);

            // add new protocol
            _protocolName = DataHelper.GenerateRandomString(10, "ProtocolName");
            var _protocolFolder = DataHelper.GenerateRandomString(10, "ProtocolFolder");

            ETLProtocolsView protocolViewPage = protocolsAdd.EnterProtocolName(_protocolName)
                                                            .EnterFolderName(_protocolFolder)
                                                            .SaveProtocol();

            // @TODO move verify notification to function 
            // check notification message
            bool isNotificationVisible = protocolViewPage.IsNotificationMessageVisible();
            this.SoftAssert.Assert(() => Assert.IsTrue(isNotificationVisible, "Notification not visible!"));

            string notificationMessage = protocolViewPage.GetNotificationMessage();
            this.SoftAssert.Assert(() => Assert.AreEqual("Your protocol has been successfully saved", notificationMessage, "Different notification!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolViewPage.IsPageLoaded(), "Protocols View not loaded!"));
        }

        [TestMethod]
        [Description("242882 | Create payload")]
        public void ETL_04_CreatePayloadTest()
        {
            var _payloadName = DataHelper.GenerateRandomString(10, "PayloadName");
            var _procedure = "GeneateCCH";
            var _recipient = "autoUser123@test.test";
            var _fileName = DataHelper.GenerateRandomString(10, "FileName") + ".XLSX";

            ETLPayloadsAdd payloadsAdd = payloadsViewPage.SelectClient(Config._clientName)
                                                         .ClickAddNewPayloadButton();
            VerifyPayloadAddPage(payloadsAdd);

            // add new payload
            payloadsAdd.EnterPayloadName(_payloadName)
                       .ChooseAnyProtocolNameInDropdown()
                       .ChooseAnyEngagementDropdown("Auto Engagement")
                       .ChooseAnyProjectDropdown("proj1")
                       .ChooseLEAnyDropdown()
                       .EnterNotificationRecipients(_recipient)
                       .ChooseProcedureInDropdown(_procedure)
                       .EnterAllFileNames()
                       .ClickBtnSave();

            // check notification message
            bool isNotificationVisible = payloadsViewPage.IsNotificationMessageVisible();
            Assert.IsTrue(isNotificationVisible, "Notification not visible!");

            var notificationMessage = payloadsViewPage.GetNotificationMessage();
            Assert.AreEqual("Your payload has been saved successfully", notificationMessage, "Different notification!");
            Assert.IsTrue(payloadsViewPage.IsPageLoaded(), "Payloads View not loaded!");
        }

        [TestMethod]
        [Description("242889 | Procedure Status")]
        public void ETL_05_CheckProcedureStatusPage()
        {
            ETLProcedureStatus procedureStatus = payloadsViewPage.ClickBtnProcedureStatus()
                                                                 .SelectClient(Config._clientName);
            VerifyProcedureStatusPage(procedureStatus);
        }

        [TestMethod]
        [Description("242885 | Overview of ETL Uploader page")]
        public void ETL_06_CheckEtlUploaderPage()
        {
            ETLUploaderUploadFile ETLUploaderPage = payloadsViewPage.ClickLnkUploadFile();

            ETLUploaderPage.ChooseAnySolutionInDropdown()
                           .ChooseAnyProcedureInDropdown();

            VerifyUploadersElements(ETLUploaderPage);

            // verify ETL Uploader - view results page
            ETLUploaderViewResults uploaderViewResults = payloadsViewPage.ClickLnkViewResults();

            VerifyUploaderViewResult(uploaderViewResults);
        }

        [TestMethod]
        [Description("262933 | Verify Create Procedure page")]
        public void ETL_07_CheckCreateProcedurePage()
        {
            ETLProcedureLibrary procedureLibrary = payloadsViewPage.ClickBtnProcedureLibrary();
            VerifyProcedureLibraryPage(procedureLibrary);

            ETLProcedureAdd procedureAdd = procedureLibrary.ClickAddNewProcedureButton();
            procedureAdd.ChooseWorkflowInDropdownElement(Config._workflow)
                        .ChooseAnySchemaTypeDropdown()
                        .ChooseAnyFileTypeDropdown()
                        .SelectAnyClient();

            VerifyAddProcedurePage(procedureAdd);
        }

        [TestMethod]
        [Description("272697 | Create Procedure")]
        public void ETL_08_CreateProcedure()
        {
            var _procedureName = DataHelper.GenerateRandomString(10, "ProcedureName");
            var _time = "123";

            ETLProcedureLibrary procedureLibrary = payloadsViewPage.ClickBtnProcedureLibrary();

            ETLProcedureAdd procedureAdd = procedureLibrary.ClickAddNewProcedureButton();
            procedureAdd.EnterProcedureNameInput(_procedureName)
                        .ChooseWorkflowInDropdownElement(Config._workflow)
                        .ChooseAnySchemaTypeDropdown()
                        .ChooseAnyFileTypeDropdown()
                        .SelectAnyClient()
                        .EnterTimeSavings(_time)
                        .ClickBtnSave();

            // check notification message
            bool isNotificationVisible = procedureLibrary.IsNotificationMessageVisible();
            this.SoftAssert.Assert(() => Assert.IsTrue(isNotificationVisible, "Notification not visible!"));

            string notificationMessage = procedureLibrary.GetNotificationMessage();

            this.SoftAssert.Assert(() => Assert.AreEqual("Your procedure has been saved successfully", notificationMessage, "Different notification!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureLibrary.IsPageLoaded(), "Procedure Library page not loaded!"));
        }

        [TestMethod]
        [Description("272698 | View Procedure")]
        public void ETL_09_ViewProcedure()
        {
            // check View Procedure page
            ETLProcedureLibrary procedureLibrary = payloadsViewPage.ClickBtnProcedureLibrary();
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureLibrary.IsPageLoaded(), "Procedure Library page not loaded!"));
            ETLProcedureView procedureView = procedureLibrary.ChooseAnyProcedureDropdown();
            VerifyViewProcedurePage(procedureView);
        }

        [TestMethod]
        [Description("242886 | ETL Configuration Upload files")]
        public void ETL_10_UploadFilesTest()
        {
            ETLUploadFileManagement ETLUploadFileManagement = new ETLUploadFileManagement(this.TestObject);
            ETLUploaderUploadFile ETLUploaderUploadFilePage = ETLUploadFileManagement.GoToETLUploadFilePage();

            var _solution = "CDS";
            var _procedure = "LE Bulk Procedure  NO OUTPUT";

            ETLUploaderUploadFilePage.ChooseSolutionInDropdown(_solution)
                                     .ChooseProcedureDropdown(_procedure);

            VerifyUploadersElements(ETLUploaderUploadFilePage);

            var _uploadFile = Path.Combine(Directory.GetCurrentDirectory(), "LegalEntity Test.xlsx");  
            ETLUploadFileManagement.UploadFiles(_uploadFile);

            ETLUploaderViewResults uploaderViewResults = ETLUploadFileManagement.ClickSubmitBtn();

            Assert.IsTrue(uploaderViewResults.UploadNotificationSnackBarElement.Displayed);
            Assert.IsTrue((uploaderViewResults.UploadedFileStatus.Text == "Completely Matched"), "Incorrect file status: " + uploaderViewResults.UploadedFileStatus.Text);

            Magenic.Maqs.Utilities.Helper.GenericWait.Wait(ETLUploadFileManagement.UploadedFileStatusChange, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5), false);
            Assert.IsTrue((uploaderViewResults.UploadedFileStatus.Text == "Successful"), "Incorrect file status: " + uploaderViewResults.UploadedFileStatus.Text);
        }

        //todo: 
        // gather all id created and use API call to clean up data 
        //clean up


        //Local verification 
        private void VerifyLandingPage()
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsClientIdDropdownDisplayed(), "Client ID dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsEngagementDropdownDisplayed(), "Engagement dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsProjectDropdownDisplayed(), "Project dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsLegalEntityDropdownDisplayed(), "Legal Entity dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsResetButtonDisplayed(), "Reset button not displayed!"));
        }

        private void VerifyProtocolAddPage(ETLProtocolsAdd protocolsAdd)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsSelectedClientTextDisplayed(), "Selected Client Text not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsProtocolNameInputDisplayed(), "Protocol Name Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsProtocolTypeDropdownDisplayed(), "Protocol Type Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsProtocolUrlInputDisplayed(), "Protocol URL Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsMdmClientIdInputDisplayed(), "MDM Client ID Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsFolderNameInputDisplayed(), "Folder Name Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsUsernameInputDisplayed(), "Username Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsPasswordInputDisplayed(), "Password Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsAdd.IsSaveButtonDisplayed(), "Save Button not displayed!"));
        }

        private void VerifyProtocolPage()
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsViewPage.IsPayloadsTabDisplayed(), "Payloads tab not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsViewPage.IsProtocolsTabDisplayed(), "Protocols tab not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(protocolsViewPage.IsAddNewProtocolButtonDisplayed(), "Add new protocol button not displayed!"));
        }

        private void VerifyPayloadPage()
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsPayloadsTabDisplayed(), "Payloads tab not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsProtocolsTabDisplayed(), "Protocols tab not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsViewPage.IsAddNewPayloadButtonDisplayed(), "Add new payload button not displayed!"));
        }

        private void VerifyViewProcedurePage(ETLProcedureView procedureView)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsProcedureNameInputDisplayed(), "Procedure Name Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsWorkflowDropdownDisplayed(), "Workflow Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsQuestionsListDisplayed(), "Questions List not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsSchemaTypeDropdownDisplayed(), "Schema Type Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsAddSchemaButtonDisplayed(), "Add Schema Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsFileTypeDropdownDisplayed(), "File Type Dropdown not displayed!"));
            //@todo: to be sure that client dropdown is displayed
            //this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsClientDropdownDisplayed(), "Client Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsTimeSavingsInputDisplayed(), "Time Savings Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureView.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
        }

        private void VerifyPayloadAddPage(ETLPayloadsAdd payloadsAdd)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsSelectedClientTextDisplayed(), "Selected Client Text not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsPayloadNameInputDisplayed(), "Payload Name Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsProtocolNameDropdownDisplayed(), "Protocol Name Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsEngagementDropdownDisplayed(), "Engagement Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsProjectDropdownDisplayed(), "Project Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsLegalEntityDropdownDisplayed(), "Legal Entity Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsNotificationRecipientsInputDisplayed(), "Notification Recipients Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsProcedureDropdownDisplayed(), "Procedure Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(payloadsAdd.IsSaveButtonDisplayed(), "Save Button not displayed!"));
        }

        private void VerifyProcedureStatusPage(ETLProcedureStatus procedureStatus)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureStatus.IsClientIdDropdownDisplayed(), "Client ID dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureStatus.IsProcedureResultsTableDisplayed(), "Procedure Results Table not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureStatus.IsRefreshButtonDisplayed(), "Refresh Button not displayed!"));
        }

        private void VerifyUploaderViewResult(ETLUploaderViewResults uploaderViewResults)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderViewResults.IsRefreshButtonDisplayed(), "Refresh Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderViewResults.IsUploadStatusTableDisplayed(), "Upload Status Table not displayed!"));
        }

        private void VerifyUploadersElements(ETLUploaderUploadFile ETLUploaderpage)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(ETLUploaderpage.IsMandatoryFilesListDisplayed(), "Mandatory Files List not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(ETLUploaderpage.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(ETLUploaderpage.IsSubmitButtonDisplayed(), "Submit Button not displayed!"));
        }

        private void VerifyETLUploaderPage(ETLUploaderUploadFile uploaderUploadFile)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderUploadFile.IsSolutionDropdownDisplayed(), "Solution Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderUploadFile.IsProcedureDropdownDisplayed(), "Procedure Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderUploadFile.IsMandatoryFilesListDisplayed(), "Mandatory Files List not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderUploadFile.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(uploaderUploadFile.IsSubmitButtonDisplayed(), "Submit Button not displayed!"));
        }

        private void VerifyQuestionArea(ETLProcedureAdd procedureAdd)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsQuestionsListDisplayed(), "Questions List not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsSchemaTypeDropdownDisplayed(), "Schema Type Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsAddSchemaButtonDisplayed(), "Add Schema Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsFileTypeDropdownDisplayed(), "File Type Dropdown not displayed!"));
        }

        private void VerifyProcedureLibraryPage(ETLProcedureLibrary procedureLibrary)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureLibrary.IsProcedureDropdownDisplayed(), "Procedure Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureLibrary.IsAddNewProcedureButtonDisplayed(), "Add New Procedure Button not displayed!"));
        }

        private void VerifyAddProcedurePage(ETLProcedureAdd procedureAdd)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsProcedureNameInputDisplayed(), "Procedure Name Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsWorkflowDropdownDisplayed(), "Workflow Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsQuestionsListDisplayed(), "Questions List not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsSchemaTypeDropdownDisplayed(), "Schema Type Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsAddSchemaButtonDisplayed(), "Add Schema Button not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsFileTypeDropdownDisplayed(), "File Type Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsClientDropdownDisplayed(), "Client Dropdown not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsTimeSavingsInputDisplayed(), "Time Savings Input not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(procedureAdd.IsCancelButtonDisplayed(), "Cancel Button not displayed!"));
        }
    }
}