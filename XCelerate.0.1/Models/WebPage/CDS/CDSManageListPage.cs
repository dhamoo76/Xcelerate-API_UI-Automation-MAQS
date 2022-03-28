using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CDSManageListPage : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        private static string s_pageUrl = SeleniumConfig.GetWebSiteBase() + "manage-lists";
        private const string _lblManageListsE2e = "view-list-header";
        private const string _btnAddNewListE2e = "addNewListButton";
        private const string _tblManageListsCss = ".p-datatable-table";
        private const string _txtSearchCss = ".p-inputtext";
        private const string _lnkFirstListInTblCss = ".p-datatable-tbody>tr>*>a";

        /// <summary>
        /// Initializes a new instance of the <see cref="CDSManageListPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CDSManageListPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        private LazyElement BtnAddNewList => GetLazyElementByCSSe2e(_btnAddNewListE2e, "Add button");

        private LazyElement TblManageLists => GetLazyElement(By.CssSelector(_tblManageListsCss), "Table");

        private LazyElement TxtSearch => GetLazyElement(By.CssSelector(_txtSearchCss), "Input Search Text Field");

        private LazyElement LblManageLists => GetLazyElementByCSSe2e(_lblManageListsE2e, "Manage List Label");

        private IWebElement LnkFirstListInTbl => GetLazyElement(By.CssSelector(_lnkFirstListInTblCss), "The First List Lnk");

        private IList <IWebElement> LnkListNames => TblManageLists.FindElements(By.CssSelector(_lnkFirstListInTblCss));
        
        
        public bool IsLblManageListsDisplayed() => LblManageLists.Displayed;

        public bool IsBtnAddNewListDisplayed() => BtnAddNewList.Displayed;


        public CDSCreateStructurePage ClickBtnAddNewList()
        {
            BtnAddNewList.Click();
            return new CDSCreateStructurePage(TestObject);
        }

        public CDSEditStructurePage ClickLnkAnyList()
        {
            LnkFirstListInTbl.Click();
            return new CDSEditStructurePage(TestObject);
        }

        public bool IsTblManageListsDisplayed() => TblManageLists.Displayed;

        /// <summary>
        /// Check if the home page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return true;
        }
    }
}

