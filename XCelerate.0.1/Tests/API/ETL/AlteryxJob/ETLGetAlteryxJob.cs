using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.ETL.AlteryxJob
{
    [TestClass]
    public class ETLGetAlteryxJob : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("232899 | GET /AlteryxJob - get all AlteryX jobs")]
        public async Task GetAllAlteryxJobsTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXJob_GetListAsync());
            result.Should().NotBeNull();
            VerifyAlteryxJobsFields(result.Results);
        }

        [TestMethod]
        [Description("310779 | GET /AlteryxJob - filter by valid AlteryxId")]
        public async Task GetAllAlteryxJobsFilterByAlteryxIdTest()
        {
            string randomAlteryxId = await GetRandomAlteryxIdAsync();
            string filterQuery = "alteryxId eq '" + randomAlteryxId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXJob_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyAlteryxJobsFields(result.Results, randomAlteryxId);
        }

        [TestMethod]
        [Description("310780 | GET /AlteryxJob - filter by empty AlteryxId")]
        public async Task GetAllProtocolsFilterByEmptyAlteryxIdTest()
        {
            string filterQuery = "alteryxId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.AlteryXJob_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("310781 | GET /AlteryxJob - filter by null AlteryxId")]
        public async Task GetAllProtocolsFilterByNullAlteryxIdTest()
        {
            string filterQuery = "alteryxId eq null";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXJob_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("310782 | GET /AlteryxJob - filter by not existing AlteryxId")]
        public async Task GetAllProtocolsFilterByNotExistingAlteryxIdTest()
        {
            string randomAlteryxId = GenerateRandomString(15);
            string filterQuery = "alteryxId eq '" + randomAlteryxId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXJob_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("310725 | GET /AlteryxJob - unauthorized request")]
        public async Task GetAllAlteryxJobsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.AlteryXJob_GetListAsync());
        }

        private static void VerifyAlteryxJobsFields(ICollection<AlteryXJobDto> alteryxJobResult, string alteryxId = "")
        {
            foreach (var item in alteryxJobResult)
            {
                VerifyAlteryxJobsFields(item);
                if(alteryxId != "")
                    VerifyAlteryxId(item, alteryxId);
            }
        }

        private static void VerifyAlteryxJobsFields(AlteryXJobDto alteryxJobActualResult)
        {
            Assert.IsNotNull(alteryxJobActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(alteryxJobActualResult.AlteryXJobId, "AlteryXJobId should not be empty");
            Assert.IsNotNull(alteryxJobActualResult.TransactionPath, "TransactionPath flag should not be empty");
        }

        private static void VerifyAlteryxId(AlteryXJobDto alteryxJobActualResult, string alteryxId)
        {
            Assert.AreEqual(alteryxId, alteryxJobActualResult.AlteryXJobId, "AlteryXJobId is not correct");
        }
    }
}