using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.LegalEntities
{
    [TestClass]
    public class GetLegalEntities : CEMBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("252560 | Get all legalEntities")]
        public async Task GetAllLegalEntitiesAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            result.Should().NotBeNull();
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("252201 | verify legal entities fields for get endpoint")]
        public async Task VerifyLegalEntitiesAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            this.VerifyEntitiesFieldsCollection(result.Results);
        }

        //TODO: Create proper method without code repetition (Implement schema Validator class). Move to dedicated utils/test class directory
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
        [Description("252202, 252207, 252203, 252540 | [Ent][API] check name of individual and non-individual entity")]
        public async Task VerifyNameLegalEntitiesAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            this.VerifyNameLegalEntitiesCollection(result.Results);
        }

        //TODO: Move to dedicated utils/test class directory
        private void VerifyNameLegalEntitiesCollection(ICollection<LegalEntityDto> results)
        {
            foreach (var item in results)
            {
                if (item.EntityType == "Individual" && item.MiddleInitial != "")
                {
                    var expectedName = String.Format("{0} {1}. {2}", item.FirstName, item.MiddleInitial, item.LastName);
                    Assert.AreEqual(expectedName, item.DisplayName);
                }
                if (item.EntityType == "Individual" && item.MiddleInitial == "")
                {
                    var expectedName = String.Format("{0} {2}", item.FirstName, item.MiddleInitial, item.LastName);
                    Assert.AreEqual(expectedName, item.DisplayName);
                }
                if ((new[] { "C-Corp", "S-Corp", "Partnership", "Trust" }).Contains(item.EntityType))
                {
                    var expectedName = item.Name;
                    Assert.AreEqual(expectedName, item.DisplayName);
                }
            }
        }

        //TODO Add project Id in case if it makes sense
        [TestMethod, TestCategory("Smoke")]
        [Description("251984,226757 | The list of entities is displayed in the context of a selected client")]
        public async Task VerifyAllEntitiesAreForOneClient()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            int entityMdmClientId = this.GetRandomLegalEntityMdmClientId(result.Results);
            string filterQuery = "MdmMasterClientId eq '" + entityMdmClientId.ToString() + "'";
            var filtered = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: filterQuery));
            this.VerifyAllEntitiesAreForOneClient(filtered.Results, entityMdmClientId);
        }

        //TODO: Move to dedicated utils/test class directory
        private int GetRandomLegalEntityMdmClientId(ICollection<LegalEntityDto> results)
        {
            int length = results.Count;
            if (length > 0)
            {
                Random rnd = new Random();
                int num = rnd.Next(0, length);
                int entityMdmClientId = results.ElementAt(num).MdmMasterClientId;
                return entityMdmClientId;
            }
            else
            {
                Console.WriteLine("There are no entities available!");
                return 0;
            }
            
        }


        //TODO: Move to dedicated utils/test class directory
        private void VerifyAllEntitiesAreForOneClient(ICollection<LegalEntityDto> results, int entityMdmClientId)
        {
            foreach (var item in results)
            {
                Assert.AreEqual(item.MdmMasterClientId, entityMdmClientId);
            }
        }

    }

}
