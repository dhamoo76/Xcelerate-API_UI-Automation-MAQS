using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Engagements
{
    [TestClass]
    public class GetEngagements : CEMBaseTest
    {
        Random rand = new Random();

        [TestMethod]
        [Description("205155| Get all engagements")]
        public async Task GetAllEngagementsAsync()
        {
            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync());
            result.Should().NotBeNull();

            this.VerifyCEMEngagementFieldsCollection(result.Results);
        }

        [TestMethod]
        [Description("303232 | Get all engagements without token")]
        public async Task GetAllEngagementsWithoutToken()
        {
            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Engagements_GetAllAsync());
        }

        private void VerifyCEMEngagementFieldsCollection(ICollection<EngagementDto> results)
        {
            EngagementDto _randomEngagement = GetRandomEngagement(results);

            Assert.IsNotNull(_randomEngagement.MdmClientId, "Mdmclientid should not be empty");
            Assert.IsNotNull(_randomEngagement.CreatorId, "CreatorID should not be empty");
            Assert.IsNotNull(_randomEngagement.Id, "Id should not be empty");
            Assert.IsNotNull(_randomEngagement.Name, "Name should not be empty");
            Assert.IsNotNull(_randomEngagement.LineOfBusiness, "LOB should not be empty");
            Assert.IsNotNull(_randomEngagement.Status, "Status should not be empty");
            Assert.IsNotNull(_randomEngagement.ScheduledStartDate, "Start date should not be empty");

        }

        private EngagementDto GetRandomEngagement(ICollection<EngagementDto> results)
        {
            int _randomEngagementIndex = rand.Next(results.Count);
            EngagementDto _randomEngagement = results.ElementAt(_randomEngagementIndex);
            return _randomEngagement;

        }

    }

}
