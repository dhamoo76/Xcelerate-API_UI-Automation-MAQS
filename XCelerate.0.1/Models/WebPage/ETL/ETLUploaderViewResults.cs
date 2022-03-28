using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class ETLUploaderViewResults : BasePage
    {
        /// <summary>
        /// The list of elements
        /// </summary>

        //elements
        public const string uploadFileTab = "div[aria-label=\"UPLOAD FILE\"]";
        public const string viewResultsTab = "div[aria-label=\"VIEW RESULTS\"]";
        public const string pageTitle = "";
        public const string refreshButton = "[e2e-id=\"RefreshButton\"]";
        public const string dateUploadedHeader = "";
        public const string procedureNameHeader = "";
        public const string procedureStatusHeader = "";
        public const string outputFilesHeader = "";
        public const string searchByProcedureNameInput = "";
        public const string searchByProcedureStatusInput = "";
        public const string searchByOutputFilesInput = "";
        public const string uploadStatusNotificationBar = "notification-snackbar";
        public const string uploadedFileStatus = "table.e2e-uploader-table > tbody > tr:nth-child(1) > td:nth-child(3) > div";
        public const string uploadStatusTable = "table.e2e-uploader-table";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public ETLUploaderViewResults(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public LazyElement UploadFileTabElement => GetLazyElement(By.CssSelector(uploadFileTab), "{Upload File Tab}");
        public LazyElement ViewResultsTabElement => GetLazyElement(By.CssSelector(viewResultsTab), "{View Results Tab}");
        public LazyElement RefreshButtonElement => GetLazyElement(By.CssSelector(refreshButton), "{Refresh Button}");
        public LazyElement UploadNotificationSnackBarElement => GetLazyElementByCSSe2e(uploadStatusNotificationBar, "Files Successfully Uploaded Notification");
        public LazyElement UploadStatusTableElement => GetLazyElement(By.CssSelector(uploadStatusTable), "{Upload Status Table}");
        public LazyElement UploadedFileStatus => GetLazyElement(By.CssSelector(uploadedFileStatus), "{Uploaded File Status}");

        public override bool IsPageLoaded() => UploadStatusTableElement.Displayed;

        public bool IsUploadFileTabDisplayed() => UploadFileTabElement.Displayed;
        public bool IsViewResultsTabDisplayed() => ViewResultsTabElement.Displayed;
        public bool IsRefreshButtonDisplayed() => RefreshButtonElement.Displayed;
        public bool IsUploadStatusTableDisplayed() => UploadStatusTableElement.Displayed;
    }
}