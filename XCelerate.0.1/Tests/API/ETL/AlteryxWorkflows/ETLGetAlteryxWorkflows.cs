using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.ETL.AlteryxWorkflows
{
    [TestClass]
    public class ETLGetAlteryxWorkflows : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("240046 | GET /AlteryxWorkflows - get all workflows")]
        public async Task GetAllAlteryxWorkflowsTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXWorkflows_GetWorkflowsAsync());
            result.Should().NotBeNull();
            VerifyAlteryxWorkflowsFields(result.Results);
        }

        [TestMethod]
        [Description("309398 | GET /AlteryxWorkflows - unauthorized request")]
        public async Task GetAllAlteryxWorkflowsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.AlteryXWorkflows_GetWorkflowsAsync());
        }

        private static void VerifyAlteryxWorkflowsFields(ICollection<AlteryxWorkflowListDto> alteryxWorkflowsResult)
        {
            foreach (var item in alteryxWorkflowsResult)
            {
                VerifyAlteryxWorkflowsFields(item);
            }
        }

        private static void VerifyAlteryxWorkflowsFields(AlteryxWorkflowListDto alteryxWorkflowsActualResult)
        {
            Assert.IsNotNull(alteryxWorkflowsActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(alteryxWorkflowsActualResult.Name, "Name should not be empty");
        }
    }
}