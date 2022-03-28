using Data.API.CDS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CDS.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.LegalEntities
{
    [TestClass]
    public class GetLegalEntities : CDSBaseTest
    {
        private LegalEntityDto _legalEntity;

        [TestInitialize]
        public void TestPreconditions() => _legalEntity = GetRandomLegalEntityAsync();

        // No Filtering

        [TestMethod]
        [Description("228304 | GET  /LegalEntities without any filter (empty filtering)")]
        public async Task GetAllEntitiesNoFilter()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync());
            VerifyCDSLegalEntitiesFieldsCollection(result.Results);
        }

        // Filter by MdmMasterClientId

        [TestMethod]
        [Description("228298 | GET  /LegalEntities filter by a NON existent client")]
        public async Task FilterByNotExistingMdmClientId()
        {
            int clientId = Config.NotExistingMdmClientId;
            string filterQuery = "MdmMasterClientId in (" + clientId + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataIsEmpty(result.Results);
        }

        [TestMethod]
        [Description("228299 | GET  /LegalEntities filter by a client with NO value within filter (no clientid)")]
        public async Task FilterByEmptyMdmClientIdValue()
        {
            string filterQuery = "MdmMasterClientId in ()";
            await CdsHttpResponseHelper.VerifyBadRequestAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
        }

        // Works with E2S token only. Because using S2S token all existing entities are return
        [TestMethod]
        [Description("239279 | GET  /LegalEntities filter by existent MDMMasterClientID (for a client you have NO access to)")]
        public async Task FilterByMdmClientIdNoAccess()
        {
            int clientId = Config.NoAccessMdmClientId;
            string filterQuery = "MdmMasterClientId in (" + clientId + ")";
            await CdsHttpResponseHelper.VerifyAccessForbiddenAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
        }

        [TestMethod]
        [Description("239294 | GET  /LegalEntities filter by existent MDMMasterClientID (for a client you have access to)")]
        public async Task FilterByValidMdmClientId()
        {
            int clientId = Config.MdmClientId;
            string filterQuery = "MdmMasterClientId in (" + clientId + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataAttributeValue(result.Results, item => item.MdmMasterClientId == clientId);
            VerifyCDSLegalEntitiesFieldsCollection(result.Results);
        }

        // Filter By Legal Entity Name

        [TestMethod]
        [Description("228300 | GET  /LegalEntities filter by a legal entity name that user has access to")]
        public async Task FilterByValidName()
        {
            string name = _legalEntity.Name;
            string filterQuery = "Name in (" + name + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataAttributeValue(result.Results, item => item.Name.Equals(name));
        }

        [TestMethod]
        [Description("228301 | GET  /LegalEntities filter by a legal entity name that does NOT exist")]
        public async Task FilterByNotExistingName()
        {
            string name = _legalEntity.Name + "notExisting";
            string filterQuery = "Name in (" + name + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataIsEmpty(result.Results);
        }

        [TestMethod]
        [Description("228302 | GET  /LegalEntities filter by an empty legal entity name within the filter")]
        public async Task FilterByEmptyName()
        {
            string filterQuery = "Name in ()";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataAttributeValue(result.Results, item => item.Name.Equals(""));
        }

        // Filter By Guid

        [TestMethod]
        [Description("239276 | GET  /LegalEntities filter by existent guid (for a client you have access to)")]
        public async Task FilterByValidId()
        {
            Guid guid = _legalEntity.Id;
            string filterQuery = "Id in (" + guid + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            Assert.AreEqual(1, result.Results.Count);
            VerifyResponseDataAttributeValue(result.Results, item => item.Id == guid);
        }

        [TestMethod]
        [Description("239277 | GET  /LegalEntities filter by non-existent guid")]
        public async Task FilterByNotExistingId()
        {
            string notExistingGuid = "00000000-0000-0000-0000-000000012345";
            string filterQuery = "Id in (" + notExistingGuid + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataIsEmpty(result.Results);
        }

        // Works with E2S token only. Because using S2S token all existing entities are return;
        // This test is not going to fail if a specified guid doesn't exist on the environment.
        // TODO: When assigning apps is available in CEM, we will have to add test preconditions in order to make sure that
        // 1 - the legal entity guid exists; 2 - the current user cannot access it.
        [TestMethod]
        [Description("239278 | GET  /LegalEntities filter by existent guid (client you have NO access to)")]
        public async Task FilterByIdNoAccess()
        {
            string guid = Config.NoAccessLegalEntityId;
            string filterQuery = "Id in (" + guid + ")";
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync(filter: filterQuery));
            VerifyResponseDataIsEmpty(result.Results);
        }


        // Private Methods

        private void VerifyCDSLegalEntitiesFieldsCollection(ICollection<LegalEntityDto> results)
        {
            if (results.Count == 0)
                Assert.Fail("No entity was found using the applied filter.");
            string assertMsg = "'{0}' data field should not be empty for a legal entity";
            foreach (var item in results)
            {
                Assert.IsNotNull(item.Id, string.Format(assertMsg, "id"));
                Assert.IsNotNull(item.Name, string.Format(assertMsg, "name"));
                Assert.IsNotNull(item.DisplayName, string.Format(assertMsg, "displayName"));
                Assert.IsNotNull(item.EntityType, string.Format(assertMsg, "entityType"));
                Assert.IsNotNull(item.GetType().GetProperty("MdmLegalEntityId"), "'mdmLegalEntityId' data field should be present");
                Assert.IsNotNull(item.Phone, string.Format(assertMsg, "phone"));
                Assert.IsNotNull(item.Email, string.Format(assertMsg, "email"));
                Assert.IsNotNull(item.Address1, string.Format(assertMsg, "address1"));
                Assert.IsNotNull(item.Address2, string.Format(assertMsg, "address2"));
                Assert.IsNotNull(item.City, string.Format(assertMsg, "city"));
                Assert.IsNotNull(item.Country, string.Format(assertMsg, "country"));
                Assert.IsNotNull(item.Zip, string.Format(assertMsg, "zip"));
                Assert.IsNotNull(item.State, string.Format(assertMsg, "state"));
                Assert.IsNotNull(item.FiscalYear, string.Format(assertMsg, "fiscalYear"));
                Assert.IsNotNull(item.IdentificationNumber, string.Format(assertMsg, "identificationNumber"));
                Assert.IsNotNull(item.MdmMasterClientId, string.Format(assertMsg, "mdmMasterClientId"));
                Assert.IsNotNull(item.FirstName, string.Format(assertMsg, "firstName"));
                Assert.IsNotNull(item.MiddleInitial, string.Format(assertMsg, "middleInitial"));
                Assert.IsNotNull(item.LastName, string.Format(assertMsg, "lastName"));
            }
        }

        private void VerifyResponseDataAttributeValue(ICollection<LegalEntityDto> results, Func<LegalEntityDto, bool> predicate)
        {
            if (results.Count == 0)
                Assert.Fail("No entity was found using the applied filter.");
            int filteredResultsCount = results.Where(predicate).Count();
            Assert.AreEqual(filteredResultsCount, results.Count,
                "Some returned entity doesn't meet search criteria.");
        }

        private void VerifyResponseDataIsEmpty(ICollection<LegalEntityDto> results)
        {
            Assert.AreEqual(0, results.Count, "Response contains some object when none is expected.");
        }

        // Returns an accessible legal entity object to be used as test data for filtering
        public LegalEntityDto GetRandomLegalEntityAsync()
        {
            try
            {
                var result = CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.LegalEntities_GetAllAsync()).Result.Results;
                var filteredResult = result.Where(item => !item.Name.Equals(""));
                int resultCount = filteredResult.Count();
                int randomNumber = new Random().Next(resultCount);
                return filteredResult.ElementAt(randomNumber);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Could not get any legal entities because of an exception:\n{ex.Message}");
                return null;
            }

        }
    }
}
