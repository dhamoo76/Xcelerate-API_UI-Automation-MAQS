using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.ETL.Clients
{
    [TestClass]
    public class ETLGetClients : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("256156 | Get all clients #smoke")]
        public async Task GetAllClientsTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Clients_GetClientsListAsync());
            result.Should().NotBeNull();
            VerifyEtlClientsFields(result.Results);
        }

        [TestMethod]
        [Description("256157 | Get all clients - unauthorized request")]
        public async Task GetAllClientsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Clients_GetClientsListAsync());
        }

        private static void VerifyEtlClientsFields(ICollection<ClientDto> clientResult)
        {
            foreach (var item in clientResult)
            {
                VerifyEtlClientsFields(item);
            }
        }

        private static void VerifyEtlClientsFields(ClientDto clientActualResult, int expectedMdmClientId = 0)
        {
            Assert.IsNotNull(clientActualResult.MdmClientId, "MdmClientId should not be empty");
            if (expectedMdmClientId != 0)
                Assert.AreEqual(clientActualResult.MdmClientId, expectedMdmClientId);

            Assert.IsNotNull(clientActualResult.MdmMasterClientId, "MdmMasterClientId should not be empty");
            Assert.IsNotNull(clientActualResult.Name, "Name should not be empty");
        }
    }
}