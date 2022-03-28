using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.ProjectTypes
{
    [TestClass]
    public class GetProjectTypes : CDSBaseTest
    {
        private readonly List<string> _projectType = new() { "Tax", "Consulting", "Audit", "Global", "Wealth Management" };

        [TestMethod]
        [Description("225328 | GET /ProjectTypes: get all instances")]
        public async Task GetAllProjectTypesTest_WithServerToken()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsServerClient.ProjectTypes_GetAllAsync());
            result.Results.Count.Should().Be(_projectType.Count);
            VerifyProjectTypeResults(result.Results);
        }

        [TestMethod]
        [Description("225328 | GET /ProjectTypes: get all instances")]
        public async Task GetAllProjectTypesTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync());
            result.Results.Count.Should().Be(_projectType.Count);
            VerifyProjectTypeResults(result.Results);
        }

        [TestMethod]
        [Description("267902 | GET /ProjectTypes: check the result by non authorized request")]
        public async Task GetInstancesAsUnauthorizedUserTest()
        {
            await CdsHttpResponseHelper.VerifyUnauthorizedAsync(() => CdsNoTokenClient.Clients_GetAllAsync());
        }

        [TestMethod]
        [Description("310764 | GET /ProjectTypes: results are filtering by valid ID")]
        public async Task FilterByIdTest()
        {
            var tempResult = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync());
            string filterQuery = "Id in (" + tempResult.Results.FirstOrDefault().Id + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(filter: filterQuery));
            result.Results.Count.Should().Be(1);
            VerifyProjectTypesResponseAttribute(result.Results, item => item.Id.Equals(tempResult.Results.FirstOrDefault().Id));
        }

        [TestMethod]
        [Description("309136 | GET /ProjectTypes: check the results with filtering by valid Name")]
        public async Task FilterByNameTest()
        {
            string filterQuery = "Name in (" + _projectType[0] + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(filter: filterQuery));
            VerifyProjectTypesResponseAttribute(result.Results, item => item.Name.Equals(_projectType[0]));
        }

        [TestMethod]
        [Description("309134 | GET /ProjectTypes: filtering by both Id and Name parameters")]
        public async Task FilterByIdAndNameTest()
        {
            // Get object by the first item name in the expected project types list
            string precondFilterQuery = $"Name in ({_projectType[0]})";
            var precondResult = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(filter: precondFilterQuery));
            int projectTypeId = precondResult.Results.ToArray()[0].Id;
            // Filter by id and name that have been received in preconditions
            string filterQuery = $"Id in ({projectTypeId}) AND Name in ({_projectType[0]})";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(filter: filterQuery));
            VerifyProjectTypesResponseAttribute(result.Results, item => item.Name.Equals(_projectType[0]) && item.Id.Equals(projectTypeId));
        }

        [TestMethod]
        [Description("309137 | GET /ProjectTypes: order by Name ascending")]
        public async Task OrderByNameAscTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(orderby: "Name ASC"));
            var sorted = result.Results.OrderBy(s => s.Name);
            CollectionAssert.AreEqual(sorted.ToList(), result.Results.ToList());
            VerifyProjectTypeResults(result.Results);
        }

        [TestMethod]
        [Description("309138 | GET /ProjectTypes: order by ID descending")]
        public async Task OrderByIdDescTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(orderby: "Id DESC"));
            var sorted = result.Results.OrderByDescending(s => s.Id);
            CollectionAssert.AreEqual(sorted.ToList(), result.Results.ToList());
            VerifyProjectTypeResults(result.Results);
        }

        [TestMethod]
        [Description("309139 | GET /ProjectTypes: check the call returns the specified amount of project types")]
        public async Task GetSomeTopInstancesTest()
        {
            int countToDisplay = _projectType.Count / 2;
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(top: countToDisplay));
            VerifyProjectTypeNamesInChunkedList(result.Results, 0, countToDisplay);
        }

        [TestMethod]
        [Description("309140 | GET /ProjectTypes: check skip parameter")]
        public async Task GetInstancesUsingSkipParameterTest()
        {
            int countToSkip = _projectType.Count / 2;
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.ProjectTypes_GetAllAsync(skip: countToSkip));
            VerifyProjectTypeNamesInChunkedList(result.Results, countToSkip, _projectType.Count - countToSkip);
        }

        private void VerifyProjectTypeResults(ICollection<LineOfBusinessDto> results)
        {
            if (results.Count == 0)
            {
                Assert.Fail("No project type was found using the applied filter.");
            }
            string nullErrorMsg = "'{0}' field should not be null";
            foreach (var item in results)
            {
                Assert.IsNotNull(item.Id, string.Format(nullErrorMsg, "Id"));
                Assert.IsTrue(_projectType.Any(s => s.Equals(item.Name)), "Invalid project type");
                Assert.IsNotNull(item.Description, string.Format(nullErrorMsg, "Description"));
                foreach (var type in item.ProjectTypes)
                {
                    Assert.IsNotNull(type.Id, string.Format(nullErrorMsg, "Id"));
                    Assert.IsNotNull(type.Name, string.Format(nullErrorMsg, "Name"));
                    Assert.IsNotNull(type.Description, string.Format(nullErrorMsg, "Description"));
                    Assert.IsNotNull(type.LineOfBusinessId, string.Format(nullErrorMsg, "LineOfBusinessId"));
                };
            };
        }

        private void VerifyProjectTypesResponseAttribute(ICollection<LineOfBusinessDto> results, Func<LineOfBusinessDto, bool> predicate)
        {
            if (results.Count == 0)
            {
                Assert.Fail("No entity was found using the applied filter.");
            }
            int filteredCount = results.Where(predicate).Count();
            Assert.AreEqual(filteredCount, results.Count, "Some returned project type doesn't meet search criteria.");
            VerifyProjectTypeResults(results);
        }

        private void VerifyProjectTypeNamesInChunkedList(ICollection<LineOfBusinessDto> results, int startIndex, int chunkSize)
        {
            List<string> expectedNames = new List<string>();
            for (int i = startIndex; i < startIndex + chunkSize; i++)
            {
                expectedNames.Add(_projectType[i]);
            }
            List<string> returnedNames = new List<string>();
            foreach (var item in results)
            {
                returnedNames.Add(item.Name);
            }
            CollectionAssert.AreEqual(expectedNames, returnedNames, "Other project type names are expected in response");
        }
    }
}