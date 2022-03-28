using System.Threading.Tasks;
using Magenic.Maqs.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.Clients
{
    [TestClass]
    public class GetClients : CDSBaseTest
    {
        [TestMethod]
        [Description("218092 | GET  /Clients")]
        public async Task GetAllClientsAsync()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.Clients_GetAllAsync());

            //result.Should().NotBeNull();
            foreach (ClientDto item in result.Results)
            {
                _VerifyClientsFields(item);
            }
        }

        [TestMethod]
        [Description("241615 | GET  /Clients without token")]
        public async Task GetAllClientsWithoutToken()
        {
            await CdsHttpResponseHelper.VerifyUnauthorizedAsync(() => CdsNoTokenClient.Clients_GetAllAsync());
        }

        [TestMethod]
        [Description("223304 | GET  /check on Lost clients where masterid = clientid should not be received")]
        public async Task VerifyLostClients_MasterIdEqualToClientId_Test()
        {
            string lostClientId = "7703458";
            string filterQuery = $"MDMClientId in ({lostClientId})";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsServerClient.Clients_GetAllAsync(filter: filterQuery));
            Assert.AreEqual(0, result.Results.Count, "Lost ClientId should not be returns in the response");
        }

        [TestMethod]
        [Description("223306 | GET  /check on Lost clients where masterid != clientid should not be received")]
        public async Task VerifyLostClients_MasterIdNotEqualToClientId_Test()
        {
            string lostClientId = "7716681";
            string filterQuery = $"MDMClientId in ({lostClientId})";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsServerClient.Clients_GetAllAsync(filter: filterQuery));
            Assert.AreEqual(0, result.Results.Count, "Lost ClientId should not be returns in the response");
        }

        private void _VerifyClientsFields(ClientDto item)
        {
            Assert.IsNotNull(item.MdmClientId, "Mdmclientid should not be empty");
            Assert.IsNotNull(item.MdmMasterClientId, "MdmMasterClientId should not be empty");
            Assert.IsNotNull(item.Name, "Name should not be empty");
            Assert.IsNotNull(item.ClientStatus, "Name should not be empty");
        }
    }
}