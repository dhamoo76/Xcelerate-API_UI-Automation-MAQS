using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;

namespace Tests.API.ETL.Clients
{
    [TestClass]
    public class ETLGetClientsPermissions : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("256158 | Get client permissions #smoke")]
        public async Task GetClientPermissionsTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Clients_GetPermissionsForClientAsync(Constants.EtlClientId));
            result.Should().NotBeNull();
        }

        [TestMethod]
        [Description("256159 | Check wrong MDM Client ID response")]
        public async Task GetClientPermissions_WrongClientIdTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Clients_GetPermissionsForClientAsync(Constants.NonExistingClientId));
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("256160 | Get one client permissions - unauthorized request")]
        public async Task GetClientPermissionsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Clients_GetPermissionsForClientAsync(Constants.EtlClientId));

        }
    }
}