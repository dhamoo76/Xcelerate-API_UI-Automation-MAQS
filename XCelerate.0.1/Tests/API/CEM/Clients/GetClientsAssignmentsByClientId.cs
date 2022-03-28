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

namespace Tests.API.CEM.Clients
{
    [TestClass]
    public class GetClientsAssignmentsByClientId : CEMBaseTest
    {
        String _activeStatus = "Active";
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
            /*var _resultClients = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetAllAsync());
            _resultClients.Should().NotBeNull();
            ClientDto _randomClient = this.GetRandomClient(_resultClients.Results);*/
            _randomClientId = Constants.CemClientId;
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
            _randomEngagementId = this.GetRandomEngagement(_resultEngagementsByMDMClientID.Results).Id;
            Console.WriteLine("Random  Engagement ID : " + _randomEngagementId);

            string filterQueryEntity = "MdmMasterClientId eq '" + _randomClientId + "'";
            var _resultEntitiesByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: filterQueryEntity));
            _resultEntitiesByMDMClientID.Should().NotBeNull();
            _randomEntityID = this.GetRandomEntity(_resultEntitiesByMDMClientID.Results).Id;
            Console.WriteLine("Random  Entity ID : " + _randomEntityID);

        }

        //@TODO :
        [TestMethod]
        [Description("289665|[X-CEM][UI][Manage_Assign] {Client centric view} GET/api/v1/Clients/{mdmClientId}/assignments is used for getting all existing user/UserGroup assignments in a client context ")]
        public async Task CreateAssignmentToJob()
        {
            //Generate special assign body :
            var _assigneEmptyEntitytToProjectRequest = new ManageEntityToProjectAssignmentsCommandRequest()
            {
                LegalEntities = new List<Guid> { }
            };

            var _resultUnassignEntity = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(_randomProjectIdWithDefinedStatus, _assigneEmptyEntitytToProjectRequest));
            _resultUnassignEntity.Should().NotBeNull();

            //Generate special assign body :
            var _assigneEntitytToProjectRequest = new ManageEntityToProjectAssignmentsCommandRequest()
            {
                LegalEntities = new List<Guid> { _randomEntityID }
            };

            var _resultAssignEntity = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(_randomProjectIdWithDefinedStatus, _assigneEntitytToProjectRequest));
            _resultAssignEntity.Should().NotBeNull();

            //find jobID
            string filterQueryJob = "mdmClientId eq '" + _randomClientId + "' and projectId eq '" + _randomProjectIdWithDefinedStatus + "' and LegalEntityId eq '" + _randomEntityID + "'";

            var _resultJobs = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Jobs_GetListAsync(filter: filterQueryJob));
            String _jobId = _resultJobs.Results.ElementAt(0).Id;
            Console.WriteLine("Job ID : " + _jobId);


            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                JobIds = new List<Guid> { Guid.Parse(_jobId) }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

            var _resultGetAllssignmentsForClient = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Clients_GetUserAssignmentListByClientAsync(_randomClientId));
            _resultGetAllssignmentsForClient.Results.Should().NotBeNull();  
        }

        private ClientDto GetRandomClient(ICollection<ClientDto> results)
        {
            Random random = new Random();
            int _randomClientIndex = random.Next(results.Count);
            ClientDto _randomClient = results.ElementAt(_randomClientIndex);
            return _randomClient;
        }

        private ApplicationDto GetRandomApplication(ICollection<ApplicationDto> results)
        {
            Random random = new Random();
            int _randomApplicationIndex = random.Next(results.Count);
            ApplicationDto _randomApplication = results.ElementAt(_randomApplicationIndex);
            return _randomApplication;
        }

        private EngagementDto GetRandomEngagement(ICollection<EngagementDto> results)
        {
            Random random = new Random();
            int _randomEngagementIndex = random.Next(results.Count);
            EngagementDto _randomEngagement = results.ElementAt(_randomEngagementIndex);
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

        private void CheckJobAssignmentIsExisting(ICollection<Object> results)
        {
       //   results

        }

    }
}
