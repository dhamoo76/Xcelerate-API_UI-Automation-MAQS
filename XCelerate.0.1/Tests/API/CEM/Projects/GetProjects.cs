using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Projects
{
    [TestClass]
    public class GetProjects : CEMBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("205127 | Get all projects")]
        public async Task GetAllProjects()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            result.Should().NotBeNull();
            VerifyProjectsFieldsCollection(result.Results);
        }

        //TODO: Create proper method without code repetition (Implement schema Validator class). Move to dedicated utils/test class directory
        private void VerifyProjectsFieldsCollection(ICollection<ProjectDto> results)
        {
            foreach (var item in results)
            {
                Assert.IsNotNull(item.Id, "Id should not be empty");
                Assert.IsNotNull(item.EngagementId, "EngagementId should not be empty");
                Assert.IsNotNull(item.MdmClientId, "MdmClientId should not be empty");
                //Assert.IsNotNull(item.ProjectYear, "ProjectYear should not be empty");
                Assert.IsNotNull(item.Name, "Name should not be empty");
                Assert.IsNotNull(item.Description, "Description should not be empty");
                Assert.IsNotNull(item.LineOfBusiness, "LineOfBusiness should not be empty");
                Assert.IsNotNull(item.Status, "Status should not be empty");
                Assert.IsNotNull(item.Type, "Type should not be empty");
                //Assert.IsNotNull(item.ScheduledStartDate, "ScheduledStartDate should not be empty");
                //Assert.IsNotNull(item.ScheduledEndDate, "ScheduledEndDate should not be empty"); //Commented up to clarification with Irina
                Assert.IsNotNull(item.CreatorId, "CreatorId should not be empty");
            }
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("233821 | Get project by ID")]
        public async Task VerifyProjectsIdAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            System.Guid projectId = this.GetRandomProjectId(result.Results);
            string filterQuery = "Id eq '" + projectId.ToString() + "'";
            var filtered = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: filterQuery));
            filtered.Should().NotBeNull();
            VerifyProjectsFieldsCollection(filtered.Results);
            foreach (var item in filtered.Results)
            {
                Assert.AreEqual(projectId, item.Id);
            }
        }

        //TODO: Move to dedicated utils/test class directory
        private System.Guid GetRandomProjectId(ICollection<ProjectDto> results)
        {
            int length = results.Count;
            if (length > 0)
            {
                Random rnd = new Random();
                int num = rnd.Next(0, length);
                System.Guid projectId = results.ElementAt(num).Id;
                return projectId;
            }
            else
            {
                Console.WriteLine("There are no projects available!");
                throw new InvalidOperationException("Id cannot be returned");
            }

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("233821 | Get project by ID")]
        public async Task VerifyProjectByIdAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            System.Guid projectId = this.GetRandomProjectId(result.Results);
            var filtered = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetByIdAsync(projectId));
            filtered.Should().NotBeNull();

            Assert.IsNotNull(filtered.Id, "Id should not be empty");
            Assert.IsNotNull(filtered.EngagementId, "EngagementId should not be empty");
            Assert.IsNotNull(filtered.MdmClientId, "MdmClientId should not be empty");
            //Assert.IsNotNull(filtered.ProjectYear, "ProjectYear should not be empty");
            Assert.IsNotNull(filtered.Name, "Name should not be empty");
            Assert.IsNotNull(filtered.Description, "Description should not be empty");
            Assert.IsNotNull(filtered.LineOfBusiness, "LineOfBusiness should not be empty");
            Assert.IsNotNull(filtered.Status, "Status should not be empty");
            Assert.IsNotNull(filtered.Type, "Type should not be empty");
            //Assert.IsNotNull(filtered.ScheduledStartDate, "ScheduledStartDate should not be empty");
            //Assert.IsNotNull(filtered.ScheduledEndDate, "ScheduledEndDate should not be empty"); //Commented up to clarification with Irina
            Assert.IsNotNull(filtered.CreatorId, "CreatorId should not be empty");

            Assert.AreEqual(projectId, filtered.Id);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("233821 | Get project Legal Entities by ID")]
        public async Task VerifyProjectLegalEntitiesAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            System.Guid projectId = this.GetRandomProjectId(result.Results);
            var filtered = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetLegalEntitiesAsync(projectId));
            filtered.Should().NotBeNull();
            VerifyEntitiesFieldsCollection(filtered.Results);
            foreach (var item in filtered.Results)
            {
                Assert.AreEqual(projectId, item.Id);
            }
        }

        private void VerifyEntitiesFieldsCollection(ICollection<LegalEntityDto> results)
        {
            foreach (var item in results)
            {
                Assert.IsNotNull(item.Id, "Id should not be empty");
                Assert.IsNotNull(item.Name, "Name should not be empty");
                Assert.IsNotNull(item.MdmMasterClientId, "MdmMasterClientId should not be empty");
                Assert.IsNotNull(item.MdmLegalEntityId, "MdmLegalEntityId should not be empty");
                Assert.IsNotNull(item.EntityType, "EntityType should not be empty");
                Assert.IsNotNull(item.Phone, "Phone should not be empty");
                Assert.IsNotNull(item.Email, "Email should not be empty");
                Assert.IsNotNull(item.Address1, "Address1 should not be empty");
                Assert.IsNotNull(item.Address2, "Address2 should not be empty");
                Assert.IsNotNull(item.City, "City should not be empty");
                Assert.IsNotNull(item.Country, "Country should not be empty");
                Assert.IsNotNull(item.Zip, "Zip should not be empty");
                Assert.IsNotNull(item.State, "State should not be empty");
                Assert.IsNotNull(item.FiscalYear, "FiscalYear should not be empty");
                Assert.IsNotNull(item.IdentificationNumber, "IdentificationNumber should not be empty");
                Assert.IsNotNull(item.FirstName, "FirstName should not be empty");
                Assert.IsNotNull(item.MiddleInitial, "MiddleInitial should not be empty");
                Assert.IsNotNull(item.LastName, "LastName should not be empty");
                Assert.IsNotNull(item.DisplayName, "LastName should not be empty");
                Assert.IsNotNull(item.Projects, "Projects should not be empty");
            }
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("233821 | Get project by ID")]
        public async Task VerifyProjectByMdmClientIdAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            int mdmClientId = GetRandomMdmClientId(result.Results);
            string filterQuery = "MdmClientId eq '" + mdmClientId.ToString() + "'";
            var filtered = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: filterQuery));
            filtered.Should().NotBeNull();
            VerifyProjectsFieldsCollection(filtered.Results);
            foreach (var item in filtered.Results)
            {
                Assert.AreEqual(mdmClientId, item.MdmClientId);
            }
        }

        private int GetRandomMdmClientId(ICollection<ProjectDto> results)
        {
            int length = results.Count;
            if (length > 0)
            {
                Random rnd = new Random();
                int num = rnd.Next(0, length);
                int mdmClientId = results.ElementAt(num).MdmClientId;
                return mdmClientId;
            }
            else
            {
                Console.WriteLine("There are no projects available!");
                throw new InvalidOperationException("Id cannot be returned");
            }

        }

    }

}
