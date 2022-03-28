using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;
using System;

namespace Tests.API.ETL.Protocols
{
    [TestClass]
    public class ETLGetProtocolById : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("219686 | GET /protocols/{id} - correct id")]
        public async Task GetProtocolByIdTest()
        {
            Guid randomProtocolId = await GetRandomProtocolIdAsync();
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetAsync(randomProtocolId));
            result.Should().NotBeNull();
            VerifyProtocolFields(result, randomProtocolId.ToString());
        }

        [TestMethod]
        [Description("219687 | GET /protocols/{id} - incorrect id")]
        public async Task GetProtocolByNotExistingIdTest()
        {
            await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Protocols_GetAsync(Guid.Parse(Config._notExistingGuid)));
        }

        [TestMethod]
        [Description("305679 | GET /protocols/{id} - unauthorized request")]
        public async Task GetProtocolByIdWithoutTokenTest()
        {
            Guid protocolId = Guid.Parse(Config._notExistingGuid);
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_GetAsync(protocolId));
        }

        private static void VerifyProtocolFields(GetProtocolByIdQueryResponse projectResult, string protocolId = "", string mdmClientId = "")
        {
            VerifyProtocolFields(projectResult);
            if (protocolId != "")
                VerifyProtocolId(projectResult, protocolId);
            if (mdmClientId != "")
                VerifyProtocolMdmClientId(projectResult, mdmClientId);
        }

        private static void VerifyProtocolFields(GetProtocolByIdQueryResponse protocolActualResult)
        {
            Assert.IsNotNull(protocolActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(protocolActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(protocolActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(protocolActualResult.ProtocolTypeId, "ProtocolTypeId should not be empty");
            if (protocolActualResult.Properties.Count != 0)
            {
                foreach (var item in protocolActualResult.Properties)
                {
                    VerifyProtocolProperties(item);
                }
            }
        }

        private static void VerifyProtocolProperties(PropertyDto propertyActualResult)
        {
            Assert.IsNotNull(propertyActualResult.Name, "Property Name should not be empty");
            Assert.IsNotNull(propertyActualResult.Value, "Property Value should not be empty");
        }


        private static void VerifyProtocolMdmClientId(GetProtocolByIdQueryResponse protocolActualResult, string mdmClientId)
        {
            Assert.AreEqual(mdmClientId, protocolActualResult.MdmClientId.ToString(), "Wrong MDM Client ID in the response.");
        }

        private static void VerifyProtocolId(GetProtocolByIdQueryResponse protocolActualResult, string protocolId)
        {
            Assert.AreEqual(protocolId, protocolActualResult.Id.ToString(), "Wrong Protocol ID in the response.");
        }
    }
}