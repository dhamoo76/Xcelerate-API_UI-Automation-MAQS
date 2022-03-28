using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.CustomList
{
    [TestClass]
    public class GetCustomListDataId : CDSBaseTest
    {
        // ToDo: Replace the static data by a dynamically created list structure
        private readonly Guid _structureId = Guid.Parse("8ba4a5d7-dfb0-43c3-9478-ef5353fb7aea");
        private List<Dictionary<string, string>> _data = new();

        [TestInitialize]
        public void PreconditionExpectedListData()
        {
            _data.Add(new Dictionary<string, string> {
                {"name", "Adam" },
                {"surname", "Sandler" },
                {"title", "Comedian" } });
            _data.Add(new Dictionary<string, string> {
                {"name", "Penelope" },
                {"surname", "Cruz" },
                {"title", "Actress" } });
            _data.Add(new Dictionary<string, string> {
                {"name", "Elon" },
                {"surname", "Musk" },
                {"title", "CEO of Tesla Motors" } });
            _data.Add(new Dictionary<string, string> {
                {"name", "Barack" },
                {"surname", "Obama" },
                {"title", "44th U.S. President" } });
            _data.Add(new Dictionary<string, string> {
                {"name", "Adam" },
                {"surname", "Mickiewicz" },
                {"title", "Poet" } });
        }

        [TestMethod]
        [Description("264400 | GET /CustomList/Data/{Id} check data is returned for an authzed user")]
        public async Task GetCustomListDataByIdTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(_structureId));
            VerifyReturnedData(_data, result.Results);
        }

        [TestMethod]
        [Description("264399 | GET /CustomList/Data/{Id} works without authorization token")]
        public async Task GetCustomListDataByIdNoTokenTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsNoTokenClient.CustomList_GetCustomListDataByIdAsync(_structureId));
            VerifyReturnedData(_data, result.Results);
        }

        [TestMethod]
        [Description("264412 | GET /CustomList/Data/{Id} check non existent in DB liststructureId")]
        public async Task GetNotExistingStructureIdTest()
        {
            Guid notExistingStructureId = Guid.Parse("1aa1a1a1-aaa1-11a1-1111-aa1111aa1aaa");
            await CdsHttpResponseHelper.VerifyNotFoundAsync(
                () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(notExistingStructureId));
        }

        [TestMethod]
        [Description("277606 | GET /CustomList/Data/{Id} searching by multiple fields")]
        public async Task FilterByMultipleFieldsTest()
        {
            // Verify that list data has correct size for testing the filtering
            VerifyExpectedListSize();
            // Get some existing list data
            Dictionary<string, string> searchValues = GetSearchCriteriaForEveryField();
            // Generate a filter query and send a request
            string filterQuery = GenerateFilterQueryByMultipleFieldsPartialMatch(searchValues);
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
               () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(_structureId, filter: filterQuery));
            // Define expected list data after the filtering and compare it with received JSON
            var filteredData = FilterListDataByMultipleFields(searchValues);
            VerifyReturnedData(filteredData, result.Results);
        }

        [TestMethod]
        [Description("309170 | GET /CustomList/Data/{Id} pagination")]
        public async Task PaginationTest()
        {
            int recordCount = (await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(_structureId))).Results.Count;
            // ToDo: When we create list structure and data dynamically, replace the assertion by adding 2 more lines
            if (recordCount < 2)
                Assert.Fail("Not enough data to verify filtering");
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(_structureId, top: recordCount / 2));
            var expectedList = ChunkList(_data, 0, recordCount / 2);
            VerifyReturnedData(expectedList, result.Results);
            // Get 'nextToken' value and reguest list data of the second page
            string nextToken = result.NextToken;
            var resultWithToken = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(_structureId, top: recordCount / 2, skipToken: nextToken));
            expectedList = ChunkList(_data, recordCount / 2, recordCount / 2);
            VerifyReturnedData(expectedList, resultWithToken.Results);
        }

        [TestMethod]
        [Description("309141 | GET /CustomList/Data/{Id} sorting by a field ASC")]
        public async Task SortByFieldAscTest()
        {
            string fieldToSortBy = _data[0].ElementAt(0).Key;
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
               () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(
                   _structureId,
                   orderby: $"{fieldToSortBy} ASC"));
            var sortedList = _data.OrderBy(x => x[fieldToSortBy]).ToList();
            VerifyReturnedData(sortedList, result.Results);
        }

        [TestMethod]
        [Description("311954 | GET /CustomList/Data/{Id} sorting by a field DESC")]
        public async Task SortByFieldDescTest()
        {
            string fieldToSortBy = _data[0].ElementAt(1).Key;
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
               () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(
                   _structureId,
                   orderby: $"{fieldToSortBy} DESC"));
            var sortedList = _data.OrderByDescending(x => x[fieldToSortBy]).ToList();
            VerifyReturnedData(sortedList, result.Results);
        }

        [TestMethod]
        [Description("309171 | GET /CustomList/Data/{Id} filtering and sorting simultaneously")]
        public async Task SortAndFilterSimultaneouslyTest()
        {
            string fieldName = _data[0].ElementAt(0).Key;
            string searchQuery = "a";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
               () => CdsUserClient.CustomList_GetCustomListDataByIdAsync(
                   _structureId,
                   filter: $"contains({fieldName},'{searchQuery}')",
                   orderby: $"{fieldName} ASC"));
            if (result.Results.Count < 3)
                Assert.Fail("Not enough data to verify filtering and sorting simultaneously. " +
                    "Try to add test data or execute the scenario manually.");
            var filteredList = FilterListDataByMultipleFields(new Dictionary<string, string> {
                { fieldName, searchQuery }
            });
            var sortedList = filteredList.OrderBy(x => x[fieldName]).ToList();
            VerifyReturnedData(sortedList, result.Results);
        }

        // Private Methods

        private void VerifyReturnedData(List<Dictionary<string, string>> expectedListData, ICollection<IDictionary<string, string>> returnedData)
        {
            if (expectedListData.Count != returnedData.Count)
                Assert.Fail($"The list contains {returnedData.Count} lines when {expectedListData.Count} ones are expected");
            List<IDictionary<string, string>> actualList = returnedData.ToList();
            for (int i = 0; i < actualList.Count; i++)
            {
                Dictionary<string, string> temp = actualList[i].ToDictionary(x => x.Key, x => x.Value);
                Assert.IsTrue(expectedListData[i].SequenceEqual(temp), "Response body doesn't match the expected list");
            }
        }

        private List<Dictionary<string, string>> ChunkList(List<Dictionary<string, string>> listData, int startIndex, int chunkSize)
        {
            List<Dictionary<string, string>> chunkedList = new();
            for (int i = startIndex; i < startIndex + chunkSize; i++)
            {
                chunkedList.Add(listData[i].ToDictionary(x => x.Key, x => x.Value));
            }
            return chunkedList;
        }

        private string GenerateFilterQueryByMultipleFieldsPartialMatch(Dictionary<string, string> searchValues)
        {
            StringBuilder filterQuery = new StringBuilder(
                $"contains({searchValues.ElementAt(0).Key},'{searchValues.ElementAt(0).Value}')");
            for (int i = 1; i < searchValues.Count; i++)
            {
                filterQuery.Append($" AND contains({searchValues.ElementAt(i).Key},'{searchValues.ElementAt(i).Value}')");
            }
            return filterQuery.ToString();
        }

        private List<Dictionary<string, string>> FilterListDataByMultipleFields(Dictionary<string, string> searchValues)
        {
            var filteredData = new List<Dictionary<string, string>>();
            foreach (var record in _data)
            {
                bool doesMatchFilter = true;
                for (int i = 0; i < searchValues.Count; i++)
                {
                    if (!record[searchValues.ElementAt(i).Key].Contains(searchValues.ElementAt(i).Value))
                    {
                        doesMatchFilter = false;
                        break;
                    }
                }
                if (doesMatchFilter)
                    filteredData.Add(record);
            }
            return filteredData;
        }

        private Dictionary<string, string> GetSearchCriteriaForEveryField()
        {
            Dictionary<string, string> searchValues = new Dictionary<string, string>();
            // Choose a random record by which data filtering will be performed
            int recordIndex = GetRandomListRecordNumber();
            for (int i = 0; i < _data[0].Count; i++)
            {
                // Grab a column name and the corresponding value in the chosen record
                string key = _data[recordIndex].ElementAt(i).Key;
                string value = _data[recordIndex][key];
                // Cut the received value to filter data by its part
                string searchValue = value.Substring(1, value.Length / 2);
                searchValues.Add(key, searchValue);
            }
            return searchValues;
        }

        private int GetRandomListRecordNumber()
        {
            Random random = new Random();
            return random.Next(0, _data.Count);
        }

        // Make sure that the list has at least one record and 2 columns
        private void VerifyExpectedListSize()
        {
            if (_data.Count == 0 || _data[0].Count < 2)
                Assert.Fail("List data is empty.");
        }

    }
}
