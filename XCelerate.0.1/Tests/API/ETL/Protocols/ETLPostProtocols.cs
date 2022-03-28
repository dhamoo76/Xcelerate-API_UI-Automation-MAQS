using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using System;
using Data.API;

namespace Tests.API.ETL.Protocols
{
    [TestClass]
    public class ETLPostProtocols : ETLBaseTest
    {
        private CreateProtocolCommandRequest _body;
        private bool clean = false;
        private Guid newProtocolId;

        [TestInitialize]
        public async Task GenerateRequestBody()
        {
            _body = await GeneratePostProtocolsBody();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("212858 | POST /Protocols (valid)")]
        public async Task PostProtocolsCorrectParametersTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
            result.Should().NotBeNull();
            VerifyPostProtocolsResponse(result);
            newProtocolId = result.Id;
            clean = true;

            var resultSavedData = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetAsync(newProtocolId));
            VerifyProtocolData(resultSavedData, _body, newProtocolId);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("212859 | POST /Protocols mandatory fields")]
        public async Task PostProtocolsMandatoryFieldsTest()
        {
            _body.Description = "";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
            result.Should().NotBeNull();
            VerifyPostProtocolsResponse(result);
            newProtocolId = result.Id;
            clean = true;

            var resultSavedData = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetAsync(newProtocolId));
            VerifyProtocolData(resultSavedData, _body, newProtocolId);
        }

        [TestMethod]
        [Description("212860 | POST /Protocols error messages checking - no name provided")]
        public async Task PostProtocolsWithoutNameTest()
        {
            _body.Name = "";
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("212860 | POST /Protocols error messages checking - no mdmClientId provided")]
        public async Task PostProtocolsWithoutMdmClientIdTest()
        {
            _body.MdmClientId = 0;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("306608 | POST /Protocols error messages checking - no protocolTypeId provided")]
        public async Task PostProtocolsWithoutProtocolTypeIdTest()
        {
            _body.ProtocolTypeId = Guid.Empty;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("306613 | POST /Protocols error messages checking - no mandatory property provided")]
        public async Task PostProtocolsWithoutMandatoryPropertyTest()
        {
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> { };
            _body.Properties = properties;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("306614 | POST /Protocols error messages checking - too long name provided")]
        public async Task PostProtocolsLongNameTest()
        {
            _body.Name = GenerateRandomString(50, "");
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("306620 | POST /Protocols error messages checking - too long property provided")]
        public async Task PostProtocolsLongPropertyTest()
        {
            _body.ProtocolTypeId = Guid.Parse(Config._protocolTypeId);
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> { };
            PropertyRequestDto property = new();
            property.Name = "folderPath";
            property.Value = GenerateRandomString(100, "FolderPath");
            properties.Add(property);
            _body.Properties = properties;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("212861 | POST /Protocols error messages checking - not existing mdmclientId provided")]
        public async Task PostProtocolsIncorrectMdmClientIdTest()
        {
            _body.MdmClientId = Int32.Parse(Config._notExistingMdmClientId);
            var result = await EtlHttpResponseHelper.VerifyAccessForbiddenAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
            result.Should().BeNull();
        }

        [TestMethod]
        [Description("306621 | POST /Protocols error messages checking - not existing protocolTypeId provided")]
        public async Task PostProtocolsIncorrectProtocolTypeIdTest()
        {
            _body.ProtocolTypeId = Guid.Parse(Config._notExistingGuid);
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_body));
        }

        [TestMethod]
        [Description("306622 | POST /Protocols - unauthorized request")]
        public async Task PostProtocolsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_CreateProtocolAsync());
        }

        [TestCleanup]
        public async Task TestsCleanup()
        {
            if (clean)
            {
                await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_DeletedByIdAsync(newProtocolId));
            }
            clean = false;
        }

        private static void VerifyPostProtocolsResponse(CreateProtocolCommandResponse protocolActualResult)
        {
            Assert.IsNotNull(protocolActualResult.Id, "Id should not be empty");
        }

        public static void VerifyProtocolData(GetProtocolByIdQueryResponse protocolActualResult, CreateProtocolCommandRequest protocolData, Guid protocolId)
        {
            Assert.AreEqual(protocolId, protocolActualResult.Id, "Protocol Id is not correct");
            Assert.AreEqual(protocolData.Name, protocolActualResult.Name, "Protocol Name is not correct");
            Assert.AreEqual(protocolData.MdmClientId, protocolActualResult.MdmClientId, "Protocol MdmClientId is not correct");
            Assert.AreEqual(protocolData.ProtocolTypeId, protocolActualResult.ProtocolTypeId, "Protocol TypeId is not correct");
        }
    }
}