using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using System;
using Data.API;

namespace Tests.API.ETL.Transactions
{
    [TestClass]
    public class ETLPutTransactionsComplete : ETLBaseTest
    {
        private CompleteTransactionAndTriggerProcedureCommandRequest _body;
        private StartTransactionWithFilesCommandRequest _bodyPost;
        private Guid _transactionId;

        [TestInitialize]
        public async Task GenerateRequestBody()
        {
            Guid _procedureId = await GetRandomProcedureIdAsync();
            _body = GeneratePutTransactionsCompleteBody(_procedureId);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("271434 | PUT /transactions/{id}/complete - correct parameters")]
        public async Task PutTransactionsCompleteCorrectParametersTest()
        {
            _bodyPost = await GeneratePostTransactionsStartwithfilesBody();
            var resultPost = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_bodyPost));
            _transactionId = resultPost.TransactionId;

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_CompleteAsync(_transactionId, _body));
            result.Should().NotBeNull();

            var resultGet = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_GetDetailsAsync(_transactionId));
            VerifyTransactionFields(resultGet, _transactionId, "InformationCollected");
        }

        [TestMethod]
        [Description("271435 | PUT /transactions/{id}/complete - not existing transaction ID")]
        public async Task PutTransactionsCompleteNotExistingTransactionIdTest()
        {
            _transactionId = Guid.Parse(Config._notExistingGuid);

            var result = await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Transactions_CompleteAsync(_transactionId, _body));
        }

        [TestMethod]
        [Description("271436 | PUT /transactions/{id}/complete - not existing procedure ID")]
        public async Task PutTransactionsCompleteNotExistingProcedureIdTest()
        {
            _bodyPost = await GeneratePostTransactionsStartwithfilesBody();
            var resultPost = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_StartTransactionWithFilesAsync(_bodyPost));
            _transactionId = resultPost.TransactionId;
            _body.ProcedureId = Guid.Parse(Config._notExistingGuid);

            var result = await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Transactions_CompleteAsync(_transactionId, _body));
        }

        [TestMethod]
        [Description("271437 | PUT /transactions/{id}/complete - unauthorized request")]
        public async Task PutTransactionsCompleteWithoutTokenTest()
        {
            _transactionId = Guid.Parse(Config._notExistingGuid);
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Transactions_CompleteAsync(_transactionId, _body));
        }

        private static CompleteTransactionAndTriggerProcedureCommandRequest GeneratePutTransactionsCompleteBody(Guid procedureId)
        {
            var _body = new CompleteTransactionAndTriggerProcedureCommandRequest()
            {
                ProcedureId = procedureId
            };

            return _body;
        }
    }
}