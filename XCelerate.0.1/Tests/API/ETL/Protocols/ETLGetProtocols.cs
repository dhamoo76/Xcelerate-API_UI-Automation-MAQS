using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.Protocols
{
    [TestClass]
    public class ETLGetProtocols : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("218478 | GET /Protocols without any filtering (valid)")]
        public async Task GetAllProtocolsWithoutFilteringTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync());
            result.Should().NotBeNull();
            VerifyProtocolsFields(result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207439 | GET /Protocols with filtering by mdmclientId (valid)")]
        public async Task GetAllProtocolsFilterByMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProtocolsFields(result.Results, Config._mdmClientId);
        }

        [TestMethod]
        [Description("306593 | GET /Protocols with filter by mdmclientid = empty")]
        public async Task GetAllProtocolsFilterByEmptyMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306594 | GET /Protocols with filter by mdmclientid = null")]
        public async Task GetAllProtocolsFilterByNullMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306595 | GET /Protocols with filter by mdmclientid = notExistingMdmClientId")]
        public async Task GetAllProtocolsFilterByNotExistingMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._notExistingMdmClientId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207333 | GET /Protocols with filtering by protocolTypeName (valid)")]
        public async Task GetAllProtocolsFilterByProtocolTypeNameTest()
        {
            string filterQuery = "protocolTypeName eq '" + Config._protocolTypeName + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProtocolsFields(result.Results, "", Config._protocolTypeName);
        }

        [TestMethod]
        [Description("306605 | GET /Protocols with filter by protocolTypeName = empty")]
        public async Task GetAllProtocolsFilterByEmptyProtocolTypeNameTest()
        {
            string filterQuery = "protocolTypeName eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306606 | GET /Protocols with filter by protocolTypeName = null")]
        public async Task GetAllProtocolsFilterByNullProtocolTypeNameTest()
        {
            string filterQuery = "protocolTypeName eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306607 | GET /Protocols with filter by protocolTypeName = notExistingProtocolTypeName")]
        public async Task GetAllProtocolsFilterByNotExistingProtocolTypeNameTest()
        {
            string randomProtocolTypeName = GenerateRandomString(5, "ProtocolType");
            string filterQuery = "protocolTypeName eq '" + randomProtocolTypeName + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207438 | GET /Protocols with filtering by protocolTypeId (valid)")]
        public async Task GetAllProtocolsFilterByProtocolTypeIdTest()
        {
            string filterQuery = "protocolTypeId eq '" + Config._protocolTypeId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProtocolsFields(result.Results, "", "", Config._protocolTypeId);
        }

        [TestMethod]
        [Description("306609 | GET /Protocols with filter by protocolTypeId = empty")]
        public async Task GetAllProtocolsFilterByEmptyProtocolTypeIdTest()
        {
            string filterQuery = "protocolTypeId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306610 | GET /Protocols with filter by protocolTypeId = null")]
        public async Task GetAllProtocolsFilterByNullProtocolTypeIdTest()
        {
            string filterQuery = "protocolTypeId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306611 | GET /Protocols with filter by protocolTypeId = notExistingProtocolTypeId")]
        public async Task GetAllProtocolsFilterByNotExistingProtocolTypeIdTest()
        {
            string filterQuery = "protocolTypeId eq '" + Config._notExistingGuid + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("306615 | GET /Protocols without filtering - unauthorized request")]
        public async Task GetAllProtocolsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_GetProtocolsAsync());
        }

        [TestMethod]
        [Description("306616 | GET /Protocols with filtering by mdmclientId - unauthorized request")]
        public async Task GetAllProtocolsFilterByMdmClientIdWithoutTokenTest()
        {
            string filterQuery = "mdmClientId eq " + Config._mdmClientId;
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306617 | GET /Protocols with filtering by protocolTypeName - unauthorized request")]
        public async Task GetAllProtocolsFilterByProtocolTypeNameWithoutTokenTest()
        {
            string filterQuery = "protocolTypeName eq " + Config._protocolTypeName;
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("306618 | GET /Protocols with filtering by protocolTypeId - unauthorized request")]
        public async Task GetAllProtocolsFilterByProtocolTypeIdWithoutTokenTest()
        {
            string filterQuery = "protocolTypeId eq " + Config._protocolTypeId;
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_GetProtocolsAsync(filter: filterQuery));
        }

        private static void VerifyProtocolsFields(ICollection<ProtocolListDto> projectResult)
        {
            foreach (var item in projectResult)
            {
                VerifyProtocolsFields(item);
            }
        }

        private static void VerifyProtocolsFields(ICollection<ProtocolListDto> projectResult, string mdmClientId = "", string protocolTypeName = "", string protocolTypeId = "")
        {
            foreach (var item in projectResult)
            {
                VerifyProtocolsFields(item);
                if(mdmClientId != "")
                    VerifyProtocolMdmClientId(item, mdmClientId);
                if (protocolTypeName != "")
                    VerifyProtocolProtocolTypeName(item, protocolTypeName);
                if (protocolTypeId != "")
                    VerifyProtocolProtocolTypeId(item, protocolTypeId);
            }
        }

        private static void VerifyProtocolsProperties(PropertyDto propertyActualResult)
        {
            Assert.IsNotNull(propertyActualResult.Name, "Property Name should not be empty");
            Assert.IsNotNull(propertyActualResult.Value, "Property Value should not be empty");
        }

        private static void VerifyProtocolsFields(ProtocolListDto protocolActualResult)
        {
            Assert.IsNotNull(protocolActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(protocolActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(protocolActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(protocolActualResult.ProtocolTypeId, "ProtocolTypeId should not be empty");
            Assert.IsNotNull(protocolActualResult.ProtocolTypeName, "ProtocolTypeName should not be empty");
            Assert.IsNotNull(protocolActualResult.PayloadCount, "PayloadCount should not be empty");
            if(protocolActualResult.ProtocolTypeName == "Internal SFTP")
            {
                protocolActualResult.Properties.Should().NotBeEmpty();
            }
            if(protocolActualResult.Properties.Count != 0)
            {
                foreach (var item in protocolActualResult.Properties)
                {
                    VerifyProtocolsProperties(item);
                }
            }
        }

        private static void VerifyProtocolMdmClientId(ProtocolListDto protocolActualResult, string mdmClientId)
        {
            Assert.AreEqual(mdmClientId, protocolActualResult.MdmClientId.ToString(), "Wrong MDM Client ID in the response.");
        }

        private static void VerifyProtocolProtocolTypeName(ProtocolListDto protocolActualResult, string protocolTypeName)
        {
            Assert.AreEqual(protocolTypeName, protocolActualResult.ProtocolTypeName, "Wrong Protocol Type Name in the response.");
        }

        private static void VerifyProtocolProtocolTypeId(ProtocolListDto protocolActualResult, string protocolTypeId)
        {
            Assert.AreEqual(protocolTypeId, protocolActualResult.ProtocolTypeId.ToString(), "Wrong Protocol Type ID in the response.");
        }
    }
}