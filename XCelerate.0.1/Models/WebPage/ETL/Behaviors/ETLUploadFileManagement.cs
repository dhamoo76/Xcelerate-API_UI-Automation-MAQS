using System;
using Magenic.Maqs.BaseSeleniumTest;

namespace Models.WebPage.ETL.Behaviors
{
    public class ETLUploadFileManagement : BasePage
    {
        private ETLUploaderUploadFile ETLUploaderPage;
        private ETLUploaderViewResults ETLUploaderViewResultsPage;

        public ETLUploadFileManagement(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        public ETLUploaderUploadFile GoToETLUploadFilePage()
        {
            ETLPayloadsView page = new ETLPayloadsView(TestObject);
            ETLUploaderPage = page.ClickLnkUploadFile();
            return ETLUploaderPage;
        }

        public ETLUploaderUploadFile UploadFiles(string fileName)
        {
            if (ETLUploaderPage.UploadFileInput.Exists)
            {
                try
                {
                    ETLUploaderPage.UploadFileInput.SendKeys(fileName);
                }
                catch (TimeoutException e)
                {
                    // Due to the way PrimeReact updates the DOM after the SendKeys event, this call will throw a TimeoutException,
                    // even though the operation completed successfully.
                }
            }

            return new ETLUploaderUploadFile(this.TestObject);
        }

        public ETLUploaderViewResults ClickSubmitBtn()
        {
            SubmitButton.Click();
            ETLUploaderViewResultsPage = new ETLUploaderViewResults(TestObject);
            return ETLUploaderViewResultsPage;
        }

        public bool UploadedFileStatusChange()
        {
            ETLUploaderViewResultsPage.RefreshButtonElement.Click();
            return ETLUploaderViewResultsPage.UploadedFileStatus.Text == "Successful";
        }
    }
}
