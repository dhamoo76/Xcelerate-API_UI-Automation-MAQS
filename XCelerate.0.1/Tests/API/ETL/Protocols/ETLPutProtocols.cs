using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Tests.API.ETL.Protocols;
using System;
using Data.API;

namespace Tests.API.ETL.Protocols
{
    [TestClass]
    public class ETLPutProtocols : ETLBaseTest
    {
        private CreateProtocolCommandRequest _bodyCreate;
        private UpdateProtocolCommandRequest _bodyUpdate;
        private bool clean = true;
        private Guid newProtocolId;

        [TestInitialize]
        public async Task GenerateNewProtocol()
        {
            _bodyCreate = await GeneratePostProtocolsBody();
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_CreateProtocolAsync(_bodyCreate));
            result.Should().NotBeNull();
            newProtocolId = result.Id;
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("212866 | PUT /protocols correct parameters (valid)")]
        public async Task PutProtocolsCorrectParametersTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
            result.Should().NotBeNull();
            VerifyPutProtocolsResponse(result, newProtocolId);

            var resultSavedData = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetAsync(newProtocolId));
            VerifyProtocolData(resultSavedData, _bodyUpdate, newProtocolId);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("212867 | PUT /Protocols mandatory fields")]
        public async Task PutProtocolsMandatoryFieldsTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Description = "";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
            result.Should().NotBeNull();
            VerifyPutProtocolsResponse(result, newProtocolId);

            var resultSavedData = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetAsync(newProtocolId));
            VerifyProtocolData(resultSavedData, _bodyUpdate, newProtocolId);
        }

        [TestMethod]
        [Description("306637 | PUT /Protocols error messages checking - no name provided")]
        public async Task PutProtocolsWithoutNameTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Name = "";
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
            
            result.Validations["name"].Length.Should().Be(2);
            result.Validations["name"][0].ErrorMessage.Should().Be("'Name' must not be empty.");
            result.Validations["name"][0].ErrorCode.Should().Be("1");
            result.Validations["name"][1].ErrorMessage.Should().Be("'Name' is not in the correct format.");
            result.Validations["name"][1].ErrorCode.Should().Be("4");
        }

        [TestMethod]
        [Description("306636 | PUT /Protocols error messages checking - no mdmClientId provided")]
        public async Task PutProtocolsWithoutMdmClientIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.MdmClientId = 0;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306639 | PUT /Protocols error messages checking - no protocolTypeId provided")]
        public async Task PutProtocolsWithoutProtocolTypeIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.ProtocolTypeId = Guid.Empty;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306635 | PUT /Protocols error messages checking - no mandatory property provided")]
        public async Task PutProtocolsWithoutMandatoryPropertyTest()
        {
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> { };
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Properties = properties;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306638 | PUT /Protocols error messages checking - no protocolId provided")]
        public async Task PutProtocolsWithoutProtocolIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Id = Guid.Empty;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306633 | PUT /Protocols error messages checking - too long name provided")]
        public async Task PutProtocolsLongNameTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Name = GenerateRandomString(50, "");
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306634 | PUT /Protocols error messages checking - too long property provided")]
        public async Task PutProtocolsLongPropertyTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> { };
            PropertyRequestDto property = new();
            property.Name = "folderPath";
            property.Value = GenerateRandomString(100, "FolderPath");
            properties.Add(property);
            _bodyUpdate.Properties = properties;
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306630 | PUT /Protocols error messages checking - not existing mdmclientId provided")]
        public async Task PutProtocolsIncorrectMdmClientIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.MdmClientId = Int32.Parse(Config._notExistingMdmClientId);
            var result = await EtlHttpResponseHelper.VerifyAccessForbiddenAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
            result.Should().BeNull();
        }

        [TestMethod]
        [Description("306632 | PUT /Protocols error messages checking - not existing protocolTypeId provided")]
        public async Task PutProtocolsIncorrectProtocolTypeIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.ProtocolTypeId = Guid.Parse(Config._notExistingGuid);
            var result = await EtlHttpResponseHelper.VerifyValidationErrorAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestMethod]
        [Description("306631 | PUT /Protocols error messages checking - not existing protocolId provided")]
        public async Task PutProtocolsIncorrectProtocolIdTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            _bodyUpdate.Id = Guid.Parse(Config._notExistingGuid);
            var result = await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Protocols_EditProtocolAsync(_bodyUpdate));
            result.Should().BeNull();
        }

        [TestMethod]
        [Description("306640 | PUT /Protocols - unauthorized request")]
        public async Task PutProtocolsWithoutTokenTest()
        {
            _bodyUpdate = await GeneratePutProtocolsBody(newProtocolId);
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Protocols_EditProtocolAsync(_bodyUpdate));
        }

        [TestCleanup]
        public async Task TestsCleanup()
        {
            if (clean)
            {
                await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_DeletedByIdAsync(newProtocolId));
            }
        }

        private static void VerifyPutProtocolsResponse(UpdateProtocolCommandResponse protocolActualResult, Guid protocolId)
        {
            Assert.AreEqual(protocolId, protocolActualResult.Id, "Id is not correct in the response");
        }

        public async Task<UpdateProtocolCommandRequest> GeneratePutProtocolsBody(Guid protocolId)
        {
            var protocolType = await GetInternalSftpProtocolTypeDataAsync();
            Guid randomProtocolTypeId = protocolType.Id;
            int mdmClientId = int.Parse(Config._mdmClientId);
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> {};

            foreach (var item in protocolType.Properties)
            {
                PropertyRequestDto property = new();
                property.Name = item.Name;
                property.Value =  GenerateRandomString(10, item.Name);
                properties.Add(property);
            }

            var body = new UpdateProtocolCommandRequest()
            {
                Name = GenerateRandomString(10, "UpdatedProtocolName"),
                MdmClientId = mdmClientId,
                Description = GenerateRandomString(10, "UpdatedDescription"),
                ProtocolTypeId = randomProtocolTypeId,
                Properties = properties,
                Id = protocolId
            };

            return body;
        }

        public static void VerifyProtocolData(GetProtocolByIdQueryResponse protocolActualResult, UpdateProtocolCommandRequest protocolData, Guid protocolId)
        {
            Assert.AreEqual(protocolId, protocolActualResult.Id, "Protocol Id is not correct");
            Assert.AreEqual(protocolData.Name, protocolActualResult.Name, "Protocol Name was not updated");
            Assert.AreEqual(protocolData.MdmClientId, protocolActualResult.MdmClientId, "Protocol MdmClientId was not updated");
            Assert.AreEqual(protocolData.ProtocolTypeId, protocolActualResult.ProtocolTypeId, "Protocol TypeId was not updated");
        }
    }
}