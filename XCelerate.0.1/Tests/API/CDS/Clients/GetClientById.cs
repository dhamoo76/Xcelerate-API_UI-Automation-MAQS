using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities;
using Tests.API.Utilities.Common;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.Clients
{
    [TestClass]
    public class GetClientById : CommonTokenBaseClass
    {
        [TestMethod]
        [Description("202518 | GET  /Client by Id")]
        public async Task GetClientById_WhenClientExists_ReturnsClientTest()
        {
            
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.Clients_GetByIdAsync(Constants.CdsClientId));
           
            TestObject.Log.LogMessage("Start Test");
            
            SoftAssert.Assert(() => Assert.IsNotNull(result.MdmClientId, "Mdmclientid should not be empty"));
            SoftAssert.Assert(() => Assert.IsNotNull(result.MdmMasterClientId, "MdmMasterClientId should not be empty"));
            SoftAssert.Assert(() => Assert.IsNotNull(result.Name, "Name should not be empty"));
            SoftAssert.Assert(() => Assert.IsNotNull(result.ClientStatus, "Name should not be empty"));
            SoftAssert.Assert(() => Assert.AreEqual(result.MdmClientId, Constants.CdsClientId, "Wrong MDMClientId"));
        }

        [TestMethod]
        [Description("202519 | GET  /Client by non existing client id")]
        public async Task GetClientById_WhenClientDoesNotExist_ReturnsNotFoundTest()
        {
            await CdsHttpResponseHelper.VerifyAccessForbiddenAsync(() => CdsUserClient.Clients_GetByIdAsync(Constants.NonExistingClientId));
        }

        [TestMethod]
        [Description("241616 | GET /clients/{id} - check on non authorized request")]
        public async Task GetClientById_WithoutTokenTest()
        {
            await CdsHttpResponseHelper.VerifyUnauthorizedAsync(() => CdsNoTokenClient.Clients_GetByIdAsync(Constants.CdsClientId));

        }
    }
}
