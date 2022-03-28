using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using System;
using Data.API;
using System.Linq;

namespace Tests.API.ETL.Transactions
{
    [TestClass]
    public class ETLPostTransactionsStartwithfiles : ETLBaseTest
    {
        private StartTransactionWithFilesCommandRequest _body;
        
        [TestInitialize]
        public async Task GenerateRequestBody()
        {
            _body = await GeneratePostTransactionsStartwithfilesBody();
        }
        
        [TestMethod, TestCategory("Smoke")]
        [Description("271420 | POST /Transactions/startwithfiles - start transaction with correct parameters")]
        public async Task PostTransactionsStartwithfilesCorrectParametersTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
            result.Should().NotBeNull();
            VerifyPostTransactionsStartwithfilesResponse(result, _body);
        }
        
        [TestMethod]
        [Description("271421 | POST /Transactions/startwithfiles - null applicationId")]
        public async Task PostTransactionsStartwithfilesNullApplicationIdTest()
        {
            _body.ApplicationId = null;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }
        
        [TestMethod]
        [Description("271422 | POST /Transactions/startwithfiles - empty applicationId")]
        public async Task PostTransactionsStartwithfilesEmptyApplicationIdTest()
        {
            _body.ApplicationId = "";
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }
        
        [TestMethod]
        [Description("271423 | POST /Transactions/startwithfiles - wrong procedureId")]
        public async Task PostTransactionsStartwithfilesNotExistingProcedureIdTest()
        {
            _body.ProcedureId = Guid.Parse(Config._notExistingGuid);
            var result = await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271425 | POST /Transactions/startwithfiles - empty procedureId")]
        public async Task PostTransactionsStartwithfilesEmptyProcedureIdTest()
        {
            _body.ProcedureId = Guid.Empty;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }
        
        [TestMethod]
        [Description("271426 | POST /Transactions/startwithfiles - empty files table")]
        public async Task PostTransactionsStartwithfilesEmptyFilesTableTest()
        {
            ICollection<FileForFileTypeDto> files = new List<FileForFileTypeDto> { };
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }
        
        [TestMethod]
        [Description("271427 | POST /Transactions/startwithfiles - one file missing for transaction")]
        public async Task PostTransactionsStartwithfilesWithoutOneFileTest()
        {
            var files = _body.Files.ToList();
            if(files.Any())
                files.RemoveAt(files.Count - 1);
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271428 | POST /Transactions/startwithfiles - wrong fileId in files table")]
        public async Task PostTransactionsStartwithfilesWrongFileIdTest()
        {
            var files = _body.Files.ToList();
            files[0].FileId = Guid.Parse(Config._notExistingGuid);
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271430 | POST /Transactions/startwithfiles - empty fileId in files table")]
        public async Task PostTransactionsStartwithfilesEmptyFileIdTest()
        {
            var files = _body.Files.ToList();
            files[0].FileId = Guid.Empty;
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271431 | POST /Transactions/startwithfiles - null fileName in files table")]
        public async Task PostTransactionsStartwithfilesNullFileNameTest()
        {
            var files = _body.Files.ToList();
            files[0].FileName = null;
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271432 | POST /Transactions/startwithfiles - empty fileName in files table")]
        public async Task PostTransactionsStartwithfilesEmptyFileNameTest()
        {
            var files = _body.Files.ToList();
            files[0].FileName = "";
            _body.Files = files;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        [TestMethod]
        [Description("271433 | POST /Transactions/startwithfiles - unauthorized request")]
        public async Task PostTransactionsStartwithfilesWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Transactions_StartTransactionWithFilesAsync(_body));
        }

        private static void VerifyPostTransactionsStartwithfilesResponse(StartTransactionWithFilesCommandResponse transactionActualResult, StartTransactionWithFilesCommandRequest requestBody)
        {
            Assert.IsNotNull(transactionActualResult.TransactionId, "Transaction Id should not be empty");
            Assert.AreEqual(requestBody.Files.Count, transactionActualResult.UploadUrls.Count, "Incorrect files number in the response");
            
            int index = 0;
            foreach(var item in transactionActualResult.UploadUrls)
            {
                VerifyTransactionFilesData(item, requestBody, index);
                index++;
            }
        }

        public static void VerifyTransactionFilesData(UploadUrlForFileDto fileActualResult, StartTransactionWithFilesCommandRequest requestBody, int index)
        {
            var requestFilesList = requestBody.Files.ToList();
            Assert.IsNotNull(fileActualResult.FileId, "File ID should not be null");
            Assert.IsNotNull(fileActualResult.UploadUrl, "Upload URL should not be null");
            Assert.AreEqual(requestFilesList[index].FileId, fileActualResult.FileId, "File ID is not correct in the response");
        }
    }
}