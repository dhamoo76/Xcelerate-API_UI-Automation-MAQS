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
    public class ETLGetApplicationsPermissions : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("310670 | GET / ApplicationsPermissions - valid")]
        public async Task GetApplicationPermissionsTest()
        {
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Applications_GetList2Async());
            _result.Should().NotBeNull();
        }

        [TestMethod]
        [Description("310671 | GET /ApplicationsPermissions - unauthorized request")]
        public async Task GetApplicationPermissionsWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Applications_GetList2Async());
        }

    }
}