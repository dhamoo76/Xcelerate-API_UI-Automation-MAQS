using Data.API.CDS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;


namespace Tests.API.CDS.CustomList
{
    [TestClass]
    public class GetCustomListStructure : CDSBaseTest
    {
        private string _clientId = "1234567";
        // ToDo: Hardcoded so far. When Post for list structures gets automated, make this test dynamic
        private readonly Guid _structureId = Guid.Parse("dc42367a-1b9e-4e36-96d8-908cdc839f0d");

        [TestMethod]
        [Description("308948 | GET /CustomList/Structures: get all objects w/o a filter")]
        public async Task GetAllListStructures()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetAllAsync());
            VerifyReturnedData(result.Results);
        }

        [TestMethod]
        [Description("301369 | GET /CustomList/Structures: check filter by MDMClientId")]
        public async Task FilterByMdmClientIdTest()
        {
            string filterQuery = $"MdmClientId eq '{_clientId}'";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetAllAsync(filter: filterQuery));
            // Verifying that a found id is equal to the requested structure id
            VerifyResponseDataAttributeValue(result.Results, item => item.Tags.Contains(_clientId));
        }

        [TestMethod]
        [Description("308887 | GET /CustomList/Structures: no filtering by ClientId")]
        public async Task NoFilteringByClientIdTest()
        {
            string filterQuery = $"ClientId eq '{_clientId}'";
            await CdsHttpResponseHelper.VerifyBadRequestAsync(
                () => CdsUserClient.CustomList_GetAllAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("308907 | GET /CustomList/Structures: no filtering by multiple MDMClientId values")]
        public async Task NoFilteringByMultipleMdmClientIdTest()
        {
            string filterQuery = $"ClientId eq '{_clientId},{Config.MdmClientId}'";
            await CdsHttpResponseHelper.VerifyBadRequestAsync(
                () => CdsUserClient.CustomList_GetAllAsync(filter: filterQuery));
        }

        // Private Methods

        private void VerifyReturnedData(ICollection<CustomListStructureDto> result)
        {
            string emptyValueMsg = "'{0}' data field should not be empty for a list structure";
            string missedFieldMsg = "'{0}' data field should be present";
            foreach (var item in result)
            {
                Assert.IsNotNull(item.Id, string.Format(emptyValueMsg, "id"));
                Assert.IsNotNull(item.Name, string.Format(emptyValueMsg, "name"));
                Assert.IsNotNull(item.IsPrivate, string.Format(emptyValueMsg, "isPrivate"));
                Assert.IsNotNull(item.ListType, string.Format(emptyValueMsg, "listType"));
                Assert.IsNotNull(item.GetType().GetProperty("Description"), string.Format(missedFieldMsg, "description"));
                Assert.IsNotNull(item.GetType().GetProperty("Tags"), string.Format(missedFieldMsg, "tags"));
                Assert.IsNotNull(item.GetType().GetProperty("Fields"), string.Format(missedFieldMsg, "fields"));
            }
        }

        private void VerifyResponseDataAttributeValue(ICollection<CustomListStructureDto> results,
                                                    Func<CustomListStructureDto, bool> predicate)
        {
            int searchResultsCount = results.Count;
            if (searchResultsCount == 0)
            {
                Assert.Fail("No list structure was found using the applied filter.");
            }
            int filteredResultsCount = results.Where(predicate).Count();
            Assert.AreEqual(filteredResultsCount, searchResultsCount,
                "Some returned list structure doesn't meet search criteria.");
        }
    }
}
