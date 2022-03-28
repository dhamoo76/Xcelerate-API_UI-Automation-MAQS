using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Engagements
{
    [TestClass]
    public class GetEngagementById : CEMBaseTest
    {
        Random rand = new Random();
        EngagementDto _engagement;

        [TestMethod]
        [Description("303220| Get engagement by ID")]
        public async Task GetEngagementsByIdAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            result.Should().NotBeNull();
            _engagement = this.GetRandomEngagement(result.Results);

            var _engagementById = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetByIdAsync(_engagement.Id));
            _engagementById.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(_engagementById, _engagement.Id);
        }

        [TestMethod]
        [Description("303323 | Get engagement by ID Without token")]
        public async Task GetEngagementByIdWithoutToken()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            _engagement = this.GetRandomEngagement(result.Results);
            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Engagements_GetByIdAsync(_engagement.Id));
        }

        private void VerifyCEMEngagementFieldsCollection(GetEngagementByIdQueryResponse _engagement, Guid _id)
        {

            Assert.IsNotNull(_engagement.MdmClientId, "Mdmclientid should not be empty");
            Assert.IsNotNull(_engagement.CreatorId, "CreatorID should not be empty");
            Assert.AreEqual(_engagement.Id, _id,"Engagement Id is not correct");
            Assert.IsNotNull(_engagement.Name, "Name should not be empty");
            Assert.IsNotNull(_engagement.LineOfBusiness, "LOB should not be empty");
            Assert.IsNotNull(_engagement.Status, "Status should not be empty");
            Assert.IsNotNull(_engagement.ScheduledStartDate, "Start date should not be empty");

        }

        private EngagementDto GetRandomEngagement(ICollection<EngagementDto> results)
        {
            int _randomEngagementIndex = rand.Next(results.Count);
            EngagementDto _randomEngagement = results.ElementAt(_randomEngagementIndex);
            return _randomEngagement;

        }

    }

}

