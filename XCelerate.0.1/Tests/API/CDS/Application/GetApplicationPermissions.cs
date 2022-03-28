using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CDS.Application
{
    [TestClass]
    public class GetApplicationPermissions : CDSBaseTest
    {
        private readonly Dictionary<string, string> _expectedPermissions = new Dictionary<string, string>(){
                {"List_Structure_Create", "Create List (Structure)"},
                {"List_Structure_Delete",  "Delete List (Structure)"},
                {"List_Structure_Update", "Update List (Structure)"},
                {"List_StructureData_Upload",  "Upload List (Structure/Data)"},
                {"List_Structure_View", "View List (Structure= list Metadata + Array of List Fields)"},
                {"List_Data_Add_Update", "Add, Update List (Data)"},
                {"List_Data_Delete", "Delete List (Data)"},
                {"List_Data_View",  "View List (Data)"} };

        [TestMethod]
        [Description("291848 | GET / All Permissions for Admin CDS")]
        public async Task GetApplicationAllPermissionsTest()
        {
            var result = await CdsHttpResponseHelper.VerifySuccessfulStatusAsync(() => CdsUserClient.Application_GetAsync());
            VerifyApplicationsFields(result.Results);
        }

        [TestMethod]
        [Description("314931 | GET / Permissions by non authorized request")]
        public async Task GetAllPermissionsWithoutTokenTest()
        {
            await CdsHttpResponseHelper.VerifyUnauthorizedAsync(() => CdsNoTokenClient.Application_GetAsync());
        }

        private void VerifyApplicationsFields(ICollection<RSM.Xcelerate.CDS.Service.Client.PermissionDto> actualResults)
        {
            Dictionary<string, string> permissionListToCompare = new Dictionary<string, string>();
            actualResults.Should().NotBeEmpty();
            permissionListToCompare = actualResults.ToDictionary(x => x.Code, x => x.Name);
            actualResults.AsParallel().ForAll(item =>
            {
                Assert.IsNotNull(item.Name, "Name should not be empty");
                Assert.IsNotNull(item.Code, "Code should not be empty");
            });
            CollectionAssert.AreEquivalent(permissionListToCompare.ToList(), _expectedPermissions.ToList(),
                "Actual permission result count should match with expected list");
        }
    }
}
