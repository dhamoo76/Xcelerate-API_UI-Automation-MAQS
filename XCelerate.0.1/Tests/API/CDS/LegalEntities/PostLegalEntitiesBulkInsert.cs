using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API.CDS;
using Models.Data.API.CDS;
using RSM.Xcelerate.CDS.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.LegalEntities
{
    [TestClass]
    public class PostLegalEntitiesBulkInsert : CDSBaseTest
    {
        private LegalEntityDto _randomLegalEntity;
        private string _randomClientId;

        [TestInitialize]
        public void TestPreconditions()
        {
            do
            {
                _randomLegalEntity = new GetLegalEntities().GetRandomLegalEntityAsync();
                _randomClientId = _randomLegalEntity.MdmMasterClientId.ToString();
            } while (_randomClientId.Length < 7);
        } 

        /* + covers '230823, 230830 | Master client number is not obligatory unique | Legal entity ID is NOT required' 
        + covers '230850 | Middle Initial is not unique' */
        [TestMethod]
        [Description("230812 | Sets a list of entities into CDS DB successfully & " +
                    "230873 | (REF) Entity type can be one of a five values: Individual, C - Corp, S - Corp, Partnership, Trust & " +
                    "230814 | If a record passes the validation and is a unique (No such MasterCLientID and EntityClientID in DB) then it's inserted into the DB")]
        public async Task BulkInsert()
        {
            var requestBody = GenerateRequestBody(_randomClientId, "S", LegalEntitiesData.EntityTypes);
            var response = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_BulkInsertAsync(requestBody));
            VerifyCDSLegalEntitiesFieldsCollection(response, LegalEntitiesData.EntityTypes.Length);
        }

        [TestMethod]
        [Description("230820 | POST /BulkInsert: Master client number is a required value " +
                    "230813 | (REF) POST /BulkInsert: When a record do not pass any validation rule then it's rejected by the system and validation rule is shown why it's rejected")]
        public async Task MissingRequiredField()
        {
            var requestBody = GenerateRequestBody(null, "S", LegalEntitiesData.EntityTypes[0]);
            var response = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_BulkInsertAsync(requestBody));
            VerifyCDSLegalEntitiesFieldsCollection(response, countRejected: 1);
        }

        [TestMethod]
        [Description("230821 | POST /BulkInsert: Master client number is a number value")]
        public async Task AlphabeticClientId()
        {
            var requestBody = GenerateRequestBody("randomClient", "S", LegalEntitiesData.EntityTypes[0]);
            var response = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_BulkInsertAsync(requestBody));
            VerifyCDSLegalEntitiesFieldsCollection(response, countRejected: 1);
        }

        [TestMethod]
        [Description("230822 | POST /BulkInsert: Master client number is a 7 digits number exactly")]
        public async Task IncorrectClientIdFormat()
        {
            var requestBody = GenerateRequestBody("123456", "S", LegalEntitiesData.EntityTypes[0]);
            var response = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_BulkInsertAsync(requestBody));
            VerifyCDSLegalEntitiesFieldsCollection(response, countRejected: 1);
        }

        [TestMethod]
        [Description("230848 | POST /BulkInsert: Middle Initial is optional")]
        public async Task NoMiddleInitialValue()
        {
            var requestBody = GenerateRequestBody(_randomClientId, "", LegalEntitiesData.EntityTypes[1]);
            var response = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_BulkInsertAsync(requestBody));
            VerifyCDSLegalEntitiesFieldsCollection(response, countInserted: 1);
        }


        // Private Methods
        private BulkInsertLegalEntitiesCommandRequest GenerateRequestBody(string mdmMasterClientId, string middleInitial, params string[] entityTypesArray)
        {
            List<LegalEntityStringDto> entities = new();
            foreach (string entityType in entityTypesArray)
            {
                entities.Add(new LegalEntityStringDto()
                {
                    MdmMasterClientId = mdmMasterClientId,
                    Name = "autoLegalEntity" + Config.Timestamp,
                    FirstName = "autoFirstName" + Config.Timestamp,
                    MiddleInitial = middleInitial,
                    LastName = "autoLastName" + Config.Timestamp,
                    EntityType = entityType
                });
            }
            return new BulkInsertLegalEntitiesCommandRequest()
            {
                LegalEntities = entities,
                CallingUserId = "autoUserId",
            };
        }

        private void VerifyCDSLegalEntitiesFieldsCollection(BulkInsertLegalEntitiesCommandResponse result, int countInserted = 0, int countUpdated = 0, int countRejected = 0)
        {
            Assert.IsNotNull(result.GetType().GetProperty("ResultRows"), "Response does not contain 'resultRows':\n" + result);
            Assert.AreEqual(countInserted, result.CountInserted, "Unexpected number of inserted legal entities");
            Assert.AreEqual(countUpdated, result.CountUpdated, "Unexpected number of updated legal entities");
            Assert.AreEqual(countRejected, result.CountRejected, "Unexpected number of rejected legal entities");
        }
    }

}
