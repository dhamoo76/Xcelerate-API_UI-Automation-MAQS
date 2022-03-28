using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Models
{
    /// <summary>
    /// Config data class for ETL UI tests
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Config data for tests
        /// </summary>

        public const string _loginUsername = "___@rsmus.com";
        public const string _loginPassword = "";
        public const string _userSurname = "Artem";

        public const string _clientId = "6557155";
        public const string _clientName = "Adaptics Creatures Inc.";
        public const string _procedure = "new0";
        public const string _solution = "Admin Center";
        public const string _workflow = "CDS List Process";
    }
}