using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;
using System;

namespace Tests.API.ETL.Procedures
{
    [TestClass]
    public class ETLGetProcedureById : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("308569 | GET /procedures/{id} - existing procedure ID")]
        public async Task GetProcedureByIdTest()
        {
            Guid randomProcedureId = await GetRandomProcedureIdAsync();
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetProcedureByIdAsync(randomProcedureId));
            result.Should().NotBeNull();
            VerifyProcedureFields(result, randomProcedureId);
        }

        [TestMethod]
        [Description("308570 | GET /procedures/{id} - not existing procedure ID")]
        public async Task GetProcedureByNotExistingIdTest()
        {
            await EtlHttpResponseHelper.VerifyNotFoundAsync(() => EtlUserClient.Procedures_GetProcedureByIdAsync(Guid.Parse(Config._notExistingGuid)));
        }

        [TestMethod]
        [Description("308571 | GET /procedures/{id} - unauthorized request")]
        public async Task GetProcedureByIdWithoutTokenTest()
        {
            Guid protocolId = Guid.Parse(Config._notExistingGuid);
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Procedures_GetProcedureByIdAsync(protocolId));
        }

        private static void VerifyProcedureFields(ProcedureListDto procedureActualResult, Guid procedureId)
        {
            Assert.AreEqual(procedureId, procedureActualResult.Id, "Id is not correct in the response");
            Assert.IsNotNull(procedureActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(procedureActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(procedureActualResult.TimeSavings, "TimeSavings should not be empty");
            Assert.IsNotNull(procedureActualResult.ExternalId, "ExternalId should not be empty");
            if (procedureActualResult.Files.Count != 0)
            {
                foreach (var item in procedureActualResult.Files)
                {
                    VerifyProcedureFilesTable(item);
                }
            }
        }

        private static void VerifyProcedureFilesTable(ProcedureFileListDto fileActualResult)
        {
            Assert.IsNotNull(fileActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(fileActualResult.SchemaTypeId, "SchemaTypeId should not be empty");
            Assert.IsNotNull(fileActualResult.FileTypeId, "FileTypeId should not be empty");
        }
    }
}