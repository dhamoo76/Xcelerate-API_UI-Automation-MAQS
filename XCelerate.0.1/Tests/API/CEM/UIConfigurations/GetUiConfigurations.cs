using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.UIConfigurations
{
    [TestClass]
    public class GetUiConfigurations : CEMBaseTest
    {

        [TestMethod]
        [Description("233041 |235242 | Get all uiConfigurations")]
        public async Task GetUIConfigurationsAsync()
        {
            var result =
                await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.UIConfigurations_GetAllAsync());
            result.Should().NotBeNull();
            this.VerifyUiConfigurationsFields(result);
        }

        [TestMethod]
        [Description("248754 | Get all uiConfigurations without token ")]
        public async Task GetUIConfigurationsWithoutToken()
        {
            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.UIConfigurations_GetAllAsync());
        }

        //@TODO : Andrei - Need add ability to verify object class property
        //@TODO : Olya - Update tests after Andrei's updates
        private void VerifyUiConfigurationsFields(GetUIConfigurationQueryResponse _responseClientDto)
        {
            _responseClientDto.EngagementStatuses.Should().NotBeNullOrEmpty();
            _responseClientDto.EngagementTypesWithGroups.Should().NotBeNullOrEmpty();
            _responseClientDto.LineOfBusiness.Should().NotBeNullOrEmpty();
            _responseClientDto.ProjectStatuses.Should().NotBeNullOrEmpty();
        }
    }
}
