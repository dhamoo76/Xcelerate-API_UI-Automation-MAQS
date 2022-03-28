using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CDSEditStructurePage : CDSCreateStructurePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        private const string _btnRemoveListE2e = "remove-list-button";
        private const string _lblEditListE2e = "edit-list-Edit List";

        private const string _lblFieldsE2e = "edit-list-Fields";
        private const string _btnAddRowE2e = "addRowButton";
        private static string _txtLabelFirstRowE2e = "fields[0].label";
        private static string _txtDescriptionFirstRowE2e = "fields[0].description";
        private static string _btnRemoveFirstRowE2e = "removeRowButton-0";

        private const string _lnkListStructureXpath = "//*[contains(text(),'List Structure')]";
        private const string _lnkListDataXpath = "//*[contains(text(),'List Data')]";
        private const string _lnkListUploadXpath = "//*[contains(text(),'List Upload')]";

        private const string _lblManageDataE2e = "edit-list-Manage Data";
        private const string _lblListUploadE2e = "edit-list-List Upload";

        private const string _lblUnderTabTitleCss = ".ml-2.mb-4"; //linkToUpload_data
        private const string _icnSortCss = ".p-sortable-column-icon";
        private const string _tblManageDataCss = ".p-datatable-table";
        private const string _lblFirstColumnNameCss = ".p-column-title>*[e2e-id=\"nonWrappedTextContainer\"]"; //add method later how to get titles

        private const string _lblUnderListUploadTabCss = ".text-small.py-2.pl-2";
        private const string _lnkListUploadOnStructureE2e = "linkToUpload_field";
        private const string _txtNewRowAnyColumnE2e = "footer";
        
        //list upload area
        private const string _btnUploadCss = ".p-fileupload-choose";

        private const string _pnlTabViewCss = ".p-tabview-panels";
        private const string _btnSaveListDataE2e = "saveButton";
        private const string _btnNoOnDialogXpath = "//*[text()='REMOVE']";
        private const string _btnYesOnDialogXpath = "//button[.='YES']";

        /// <summary>
        /// Initializes a new instance of the <see cref="CDSEditStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CDSEditStructurePage(ISeleniumTestObject testObject) : base(testObject)
        {
            WebDriver.Wait().ForClickableElement(By.CssSelector(_lblUnderTabTitleCss));
        }

        /// <summary>
        /// Gets user name box
        /// </summary>
        private LazyElement BtnRemoveList => GetLazyElementByCSSe2e(_btnRemoveListE2e, "Remove List Button");

        private LazyElement LblEditList => GetLazyElementByCSSe2e(_lblEditListE2e, "Edit List Title");

        private LazyElement LblFields => GetLazyElementByCSSe2e(_lblFieldsE2e, "Fields Label");

        private LazyElement LnkListUploadOnStructure => GetLazyElementByCSSe2e(_lnkListUploadOnStructureE2e, "List Upload link");

        private LazyElement BtnAddRow => GetLazyElementByCSSe2e(_btnAddRowE2e, "Add Row Button");

        private LazyElement TxtLabelFirstRow => GetLazyElementByCSSe2e(_txtLabelFirstRowE2e, "Label FirstText Field");

        private LazyElement TxtDescriptionFirstRow => GetLazyElementByCSSe2e(_txtDescriptionFirstRowE2e, "Description First TextField");

        private LazyElement BtnRemoveFirstRow => GetLazyElementByCSSe2e(_btnRemoveFirstRowE2e, "Remove Icon");

        protected LazyElement LnkListStructure => GetLazyElement(By.XPath(_lnkListStructureXpath), "List Structure tab");

        protected LazyElement LnkListData => GetLazyElement(By.XPath(_lnkListDataXpath), "List Data tab");

        protected LazyElement LnkListUpload => GetLazyElement(By.XPath(_lnkListUploadXpath), "List Upload tab");

        private LazyElement LblUnderTabTitle => GetLazyElement(By.CssSelector(_lblUnderTabTitleCss), "Description First TextField");

        private LazyElement LblManageData => GetLazyElementByCSSe2e(_lblManageDataE2e, "Manage Data label");

        private LazyElement LblListUpload => GetLazyElementByCSSe2e(_lblListUploadE2e, "List upload label");

        private LazyElement IcnSort => GetLazyElement(By.CssSelector(_icnSortCss), "Sort Icon");

        private LazyElement TblManageData => GetLazyElement(By.CssSelector(_tblManageDataCss), "Table");

        private LazyElement LblFirstColumnName => GetLazyElement(By.CssSelector(_lblFirstColumnNameCss), "Title of first column");

        //area under the tab
        private LazyElement PnlTabView => GetLazyElement(By.CssSelector(_pnlTabViewCss), "FF area");

        private LazyElement SaveButtonFFElement => GetLazyElementByCSSe2e(PnlTabView, _btnSaveE2e, "Save button on Tab");

        private LazyElement SaveButtonLDTabElement => GetLazyElementByCSSe2e(_btnSaveListDataE2e, "Save button on LD tab");

        private LazyElement CancelButtonFFElement => GetLazyElementByCSSe2e(PnlTabView, _btnCancelE2e, "Cancel button on Tab");

        private LazyElement RemoveButtonFFElement => GetLazyElementByCSSe2e(PnlTabView, _btnDeleteE2e, "Remove button on Tab");

        private LazyElement BtnUpload => GetLazyElement(By.CssSelector(_btnUploadCss), "Upload button");

        private LazyElement LblUnderListUploadTab => GetLazyElement(By.CssSelector(_lblUnderListUploadTabCss), "Text under Label");

        private LazyElement TxtNewRowAnyColumn => GetLazyElementByE2eContains(_txtNewRowAnyColumnE2e, "Input Field");

        private LazyElement BtnNoOnDialog => GetLazyElement(By.XPath(_btnNoOnDialogXpath), "NO button on dialog");

        private LazyElement BtnYesOnDialog => GetLazyElement(By.XPath(_btnYesOnDialogXpath), "Yes button on alert");

        private LazyElement BtnSaveListData => GetLazyElementByCSSe2e(_btnSaveListDataE2e, "Save Button");

        // properties

        public bool IsLblEditListDisplayed() => LblEditList.Displayed;

        public bool IsBtnRemoveListDisplayed() => BtnRemoveList.Displayed;

        public bool IsLnkListStructureDisplayed() => LnkListStructure.Displayed;

        public bool IsLnkListUploadDisplayed() => LnkListUpload.Displayed;

        public bool IsLnkListDataDisplayed() => LnkListData.Displayed;

        // List Structure
        public bool IsLblFieldsDisplayed() => LblFields.Displayed;

        public bool IsTxtLabelFirstRowDisplayed()
        {
            try
            {
                return TxtLabelFirstRow.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsTxtDescriptionFirstRowDisplayed() => IsElementDisplayed(TxtDescriptionFirstRow);

        public bool IsLblUnderTabTitleDisplayed() => LblUnderTabTitle.Displayed;

        public bool IsLnkListUploadOnStructureDisplayed() => LnkListUploadOnStructure.Displayed;

        public bool IsBtnRemoveFirstRowDisplayed()
        {
            try
            {
                return BtnRemoveFirstRow.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsBtnAddRowDisplayed() => BtnAddRow.Displayed;

        public bool IsSaveButtonFFDisplayed() => SaveButtonFFElement.Displayed;

        public bool IsSaveButtonLDTabElement() => IsElementDisplayed(SaveButtonLDTabElement);

        public bool IsCancelButtonFFDisplayed() => IsElementDisplayed(CancelButtonFFElement);

        public bool IsLblManageDataDisplayed() => LblManageData.Displayed;

        public bool IsTblManageDataDisplayed() => IsElementDisplayed(TblManageData);

        public bool IsRemoveButtonFFDisplayed() => IsElementDisplayed(RemoveButtonFFElement);

        public bool IsLblListUploadDisplayed() => LblListUpload.Displayed;

        public bool IsBtnUploadDisplayed() => BtnUpload.Displayed;

        public bool IsLblUnderListUploadTabDisplayed() => LblUnderListUploadTab.Displayed;

        public string GetFirstColumnNameText() => LblFirstColumnName.Text;

        // actions
        public CDSEditStructurePage ClickBtnAddRow()
        {
            BtnAddRow.ScrollIntoView();
            BtnAddRow.Click();
            return this;
        }

        public CDSEditStructurePage EnterLabelValueFirstRow(string label)
        {
            Clear(TxtLabelFirstRow);
            TxtLabelFirstRow.SendKeys(label);
            return this;
        }

        public CDSEditStructurePage EnterDescriptionValueFirstRow(string description)
        {
            TxtDescriptionFirstRow.SendKeys(description);
            return this;
        }

        public CDSEditStructurePage ClickListDataTab()
        {
            WebDriver.Wait().ForClickableElement(By.CssSelector(_lblUnderTabTitleCss));
            LnkListData.Click();
            return this;
        }

        public CDSEditStructurePage ClickListStructureTab()
        {
            WebDriver.Wait().ForClickableElement(By.CssSelector(_lblUnderTabTitleCss));
            LnkListStructure.Click();
            return this;
        }

        public CDSEditStructurePage ClickListUploadTab()
        {
            LnkListUpload.Click();
            return this;
        }

        public CDSEditStructurePage ClickSaveButtonFF()
        {
            SaveButtonFFElement.Click();
            return this;
        }

        public CDSEditStructurePage ClickBtnRemoveFirstRow()
        {

            BtnRemoveFirstRow.Click();
            return this;
        }

        public CDSEditStructurePage ClickBtnNoOnDialog()
        {
            BtnNoOnDialog.Click();
            return this;
        }

        public CDSEditStructurePage ClickBtnRemoveList()
        {
            BtnRemoveList.Click();
            return this;
        }

        public CDSEditStructurePage ClickBtnYesOnDialog()
        {
            BtnYesOnDialog.Click();
            return this;
        }

        public CDSEditStructurePage EnterFieldInListData(string text, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                TxtNewRowAnyColumn.SendKeys(text);
                TxtNewRowAnyColumn.SendKeys(Keys.Tab);
            }
            return this;
        }

        public CDSEditStructurePage ClickBtnSaveInListStructure()
        {
            SaveButtonFFElement.Click();
            return this;
        }

        public CDSEditStructurePage ClickBtnSaveListData()
        {
            BtnSaveListData.Click();
            return this;
        }
    }
}

