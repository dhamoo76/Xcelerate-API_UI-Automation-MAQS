using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;
using ClientDto = RSM.Xcelerate.CEM.Service.Client.ClientDto;

namespace Tests.API.CEM.Assignments
{
    [TestClass]
    public class DeleteAssignments : CEMBaseTest
    {
        String _activeStatus = "Active";
        String _inactiveStatus = "Inactive";
        // String _randomAppName;
        String _randomAppId;
        int _randomClientId;
        String _randomRoleIDForApp;
        RolesToAppDto roles = new RolesToAppDto();
        String _randomUserID;
        Guid _randomEngagementId;
        Guid _randomProjectIdWithDefinedStatus;
        Guid _randomEntityID;

        [TestInitialize]
        public async Task CollectDataForAssignment()
        {
            /*  var _resultClients = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetAllAsync());
              _resultClients.Should().NotBeNull();
              ClientDto _randomClient = this.GetRandomClient(_resultClients.Results);*/
            _randomClientId = Constants.CemClientId;// _randomClient.MdmClientId;
            Console.WriteLine("Random Client Id : " + _randomClientId);

            var _resultApplications = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Applications_GetAllAsync());
            _resultApplications.Should().NotBeNull();

            _randomAppId = this.GetAppWithRoles(_resultApplications.Results);
            Console.WriteLine("Random  Application ID : " + _randomAppId);

            string filterQueryApp = "Id eq '" + _randomAppId + "'";
            var _resultUserRolesForApp = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Applications_GetAllAsync(filter: filterQueryApp));
            _randomRoleIDForApp = this.GetRandomRole(_resultUserRolesForApp.Results);
            Console.WriteLine("Random  Role ID : " + _randomRoleIDForApp);

            roles.RoleId = Guid.Parse(_randomRoleIDForApp);
            roles.ApplicationId = _randomAppId;

            var _resultUsersByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetUserListAsync(_randomClientId));
            _resultUsersByMDMClientID.Should().NotBeNull();
            _randomUserID = this.GetRandomUserId(_resultUsersByMDMClientID.Results);
            Console.WriteLine("Random  User ID : " + _randomUserID);

            string filterQuery = "mdmClientId eq '" + _randomClientId + "'";
            var _resultProjectsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: filterQuery));
            _resultProjectsByMDMClientID.Should().NotBeNull();
            _randomProjectIdWithDefinedStatus = this.GetRandomProjectWithDefinedStatus(_resultProjectsByMDMClientID.Results, _activeStatus);
            Console.WriteLine("Random  Project ID : " + _randomProjectIdWithDefinedStatus);

            var _resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            _resultEngagementsByMDMClientID.Should().NotBeNull();
            _randomEngagementId = this.GetRandomEngagementWithDefinedStatus(_resultEngagementsByMDMClientID.Results,_activeStatus).Id;
            Console.WriteLine("Random  Engagement ID : " + _randomEngagementId);

            string filterQueryEntity = "MdmMasterClientId eq '" + _randomClientId + "'";
            var _resultEntitiesByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: filterQueryEntity));
            _resultEntitiesByMDMClientID.Should().NotBeNull();
            _randomEntityID = this.GetRandomEntity(_resultEntitiesByMDMClientID.Results).Id;
            Console.WriteLine("Random  Entity ID : " + _randomEntityID);

        }

        [TestMethod]
        [Description("230804 | Delete assignments")]
        public async Task DeleteOneAssignment()
        {

            IEnumerable<Guid> ids = new List<Guid>();

            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ClientIds = new List<int> { _randomClientId },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();
            ids = ids.Append(_resultCreateAssignments.Id);
            var _resultDeleteAssignmet = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_DeleteAsync(ids));
        }

        [TestMethod]
        [Description("230805 | Delete few assignments")]
        public async Task DeleteFewAssignment()
        {

            IEnumerable<Guid> ids = new List<Guid>();

            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ClientIds = new List<int> { _randomClientId },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();
            ids = ids.Append(_resultCreateAssignments.Id);

            var requestTheSecond = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ClientIds = new List<int> { _randomClientId },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignmentsTheSecond = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(requestTheSecond));
            _resultCreateAssignmentsTheSecond.Should().NotBeNull();
            ids = ids.Append(_resultCreateAssignmentsTheSecond.Id);

            var _resultDeleteAssignmet = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_DeleteAsync(ids));
        }

        private ClientDto GetRandomClient(ICollection<ClientDto> results)
        {
            Random random = new Random();
            int _randomClientIndex = random.Next(results.Count);
            ClientDto _randomClient = results.ElementAt(_randomClientIndex);
            return _randomClient;
        }

        private EngagementDto GetRandomEngagementWithDefinedStatus(ICollection<EngagementDto> results, string _status)
        {
            string _statusExpected = "";
            EngagementDto _randomEngagement = new EngagementDto();
            while ( _statusExpected != _status)
            {
                Random random = new Random();
                int _randomEngagementIndex = random.Next(results.Count);
                _randomEngagement = results.ElementAt(_randomEngagementIndex);
                _statusExpected = _randomEngagement.Status;
            }

            return _randomEngagement;
           
        }

        private LegalEntityDto GetRandomEntity(ICollection<LegalEntityDto> results)
        {
            Random random = new Random();
            int _randomEntityIndex = random.Next(results.Count);
            LegalEntityDto _randomEntity = results.ElementAt(_randomEntityIndex);
            return _randomEntity;
        }

        private String GetAppWithRoles(ICollection<ApplicationDto> results)
        {

            ApplicationDto _appWithRoles = new ApplicationDto();

            foreach (ApplicationDto _app in results)
            {
                if (_app.Roles.Count > 0)
                {
                    _appWithRoles = _app;
                    break;
                }

            }

            return _appWithRoles.Id;
        }

        private String GetRandomRole(ICollection<ApplicationDto> results)
        {
            Random random = new Random();
            ApplicationDto _application = results.ElementAt(0);
            RoleNameDto _randomRole = _application.Roles.ElementAt(random.Next(_application.Roles.Count));
            return _randomRole.Id;

        }

        private String GetRandomUserId(ICollection<UserDto> results)
        {
            string _status = "Inactive";
            UserDto _randomUser = new UserDto();
            while (_status == "Inactive")
            {
                Random random = new Random();
                int _randomUserIndex = random.Next(results.Count);
                _randomUser = results.ElementAt(_randomUserIndex);
                _status = _randomUser.Status;
            }

            return _randomUser.Id;
        }

        private Guid GetRandomProjectWithDefinedStatus(ICollection<ProjectDto> results, String status)
        {
            ProjectDto _randomProject = new ProjectDto();
            foreach (ProjectDto _project in results)
            {
                if (_project.Status.Equals(status))
                {
                    _randomProject = _project;
                    break;
                }
            }

            return _randomProject.Id;

        }

    }
}
