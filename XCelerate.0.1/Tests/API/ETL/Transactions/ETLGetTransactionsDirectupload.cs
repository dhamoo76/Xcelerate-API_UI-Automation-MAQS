using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.ETL.Transactions
{
    [TestClass]
    public class ETLGetTransactionsDirectupload : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("271446 | GET /transactions/directupload - verify all results response")]
        public async Task GetAllTransactionsDirectuploadTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_GetEtlUploaderTransactionsAsync());
            result.Should().NotBeNull();
            VerifyTransactionsDirectuploadFields(result.Results);
        }

        [TestMethod]
        [Description("271447 | GET /transactions/directupload - unauthorized request")]
        public async Task GetAllTransactionsDirectuploadWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Transactions_GetEtlUploaderTransactionsAsync());
        }

        private static void VerifyTransactionsDirectuploadFields(ICollection<TransactionDetailsDto> transactionDetailsResult)
        {
            foreach (var item in transactionDetailsResult)
            {
                VerifyTransactionsDirectuploadFields(item);
            }
        }

        private static void VerifyTransactionsDirectuploadFields(TransactionDetailsDto transactionDetailsActualResult)
        {
            Assert.IsNotNull(transactionDetailsActualResult.TransactionId, "TransactionId should not be empty");
            Assert.IsNotNull(transactionDetailsActualResult.StartDate, "StartDate should not be empty");
            Assert.IsNotNull(transactionDetailsActualResult.Status, "Status should not be empty");
            Assert.IsNotNull(transactionDetailsActualResult.ProcedureName, "ProcedureName should not be empty");
        }
    }
}