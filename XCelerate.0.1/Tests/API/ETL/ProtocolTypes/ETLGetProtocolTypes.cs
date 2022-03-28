using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;

namespace Tests.API.ETL.ProtocolTypes
{
    [TestClass]
    public class ETLGetProtocolTypes : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("222737 | GET /protocolTypes #smoke")]
        public async Task GetAllProtocolTypesTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.ProtocolTypes_GetAsync());
            result.Should().NotBeNull();
            VerifyProtocolTypesFields(result.Results);
        }

        [TestMethod]
        [Description("293184 | Get protocolTypes - unauthorized request")]
        public async Task GetAllProtocolTypesWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.ProtocolTypes_GetAsync());
        }

        private static void VerifyProtocolTypesFields(ICollection<ProtocolTypeDto> protocolTypeResult)
        {
            foreach (var item in protocolTypeResult)
            {
                VerifyProtocolTypesFields(item);
            }
        }

        private static void VerifyProtocolTypesFields(ProtocolTypeDto protocolTypeActualResult)
        {
            Assert.IsNotNull(protocolTypeActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(protocolTypeActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(protocolTypeActualResult.Disabled, "Disabled flag should not be empty");
            Assert.IsNotNull(protocolTypeActualResult.Properties, "Response should contain Properties table");
        }
    }
}