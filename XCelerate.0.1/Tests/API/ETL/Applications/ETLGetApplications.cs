using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;
using System;

namespace Tests.API.ETL.Applications
{
    [TestClass]
    public class ETLGetApplications : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("310668 | GET / Applications - valid")]
        public async Task GetApplicationsTest()
        {
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Applications_GetListAsync());
            _result.Should().NotBeNull();
            VerifyApplicationsFields(_result.Results);
        }

        [TestMethod]
        [Description("310669 | GET /Applications - unauthorized request")]
        public async Task GetApplicationsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Applications_GetListAsync());
        }

        private static void VerifyApplicationsFields(ICollection<ApplicationListDto> _results)
        {
            foreach (var item in _results)
            {
                VerifyApplicationsFields(item);
            }
        }
        private static void VerifyApplicationsFields(ApplicationListDto _applicationsActualResult)
        {
            Assert.IsNotNull(_applicationsActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(_applicationsActualResult.Name, "Name should not be empty");
            Assert.IsNotNull(_applicationsActualResult.Description, "Name should not be empty");
        }
    }

}