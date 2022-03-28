using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.MatchedFiles
{
    [TestClass]
    public class ETLGetMatchedFiles : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("209882 | GET /matchedFiles -  Get all matched files without filtering")]
        public async Task GetAllMatchedFilesTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.MatchedFiles_GetTransactionsAsync());
            result.Should().NotBeNull();
            VerifyMatchedFilesFields(result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("258038 | GET /matchedfiles with filter by mdmclientid (correct)")]
        public async Task GetAllMatchedFilesFilterByMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.MatchedFiles_GetTransactionsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyMatchedFilesFields(result.Results, Config._mdmClientId);
        }

        [TestMethod]
        [Description("258042 | GET /matchedfiles with filter by mdmclientid (empty)")]
        public async Task GetAllMatchedFilesFilterByEmptyMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.MatchedFiles_GetTransactionsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("258043 | GET /matchedfiles with filter by mdmclientid (null)")]
        public async Task GetAllMatchedFilesFilterByNullMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.MatchedFiles_GetTransactionsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("258044 | GET /matchedfiles with filter by mdmclientid (not existing)")]
        public async Task GetAllMatchedFilesFilterByNotExistingMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._notExistingMdmClientId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.MatchedFiles_GetTransactionsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("258140 | GET /matchedfiles - unauthorized request")]
        public async Task GetAllMatchedFilesWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.MatchedFiles_GetTransactionsAsync());
        }

        [TestMethod]
        [Description("296749 | GET /matchedfiles with filtering by mdmclientId - unauthorized request")]
        public async Task GetAllMatchedFilesWithFilterWithoutTokenTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.MatchedFiles_GetTransactionsAsync(filter: filterQuery));
        }

        private static void VerifyMatchedFilesFields(ICollection<MatchedFileDto> matchedFilesResult)
        {
            foreach (var item in matchedFilesResult)
            {
                VerifyMatchedFilesFields(item);
            }
        }

        private static void VerifyMatchedFilesFields(ICollection<MatchedFileDto> matchedFilesResult, string mdmClientId)
        {
            foreach (var item in matchedFilesResult)
            {
                VerifyMatchedFilesFields(item);
                VerifyMatchedFilesMdmClientId(item, mdmClientId);
            }
        }

        private static void VerifyMatchedFilesFields(MatchedFileDto matchedFilesActualResult)
        {
            Assert.IsNotNull(matchedFilesActualResult.FileName, "FileName should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.TransactionId, "TransactionId should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.PayloadId, "PayloadId should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.PayloadName, "PayloadName should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.MatchComplete, "MatchComplete flag should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.ProcedureId, "ProcedureId should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.ProcedureName, "ProcedureName should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.ReceivedDate, "ReceivedDate should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.UpdatedDate, "UpdatedDate should not be empty");
            Assert.IsNotNull(matchedFilesActualResult.Status, "Status should not be empty");
        }

        private static void VerifyMatchedFilesMdmClientId(MatchedFileDto matchedFilesActualResult, string mdmClientId)
        {
            Assert.AreEqual(mdmClientId, matchedFilesActualResult.MdmClientId.ToString(), "Wrong MDM Client ID in the response.");
        }
    }
}