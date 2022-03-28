using Data.API.CDS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.CustomListData
{
    [TestClass]
    public class PostCustomListData : CDSBaseTest
    {
        private readonly Guid _structureId = Guid.Parse("8ba4a5d7-dfb0-43c3-9478-ef5353fb7aea");
        private List<string> _structureFieldNames = new();
        private Dictionary<string, string> _newRecord = new();
        private string _newRecordId = null;

        // Get the field names of a structure
        [TestInitialize]
        public async Task GetTestData()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsNoTokenClient.CustomList_GetCustomListDataByIdAsync(_structureId));
            _structureFieldNames = result.Results.ElementAt(0).Keys.ToList();
            if (_structureFieldNames.Count == 0)
                Assert.Fail("List structure contains no fields. Error while generating test data.");
        }

        [TestMethod]
        [Description("312905 | POST /CustomListData: Add list data: one row, all fields have values")]
        public async Task PostCustomListDataTest()
        {
            var requestBody = GenerateRequestBody();
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_CreateCustomListDataAsync(requestBody));
            VerifyReturnedData(result.Data);
        }

        [TestCleanup]
        public void CleanUpTestData()
        {
            if (_newRecordId != null)
            {
                CdsUserClient.CustomListData_DeleteCustomListDataAsync(_structureId, new[] { Guid.Parse(_newRecordId) });
            }
        }

        private CreateCustomListDataCommandRequest GenerateRequestBody()
        {
            // Generate values for every structure field
            foreach (var field in _structureFieldNames)
            {
                _newRecord[field] = $"{field}AutoTest{Config.Timestamp}";
            }
            return new CreateCustomListDataCommandRequest()
            {
                CustomListStructureId = _structureId,
                Data = new List<IDictionary<string, string>>() { _newRecord }
            };
        }


        private void VerifyReturnedData(ICollection<CreateCustomListDataCommandResponseDto> data)
        {
            foreach (var item in data)
            {
                Assert.IsNotNull(item.Id, "\'id\' should not be empty");
                _newRecordId = item.Id.ToString();
                for (int i = 0; i < _structureFieldNames.Count; i++)
                {
                    Assert.IsNotNull(item.Data.ElementAt(i).FieldId, "\'fieldId\' should not be empty");
                    Assert.AreEqual(item.Data.ElementAt(i).FieldName.ToLower(), _structureFieldNames[i].ToLower());
                    Assert.AreEqual(item.Data.ElementAt(i).Value, _newRecord[_structureFieldNames[i]]);
                }
            }
        }
    }
}
