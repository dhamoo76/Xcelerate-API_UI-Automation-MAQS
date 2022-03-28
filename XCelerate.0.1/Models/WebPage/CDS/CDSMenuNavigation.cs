using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CDSMenuNavigation : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>
        /// RSMLLOGO
        

        /// <summary>
        /// Initializes a new instance of the <see cref="CDSManageListPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CDSMenuNavigation(ISeleniumTestObject testObject) : base(testObject)
        {
            
        }

        
        /// Add button element
        
        public override bool IsPageLoaded()
        {
            return true;
        }

        public bool IsIcnRsmCdsLogoDisplayed() => IcnRsmCdsLogo.Displayed;

        public bool IsMnuManageListDisplayed() => MnuManageList.Displayed;

        public bool IsBtnUserHeaderDisplayed() => BtnUserHeader.Displayed;

        public bool IsBtnBellHeaderDisplayed() => BtnBellHeader.Displayed;
    }
}

