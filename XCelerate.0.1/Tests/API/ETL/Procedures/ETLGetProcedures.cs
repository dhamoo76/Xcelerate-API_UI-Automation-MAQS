using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.Procedures
{
    [TestClass]
    public class ETLGetProcedures : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("231926 | GET /Procedures without any filter")]
        public async Task GetAllProceduresWithoutFilteringTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync());
            result.Should().NotBeNull();
            VerifyProceduresFields(result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("230772 | GET /Procedures with filtering by mdmclientId (valid)")]
        public async Task GetAllProceduresFilterByMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProceduresFields(result.Results, Config._mdmClientId);
        }

        [TestMethod]
        [Description("231934 | GET /Procedures with filter by mdmclientid = empty")]
        public async Task GetAllProceduresFilterByEmptyMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("231923 | GET /Procedures with filter by mdmclientid = null")]
        public async Task GetAllProceduresFilterByNullMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("231924 | GET /Procedures with filter by mdmclientid = long value")]
        public async Task GetAllProceduresFilterByLongMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + GenerateRandomString(100) + "'";

            var result = await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
            result.Should().BeNull();
        }

        [TestMethod]
        [Description("231935 | GET /Procedures with filter by mdmclientid = notExistingMdmClientId")]
        public async Task GetAllProceduresFilterByNotExistingMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._notExistingMdmClientId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProceduresFields(result.Results, "0");
        }
        
        [TestMethod, TestCategory("Smoke")]
        [Description("230773 | GET /Procedures with filtering by valid applicationId")]
        public async Task GetAllProceduresFilterByApplicationIdTest()
        {
            string applicationId = await GetRandomApplicationIdAsync();
            string filterQuery = "applicationId eq '" + applicationId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProceduresFields(result.Results, "", applicationId);
        }

        [TestMethod]
        [Description("231936 | GET /Procedures with filter by applicationId = empty")]
        public async Task GetAllProceduresFilterByEmptyApplicationIdTest()
        {
            string filterQuery = "applicationId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("231920 | GET /Procedures with filter by applicationId = null")]
        public async Task GetAllProceduresFilterByNullApplicationIdTest()
        {
            string filterQuery = "applicationId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("231921 | GET /Procedures with filter by applicationId = long value")]
        public async Task GetAllProceduresFilterByLongApplicationIdTest()
        {
            string filterQuery = "applicationId eq '" + GenerateRandomString(100) + "'";

            var result = await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("231922 | GET /Procedures with filter by applicationId = notExistingApplicationId")]
        public async Task GetAllProceduresFilterByNotExistingApplicationIdTest()
        {
            string filterQuery = "applicationId eq '" + Config._notExistingMdmClientId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProceduresFields(result.Results, "", "0");
        }

        [TestMethod]
        [Description("307869 | GET /Procedures without filter - unauthorized request")]
        public async Task GetAllProceduresWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Projects_GetAllProjectsAsync());
        }

        [TestMethod]
        [Description("307870 | GET /Procedures with filtering by applicationId - unauthorized request")]
        public async Task GetAllProceduresFilterByApplicationIdWithoutTokenTest()
        {
            string randomApplicationId = await GetRandomApplicationIdAsync();
            string filterQuery = "mdmClientId eq '" + randomApplicationId + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Projects_GetAllProjectsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("307871 | GET /Procedures with filtering by mdmClientId - unauthorized request")]
        public async Task GetAllProceduresFilterByMdmClientIdWithoutTokenTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Projects_GetAllProjectsAsync(filter: filterQuery));
        }

        private static void VerifyProceduresFields(ICollection<ProcedureListDto> procedureResult)
        {
            foreach (var item in procedureResult)
            {
                VerifyProceduresFields(item);
            }
        }

        private static void VerifyProceduresFields(ICollection<ProcedureListDto> procedureResult, string mdmClientId = "", string applicationId = "")
        {
            foreach (var item in procedureResult)
            {
                VerifyProceduresFields(item);
                if(mdmClientId != "")
                    VerifyProcedureMdmClientId(item, mdmClientId);
                if(applicationId != "")
                    VerifyProcedureApplicationId(item, applicationId);
            }
        }

        private static void VerifyProceduresFields(ProcedureListDto procedureActualResult)
        {
            Assert.IsNotNull(procedureActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(procedureActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(procedureActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(procedureActualResult.TimeSavings, "TimeSavings should not be empty");
            Assert.IsNotNull(procedureActualResult.ExternalId, "ExternalId should not be empty");
            if(procedureActualResult.Files.Count != 0)
            {
                foreach(var item in procedureActualResult.Files)
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

        private static void VerifyProcedureMdmClientId(ProcedureListDto procedureActualResult, string mdmClientId)
        {
            if (procedureActualResult.MdmClientId != 0)
            {
                Assert.AreEqual(mdmClientId, procedureActualResult.MdmClientId.ToString(), "Wrong MDM Client ID in the response.");
            }
            else
            {
                Assert.AreEqual(0, procedureActualResult.MdmClientId, "Wrong MDM Client ID in the response.");
            }
        }

        private static void VerifyProcedureApplicationId(ProcedureListDto procedureActualResult, string applicationId)
        {
            if (procedureActualResult.ApplicationId != "0")
            {
                Assert.AreEqual(applicationId, procedureActualResult.ApplicationId, "Wrong Application ID in the response.");
            }
            else 
            {
                Assert.AreEqual("0", procedureActualResult.ApplicationId, "Wrong Application ID in the response.");
            }
        }
    }
}