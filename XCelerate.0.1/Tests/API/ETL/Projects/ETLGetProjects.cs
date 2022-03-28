using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;

namespace Tests.API.ETL.Projects
{
    [TestClass]
    public class ETLGetProjects : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("296138 | GET /Projects without any filtering (valid)")]
        public async Task GetAllProjectsWithoutFilteringTest()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Projects_GetAllProjectsAsync());
            result.Should().NotBeNull();
            VerifyProjectsFields(result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("258726 | GET /Projects with filtering by mdmclientId (valid)")]
        public async Task GetAllProjectsFilterByMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Projects_GetAllProjectsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            VerifyProjectsFields(result.Results, Config._mdmClientId);
        }

        [TestMethod]
        [Description("258738 | GET /Projects with filter by mdmclientid = empty")]
        public async Task GetAllProjectsFilterByEmptyMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Projects_GetAllProjectsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("258739 | GET /Projects with filter by mdmclientid = null")]
        public async Task GetAllProjectsFilterByNullMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Projects_GetAllProjectsAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("258740 | GET /Projects with filter by mdmclientid = notExistingMdmClientId")]
        public async Task GetAllProjectsFilterByNotExistingMdmClientIdTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._notExistingMdmClientId + "'";

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Projects_GetAllProjectsAsync(filter: filterQuery));
            result.Should().NotBeNull();
            result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("258741 | GET /Projects with filtering by mdmclientId (without token)")]
        public async Task GetAllProjectsWithoutTokenTest()
        {
            string filterQuery = "mdmClientId eq '" + Config._mdmClientId + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Projects_GetAllProjectsAsync(filter: filterQuery));
        }

        private static void VerifyProjectsFields(ICollection<ProjectDto> projectResult)
        {
            foreach (var item in projectResult)
            {
                VerifyProjectsFields(item);
            }
        }

        private static void VerifyProjectsFields(ICollection<ProjectDto> projectResult, string mdmClientId)
        {
            foreach (var item in projectResult)
            {
                VerifyProjectsFields(item);
                VerifyProjectMdmClientId(item, mdmClientId);
            }
        }

        private static void VerifyProjectsFields(ProjectDto projectActualResult)
        {
            Assert.IsNotNull(projectActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(projectActualResult.EngagementId, "EngagementId should not be empty");
            Assert.IsNotNull(projectActualResult.MdmClientId, "MdmClientId should not be empty");
            Assert.IsNotNull(projectActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(projectActualResult.Description, "Name should not be empty");
            Assert.IsNotNull(projectActualResult.LineOfBusiness, "Name should not be empty");
            Assert.IsNotNull(projectActualResult.Status, "Name should not be empty");
            Assert.IsNotNull(projectActualResult.Type, "Name should not be empty");
            Assert.IsNotNull(projectActualResult.CreatorId, "Name should not be empty");
        }

        private static void VerifyProjectMdmClientId(ProjectDto projectActualResult, string mdmClientId)
        {
            Assert.AreEqual(mdmClientId, projectActualResult.MdmClientId.ToString(), "Wrong MDM Client ID in the response.");
        }
    }
}