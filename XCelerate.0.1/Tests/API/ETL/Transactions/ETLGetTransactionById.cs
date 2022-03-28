using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.Transactions
{
    [TestClass]
    public class ETLGetTransactionById : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("271441 | GET /transactions/{id} - get details of existing transaction")]
        public async Task GetTransactionByIdTest()
        {
            Guid randomTransactionId = await GetRandomTransactionIdAsync();
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_GetDetailsAsync(randomTransactionId));
            result.Should().NotBeNull();
            VerifyTransactionFields(result, randomTransactionId);
        }

        [TestMethod]
        [Description("271442 | GET /transactions/{id} - not existing transaction")]
        public async Task GetTransactionByNotExistingIdTest()
        {
            await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Transactions_GetDetailsAsync(Guid.Parse(Config._notExistingGuid)));
        }

        [TestMethod]
        [Description("271443 | GET /transactions/{id} - unauthorized request")]
        public async Task GetTransactionByIdWithoutTokenTest()
        {
            Guid transactionId = Guid.Parse(Config._notExistingGuid);
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Transactions_GetDetailsAsync(transactionId));
        }
    }
}