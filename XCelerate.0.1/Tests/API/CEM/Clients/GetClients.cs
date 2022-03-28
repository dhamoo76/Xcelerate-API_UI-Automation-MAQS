using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Clients
{
    [TestClass]
    public class GetClients : CEMBaseTest
    {
        [TestMethod]
        [Description("202522 | Get all clients")]
        public async Task GetAllClientsAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetAllAsync());
            result.Should().NotBeNull();
            this.VerifyCEMClientsFieldsCollection(result.Results);
        }

        [TestMethod]
        [Description("238769 | Get all clients - unauthorized request")]
        public async Task GetAllClientsWithoutToken()
        {
            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Clients_GetAllAsync());
        }

        private void VerifyCEMClientsFieldsCollection(ICollection<ClientDto> results)
        {
            foreach (var item in results)
            {
                Assert.IsNotNull(item.MdmClientId, "Mdmclientid should not be empty");
                Assert.IsNotNull(item.MdmMasterClientId, "MdmMasterClientId should not be empty");
                Assert.IsNotNull(item.Name, "Name should not be empty");
            }
        }

    }

}
