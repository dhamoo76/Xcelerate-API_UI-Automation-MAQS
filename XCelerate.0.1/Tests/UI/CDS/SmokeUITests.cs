using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseWebServiceTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Data;
using System.Linq;
using Tests.UI.Data;

namespace Tests
{
    /// <summary>
    /// Composite Selenium test class
    /// </summary>
    [TestClass]
    public class SmokeUITests : BaseSeleniumTest
    {
        private LoginPage _page;
        private CDSManageListPage _manageListPage;
        private CDSMenuNavigation _menuNavigation;
        private CDSEditStructurePage _editStructurePage;
        private CDSCreateStructurePage _createStructurePage;

        private string _username = Config._loginUsername;
        private CDSStructureItem _structureItem = new();
        private readonly string _timestamp = DateTime.Now.Ticks.ToString();

        [TestInitialize]
        public void Login()
        {
            _page = new LoginPage(this.TestObject);
            _manageListPage = _page.OpenLoginPage("cds").LoginWithValidCredentialsCDS(_username);
            _menuNavigation = new CDSMenuNavigation(this.TestObject);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("249012 | Check a user can login into CDS Manage List Page")]
        public void ManageListPageTest()
        {
            VerifyNavigationCDSItems();
            VerifyManageListPage();
            //to be add waitforPageLaod()
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("256717 | Add new list page : check metadata fields are in place")]
        public void CreateStructurePageTest()
        {
            _createStructurePage = _manageListPage.ClickBtnAddNewList();
            VerifyAddNewListPage(_createStructurePage);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("287013 | CRUD of a List  -Add, Edit & Remove List")]
        public void CreateStructureTest()
        {
            string _labelValue = "ColumnA";
            string _descriptionValue = "ColumnA description";

            _structureItem = SetUpData();
            CreateStructure();

            VerifyEditListPageElements(_editStructurePage);
            VerifyListSructureForm(_editStructurePage);
            //to add fields in Lists structure tab
            _editStructurePage.ClickBtnAddRow()
                              .EnterLabelValueFirstRow(_labelValue)
                              .EnterDescriptionValueFirstRow(_descriptionValue)
                              .ClickSaveButtonFF();
            //To vrify the List structure
            VerifyFieldsItems(_editStructurePage);
            _editStructurePage.ClickListDataTab();
            //To verify the List data tab
            VerifyListDataForm(_editStructurePage);
            VerifyTable(_editStructurePage, _labelValue);
            _editStructurePage.ClickListStructureTab();
            _structureItem = FillAllFields(_structureItem, true);
            _editStructurePage = _createStructurePage.ClickBtnSave();

            _editStructurePage.ClickBtnRemoveList()
                              .ClickBtnYesOnDialog();
            VerifyManageListPage();
        }


        [TestMethod, TestCategory("Smoke")]
        [Description("258856 | Verify Edit List page & its metadata if open from Manage List Page")]
        public void EditStructureTest()
        {
            _editStructurePage = _manageListPage.ClickLnkAnyList();

            VerifyEditListPageElements(_editStructurePage);
            VerifyListSructureForm(_editStructurePage);

            _editStructurePage.ClickListDataTab();
            VerifyListDataForm(_editStructurePage);

            _editStructurePage.ClickListUploadTab();
            VerifyListUploadForm(_editStructurePage);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("296142 |  CRUD of a List Structure while adding new List")]
        public void AddAndDeleteStructureTest()
        {
            string labelValue = "ColumnA";
            string descriptionValue = "ColumnA description";
            //Create a New List
            _structureItem = SetUpData();
            CreateStructure();
            //Add Label & its decrption in the tab -List structure
            VerifyEditListPageElements(_editStructurePage);
            VerifyListSructureForm(_editStructurePage);
            _editStructurePage.ClickBtnAddRow()
                              .EnterLabelValueFirstRow(labelValue)
                              .EnterDescriptionValueFirstRow(descriptionValue)
                              .ClickSaveButtonFF();
            //Verify the fields in List Structure tab
            VerifyFieldsItems(_editStructurePage);
            //To navigate the tab - List data
            _editStructurePage.ClickListDataTab();
            //Verify the fields & table in the List data tab
            VerifyListDataForm(_editStructurePage);
            VerifyTable(_editStructurePage, labelValue);
            //to move back the List structure tab and modify the Label and description
            _editStructurePage.ClickListStructureTab()
                              .EnterLabelValueFirstRow(labelValue + "updated")
                              .EnterDescriptionValueFirstRow(descriptionValue + " updated")
                              .ClickSaveButtonFF();
            //again move to the tab - List data
            _editStructurePage.ClickListDataTab();
            //Verify the List data fields and table
            VerifyListDataForm(_editStructurePage);
            VerifyTable(_editStructurePage, labelValue);
            //delete label and verify it
            _editStructurePage.ClickListStructureTab()
                              .ClickBtnRemoveFirstRow()
                              .ClickBtnNoOnDialog()
                              .ClickSaveButtonFF();
            _editStructurePage.ClickListDataTab();
            VerifyFieldsItemsNotDisplayed(_editStructurePage);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("296143 | CRUD of a List data")]
        public void AddAndDeleteListDataTest()
        {
            string labelValue = "New value";
            string updatedLabelValue = labelValue + "_update";
            string descriptionValue = "New description value";

            _structureItem = SetUpData();
            CreateStructure();
            //To Add the fields in the List structure tab
            _editStructurePage.ClickBtnAddRow()
                            .EnterLabelValueFirstRow(labelValue)
                            .EnterDescriptionValueFirstRow(descriptionValue)
                            .ClickBtnSaveInListStructure();
            _editStructurePage.ClickListDataTab();
            //create label and verify it
            VerifyTable(_editStructurePage, labelValue);
            //By editing the label in Listr structure tab and verify the same in the tab -List Data
            _editStructurePage.ClickListStructureTab()
                              .EnterLabelValueFirstRow(updatedLabelValue)
                              .ClickSaveButtonFF();
            _editStructurePage.ClickListDataTab();
            VerifyTable(_editStructurePage, updatedLabelValue);
            //to Add value into List data tab and verify the saved message
            _editStructurePage.EnterFieldInListData(labelValue, 2)
                              .ClickBtnSaveListData();
            //to update the existing value of the field in List data tab and verify the saved message
            _editStructurePage.EnterFieldInListData(updatedLabelValue)
                              .ClickBtnSaveListData();
        }

        private void VerifyNavigationCDSItems()
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(_menuNavigation.IsIcnRsmCdsLogoDisplayed(), "RSM Logo is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_menuNavigation.IsMnuManageListDisplayed(), "Manage List Icon is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_menuNavigation.IsBtnUserHeaderDisplayed(), "User Icon is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_menuNavigation.IsBtnBellHeaderDisplayed(), "Bar Menu icon is not displayed!"));
        }

        private void VerifyManageListPage()
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsTblManageListsDisplayed(), "Table is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsLblManageListsDisplayed(), "Manage List Label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsBtnAddNewListDisplayed(), "ADD NEW button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsBtnPreviousDisplayed(), "Previous button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsBtnNextDisplayed(), "Next button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsSelNumberPerPageDisplayed(), "Count Dropdown is displayed!"));
        }

        private void VerifyAddNewListPage(CDSCreateStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblAddNewListDisplayed(), "Add new list label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtNameDisplayed(), "Name field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtDescriptionDisplayed(), "Description field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblVisibilityDisplayed(), "Visibility label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSldPrivateDisplayed(), "Private Radio btn is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSelTypeDisplayed(), "Type Dropdown is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtTagsDisplayed(), "Tags is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnSaveDisplayed(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnCancelDisplayed(), "Cancel button is not displayed!"));
            // add values by default
        }

        private void VerifyEditListPageElements(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblEditListDisplayed(), "Edit list label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtNameDisplayed(), "Name field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtDescriptionDisplayed(), "Description field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblVisibilityDisplayed(), "Visibility label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSelTypeDisplayed(), "Type Dropdown is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtTagsDisplayed(), "Tags is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnSaveDisplayed(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnCancelDisplayed(), "Cancel button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnRemoveListDisplayed(), "Remove list button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLnkListStructureDisplayed(), "List Structure tab is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLnkListDataDisplayed(), "List Data tab is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLnkListUploadDisplayed(), "List Upload displayed!"));
            // add that required fields are not empty
        }

        private void VerifyListSructureForm(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblFieldsDisplayed(), "Fields label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblUnderTabTitleDisplayed(), "Text is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnAddRowDisplayed(), "Plus Icon is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSaveButtonFFDisplayed(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsCancelButtonFFDisplayed(), "Cancel button is not displayed!"));
            //add default values
        }

        private void VerifyFieldsItems(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtLabelFirstRowDisplayed(), "Label field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTxtDescriptionFirstRowDisplayed(), "Description field is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnRemoveFirstRowDisplayed(), "Delete Icon is not displayed!"));
        }

        private void VerifyFieldsItemsNotDisplayed(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsTxtLabelFirstRowDisplayed(), "Label field is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsTxtDescriptionFirstRowDisplayed(), "Description field isdisplayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsBtnRemoveFirstRowDisplayed(), "Delete Icon is displayed!"));
        }

        private void VerifyListDataForm(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblManageDataDisplayed(), "Manage Data label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblUnderTabTitleDisplayed(), "Text is not displayed!"));
        }

        private void VerifyTable(CDSEditStructurePage page, string label = "")
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsTblManageDataDisplayed(), "Table is not displayed!"));
            if (String.IsNullOrEmpty(label))
                this.SoftAssert.Assert(() => Assert.AreEqual(label, page.GetFirstColumnNameText(), "Title of first column is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsBtnPreviousDisplayed(), "Previous button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsBtnNextDisplayed(), "Next button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(_manageListPage.IsSelNumberPerPageDisplayed(), "Count Dropdown is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsRemoveButtonFFDisplayed(), "Remove button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSaveButtonLDTabElement(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsCancelButtonFFDisplayed(), "Cancel button is not displayed!"));

        }

        private void VerifyTableNotDisplayed(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsTblManageDataDisplayed(), "Table is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(_manageListPage.IsBtnPreviousDisplayed(), "Previous button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(_manageListPage.IsBtnNextDisplayed(), "Next button is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(_manageListPage.IsSelNumberPerPageDisplayed(), "Count Dropdown is displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsRemoveButtonFFDisplayed(), "Remove button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsSaveButtonLDTabElement(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsFalse(page.IsCancelButtonFFDisplayed(), "Cancel button is not displayed!"));
        }

        private void VerifyListUploadForm(CDSEditStructurePage page)
        {
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblListUploadDisplayed(), "List Upload label is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsLblUnderListUploadTabDisplayed(), "Text is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsBtnUploadDisplayed(), "Upload button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsSaveButtonFFDisplayed(), "Save button is not displayed!"));
            this.SoftAssert.Assert(() => Assert.IsTrue(page.IsCancelButtonFFDisplayed(), "Cancel button is not displayed!"));
        }

        private void FillAllFields()
        {
            _structureItem = FillAllFields(_structureItem);
        }

        private CDSStructureItem FillAllFields(CDSStructureItem item, bool isEditMode = false)
        {
            _createStructurePage.EnterNameValue(item.Name);
            _createStructurePage.EnterDescriptionValue(item.Description);
            if (!isEditMode)
            {
                _createStructurePage.ClickSldPrivate();
                _createStructurePage.SelectSelPrivacyTypeFirstValue();
            }
            _createStructurePage.SelectSelTypeFirstValue();
            foreach (string i in item.Tags)
                _createStructurePage.EnterTagValue(i);
            if (isEditMode)
                _createStructurePage.EnterTagValue(item.Tags[0] + "new");
            item.Type = _createStructurePage.GetSelTypeValue();
            return item;
        }

        private void VerifyEditFields(CDSStructureItem item)
        {
            this.SoftAssert.Assert(() => Assert.AreEqual(item.Name, _editStructurePage.GetTxtNameValue(), "Name value matching"));
            this.SoftAssert.Assert(() => Assert.AreEqual(item.Description, _editStructurePage.GetTxtDescriptionValue(), "Descirption value matching"));
            this.SoftAssert.Assert(() => Assert.AreEqual(item.Type, _editStructurePage.GetSelTypeValue(), "Type matching"));
            //tags verification
        }

        private string GetId()
        {
            return WebDriver.Url;
        }

        private string CreateStructure()
        {
            _createStructurePage = _manageListPage.ClickBtnAddNewList();
            FillAllFields();
            _editStructurePage = _createStructurePage.ClickBtnSave();
            return GetId();
        }

        private CDSStructureItem SetUpData()
        {
            return SetUpData(false);
        }

        private CDSStructureItem SetUpData(bool IsUpdate)
        {
            CDSStructureItem item = new();
            item.IsPublic = false;
            if (IsUpdate)
            {
                item.Name = _structureItem.Name + "_update";
                item.Tags.Add("Update");
                item.IsPublic = true;
                item.Description = _structureItem.Description + "_update";
            }
            else
            {
                item.Name = "Auto_Name" + _timestamp;
                item.Description = "description" + _timestamp;
            }
            item.Tags.Add("Auto");
            return item;
        }
    }
}