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
    public class PostAssignments : CEMBaseTest
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

            //@TODO - Commented while clients cant be getted ftom BE with a list
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
        [Description("312513 | Check assignment can be created - create assignment to client")]
        public async Task CreateAssignmentToAClientt()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ClientIds = new List<int> { _randomClientId },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();
            
        }

        [TestMethod]
        [Description("243530|Check assignment cant be created without token - 401")]
        public async Task CreateAssignmentWithoutToken()
        {

            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ClientIds = new List<int> { _randomClientId },
                ApplicationIds = new List<string> { _randomAppId },

            };

            await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemNoTokenClient.Assignments_CreateAsync(request));

        }

        [TestMethod]
        [Description("312514 | Check assignment can be created - create assignment to Engagement")]
        public async Task CreateAssignmentToEngagement()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                EngagementIds = new List<Guid> { _randomEngagementId }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("312515 | Check assignment can be created - create assignment to Project")]
        public async Task CreateAssignmentToProject()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                ProjectIds = new List<Guid> { _randomProjectIdWithDefinedStatus }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("312516 | Check assignment can be created - create assignment to Entity")]
        public async Task CreateAssignmentToEntity()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                EntityIds = new List<Guid> { _randomEntityID }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("230343| Check assignment can be created - create assignment to Job")]
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
            Console.WriteLine("Job ID : "+_jobId);


            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                JobIds = new List<Guid> { Guid.Parse(_jobId) }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

       

        [TestMethod]
        [Description("251206| POST/assignments- check validation appears when jobId is inputted incorrectly")]
        public async Task ValidateCreateAssignmentToNonValidJob()
        {
           
            String _jobId = "7ef80a2d-0648-4964-6e7e-08d9c0793d64";
            Console.WriteLine("Job ID : " + _jobId);


            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                JobIds = new List<Guid> { Guid.Parse(_jobId) }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifyNotFoundAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().BeNull();

        }

        [TestMethod]
        [Description("230342| Check Validation for assignment without role")]
        public async Task CreateAssignmentRoleToUserWithNoRole()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("312504 | Check Validation for assignment without User")]
        public async Task CreateAssignmentRoleToUserWithNoUser()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("294614 | Check assignment is possible when NO object to assign")]
        public async Task CreateAssignmentUserToRole()
        {
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

        }

        [TestMethod]
        [Description("312522| Check no assignments to INACTIVE objects are allowed - engagement")]
        public async Task CreateAssignmentToInactiveEngagement()
        {
            string filterQuery = "mdmClientId eq '" + _randomClientId + "'";
            var _resultEngagementsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_GetAllAsync(filter: filterQuery));
            Guid _randomEngagementIdInactive = this.GetRandomEngagementWithDefinedStatus(_resultEngagementsByMDMClientID.Results, _inactiveStatus).Id;
            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                EngagementIds = new List<Guid> { _randomEngagementIdInactive }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Assignments_CreateAsync(request));

        }

        [TestMethod]
        [Description("230345 | Check no assignments to INACTIVE objects are allowed - project")]
        public async Task CreateAssignmentToInactiveProject()
        {
            string filterQuery = "mdmClientId eq '" + _randomClientId + "'";
            var _resultProjectsByMDMClientID = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: filterQuery));
            Guid _randomProjectIdInactive = this.GetRandomProjectWithDefinedStatus(_resultProjectsByMDMClientID.Results, _inactiveStatus);

            var request = new CreateUserAssignmentCommandRequest()
            {
                UserIds = new List<string> { _randomUserID },
                Roles = new List<RolesToAppDto> { roles },
                ApplicationIds = new List<string> { _randomAppId },
                ProjectIds = new List<Guid> { _randomProjectIdInactive }

            };

            var _resultCreateAssignments = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Assignments_CreateAsync(request));
            _resultCreateAssignments.Should().NotBeNull();

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
