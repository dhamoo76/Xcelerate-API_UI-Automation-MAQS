using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class CEMLandingPage : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        private const string DashboardsCSS = "div[class^=\"ChartTitle_root_\"]"; // WHAT TO DO WITH LISTS
        private const string DashboardName = "*[div.p-card-content div[id]]"; //SAME AS ID?
        private const string DashboardTotal = "[*span[class^=\"GroupedChart_total_\"]]";
        private const string DashboardMenuButton = "*[span.pi]";
        private const string DashboardMenuPopUp = "div.p-menu";
        private const string DashboardMenuOption = "*[li.p-menuitem]";
        private const string CurrentUserLogoCSS = "button[aria-label=\"account of current user\"]";

        private IWebElement DashBoard => this.GetElementByCSS(DashboardsCSS);
        private IWebElement CurrentUserLogo => this.GetElementByCSS(CurrentUserLogoCSS);
        private ICollection<IWebElement> DashBoardsCollection => this.GetElementsByCSS(DashboardsCSS);


        /// <summary>
        /// Initializes a new instance of the <see cref="CEMLandingPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public CEMLandingPage(ISeleniumTestObject testObject) : base(testObject)
        {

        }

        public int DashboardsCount()
        {
            int a = DashBoardsCollection.Count;
            return a;
        }

        public override bool IsPageLoaded()
        {
            return this.CurrentUserLogo.Displayed;
        }

    }
}

