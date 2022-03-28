using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.CustomListData
{
    [TestClass]
    public class GetCustomListData : CDSBaseTest
    {
        private readonly Guid _structureId = Guid.Parse("8ba4a5d7-dfb0-43c3-9478-ef5353fb7aea");

        [TestMethod]
        [Description("266475 | GET CustomListData/{id}: check by existent in DB list id")]
        public async Task GetCustomListDataByIdTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(
                () => CdsUserClient.CustomListData_GetByCustomListStructureIdAsync(_structureId));
            VerifyReturnedData(result.Results);
        }

        public static bool IsRecordIdReturned(ICollection<GetCustomListDataInternalQueryResponseDto> result, Guid recordId)
        {
            bool isRecordReturned = false;
            foreach (var item in result)
            {
                if (item.Id == recordId)
                {
                    isRecordReturned = true;
                    break;
                }
            }
            return isRecordReturned;
        }

        private static void VerifyReturnedData(ICollection<GetCustomListDataInternalQueryResponseDto> result)
        {
            string emptyValueMsg = "'{0}' data field should not be empty for a legal entity";
            string missedFieldMsg = "'{0}' property should be present";
            foreach (var item in result)
            {
                Assert.IsNotNull(item.Id, string.Format(emptyValueMsg, "id"));
                foreach (var field in item.Data)
                {
                    // TODO: Validate actual values in the future
                    Assert.IsNotNull(field.FieldId, string.Format(emptyValueMsg, "fieldId"));
                    Assert.IsNotNull(field.FieldName, string.Format(emptyValueMsg, "fieldName"));
                    Assert.IsNotNull(field.GetType().GetProperty("Value"), string.Format(missedFieldMsg, "value"));
                }
            }
        }
    }
}
