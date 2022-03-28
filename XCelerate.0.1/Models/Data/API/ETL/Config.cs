using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Data.API
{
    /// <summary>
    /// Config data class for ETL UI tests
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Config data for tests
        /// </summary>

        public const string _mdmClientId = "6557155";
        public const string _clientName = "Adaptics Creatures Inc.";
        public const string _protocolTypeId = "00000000-0000-0000-0000-000000000001";
        public const string _protocolTypeName = "Internal SFTP";
        public const string _fileType = "XLSX";

        public const string _notExistingMdmClientId = "1234567";
        public const string _notExistingGuid = "00000000-0000-0000-0000-000000012345";

        // TODO: Refactor test data state to better encapsulate data.

        /// <summary>
        /// Name for the protocol under test.
        /// </summary>
        public string protocolName = "Test Protocol 304003 ";
        /// <summary>
        /// Folder path for the protocol under test.
        /// </summary>
        public string folderPath = "Test304003Payload";
        /// <summary>
        /// Internal SFTP url for the protocol under test.
        /// </summary>
        public string internalSftpUrl = "https://qa-xceleratesftp.rsmus.com";
        /// <summary>
        /// Protocol GUID for the protocol under test.
        /// </summary>
        public Guid protocolGuid;

        /// <summary>
        /// Payload name for the payload under test.
        /// </summary>
        public string payloadName = "Test Payload 304003 ";
        ///<summary>
        /// List of engagements for the payload under test.
        ///</summary>
        public List<Guid> payloadEngagementIds = new List<Guid> { new("820a5deb-7472-473a-80a0-0c62187fe92e") };
        ///<summary>
        /// List of projects for the payload under test.
        ///</summary>
        public List<Guid> payloadProjectIds = new List<Guid> { new("1b126573-86f5-42bd-8c51-1bae921801e1") };
        ///<summary>
        /// List of legal entities for the payload under test.
        ///</summary>
        public List<Guid> payloadLegalEntityIds = new List<Guid> { new("1dac4cad-cc0c-472f-59af-08d9c07a2363") };
        /// <summary>
        /// ID for the procedure for the payload under test.
        /// </summary>
        public Guid payloadProcedureId = new Guid("a06c5e9a-db5f-4b56-b1f9-c447a71a66e2");
        /// <summary>
        /// ID For the file for the payload under test.
        /// </summary>
        public Guid payloadFileId = new Guid("e13aa028-4cea-4e36-880d-421e08f7b51a");
        /// <summary>
        /// Name of file to be supported by the payload. Wildcards are supported. 
        /// </summary>
        public string payloadFilenamePattern = "legalentities*.xlsx";

        /// <summary>
        /// RSM ETL username.
        /// </summary>
        public string username = "";

        /// <summary>
        /// MDM client ID for the client under test.
        /// </summary>
        public readonly int mdmClientId = int.Parse(_mdmClientId);
        public const string _notExistingFileType = "SD324DWW";
    }
}