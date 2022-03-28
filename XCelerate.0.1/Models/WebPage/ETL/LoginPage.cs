using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class LoginPage : BaseSeleniumPageModel
    {
        /// <summary>
        /// The page url
        /// </summary>
        private string PageUrl = SeleniumConfig.GetWebSiteBase();
        private static string browserName = SeleniumConfig.GetBrowserName();
        private const string UsernameField = "#idp-discovery-username";
        private const string PasswordField = "#okta-signin-password";
        private const string NextButtonElement = "#idp-discovery-submit";
        private const string SignButtonElement = "#okta-signin-password";
        private const string MicrosoftEmailField = "#i0116";
        private const string MicrosoftNextButton = "#idSIButton9";
        private const string rsmUsernameField = "#userNameInput";
        private const string rsmPasswordField = "#passwordInput";
        private const string rsmSIgnInButton = "#submitButton";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        /// <param name="testObject">The test object</param>
        public LoginPage(ISeleniumTestObject testObject) : base(testObject)
        {
        }

        /// <summary>
        /// Gets user name box
        /// </summary>
        private LazyElement UserNameInput
        {
            get { return this.GetLazyElement(By.CssSelector(UsernameField), "User name input"); }
        }

        /// <summary>
        /// Gets password box
        /// </summary>
        private LazyElement PasswordInput
        {
            get { return this.GetLazyElement(By.CssSelector(PasswordField), "Password input"); }
        }

        private LazyElement SigninButton
        {
            get { return this.GetLazyElement(By.CssSelector(SignButtonElement), "Login button"); }
        }

        private LazyElement NextButton
        {
            get { return this.GetLazyElement(By.CssSelector(NextButtonElement), "Next button"); }
        }

        private LazyElement MicrosoftEmailFieldElement
        {
            get { return this.GetLazyElement(By.CssSelector(MicrosoftEmailField), "Microsoft Email Field"); }
        }

        private LazyElement MicrosoftNextButtonElement
        {
            get { return this.GetLazyElement(By.CssSelector(MicrosoftNextButton), "Microsoft next button"); }
        }

        private LazyElement RsmUsernameFieldElement
        {
            get { return this.GetLazyElement(By.CssSelector(rsmUsernameField), "RSM username field"); }
        }

        private LazyElement RsmPasswordFieldElement
        {
            get { return this.GetLazyElement(By.CssSelector(rsmPasswordField), "RSM password field"); }
        }

        private LazyElement RsmSIgnInButtonElement
        {
            get { return this.GetLazyElement(By.CssSelector(rsmSIgnInButton), "RSM Sign In button"); }
        }

        /// <summary>
        /// Open the login page
        /// </summary>
        public LoginPage OpenLoginPage(string stream="")
        {
            var _env = "env";
            if (!string.IsNullOrEmpty(stream)) 
            {
                
                PageUrl = PageUrl.Replace("cem", _env).Replace("etl", _env).Replace("cds", _env);
                PageUrl = PageUrl.Replace(_env, stream);
            }
            this.TestObject.WebDriver.Navigate().GoToUrl(PageUrl);
            return this;
        }


        /// <summary>
        /// Enter the use credentials
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        public LoginPage EnterCredentials(string userName)
        {
            this.UserNameInput.SendKeys(userName);
            return this;
        }

        /// <summary>
        /// Enter the use credentials and log in - Navigation sample
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns>The home page</returns>
        public ETLPayloadsView LoginWithValidCredentialsETL(string userName, string password)
        {
            EnterCredentials(userName);
            NextButton.Click();
            if (browserName == "Chrome")
            {
                MicrosoftEmailFieldElement.SendKeys(userName);
                MicrosoftNextButtonElement.Click();
                RsmUsernameFieldElement.Clear();
                RsmUsernameFieldElement.SendKeys(userName);
                RsmPasswordFieldElement.SendKeys(password);
                RsmSIgnInButtonElement.Click();
            }

            return new ETLPayloadsView(this.TestObject);
        }
        public CEMLandingPage LoginWithValidCredentialsCEM(string userName)
        {
            this.EnterCredentials(userName);
            this.NextButton.Click();
            this.WebDriver.Wait().ForClickableElement(By.CssSelector("div.p-chart"));//Locator will be moved when method will be moved
                                                                                     // this.WebDriver.Wait().ForClickableElement(By.CssSelector("a[href=\"/clients\"]"));
            return new CEMLandingPage(this.TestObject);
        }

        public CDSManageListPage LoginWithValidCredentialsCDS(string userName)
        {
            this.EnterCredentials(userName);
            this.NextButton.Click();

            return new CDSManageListPage(this.TestObject);
        }


        /// <summary>
        /// Check if the home page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return this.UserNameInput.Displayed && this.PasswordInput.Displayed && this.SigninButton.Displayed;
        }
    }
}