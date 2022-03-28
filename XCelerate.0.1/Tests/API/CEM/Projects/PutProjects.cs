using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.CEM.Service.Client;
using Tests.API.Utilities;
using Tests.API.Utilities.Helpers;

namespace Tests.API.CEM.Projects
{
    [TestClass]
    public class PutProjects : CEMBaseTest
    {
        String maxLengthName = GenerateRandomString(50);
        String tooLongName = GenerateRandomString(51);
        String tooLongDescription = GenerateRandomString(151);
        String maxLengthDescription = GenerateRandomString(150);

        const Int32 validYearMin = 2010;
        const Int32 invalidYearMin = 2009;
        const Int32 validYearMax = 9999;
        const Int32 invalidYearMax = 10000;

        // Generate Random String
        public static string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return finalString;

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207398 |PUT /projects with all blank schema values: with attributes without values")]
        public async Task PutBlankProjectAuthorizedAsync()
        {

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = 0,
                Name = "",
                Description = "",
                LineOfBusiness = "",
                Type = "",
                ScheduledStartDate = null,
                ScheduledEndDate = null,
                Id = Guid.Empty,
                Status = ""
            };

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            Guid projectId = GetRandomProjectId(projects.Results);

            var result = await CEMHttpResponseHelper.VerifyBadRequestAsync(() => CemUserClient.Projects_PutAsync(projectId, request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207399|224329 |PUT /projects with all blank schema values: with attributes without values without token")]
        public async Task PutBlankProjectUnauthorizedAsync()
        {

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = 0,
                Name = "",
                Description = "",
                LineOfBusiness = "",
                Type = "",
                ScheduledStartDate = null,
                ScheduledEndDate = null,
                Id = Guid.Empty,
                Status = ""
            };

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            Guid projectId = GetRandomProjectId(projects.Results);

            var result = await CEMHttpResponseHelper.VerifyUnauthorizedAsync(() => CemUserClient.Projects_PutAsync(projectId, request));

        }

        private System.Guid GetRandomProjectId(ICollection<ProjectDto> results)
        {
            int length = results.Count;
            if (length > 0)
            {
                Random rnd = new Random();
                int num = rnd.Next(0, length);
                Guid projectId = results.ElementAt(num).Id;
                return projectId;
            }
            else
            {
                Console.WriteLine("There are no projects available!");
                throw new InvalidOperationException("Id cannot be returned");
            }

        }

        private static IEnumerable<object[]> GetCasesValidYear()
        {
            var ValidYears = new List<Int32>
            {
                validYearMin,
                validYearMax
            };

            return IterateParams(ValidYears);
        }

        private static IEnumerable<object[]> GetCasesNotValidYear()
        {
            var ValidYears = new List<Int32>
            {
                invalidYearMin,
                invalidYearMax
            };

            return IterateParams(ValidYears);
        }

        private static IEnumerable<object[]> IterateParams(List<Int32> years)
        {
            for (var i = 0; i < years.Count; i++)
            {
                yield return new GetTestCase
                {
                    year = years[i]
                }.ToObjectArray();
            }
        }

        private struct GetTestCase
        {
            public Int32 year { get; set; }

            public object[] ToObjectArray()
            {
                return new object[] { year };
            }
        }

        [TestMethod, TestCategory("Smoke")]
        [DynamicData(nameof(GetCasesValidYear), DynamicDataSourceType.Method)]
        [Description("207889 | 233477 | 207396 |PUT /projects with all valid schema values - max/min Year border")]
        public async Task PutProjectWithMinMaxYearAsync(Int32 validYear)
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYear,
                Name = maxLengthName,
                Description = maxLengthDescription,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));
            Console.WriteLine("Resulting LOB: " + result.LineOfBusiness);
            string jsonString = JsonSerializer.Serialize(request);
            Console.WriteLine("Json: " + jsonString);
            VerifyCEMProjectUpdateFieldsCollection(result, request);
            //Can't change "lineOfBusiness" potentially a bug / created unable from UI combination 
            /*{
            "id": "a8e92253-0fd7-449e-8f7b-59f14d5f5c0c",
            "engagementId": "5ca8284c-0193-4ad4-b384-2bf2e97a5cc6",
            "mdmClientId": 7716836,
            "projectYear": 9999,
            "name": "AEZaWC85rrtvQP86V5uC2WQdaCMs8mEQfucwxN6naeubBtdXvV",
            "lineOfBusiness": "Wealth Management",
            "description": "V7rmcr4mihVl2DrjhBl9GuHJx9ytGr6sq7ul1HDIIRuR331QIvC08RIMUg3osFAL7JjnnF2JRswIT3zMRvcuZggFg6QsHgmv2VCk6GLPwqtNS381gYCk3ksDAUyk3dEU4BnpgIj7yDio9TnHPdi2Uf",
            "status": "Inactive",
            "type": "Compilation",
            "scheduledStartDate": "2022-03-16T00:14:03.6433824+01:00",
            "scheduledEndDate": "2022-03-17T00:14:03.6433824+01:00",
            "creatorId": "E092080"
            }*/
        }

        [TestMethod, TestCategory("Smoke")]
        [DynamicData(nameof(GetCasesNotValidYear), DynamicDataSourceType.Method)]
        [Description("207889 |PUT /projects with Not valid schema values - min/max Year boarder")]
        public async Task PutProjectWithMinMaxYearNotValidAsync(Int32 notValidYear)
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = notValidYear,
                Name = maxLengthName,
                Description = maxLengthDescription,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207392-6 |PUT /projects with Not valid schema values - too Long Description edge case")]
        public async Task PutProjectTooLongDescriptionAsync()
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = maxLengthName,
                Description = tooLongDescription,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207430 |PUT /projects with Not valid schema values - too Long Name edge case")]
        public async Task PutProjectTooLongNameAsync()
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = tooLongName,
                Description = maxLengthDescription,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207430 |PUT /projects with Not valid schema values - scheduledStartDate > scheduledEndDate")]
        public async Task PutProjectStartLaterThanEndAsync()
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today.AddDays(1),
                ScheduledEndDate = today,
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207430 |POST /projects with  Not valid schema values - scheduledStartDate = scheduledEndDate")]
        public async Task PutProjectStartEqualToEndAsync()
        {
            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var _projectLineOfBusiness = project.LineOfBusiness;
            string _ptojectLOBForUpdate;
            string _projectType;
            if (_projectLineOfBusiness == "Audit")
            {
                _ptojectLOBForUpdate = "Tax";
                _projectType = "Tax Return";
            }
            else
            {
                _ptojectLOBForUpdate = "Audit";
                _projectType = "Audit";
            }

            Console.WriteLine("Project status: " + _projectStatus);
            Console.WriteLine("Project status for update: " + _projectStatusForUpdate);
            Console.WriteLine("Project lineofbusiness for update: " + _ptojectLOBForUpdate);
            Console.WriteLine("Project type for update: " + _projectType);

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = _ptojectLOBForUpdate,
                Type = _projectType,
                ScheduledStartDate = today.AddDays(1),
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("207647 |PUT Its forbidden to Update project for engagement with No Active status")]
        public async Task PutProjectForNotActiveEngagementIsForbiddenAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            // Post New Engagement and remember its ID
            var requestToCreateEngagement = new CreateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(2),
                Sow = "",
                SowDate = today,
                MdmClientId = Constants.CemClientId
            };

            var _responseCreateEngagement = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_CreateAsync(requestToCreateEngagement));
            Assert.IsNotNull(_responseCreateEngagement, "Error of Engagement creation. Response is empty");
            Guid _engagementId = _responseCreateEngagement.Id;

            //Post new Project for this Engagement
            var requestToCreateProject = new CreateProjectCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                EngagementId = _engagementId,
                ProjectYear = validYearMin,
                MdmClientId = Constants.CemClientId
            };

            var _responseCreateProject = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_CreateAsync(requestToCreateProject));
            Assert.IsNotNull(_responseCreateProject, "Error of Project creation. Response is empty");
            var _projectId = _responseCreateProject.Id;

            //Update engagement with No active status
            var requestToUpdateEngagement = new UpdateEngagementCommandRequest()
            {
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Tax",
                Type = "Tax Return",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(2),
                Sow = "",
                SowDate = today,
                Id = _engagementId,
                Status = "Inactive"
            };

            var _responseToUpdateEngagement = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Engagements_PutAsync(_engagementId, requestToUpdateEngagement));
            Assert.IsNotNull(_responseToUpdateEngagement, "Error of Engagement updating. Response is empty");

            //Update project for No active Engagement
            var requestToUpdateProject = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _projectId,
                Status = "Inactive"
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_projectId, requestToUpdateProject));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("233478  | PUT /projects/{id} with inconsistent LOB")]
        public async Task PutProjectUpdateWithNotValidLOBAsync()
        {

            var timestamp = DateTime.Now.ToFileTime();
            DateTimeOffset today = DateTimeOffset.Now;

            var projects = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync());
            var _randomProjectId = GetRandomProjectId(projects.Results);
            Console.WriteLine("Project ID: " + _randomProjectId);
            var project = CemUserClient.Projects_GetByIdAsync(_randomProjectId).Result;
            var _clientId = project.MdmClientId;
            Console.WriteLine("Client ID: " + _clientId);
            var _engagementId = project.EngagementId;
            Console.WriteLine("Engagement ID: " + _engagementId);

            var _projectStatus = project.Status;
            string _projectStatusForUpdate;
            if (_projectStatus == "Active")
            {
                _projectStatusForUpdate = "Inactive";
            }
            else
            {
                _projectStatusForUpdate = "Active";
            }

            var request = new UpdateProjectCommandRequest()
            {
                ProjectYear = validYearMin,
                Name = "AutoName_" + timestamp,
                Description = "AutoDescription_" + timestamp,
                LineOfBusiness = "Audit2",
                Type = "Audit",
                ScheduledStartDate = today,
                ScheduledEndDate = today.AddDays(1),
                Id = _randomProjectId,
                Status = _projectStatusForUpdate
            };

            var result = await CEMHttpResponseHelper.VerifyValidationErrorAsync(() => CemUserClient.Projects_PutAsync(_randomProjectId, request));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("238757 | 238784 | API call allows to assign ONE entity to a particular project")]
        public async Task PutProjectAssignEntityAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };

            var request = GetProjectsManageLERequest(legalEntityId_1);

            var response = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, request));
            Assert.IsTrue(response.Assigned.Any(p => p == ToStringList(legalEntityId_1)));
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("238280, 238759 | API request is called to assign/un-assign an array of a given client\'s legal entities to a project within that client")]
        public async Task PutProjectAssignUnassignEntityAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_2_3 = new List<Guid> { legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };

            var requestOne = GetProjectsManageLERequest(legalEntityId_1);
            var responseOne = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestOne));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_1)));

            var requestTwo = GetProjectsManageLERequest(legalEntityId_2_3);
            var responseTwo = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestTwo));
            Assert.IsTrue(responseTwo.Assigned.Any(p => p == ToStringList(legalEntityId_2)));
            Assert.IsTrue(responseTwo.Assigned.Any(p => p == ToStringList(legalEntityId_3)));
            Assert.IsFalse(responseTwo.Assigned.Any(p => p == ToStringList(legalEntityId_1)));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("236364, 236057 | API call allows to assign A SCOPE of entities to a particular project")]
        public async Task PutProjectAssignScopeOfEntitiesAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_2_3 = new List<Guid> { legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };

            var request = GetProjectsManageLERequest(legalEntityId_2_3);
            var response = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, request));
            Assert.IsTrue(response.Assigned.Any(p => p == ToStringList(legalEntityId_2)));
            Assert.IsTrue(response.Assigned.Any(p => p == ToStringList(legalEntityId_3)));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("238758 | Request body should always contain the ids of Legal entities already assigned to the project")]
        public async Task PutProjectRequestShouldAlwaysContainTheIdsAssignedAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_1_2_3 = new List<Guid> { legalEntitiesTuple.Item1, legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };

            var request = GetProjectsManageLERequest(legalEntityId_1_2_3);
            var response = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, request));
            Assert.IsTrue(response.Assigned.Any(p => p == ToStringList(legalEntityId_1)));

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("238302 | When the request body contains all the entities assigned, the system will return an empty response")]
        public async Task PutProjectEmptyResponseIfAllEntitiesAssignedAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_1_2_3 = new List<Guid> { legalEntitiesTuple.Item1, legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };

            var requestOne = GetProjectsManageLERequest(legalEntityId_1_2_3);
            var responseOne = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestOne));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_1)));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_2)));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_3)));
            var responseTwo = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestOne));

            Assert.IsTrue(responseTwo.Assigned.Count == 0);
            Assert.IsTrue(responseTwo.UnAssigned.Count == 0);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("238279 | GET ​/api​/v1​/Projects​/{id}​/legalEntity is able to be called to get all legal entities for a given project")]
        public async Task PutProjectIsAbleToGetAllLegalEntitiesAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            string entityByProject_filterQuery = "ProjectId eq '" + randomProjectId + "'";
            var entitiesByProjectResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entityByProject_filterQuery));


            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_1_2_3 = new List<Guid> { legalEntitiesTuple.Item1, legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };

            VerifyEntitiesFieldsCollection(entitiesByProjectResponse.Results);

        }

        [TestMethod, TestCategory("Smoke")]
        [Description("236365 | It\'s possible to un-assign ONE given client\'s legal entities from a project within an API call")]
        public async Task PutProjectPossibleToUnassignOneClientAsync()
        {
            var entitites = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync());
            string entity_filterQuery = "MdmMasterClientId eq '" + Constants.CemClientId + "'";
            var entitiesResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.LegalEntities_GetListAsync(filter: entity_filterQuery));
            string project_filterQuery = "MdmClientId eq '" + Constants.CemClientId + "'";
            var projectsResponse = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_GetListAsync(filter: project_filterQuery));
            var randomProjectId = GetRandomProjectId(projectsResponse.Results);

            Tuple<Guid, Guid, Guid> legalEntitiesTuple = GetLegalEntitiesTuple(entitiesResponse);
            ICollection<Guid> legalEntityId_1 = new List<Guid> { legalEntitiesTuple.Item1 };
            ICollection<Guid> legalEntityId_2 = new List<Guid> { legalEntitiesTuple.Item2 };
            ICollection<Guid> legalEntityId_3 = new List<Guid> { legalEntitiesTuple.Item3 };

            ICollection<Guid> legalEntityId_1_2_3 = new List<Guid> { legalEntitiesTuple.Item1, legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };
            ICollection<Guid> legalEntityId_2_3 = new List<Guid> { legalEntitiesTuple.Item2, legalEntitiesTuple.Item3 };


            var requestOne = GetProjectsManageLERequest(legalEntityId_1_2_3);
            var responseOne = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestOne));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_1)));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_2)));
            Assert.IsTrue(responseOne.Assigned.Any(p => p == ToStringList(legalEntityId_3)));

            var requestTwo = GetProjectsManageLERequest(legalEntityId_2_3);
            var responseTwo = await CEMHttpResponseHelper.VerifySuccessfulStatusAsync(() => CemUserClient.Projects_ManageLEAsync(randomProjectId, requestTwo));
            Assert.IsTrue(responseTwo.Assigned.Count == 0);
            Assert.IsTrue(responseTwo.UnAssigned.Any(p => p == ToStringList(legalEntityId_1)));

        }

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

        public string ToStringList<T>(ICollection<T> list)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var elem in list)
            {
                sb.Append(elem.ToString());
                if (i++ != list.Count - 1) sb.Append(", ");
            }
            return sb.ToString();
        }

        private ManageEntityToProjectAssignmentsCommandRequest GetProjectsManageLERequest(ICollection<Guid> legalEntityId)
        {

            var legalEntitiesIDList = new List<Guid> { };

            foreach (var entity in legalEntityId)
            {
                legalEntitiesIDList.Add(entity);
            }

            return new ManageEntityToProjectAssignmentsCommandRequest()
            { 
                LegalEntities = legalEntityId
            };
        }

        private Tuple <Guid, Guid, Guid> GetLegalEntitiesTuple(GetLegalEntityListQueryResponse entitiesResponse)
        {
            var legalEntities = entitiesResponse.Results;

            var legalEntitiesIDList = new List<Guid> { };

            foreach (var item in legalEntities)
            {
                legalEntitiesIDList.Add(item.Id);
            }
            try
            {
                Guid legalEntityId_1 = legalEntitiesIDList[0];
                Guid legalEntityId_2 = legalEntitiesIDList[1];
                Guid legalEntityId_3 = legalEntitiesIDList[2];
                return Tuple.Create(legalEntityId_1, legalEntityId_2, legalEntityId_3);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

            /*if (legalEntitiesIDList.Count > 0)
            {
                legalEntityId_1 = legalEntitiesIDList[1];
            }

            else if (legalEntities.Count > 1)
            {
                legalEntityId_2 = legalEntitiesIDList[2];
            }

            else if (legalEntities.Count > 2)
            {
                legalEntityId_3 = legalEntitiesIDList[3];
            }

            else if (legalEntities.Count == 1)
            {
                Console.WriteLine("There is only one entity available.");
            }

            else if (legalEntities.Count == 2)
            {
                Console.WriteLine("There are only two entities available.");
            }

            else
            {
                Console.WriteLine("WARNING. No Entities found");
            }*/
        }

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

        private void VerifyCEMProjectUpdateFieldsCollection(UpdateProjectCommandResponse _results, UpdateProjectCommandRequest _request)
        {
            Assert.AreEqual(_request.Name, _results.Name);
            Assert.AreEqual(_request.Description, _results.Description);
            Assert.AreEqual(_request.LineOfBusiness, _results.LineOfBusiness);
            Assert.AreEqual(_request.Type, _results.Type);
            Assert.AreEqual(_request.ProjectYear, _results.ProjectYear);

            Assert.IsNotNull(_results.CreatorId, "CreatorID should not be empty");
            Assert.IsNotNull(_results.Id, "Id should not be empty");
            Assert.AreEqual(_results.Status, "Active");

        }


    }

}
