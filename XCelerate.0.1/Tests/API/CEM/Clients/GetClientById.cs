using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Clients
{
    [TestClass]
    public class GetClientById : CEMBaseTest
    {
        [TestMethod]
        [Description("238770 | Get one client data")]
        public async Task GetClientById_WhenClientExists_ReturnsClientTest()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetByMdmIdAsync(Constants.CemClientId));
            result.Should().NotBeNull();

            Assert.IsNotNull(result.MdmClientId, "Mdmclientid should not be empty");
            Assert.IsNotNull(result.MdmMasterClientId, "MdmMasterClientId should not be empty");
            Assert.IsNotNull(result.Name, "Name should not be empty");
            Assert.AreEqual(result.MdmClientId, Constants.CemClientId, "Wrong MDMClientId");
        }

        [TestMethod]
        [Description("238771 | Check wrong MDM Client ID response")]
        public async Task GetClientById_WhenClientDoesNotExist_ReturnsNotFoundTest()
        {
            await CEMHttpResponseHelper.VerifyNotFoundAsync(() => CemUserClient.Clients_GetByMdmIdAsync(Constants.NonExistingClientId));
        }

        [TestMethod]
        [Description("238772 | Get one client data - unauthorized request")]
        public async Task GetClientById_WithoutTokenTest()
        {
            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Clients_GetByMdmIdAsync(Constants.CemClientId));
        }

    }
}
