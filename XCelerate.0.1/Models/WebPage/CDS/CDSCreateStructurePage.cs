using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CDSCreateStructurePage : BasePage
    {
        public static readonly string[] ListType = {
            "Core Metadata",
            "Client Metadata",
            "Cross Reference Metadata",
            "Reference Data",
            "Validation Library"
        };
        public static readonly string[] ListPrivacyType = {
            "Multiple Clients"
        };

        /// <summary>
        /// The list of elements
        /// </summary>
        private static string s_pageUrl = SeleniumConfig.GetWebSiteBase() + "manage-lists/create-structure";
        private const string _lblAddNewListE2e = "create-list-header";
        private const string _lblTagE2e = "nonWrappedTextContainer";
        private const string _lblVisibilityXpath = $"//*[@e2e-id='isPrivate']//label";
        private const string _sldIsPrivateId = "isPrivate";
        private const string _selPrivacyTypeE2e = "listPrivacyType";
        private const string _selTypeId = "listType";
        private const string _txtDescriptionE2e = "description";
        private const string _txtNameE2e = "name";
        private const string _txtTagsCss = "input[name=tags]";
        // Every expanded drop-down menu and visible drop-down item have these locators
        private const string _pnlExpandedSelCss = ".p-dropdown-panel";
        private const string _selItemCss = ".p-dropdown-item";


        /// <summary>
        /// Initializes a new instance of the <see cref="CDSCreateStructurePage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CDSCreateStructurePage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        protected LazyElement TxtName => GetLazyElementByCSSe2e(_txtNameE2e, "Name Text Field");

        protected LazyElement LblAddNewList => GetLazyElementByCSSe2e(_lblAddNewListE2e, "Add New List Label");

        protected LazyElement TxtDescription => GetLazyElementByCSSe2e(_txtDescriptionE2e, "Description Text Field");

        protected ICollection<IWebElement> LblTags => WebDriver.Find().Elements(By.XPath(_lblTagE2e));

        protected LazyElement LblVisibility => GetLazyElement(By.XPath(_lblVisibilityXpath), "Visibility Label");

        protected LazyElement SldPrivate => GetLazyElement(By.Id(_sldIsPrivateId), "Private Slider");

        protected LazyElement SelPrivacyType => GetLazyElementByCSSe2e(_selPrivacyTypeE2e, "Privacy Type Dropdown List");

        protected LazyElement SelType => GetLazyElement(By.Id(_selTypeId), "Type Dropdown List");

        protected LazyElement TxtTags => GetLazyElement(By.CssSelector(_txtTagsCss), "Tags Text Field");

        protected IWebElement GetSelOptionByText(string text)
        {
            return WebDriver.Find().ElementWithText(By.CssSelector(_selItemCss), text);
        }


        // property
        public bool IsLblAddNewListDisplayed() => LblAddNewList.Displayed;

        public string GetTxtNameValue() => TxtName.GetValue();

        public bool IsTxtNameDisplayed() => TxtName.Displayed;

        public string GetTxtDescriptionValue() => TxtDescription.GetValue();

        public bool IsTxtDescriptionDisplayed() => TxtDescription.Displayed;

        public bool IsLblVisibilityDisplayed() => LblVisibility.Displayed;

        public bool IsSldPrivateDisplayed() => SldPrivate.Displayed;

        public bool IsSelTypeDisplayed() => SelType.Displayed;

        public string GetSelTypeValue() => SelType.GetValue();

        public bool IsTxtTagsDisplayed() => TxtTags.Displayed;

        public List<string> GetLblTagsValues()
        {
            List<string> tagsList = new();
            foreach (IWebElement item in LblTags)
            {
                tagsList.Add(item.Text);
            }
            return tagsList;
        }

        public CDSCreateStructurePage EnterNameValue(string name)
        {
            Clear(TxtName);
            TxtName.SendKeys(name);
            return this;
        }

        public CDSCreateStructurePage EnterDescriptionValue(string description)
        {
            Clear(TxtDescription);
            TxtDescription.SendKeys(description);
            return this;
        }

        public CDSCreateStructurePage ClickSldPrivate()
        {
            SldPrivate.Click();
            return this;
        }

        public CDSCreateStructurePage EnterTagValue(string tag)
        {
            TxtTags.SendKeys(tag);
            TxtTags.SendKeys(Keys.Enter);
            return this;
        }

        public CDSEditStructurePage ClickBtnSave()
        {
            BtnSave.Click();
            return new CDSEditStructurePage(this.TestObject);
        }

        public void SelectSelTypeFirstValue()
        {
            SelType.Click();
            GetSelOptionByText(ListType[0]).Click();
        }

        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return this.TxtName.Displayed;
        }

        public void SelectSelPrivacyTypeFirstValue()
        {
            SelPrivacyType.Click();
            GetSelOptionByText(ListPrivacyType[0]).Click();
        }
    }
}

