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
    public class DeleteCustomListData : CDSBaseTest
    {
        private readonly Guid _structureId = Guid.Parse("8ba4a5d7-dfb0-43c3-9478-ef5353fb7aea");
        private List<string> _structureFieldNames = new();
        private Dictionary<string, string> _newRecord = new();
        private string _newRecordId = null;

        [TestInitialize]
        public async Task GetTestData()
        {
            var getResult = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsNoTokenClient.CustomList_GetCustomListDataByIdAsync(_structureId));
            _structureFieldNames = getResult.Results.ElementAt(0).Keys.ToList();
            if (_structureFieldNames.Count == 0)
                Assert.Fail("List structure contains no fields. Error while generating test data.");
            var requestBody = GeneratePostRequestBody();
            var postResult = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_CreateCustomListDataAsync(requestBody));
            _newRecordId = postResult.Data.ElementAt(0).Id.ToString();
        }

        [TestMethod]
        [Description("266023 | DELETE /CustomListData: one record removal")]
        public void DeleteCustomListDataTest()
        {
            Guid deletedRecordId = Guid.Parse(_newRecordId);
            Task t = CdsUserClient.CustomListData_DeleteCustomListDataAsync(_structureId, new[] { deletedRecordId });
            if (!t.Wait(3000))
                Assert.Fail("Removing a record could not get completed within a specified time interval.");
            Assert.AreEqual(t.Status.ToString(), "RanToCompletion", "A record wasn't removed successfully whitin an expected time period");
            // Make sure that the record is no longer returned in the response
            var result = CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_GetByCustomListStructureIdAsync(_structureId));
            Assert.IsFalse(GetCustomListData.IsRecordIdReturned(result.Result.Results, deletedRecordId));
        }

        // Private methods

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
    }
}
