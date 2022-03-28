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
    public class PutCustomListData : CDSBaseTest
    {
        private readonly Guid _structureId = Guid.Parse("8ba4a5d7-dfb0-43c3-9478-ef5353fb7aea");
        private List<string> _structureFieldNames = new();
        private Dictionary<string, string> _newRecord = new();
        private string _newRecordId = null;
        private CreateCustomListDataCommandResponse _postResponse = null;

        // Create a new record, get it guid in order to edit in the future
        [TestInitialize]
        public async Task GetTestData()
        {
            var getResult = CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsNoTokenClient.CustomList_GetCustomListDataByIdAsync(_structureId));
            _structureFieldNames = getResult.Result.Results.ElementAt(0).Keys.ToList();
            if (_structureFieldNames.Count == 0)
                Assert.Fail("List structure contains no fields. Error while generating test data.");
            var requestBody = GeneratePostRequestBody();
            _postResponse = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_CreateCustomListDataAsync(requestBody));
            _newRecordId = _postResponse.Data.ElementAt(0).Id.ToString();
        }

        [TestMethod]
        [Description("312977 | PUT /CustomListData/{id} schema validation + edited valid values")]
        public async Task PutCustomListDataTest()
        {
            var requestBody = GeneratePutRequestBody(_postResponse);
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_PutCustomListDataAsync(_structureId, new[] { requestBody }));
            VerifyReturnedData(result.Data, requestBody);
        }

        [TestCleanup]
        public void CleanUpTestData()
        {
            if (_newRecordId != null)
            {
                CdsUserClient.CustomListData_DeleteCustomListDataAsync(_structureId, new[] { Guid.Parse(_newRecordId) });
            }
        }

        private CreateCustomListDataCommandRequest GeneratePostRequestBody()
        {
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

        private UpdateCustomListDataCommandDtoRequest GeneratePutRequestBody(CreateCustomListDataCommandResponse postedRecord)
        {
            var updatedFields = new List<UpdateCustomListDataValuesDto>();
            for (int i = 0; i < postedRecord.Data.ElementAt(0).Data.Count; i++)
            {
                UpdateCustomListDataValuesDto editedField = new UpdateCustomListDataValuesDto();
                editedField.FieldId = postedRecord.Data.ElementAt(0).Data.ElementAt(i).FieldId;
                editedField.FieldName = postedRecord.Data.ElementAt(0).Data.ElementAt(i).FieldName;
                editedField.Value = $"edited{editedField.FieldName}AutoTest{Config.Timestamp}";
                updatedFields.Add(editedField);
            }
            return new UpdateCustomListDataCommandDtoRequest()
            {
                Id = Guid.Parse(_newRecordId),
                Data = updatedFields
            };
        }


        private void VerifyReturnedData(ICollection<CreateCustomListDataCommandResponseDto> data, UpdateCustomListDataCommandDtoRequest updatedField)
        {
            foreach (var item in data)
            {
                Assert.AreEqual(item.Id, updatedField.Id);
                for (int i = 0; i < _structureFieldNames.Count; i++)
                {
                    var returnedFieldInfo = item.Data.ElementAt(i);
                    var expectedFieldInfo = updatedField.Data.ElementAt(i);
                    Assert.AreEqual(returnedFieldInfo.FieldId, expectedFieldInfo.FieldId);
                    Assert.AreEqual(returnedFieldInfo.FieldName.ToLower(), expectedFieldInfo.FieldName.ToLower());
                    Assert.AreEqual(returnedFieldInfo.Value, expectedFieldInfo.Value);
                }
            }
        }
    }
}
